using DroneWebApp.Models;
using DroneWebApp.Models.DataExport;
using DroneWebApp.Models.Helper;
using DroneWebApp.Models.SimpleFactoryPattern;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;


namespace DroneWebApp.Controllers
{
    [Authorize]
    public class FilesController : Controller
    {
        public DroneDBEntities Db { get; set; }
        private ApplicationDbContext applicationDb = new ApplicationDbContext();
        private Creator creator;
        private readonly List<string> validExtensions = new List<string>(){ ".pdf", ".dat", ".txt", ".csv", ".xyz", ".tfw", ".jpg"};
        // Parsing variables
        private static Dictionary<string, bool> results;
        private static string currentFileName; // the current file that is being processed
        private static bool currentParseResult; // the current parse result
        private static int totalFilesToParse = 0; // the total amount of files that must be parsed
        private static int filesLeft = 0; // the amount of files that still have to be parsed

        // Constructor
        public FilesController(DbContext db)
        {
            Db = (DroneDBEntities)db;
            creator = new Creator(Db);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public ActionResult Index(int? id)
        {
            DroneFlight droneFlight = Db.DroneFlights.Find(id);
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify a Drone Flight in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            else if (droneFlight == null)
            {
                ViewBag.ErrorMessage = "This Drone Flight does not exist.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            ViewBag.FlightId = (int)id;
            ViewBag.Location = droneFlight.Location;
            string date = "TBD";
            if (droneFlight.Date != null)
            {
                date = ((DateTime)droneFlight.Date).ToString("dd/MM/yyyy, HH:mm:ss");
            }
            ViewBag.Date = date;
            return View("Index");
        }

        // Return values for error-handling: 
        // 1: success
        // 0: no files submitted
        // 2: no drone flight specified
        // 3: drone flight does not exist
        // 4: someone else is already uploading
        // 5: invalid file type
        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public int Index(int? id, List<HttpPostedFileBase> files)
        {
            Helper.progress = 0; //Set file upload progression to zero
            Thread.Sleep(2000); // wait for client-side to sync
            // Prevent users from parsing files at the same time (solution may be Websockets)
            if (filesLeft > 0)
            {
                return 4;
            }
            // How many files must be parsed?
            totalFilesToParse = files.Count;
            filesLeft = totalFilesToParse;
            
            DroneFlight droneFlight = Db.DroneFlights.Find(id);
            // Check if an id was submitted
            if (id == null)
            {
                return 2;
            }
            // Check whether a drone flight with this id exists  
            else if (droneFlight == null)
            {
                return 3;
            }
            // Check whether files were submitted
            else if (files.Count == 0)
            {
                return 0;
            }
            
            // Keep track of which files were successfully read
            results = new Dictionary<string, bool>();

            // Parse all submitted files
            foreach (HttpPostedFileBase file in files)
            {
                 // Verify that the file provided exists
                if (file != null)
                {
                    currentFileName = "";
                    // Verify that the user selected a file
                    var path = "";
                    if (file != null && file.ContentLength > 0)
                    {
                        // extract only the filename
                        currentFileName = Path.GetFileName(file.FileName); // set the current file name
                                                                           // add this file name to the list of files  
                        // store the file inside ~/files/ folder
                        path = Path.Combine(Server.MapPath("~/files"), currentFileName);
                        file.SaveAs(path);
                    }

                    string fileExtension = currentFileName.Substring(currentFileName.Length - 4);
                    // Check that the user's file is an appropriate file type
                    if (!validExtensions.Contains(fileExtension.ToLower()))
                    {
                        return 5;
                    }
                    else
                    {
                        // Check if uploaded file is a drone log file
                        if(path.Contains("FLY"))
                        {
                            currentParseResult = creator.GetParser(".dat", path, (int)id);
                            results.Add(currentFileName, currentParseResult);
                        }
                        else
                        {
                            // Parse the submitted file
                            currentParseResult = creator.GetParser(fileExtension, path, (int)id);
                            results.Add(currentFileName, currentParseResult);
                        }
                    }
                    // Wait a bit so the ajax call can correctly happen in case of uploading 1 file that is already present (parser returns false very quickly)
                    if(filesLeft == 1)
                    {
                        Thread.Sleep(1000);
                    }
                    filesLeft--;
                }
                
            } // end of foreach loop
            return 1; // success
        }

        // Export pilot or drone data to a log file
        [Authorize(Roles = "Admin,User")]
        public ActionResult Export(int? id, string extension, string type)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Please specify an id in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }
            if (extension == null || type == null)
            {
                ViewBag.ErrorMessage = "Please specify an extension and/or type in your URL.";
                return View("~/Views/ErrorPage/Error.cshtml");
            }

            IExport export = null;

            if (extension.Equals("csv"))
            {
                export = new ExportCSV();
                if (type.Equals("drone"))
                {
                    export.CreateDroneLog((int)id, Db, HttpContext);
                }
                else if (type.Equals("pilot"))
                {
                    export.CreatePilotLog((int)id, Db, HttpContext);
                }
            }
            else if (extension.Equals("pdf"))
            {
                export = new ExportPDF();
                if (type.Equals("drone"))
                {
                    export.CreateDroneLog((int)id, Db, HttpContext);
                }
                else if (type.Equals("pilot"))
                {
                    export.CreatePilotLog((int)id, Db, HttpContext);
                }
            }

            return View("Export");
        }

        // Return the Status of the parsing to the Client
        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public ActionResult GetStatus()
        {
            // Build the list of failed files (where the parser returned false) to pass to Client
            List<string> failed = new List<string>();
            if (filesLeft == 0 && (results != null))
            {
                Thread.Sleep(1000);
                foreach (KeyValuePair<string, bool> entry in results)
                {
                    if (entry.Value == false)
                    {
                        failed.Add(entry.Key);
                    }
                }
            }

            // Convert parseResult to an int to pass to Client
            int parseResultToInt;
            if (currentParseResult)
            {
                parseResultToInt = 1;
            }
            else
            {
                parseResultToInt = 0;
            }

            // Create struct
            var result = (new
            {
                currProgress = Helper.progress,
                currParseResult = parseResultToInt,
                currFileName = currentFileName,
                currFilesLeft = filesLeft,
                failedFiles = failed,
            });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // Help page for uploading files
        public ActionResult Help(int? id)
        {
            ViewBag.FlightId = (int)id;
            return View("Help");
        }
    }
}
