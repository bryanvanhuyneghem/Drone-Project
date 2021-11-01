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
    public class GCPController : ApiController
    {
        private DroneDBEntities db = new DroneDBEntities();

        // GET: WebAPI/api/GCP/5
        public HttpResponseMessage GetGroundControlPointsByFlightID(int id)
        {
            var Flight = db.DroneFlights.Find(id);   // Find the right flight 
            if (Flight == null || !Flight.hasGCPs)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            var GroundControlPoints = Flight.GroundControlPoints.Select(
                gcp => new { gcp.GCPId, gcp.GCPName, gcp.X, gcp.Y, gcp.Z, gcp.FlightId }).ToList();

            //config to set to json 
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(GroundControlPoints));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return response;
        }  
    }
}