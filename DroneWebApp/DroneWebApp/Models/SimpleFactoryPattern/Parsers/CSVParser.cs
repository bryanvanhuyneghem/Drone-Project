using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace DroneWebApp.Models.SimpleFactoryPattern.Parsers
{
    public class CSVParser : IParser
    {
        public bool Parse(string path, int flightId, DroneDBEntities db)
        {
            DroneFlight droneFlight = db.DroneFlights.Find(flightId);
            GroundControlPoint gcp;
            CTRLPoint ctrl;
            bool hasCTRLPoints = droneFlight.hasCTRLs; 
            bool hasGCPPoints = droneFlight.hasGCPs;
            bool readAPoint = false;

            // calculate the total amount of lines by going through the whole file once
            int totalLines = Helper.Helper.CountFileLines(path);
            

            // Parse
            using (TextFieldParser parser = new TextFieldParser(path))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                int lineNo = 0;

                // Set culture
                CultureInfo customCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
                customCulture.NumberFormat.NumberDecimalSeparator = ".";
                System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

                IList<string> fields_string = null;
                IList<double> fields_double = null;
                // Read data
                while (!parser.EndOfData)
                {
                    try
                    {
                        fields_string = parser.ReadFields();
                        fields_double = new List<double>();

                        for (int i = 1; i < 4; i++)
                        {
                            fields_double.Add(double.Parse(fields_string[i], customCulture));
                        }

                        if (fields_string[0].Contains("gcp") && !hasGCPPoints) // If the drone flight initially has GCPs, don't read the new GCP points
                        {
                            readAPoint = true;
                            gcp = new GroundControlPoint
                            {
                                GCPName = fields_string[0],
                                X = fields_double[0],
                                Y = fields_double[1],
                                Z = fields_double[2]
                            };
                            //Assign data the appropriate FlightId
                            gcp.FlightId = droneFlight.FlightId;

                            //Add to list of GroundControlPoints to be added to the database
                            db.GroundControlPoints.Add(gcp);
                            // Set hasCTRLs to true
                            droneFlight.hasGCPs = true;
                        }
                        else if ((fields_string[0].Contains("ctrl") || fields_string[0].Contains("crtl")) && !hasCTRLPoints) // If the drone flight initially has CTRLs, don't read the new CTRL points
                        {
                            readAPoint = true;
                            ctrl = new CTRLPoint
                            {
                                CTRLName = fields_string[0],
                                X = fields_double[0],
                                Y = fields_double[1],
                                Z = fields_double[2]
                            };
                            //Assign data the appropriate FlightId
                            ctrl.FlightId = droneFlight.FlightId;

                            //Add to list of CTRLPoints to be added to the database
                            db.CTRLPoints.Add(ctrl);
                            // Set hasCTRLs to true
                            droneFlight.hasCTRLs = true;
                        }
                        lineNo++;
                        Helper.Helper.SetProgress((lineNo / (double)totalLines) * 100);
                        // Save changes to the database
                        db.SaveChanges();
                        
                    }
                    catch (Exception ex) {
                        Helper.Helper.SetProgress(100);
                        System.Diagnostics.Debug.WriteLine("Exception: " + ex.Message);
                        return false;
                    }
                }
            }
            Helper.Helper.SetProgress(100);
            return readAPoint; // returns true (success) if it read points; returns false if it didn't read anything, because it already had all the points initially
        }
    }
}