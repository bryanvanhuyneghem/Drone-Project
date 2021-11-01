using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Globalization;

namespace DroneWebApp.Models.SimpleFactoryPattern.Parsers
{
	public class XYZParser : IParser
	{
		private ConnectionStringSettings connStringSet;
		private DbProviderFactory factory; 

		public XYZParser()
		{
			connStringSet = ConfigurationManager.ConnectionStrings["DroneDB_ADONET"];
			factory = DbProviderFactories.GetFactory(connStringSet.ProviderName);
		}

		public bool Parse(string path, int flightId, DroneDBEntities db)
		{
			// Get the approriate DroneFlight that goes with this data
			DroneFlight droneFlight = db.DroneFlights.Find(flightId);


			// Do not parse a new file, if this flight already has an XYZ file
			if (droneFlight.hasXYZ)
			{
				Helper.Helper.SetProgress(100);
				return false;
			}

			try
			{
				// calculate the total amount of lines by going through the whole file once
				int totalLines = Helper.Helper.CountFileLines(path);

				// Parse
				using (TextFieldParser parser = new TextFieldParser(path))
				{
					using (DbConnection connection = factory.CreateConnection())
					{
						connection.ConnectionString = connStringSet.ConnectionString;
						connection.Open();

						int lineNo = 0; //used for progress
						string[] splitLine; //the line to be read

						#region Set culture to ensure decimal point
						CultureInfo customCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
						customCulture.NumberFormat.NumberDecimalSeparator = ".";
						System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
						#endregion

						#region Create command
						DbCommand command = connection.CreateCommand();
						command.Connection = connection;
						command.CommandText = "insert into PointCloudXYZ " +
							"(X, Y, Z, Red, Green, Blue, Intensity, FlightId)" +
							"VALUES (@X, @Y, @Z, @Red, @Green, @Blue, @Intensity, @FlightId)";
						#endregion

						#region Create parameters
						DbParameter param = factory.CreateParameter();
						param.ParameterName = "@X";
						param.DbType = DbType.Double;
						command.Parameters.Add(param);

						DbParameter param2 = factory.CreateParameter();
						param2.ParameterName = "@Y";
						param2.DbType = DbType.Double;
						command.Parameters.Add(param2);

						DbParameter param3 = factory.CreateParameter();
						param3.ParameterName = "@Z";
						param3.DbType = DbType.Double;
						command.Parameters.Add(param3);

						DbParameter param4 = factory.CreateParameter();
						param4.ParameterName = "@Red";
						param4.DbType = DbType.Int16;
						command.Parameters.Add(param4);

						DbParameter param5 = factory.CreateParameter();
						param5.ParameterName = "@Green";
						param5.DbType = DbType.Int16;
						command.Parameters.Add(param5);

						DbParameter param6 = factory.CreateParameter();
						param6.ParameterName = "@Blue";
						param6.DbType = DbType.Int16;
						command.Parameters.Add(param6);

						DbParameter param7 = factory.CreateParameter();
						param7.ParameterName = "@Intensity";
						param7.DbType = DbType.Double;
						command.Parameters.Add(param7);

						DbParameter param8 = factory.CreateParameter();
						param8.ParameterName = "@FlightId";
						param8.DbType = DbType.Int64;
						command.Parameters.Add(param8);

						#endregion

						// Read data
						while (!parser.EndOfData)
						{
							try
							{
								splitLine = parser.ReadLine().Split(' ');

								#region set parameters
								command.Parameters["@X"].Value = double.Parse(splitLine[0], customCulture);
								command.Parameters["@Y"].Value = double.Parse(splitLine[1], customCulture);
								command.Parameters["@Z"].Value = double.Parse(splitLine[2], customCulture);

								if (splitLine.Length == 7)
								{
									command.Parameters["@Intensity"].Value = double.Parse(splitLine[3], customCulture);

									command.Parameters["@Red"].Value = splitLine[4];
									command.Parameters["@Green"].Value = splitLine[5];
									command.Parameters["@Blue"].Value = splitLine[6];
								}
								else //splitline length 6 
								{
									command.Parameters["@Intensity"].Value = DBNull.Value;

									command.Parameters["@Red"].Value = splitLine[3];
									command.Parameters["@Green"].Value = splitLine[4];
									command.Parameters["@Blue"].Value = splitLine[5];
								}

								command.Parameters["@FlightId"].Value = droneFlight.FlightId;
								#endregion

								command.ExecuteNonQuery(); //execute

								#region progressbar
								lineNo++;
								if ((lineNo % 10) == 0)
								{
									Helper.Helper.SetProgress((lineNo / (double)totalLines) * 100);
								}
								#endregion
							}
							catch (Exception ex)
							{
								System.Diagnostics.Debug.WriteLine(ex);
								return false;
							}
						}
						connection.Close();
					}
				}
				//Set hasXYZ to true
				droneFlight.hasXYZ = true;
				db.SaveChanges();

				Helper.Helper.SetProgress(100);
				return true;
			}
			catch (Exception)
			{
				Helper.Helper.SetProgress(100);
				return false;
			}
		}
	}
}