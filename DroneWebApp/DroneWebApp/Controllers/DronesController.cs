using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using DroneWebApp.Models;
using DroneWebApp.Models.Helper;

namespace DroneWebApp.Controllers
{
    [Authorize]
    public class DronesController : Controller
    {
        private DroneDBEntities db;
        private ApplicationDbContext applicationDb = new ApplicationDbContext();
        // thresholdTime is the amount of time after which a drone has to be checked to verify it is safe to fly, from its last check-up (or from 0 flight time)   
        private static TimeSpan thresholdTime = new TimeSpan(2, 2, 0, 0); // Here set to 50 hours. (2*24h + 2h = 50h)

        // Constructor
        public DronesController(DbContext db)
        {
            this.db = (DroneDBEntities) db;
        }

        // GET: Drones
        [Authorize(Roles = "Admin,User")]
        public ActionResult Index()
        {
            return View("Index", db.Drones.ToList());
        }

        // GET: Drones/Details/5
        [Authorize(Roles = "Admin,User")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Drone in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            Drone drone = db.Drones.Find(id);
            if (drone == null)
            {
                ViewBag.ErrorMessage = "Pilot could not be found.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            return View("Details", drone);
        }

        // GET: Drones/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View("Create");
        }

        // POST: Drones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "DroneId,Registration,DroneType, DroneName, Notes")] Drone drone)
        {
            if (ModelState.IsValid)
            {
                if (drone.DroneName == null)
                {
                    drone.DroneName = drone.DroneType + ":" + drone.Registration;
                }
                drone.TotalFlightTime = 0; // no time flown yet
                drone.needsCheckUp = false; // does not yet need a check-up

                drone.nextTimeCheck = (long)thresholdTime.TotalSeconds; // Save the next time check as total amount of ticks (bigint in database, long in application).
                                                                        // This is because SQL Server cannot store times > 24 hours; solution: store time as seconds
                db.Drones.Add(drone);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(drone);
        }

        // GET: Drones/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Drone in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            Drone drone = db.Drones.Find(id);
            if (drone == null)
            {
                ViewBag.ErrorMessage = "Drone could not be found.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            return View("Edit", drone);
        }

        // POST: Drones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "DroneId,Registration,DroneType,DroneName,needsCheckUp, Notes")] Drone drone)
        {
            if (ModelState.IsValid)
            {
                Drone dr = db.Drones.Find(drone.DroneId);
                UpdateDroneFields(drone, dr); // Update the fields
                db.Entry(dr).State = EntityState.Modified;
                db.SaveChanges();
                // Update the total time drones have flown in case the drone flight's drone has been changed by the user
                Helper.UpdateTotalDroneFlightTime(this.db);
                return RedirectToAction("Index");
            }
            return View(drone);
        }

        // Update the fields of the Drone that has been found by DroneId with the fields of the posted Drone, a.k.a. the drone 
        // the user has submitted
        private static void UpdateDroneFields(Drone postedDrone, Drone dr)
        {
            dr.Registration = postedDrone.Registration;
            dr.DroneType = postedDrone.DroneType;
            dr.DroneName = postedDrone.DroneName;
            if(string.IsNullOrWhiteSpace(postedDrone.DroneName))
            {
                dr.DroneName = postedDrone.DroneType + ":" + postedDrone.Registration;
            }
            else
            {
                dr.DroneName = postedDrone.DroneName;
            }
            // User has manually indicated that a drone needs a check-up, before the threshold has been met
            if(postedDrone.needsCheckUp == true && dr.needsCheckUp == false)
            {
                dr.needsCheckUp = postedDrone.needsCheckUp; // drone needs a check-up
            }
            // User has manually indicated that a drone that needed checking, has been checked
            if(postedDrone.needsCheckUp == false && dr.needsCheckUp == true)
            {
                dr.needsCheckUp = postedDrone.needsCheckUp;
                dr.nextTimeCheck = (long)dr.TotalFlightTime + (long)thresholdTime.TotalSeconds; // calculate the new next time check
            }
            // User edited the drone but did not apply any changes, because both are still false
            else if (postedDrone.needsCheckUp == false && dr.needsCheckUp == false)
            {
                dr.needsCheckUp = postedDrone.needsCheckUp;
            }
            // User edited the drone but did not apply any changes, because both are still true
            else if (postedDrone.needsCheckUp == false && dr.needsCheckUp == false)
            {
                dr.needsCheckUp = postedDrone.needsCheckUp;
            }
            dr.Notes = postedDrone.Notes;
        }

        // GET: Drones/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Drone in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            Drone drone = db.Drones.Find(id);

            if (drone == null)
            {
                ViewBag.ErrorMessage = "Drone could not be found.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            // Count its total flights
            ViewBag.TotalFlights = drone.DroneFlights.Count;
            return View("Delete", drone);
        }

        // POST: Drones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int? id)
        {
            Drone drone = db.Drones.Find(id);
            try
            {
                db.Drones.Remove(drone);
                db.SaveChanges();
            }
            catch (Exception)
            {
                ViewBag.ErrorDroneDelete = "Cannot delete this Drone. " + drone.DroneName + " is assigned to one or more Flight.";
                return View(drone);
            }
            // Count its total flights
            ViewBag.TotalFlights = drone.DroneFlights.Count;
            return RedirectToAction("Index");
        }

        // GET: Drones/DroneFlights/5
        [Authorize(Roles = "Admin,User")]
        public ActionResult DroneFlights(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Drone in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            Drone drone = db.Drones.Find(id);
            if (drone == null)
            {
                ViewBag.ErrorMessage = "Drone could not be found.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            ViewBag.DroneName = drone.DroneName;
            ViewBag.DroneId = id;
            return View("DroneFlights", drone.DroneFlights.ToList());
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
