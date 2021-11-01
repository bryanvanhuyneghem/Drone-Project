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
    public class PointCloudXYZsController : ApiController
    {
        private DroneDBEntities db = new DroneDBEntities();

        // GET: WebAPI/api/PointCloudXYZs/5
        // Get pointcloud by flight id 
        public HttpResponseMessage GetPointCloudXYZByFlightID(int id)
        {
            var Flight = db.DroneFlights.Find(id);
            if (Flight == null || !Flight.hasXYZ) return new HttpResponseMessage(HttpStatusCode.NotFound);

            //data projection
            var PointCloudXYZ = Flight.PointCloudXYZs.Select(p => new { p.PointCloudXYZId,  p.X, p.Y, p.Z, p.Red, p.Green, p.Blue, p.Intensity, p.FlightId}).ToList();

            //config to set to JSON 
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(PointCloudXYZ));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return response;
        }    
    }
}