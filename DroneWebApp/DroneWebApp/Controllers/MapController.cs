using DroneWebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DroneWebApp.Controllers
{
    [AllowAnonymous]
    public class MapController : Controller
    {
        private DroneDBEntities db;

        //Constructor
        public MapController(DbContext db)
        {
            this.db = (DroneDBEntities)db;
        }

        [AllowAnonymous]
        public ActionResult ViewMap(int? id)
        {
            if (id == null) // general overview of all Flights
            {
                return View("ViewMap");
            }
            else // specific Flight
            {
                DroneFlight droneFlight = db.DroneFlights.Find(id);
                if (droneFlight == null)
                {
                    ViewBag.ErrorMessage = "Drone Flight could not be found.";
                    return View("~/Views/ErrorPage/Error.cshtml");
                }
                ViewBag.id = id; 
                // The user can only navigate to the Map of a specific Flight, if that Flight has a Drone Log uploaded to it
                if( droneFlight.hasDroneLog == false)
                {
                    ViewBag.ErrorMessage = "Please upload a Drone Log for this Flight first.";
                    return View("~/Views/ErrorPage/Error.cshtml");
                }
            }
            return View("ViewMap"); 
        }
    }
}