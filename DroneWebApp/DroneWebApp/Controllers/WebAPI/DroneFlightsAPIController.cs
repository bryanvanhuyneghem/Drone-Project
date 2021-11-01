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
	public class DroneFlightsAPIController : ApiController
	{
		private DroneDBEntities db = new DroneDBEntities();

		// Get one single drone flight by id
		// GET: WebAPI/api/DroneFlightsAPI/5
		public HttpResponseMessage GetDroneFlight(int id)
		{
			var Flight = db.DroneFlights.Find(id); // Find the right flight

			if (Flight == null || Flight.DepartureInfo == null || Flight.DestinationInfo == null) return new HttpResponseMessage(HttpStatusCode.NotFound);

			//data projection
			var flightProjected = (new
			{
				Flight.FlightId,

				Flight.Pilot.PilotName,
				Flight.Drone.DroneName,

				DepartureUTC = Flight.DepartureInfo.UTCTime,
				DepartureLatitude = Flight.DepartureInfo.Latitude,
				DepartureLongitude = Flight.DepartureInfo.Longitude,

				DestinationUTC = Flight.DestinationInfo.UTCTime,
				DestinationLatitude = Flight.DestinationInfo.Latitude,
				DestinationLongitude = Flight.DestinationInfo.Longitude

			});

			//config to set to JSON 
			var response = new HttpResponseMessage(HttpStatusCode.OK);
			response.Content = new StringContent(JsonConvert.SerializeObject(flightProjected));
			response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			return response;
		}

		// Get all drone flights
		public HttpResponseMessage getAllDroneFlights()
		{

			var droneFlightsProjected = db.DroneFlights.Select(
				df => new {

					df.FlightId,

					df.Pilot.PilotName,
					df.Drone.DroneName,

					DepartureUTC = df.DepartureInfo.UTCTime,
					DepartureLatitude = df.DepartureInfo.Latitude,
					DepartureLongitude = df.DepartureInfo.Longitude,

					DestinationUTC = df.DestinationInfo.UTCTime,
					DestinationLatitude = df.DestinationInfo.Latitude,
					DestinationLongitude = df.DestinationInfo.Longitude
				}).ToList();

			//config to set to json 
			var response = new HttpResponseMessage(HttpStatusCode.OK);
			response.Content = new StringContent(JsonConvert.SerializeObject(droneFlightsProjected));
			response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			return response;
		}

	}
}