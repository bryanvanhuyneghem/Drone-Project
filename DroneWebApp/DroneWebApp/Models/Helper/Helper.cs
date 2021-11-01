using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace DroneWebApp.Models.Helper
{
    public class Helper
    {
        public static double progress = 0;

        // Calculate and update the total flight time drone
        // Must run through all of them every time to verify integrity (in case of reassignment of drone to droneflight)
        public static void UpdateTotalDroneFlightTime(DroneDBEntities db)
        {
            List<Drone> drones = db.Drones.ToList();
            // Calculate the total flight time for each drone
            foreach(Drone d in drones)
            {
                TimeSpan totalTime = new TimeSpan(0, 0, 0, 0);
                // Sum the drone's drone flights' times
                foreach (DroneFlight df in d.DroneFlights)
                {
                    if (df != null && df.hasDroneLog)
                    {
                        totalTime = totalTime.Add(((TimeSpan)df.DestinationInfo.UTCTime).Subtract((TimeSpan)df.DepartureInfo.UTCTime));
                    }
                }
                d.TotalFlightTime = (long)totalTime.TotalSeconds;
                db.SaveChanges();
                // check if the drone needs a check up
                if (!d.needsCheckUp)
                {
                    // if the total flight time is not 0 ticks (0d0h0m0s) 
                    // and the total travelled time (in ticks) is greater than or equal to the next time check (in ticks)
                    if ((totalTime.TotalSeconds != 0) && (totalTime.TotalSeconds >= d.nextTimeCheck))
                    {
                        d.needsCheckUp = true; // drone needs a check-up
                    }
                }
                db.SaveChanges();
            }
           
        }

        // Runs through a file once to count its total amount of lines
        public static int CountFileLines(string path)
        {
            int totalLines = 0;
            using (StreamReader r = new StreamReader(path))
            {
                while (r.ReadLine() != null)
                {
                    totalLines++;
                }
            }
            return totalLines;
        }

        // Set the parsing progress so that it can be communicated to the front-end for the progress bar
        public static void SetProgress(double newProgress)
        {
            progress = newProgress;
        }
    }
}