using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneWebApp.Models.DataExport
{
    public abstract class AExport : IExport
    {
        public abstract void CreateDroneLog(int DroneId, DroneDBEntities db, HttpContextBase context);

        public abstract void CreatePilotLog(int PilotId, DroneDBEntities db, HttpContextBase context);

        // get the flight time of a drone flight
        protected TimeSpan? GetFlightTime(DroneFlight flight)
        {
            TimeSpan totalTime = new TimeSpan(0, 0, 0);
            if (flight != null && flight.hasDroneLog)
            {
                totalTime = totalTime.Add(((TimeSpan)flight.DestinationInfo.UTCTime).Subtract((TimeSpan)flight.DepartureInfo.UTCTime));
            }
            return totalTime;
        }

        // replace invalid characters or characters that create problems in the filename (':', ' ', ...)
        protected string ReplaceInvalidChars(string filename)
        {
            string result = string.Join("_", filename.Split(System.IO.Path.GetInvalidFileNameChars()));
            result = string.Join("", result.Split(' '));

            return result;
        }
    }
}