using Microsoft.VisualStudio.TestTools.UnitTesting;
using DroneWebApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DroneWebApp.Controllers.Tests
{
    [TestClass()]
    public class HomeControllerTests
    {
        [TestMethod()]
        public void IndexTest_ActionExecutes_ReturnsViewForIndex()
        {
            HomeController controller = new HomeController();

            var result = controller.Index() as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod()]
        public void AboutTest_ActionExecutes_ReturnsViewForAbout()
        {
            HomeController controller = new HomeController();

            var result = controller.About() as ViewResult;
            Assert.AreEqual("About", result.ViewName);
        }

        [TestMethod()]
        public void ContactTest_ActionExecutes_ReturnsViewForContact()
        {
            HomeController controller = new HomeController();

            var result = controller.Contact() as ViewResult;
            Assert.AreEqual("Contact", result.ViewName);
        }
    }
}