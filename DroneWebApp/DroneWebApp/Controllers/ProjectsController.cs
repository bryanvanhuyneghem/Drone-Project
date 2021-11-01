using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DroneWebApp.Models;
using DroneWebApp.Models.Helper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DroneWebApp.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private DroneDBEntities db;
        private ApplicationDbContext applicationDb = new ApplicationDbContext();

        // Constructor
        public ProjectsController(DbContext db)
        {
            this.db = (DroneDBEntities)db;
        }

        // GET: Projects
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View("Index", db.Projects.ToList());
        }

        // GET: Projects/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Project in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                ViewBag.ErrorMessage = "Project could not be found.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            return View("Details", project);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View("Create");
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "ProjectId,ProjectCode,SiteRefCode,VerticalRef,CoordSystem")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Project in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                ViewBag.ErrorMessage = "Project could not be found.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            return View("Edit", project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "ProjectId,ProjectCode,SiteRefCode,VerticalRef,CoordSystem")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Project in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            // Find the project
            Project project = db.Projects.Find(id);
            // Count its flights
            ViewBag.TotalFlights = project.DroneFlights.Count;
            if (project == null)
            {
                ViewBag.ErrorMessage = "Project could not be found.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            return View("Delete", project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            // Find the project
            Project project = db.Projects.Find(id);
            // Count its flights
            ViewBag.TotalFlights = project.DroneFlights.Count;
            try
            {
                db.Projects.Remove(project);
                db.SaveChanges();
            }
            catch (Exception)
            {
                ViewBag.ErrorProjectstDelete = "Cannot delete this Project.";
                return View(project);
            }
            // Update the total time drones have flown in case the drone flight's drone has been changed by the user
            Helper.UpdateTotalDroneFlightTime(this.db);
            return RedirectToAction("Index");
        }

        // GET: Project/DroneFlights/5
        [AllowAnonymous]
        public ActionResult DroneFlights(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Project in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                ViewBag.ErrorMessage = "Project could not be found.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            ViewBag.Project = project.ProjectCode;
            ViewBag.ProjectId = id;
            return View("DroneFlights", project.DroneFlights.ToList());
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
