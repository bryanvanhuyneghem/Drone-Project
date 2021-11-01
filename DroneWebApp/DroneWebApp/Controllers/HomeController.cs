using DroneWebApp.Models;
using DroneWebApp.Models.SimpleFactoryPattern;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DroneWebApp.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View("Index");
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View("About");
        }

        [AllowAnonymous]
        public void Manual()
        {
            /*
            // Create New instance of FileInfo class to get the properties of the file being downloaded
            FileInfo file = new FileInfo("/Content/Manual/Gebruikershandleiding.pdf");

            // Checking if file exists
            if (file.Exists)
            {
                // Clear the content of the response
                Response.ClearContent();

                // Set the ContentType
                Response.ContentType = "application/pdf";

                // LINE1: Add the file name and attachment, which will force the open/cance/save dialog to show, to the header
                Response.AddHeader("Content-Disposition", "attachment;filename=" + file.Name);

                // Write the file into the response (TransmitFile is for ASP.NET 2.0. In ASP.NET 1.1 you have to use WriteFile instead)
                Response.Write(file);

                // End the response
                Response.End();
            }
            */
            Response.ClearContent();
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=Manual.pdf");
            Response.TransmitFile(Server.MapPath("~/Content/Manual/Gebruikershandleiding.pdf"));
            Response.End();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Please contact us:";

            return View("Contact");
        }
    }
}