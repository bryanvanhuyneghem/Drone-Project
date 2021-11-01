using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace DroneWebApp.Models.SimpleFactoryPattern.Parsers
{
    public class TFWParser : IParser
    {
        public bool Parse(string path, int flightId, DroneDBEntities db)
        {
            //Get the appropriate DroneFlight that goes with this data
            DroneFlight droneFlight = db.DroneFlights.Find(flightId);
            TFW tfw;

            // Do not parse a new file, if this flight already has a TFW file
            if (droneFlight.hasTFW)
            {
                Helper.Helper.SetProgress(100);
                return false;
            }

            //Parse
            using (TextFieldParser parser = new TextFieldParser(path))
            {
                //Set culture to ensure decimal point
                CultureInfo customeCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
                customeCulture.NumberFormat.NumberDecimalSeparator = ".";
                System.Threading.Thread.CurrentThread.CurrentCulture = customeCulture;
                try
                {
                    //Create ORM-object for database mapping
                    tfw = new TFW
                    {
                        //Process all elements and store in the right variables
                        xScale_X = float.Parse(parser.ReadLine()),
                        xRotationTerm_Y = float.Parse(parser.ReadLine()),
                        yRotationTerm_X = float.Parse(parser.ReadLine()),
                        yNegativeScale_Y = float.Parse(parser.ReadLine()),
                        TranslationTerm_X = float.Parse(parser.ReadLine()),
                        TranslationTerm_Y = float.Parse(parser.ReadLine())
                    };

                    //Assign data the appropriate flightId
                    tfw.TFWId = droneFlight.FlightId;

                    //Add to list of TFWs to be added to the database
                    db.TFWs.Add(tfw);

                    //Set hasTFW to true
                    droneFlight.hasTFW = true;
                    Helper.Helper.SetProgress(100);
                    //Save changes to the database
                    db.SaveChanges();
                }
                catch(Exception ex) {
                    Helper.Helper.SetProgress(100);
                    System.Diagnostics.Debug.WriteLine(ex);
                    return false;
                }
            }
            return true;
        }
    }
}