using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Description;
using DroneWebApp.Models;
using Newtonsoft.Json;

namespace DroneWebApp.Controllers
{
    public class DroneGPsController : ApiController
    {
        private DroneDBEntities db = new DroneDBEntities();
  
        // GET: WebAPI/api/DroneGPs/5
        public HttpResponseMessage GetDroneGP(int id)
        {
            var Flight = db.DroneFlights.Find(id);
            if (Flight == null || !Flight.hasDroneLog) return new HttpResponseMessage(HttpStatusCode.NotFound);

            var droneLogEntries = Flight.DroneLogEntries.ToList();
            List<DroneGP> droneGPs = new List<DroneGP>();

            foreach (DroneLogEntry log in droneLogEntries)
            {
                droneGPs.Add(log.DroneGP);
            }

            //data projection
            var GPs = droneGPs.Select(gp => new { 
                gp.GPSId, 
                gp.Long, 
                gp.Lat, 
                gp.Date, 
                gp.Time, 
                gp.DateTimeStamp, 
                gp.HeightMSL, 
                gp.HDOP, 
                gp.PDOP, 
                gp.SAcc, 
                gp.NumGPS, 
                gp.NumGLNAS, 
                gp.NumSV, 
                gp.VelN, 
                gp.VelE, 
                gp.VelD,

                gp.DroneLogEntry.BatteryPercentage, 
                gp.DroneLogEntry.DroneIMU_ATTI.VelComposite

            }).ToList();

            //config to set to JSON 
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(GPs));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return response;
        }     
    }
}