using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DroneWebApp.Models;
using DroneWebApp.Models.Helper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DroneWebApp.Controllers
{
    [Authorize]
    public class DroneFlightsController : Controller
    {
        private DroneDBEntities db;
        private ApplicationDbContext applicationDb = new ApplicationDbContext();

        //Constructor
        public DroneFlightsController(DbContext db)
        {
            this.db = (DroneDBEntities) db;
        }

        // GET: DroneFlights
        [AllowAnonymous]
        public ActionResult Index()
        {
            var droneFlights = db.DroneFlights;
            return View("Index", droneFlights.ToList());
        }

        // GET: DroneFlights/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Drone Flight in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            DroneFlight droneFlight = db.DroneFlights.Find(id);
            if (droneFlight == null)
            {
                ViewBag.ErrorMessage = "Drone Flight could not be found.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            return View("Details", droneFlight);
        }

        // GET: DroneFlights/Create
        [Authorize(Roles = "Admin,User")]
        public ActionResult Create(int? pilotId, int? projectId, int? droneId)
        {
            if (pilotId != null)
            {
                ViewBag.PilotId = new SelectList(db.Pilots, "PilotId", "PilotName", pilotId);
            }
            else
            {
                ViewBag.PilotId = new SelectList(db.Pilots, "PilotId", "PilotName");
            }
            if (droneId != null)
            {
                ViewBag.DroneId = new SelectList(db.Drones, "DroneId", "DroneName", droneId);
            }
            else
            {
                ViewBag.DroneId = new SelectList(db.Drones, "DroneId", "DroneName");
            }
            if (projectId != null)
            {
                ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectCode", projectId);
            }
            else
            {
                ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectCode");
            }
            ViewBag.DroneId = new SelectList(db.Drones, "DroneId", "DroneName");
            return View("Create");
        }

        // POST: DroneFlights/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,User")]
        public ActionResult Create([Bind(Include = "FlightId, DroneId, PilotId, ProjectId, Location, Date, TypeOfActivity, Other, Simulator, Instructor, Remarks, hasTFW, hasGCPs, hasCTRLs, hasDepInfo, hasDestInfo, hasQR, hasXYZ, hasDroneLog")] DroneFlight droneFlight)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(droneFlight.Location))
                {
                    droneFlight.Location = "TBD"; // TBD = to be determined; indicates no location was set during creation of flight
                }
                db.DroneFlights.Add(droneFlight);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectCode", droneFlight.ProjectId);
            ViewBag.DroneId = new SelectList(db.Drones, "DroneId", "DroneName", droneFlight.DroneId);
            ViewBag.PilotId = new SelectList(db.Pilots, "PilotId", "PilotName", droneFlight.PilotId);
            return View(droneFlight);
        }

        // GET: DroneFlights/Edit/5
        [Authorize(Roles = "Admin,User")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Drone Flight in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            DroneFlight droneFlight = db.DroneFlights.Find(id);
            if (droneFlight == null)
            {
                ViewBag.ErrorMessage = "Drone Flight could not be found.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectCode", droneFlight.ProjectId);
            ViewBag.DroneId = new SelectList(db.Drones, "DroneId", "DroneName", droneFlight.DroneId);
            ViewBag.PilotId = new SelectList(db.Pilots, "PilotId", "PilotName", droneFlight.PilotId);
            return View("Edit", droneFlight);
        }

        // POST: DroneFlights/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,User")]
        public ActionResult Edit([Bind(Include = "FlightId, DroneId, PilotId, ProjectId, Location, Date, TypeOfActivity, Other, Simulator, Instructor, Remarks")] DroneFlight droneFlight)
        {
            if (ModelState.IsValid)
            {
                DroneFlight df = db.DroneFlights.Find(droneFlight.FlightId);
                UpdateFlightFields(droneFlight, df); // Update fields
                db.Entry(df).State = EntityState.Modified;
                db.SaveChanges();
                // Update the total time drones have flown in case the drone flight's drone has been changed by the user
                Helper.UpdateTotalDroneFlightTime(this.db);
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectCode", droneFlight.ProjectId);
            ViewBag.DroneId = new SelectList(db.Drones, "DroneId", "DroneName", droneFlight.DroneId);
            ViewBag.PilotId = new SelectList(db.Pilots, "PilotId", "PilotName", droneFlight.PilotId);
            return View(droneFlight);
        }

        // Update the fields of the DroneFlight that has been found by FlightId with the fields of the posted DroneFlight, a.k.a. the drone flight 
        // the user has submitted
        private static void UpdateFlightFields(DroneFlight postedDroneFlight, DroneFlight df)
        {
            df.DroneId = postedDroneFlight.DroneId;
            df.PilotId = postedDroneFlight.PilotId;
            df.ProjectId = postedDroneFlight.ProjectId;
            df.Location = postedDroneFlight.Location;
            // keep the old time portion of the date; user cannot update time, only date
            if(df.Date != null)
            {
                df.Date = new DateTime(((DateTime)postedDroneFlight.Date).Year, ((DateTime)postedDroneFlight.Date).Month, ((DateTime)postedDroneFlight.Date).Day, ((DateTime)df.Date).Hour, ((DateTime)df.Date).Minute, ((DateTime)df.Date).Second);
            }
            else
            {
                df.Date = postedDroneFlight.Date;
            }
            df.TypeOfActivity = postedDroneFlight.TypeOfActivity;
            df.Other = postedDroneFlight.Other;
            df.Simulator = postedDroneFlight.Simulator;
            df.Instructor = postedDroneFlight.Instructor;
            df.Remarks = postedDroneFlight.Remarks;
        }

        // GET: DroneFlights/Delete/5
        [Authorize(Roles = "Admin,User")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Drone Flight in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            DroneFlight droneFlight = db.DroneFlights.Find(id);
            if (droneFlight == null)
            {
                ViewBag.ErrorMessage = "Drone Flight could not be found.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            return View("Delete", droneFlight);
        }

        // POST: DroneFlights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,User")]
        public ActionResult DeleteConfirmed(int? id)
        {
            DroneFlight droneFlight = db.DroneFlights.Find(id);

            // Calculate the flight time of this drone flight
            TimeSpan totalTime = new TimeSpan(0, 0, 0, 0);
            if (droneFlight.hasDepInfo && droneFlight.hasDestInfo)
            {
                totalTime = totalTime.Add(((TimeSpan)droneFlight.DestinationInfo.UTCTime).Subtract((TimeSpan)droneFlight.DepartureInfo.UTCTime));
                // Update the threshold time for the drone that was assigned to this flight to account for this deletion
                droneFlight.Drone.nextTimeCheck = droneFlight.Drone.nextTimeCheck - (long)totalTime.TotalSeconds;
                droneFlight.Drone.needsCheckUp = false; // Reset to false; Helper.UpdateTotalDroneFlightTime will re-evaluate whether or not this needs to stay false
            }
            // Remove this drone flight
            try
            {
                db.DroneFlights.Remove(droneFlight);
                db.SaveChanges();
            }
            catch (Exception)
            {
                ViewBag.ErrorDroneFlightDelete = "Cannot delete this Drone Flight.";
                return View(droneFlight);
            }

            // Update the total time drones have flown in case the drone flight's drone has been changed by the user
            Helper.UpdateTotalDroneFlightTime(this.db);
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult QualityReport(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Drone Flight in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            // Get the appropriate drone flight per id
            DroneFlight droneFlight = db.DroneFlights.Find(id);
            // Get the drone flight's Quality Report
            QualityReport qualityReport = droneFlight.QualityReport;
            // Pass to DroneFlight object and its Id to View for use
            ViewBag.droneFlight = droneFlight;
            ViewBag.DroneFlightId = id;
            if (droneFlight == null)
            {
                ViewBag.ErrorMessage = "Drone Flight could not be found.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            return View("QualityReport", qualityReport);
        }

        [AllowAnonymous]
        public ActionResult CTRLPoints(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Drone Flight in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            // Get the appropriate drone flight per id
            DroneFlight droneFlight = db.DroneFlights.Find(id);
            // Pass to DroneFlight object and its Id to View for use
            ViewBag.droneFlight = droneFlight;
            ViewBag.DroneFlightId = id;
            if (droneFlight == null)
            {
                ViewBag.ErrorMessage = "Drone Flight could not be found.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            return View("CTRLPoints", droneFlight.CTRLPoints.ToList());
        }

        [AllowAnonymous]
        public ActionResult GCPPoints(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Drone Flight in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            // Get the appropriate drone flight per id
            DroneFlight droneFlight = db.DroneFlights.Find(id);
            // Pass to DroneFlight object and its Id to View for use
            ViewBag.droneFlight = droneFlight;
            ViewBag.DroneFlightId = id;
            if (droneFlight == null)
            {
                ViewBag.ErrorMessage = "Drone Flight could not be found.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            return View("GCPPoints", droneFlight.GroundControlPoints.ToList());
        }

        [AllowAnonymous]
        public ActionResult TFW(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Drone Flight in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            // Get the appropriate drone flight per id
            DroneFlight droneFlight = db.DroneFlights.Find(id);
            // Pass to DroneFlight object and its Id to View for use
            ViewBag.droneFlight = droneFlight;
            ViewBag.DroneFlightId = id;
            if (droneFlight == null)
            {
                ViewBag.ErrorMessage = "Drone Flight could not be found.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            return View("TFW", droneFlight.TFW);
        }

        [AllowAnonymous]
        public ActionResult RawImages(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Drone Flight in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Get the appropriate drone flight per id
            DroneFlight droneFlight = db.DroneFlights.Find(id);
            // Pass to DroneFlight object and its Id to View for use
            ViewBag.droneFlight = droneFlight;
            ViewBag.DroneFlightId = id;
            if (droneFlight == null)
            {
                ViewBag.ErrorMessage = "Drone Flight could not be found.";
                return View("~/Views/ErrorPage/Error.cshtml");
                //return HttpNotFound();
            }
            return View("RawImages", droneFlight.RawImages);
        }

        [AllowAnonymous]
        public ActionResult Map(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Drone Flight in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            // Get the appropriate drone flight per id
            DroneFlight droneFlight = db.DroneFlights.Find(id);
            // Pass to DroneFlight object and its Id to View for use
            ViewBag.droneFlight = droneFlight;
            ViewBag.DroneFlightId = id;
            if (droneFlight == null)
            {
                ViewBag.ErrorMessage = "Drone Flight could not be found.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            return View(droneFlight);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
