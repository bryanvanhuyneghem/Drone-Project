using Microsoft.VisualStudio.TestTools.UnitTesting;
using DroneWebApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using DroneWebApp.Models;
using System.Web.Mvc;
using System.Data.Entity;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq.Expressions;
using System.Web.Routing;
using DroneWebApp.Models.Helper;
using Newtonsoft.Json;

namespace DroneWebApp.Controllers.Tests
{
    [TestClass()]
    public class FilesControllerTests
    {
        [TestMethod()]
        public void IndexTest1_ActionExecutes_ReturnsViewForIndex()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            FilesController controller = new FilesController(mockContext.Object);

            // Create a mock DbSet
            List<DroneFlight> flights = GetFlights();
            var mockSet = CreateMockSet(flights);
            // Set up the DroneFlights property so it returns the mocked DbSet
            mockContext.Setup(o => o.DroneFlights).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockContext.Setup(c => c.DroneFlights.Find(It.IsAny<object[]>())).Returns((object[] input) => flights.SingleOrDefault(x => x.FlightId == (int)input.First()));

            var result = controller.Index(3) as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod()]
        public void IndexTest2_Returns1ForSucces()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            FilesController controller = new FilesController(mockContext.Object);

            string path = AppDomain.CurrentDomain.BaseDirectory + @"\testfiles";

            var httpContextMock = new Mock<HttpContextBase>();
            var serverMock = new Mock<HttpServerUtilityBase>();
            serverMock.Setup(x => x.MapPath("~/files")).Returns(path);
            httpContextMock.Setup(x => x.Server).Returns(serverMock.Object);
            //httpContextMock.Setup(x => x.User.Identity.Name).Returns("testcase@user.net");
            controller.ControllerContext = new ControllerContext(httpContextMock.Object, new RouteData(), controller);

            string filePath = path + @"\harelbeke_drone_191210.csv";
            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            int contentLength = (int)fileStream.Length;
            fileStream.Close();
            Mock<HttpPostedFileBase> uploadedFile = new Mock<HttpPostedFileBase>();
            uploadedFile.Setup(x => x.FileName).Returns("harelbeke_drone_191210.csv");
            uploadedFile.Setup(x => x.ContentType).Returns("text/csv");
            uploadedFile.Setup(x => x.ContentLength).Returns(contentLength);
            //uploadedFile.Setup(x => x.InputStream).Returns(fileStream);

            List<HttpPostedFileBase> postedFiles = new List<HttpPostedFileBase>();
            postedFiles.Add(uploadedFile.Object);

            // Create a mock DbSet
            List<DroneFlight> flights = GetFlights();
            var mockSet = CreateMockSet(flights);
            // Set up the DroneFlights property so it returns the mocked DbSet
            mockContext.Setup(o => o.DroneFlights).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockContext.Setup(c => c.DroneFlights.Find(It.IsAny<object[]>())).Returns((object[] input) => flights.SingleOrDefault(x => x.FlightId == (int)input.First()));            

            int result = controller.Index(3, postedFiles);

            Assert.AreEqual(1, result);
        }

        [TestMethod()]
        public void ExportTest()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            FilesController controller = new FilesController(mockContext.Object);

            // Create a mock DbSet
            List<DroneFlight> flights = GetFlights();
            List<Drone> drones = GetDrones();
            var mockSetFlights = CreateMockSet(flights);
            var mockSetDrones = CreateMockSet(drones);
            // Set up the DroneFlights property so it returns the mocked DbSet
            mockContext.Setup(o => o.DroneFlights).Returns(() => mockSetFlights.Object);
            // Set up the Drones property so it returns the mocked DbSet
            mockContext.Setup(o => o.Drones).Returns(() => mockSetDrones.Object);
            // Set up the Find method for the mocked DbSet
            mockSetFlights.Setup(s => s.Find(It.IsAny<object[]>())).Returns((object[] input) => flights.SingleOrDefault(x => x.FlightId == (int)input.First()));
            // Set up the Find method for the mocked DbSet
            mockSetDrones.Setup(s => s.Find(It.IsAny<object[]>())).Returns((object[] input) => drones.SingleOrDefault(x => x.DroneId == (int)input.First()));

            var httpContext = new Mock<HttpContextBase>();
            var httpResponse = new Mock<HttpResponseBase>();
            httpContext.Setup(c => c.Response).Returns(httpResponse.Object);

            controller.ControllerContext = new ControllerContext(httpContext.Object, new RouteData(), controller);

            var result = controller.Export(1, "csv", "drone") as ViewResult;

            Assert.AreEqual("Export", result.ViewName);
        }

        [TestMethod()]
        public void GetStatusTest()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            FilesController controller = new FilesController(mockContext.Object);

            string path = AppDomain.CurrentDomain.BaseDirectory + @"\testfiles";

            var httpContextMock = new Mock<HttpContextBase>();
            var serverMock = new Mock<HttpServerUtilityBase>();
            serverMock.Setup(x => x.MapPath("~/files")).Returns(path);
            httpContextMock.Setup(x => x.Server).Returns(serverMock.Object);
            //httpContextMock.Setup(x => x.User.Identity.Name).Returns("testcase@user.net");
            controller.ControllerContext = new ControllerContext(httpContextMock.Object, new RouteData(), controller);

            string filePath = path + @"\harelbeke_drone_191210.csv";
            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            int contentLength = (int)fileStream.Length;
            fileStream.Close();
            Mock<HttpPostedFileBase> uploadedFile = new Mock<HttpPostedFileBase>();
            uploadedFile.Setup(x => x.FileName).Returns("harelbeke_drone_191210.csv");
            uploadedFile.Setup(x => x.ContentType).Returns("text/csv");
            uploadedFile.Setup(x => x.ContentLength).Returns(contentLength);
            //uploadedFile.Setup(x => x.InputStream).Returns(fileStream);

            List<HttpPostedFileBase> postedFiles = new List<HttpPostedFileBase>();
            postedFiles.Add(uploadedFile.Object);

            // Create a mock DbSet
            List<DroneFlight> flights = GetFlights();
            var mockSet = CreateMockSet(flights);
            // Set up the DroneFlights property so it returns the mocked DbSet
            mockContext.Setup(o => o.DroneFlights).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockContext.Setup(c => c.DroneFlights.Find(It.IsAny<object[]>())).Returns((object[] input) => flights.SingleOrDefault(x => x.FlightId == (int)input.First()));

            controller.Index(3, postedFiles);

            var result = controller.GetStatus();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(JsonResult));
        }

        private List<Drone> GetDrones()
        {
            List<Drone> drones = new List<Drone>();

            Pilot pilot = new Pilot
            {
                PilotId = 1,
                PilotName = "Pilot"
            };

            Project project = new Project
            {
                ProjectId = 1,
                ProjectCode = "Project"
            };

            for (int i = 1; i <= 10; i++)
            {
                Drone drone = new Drone
                {
                    DroneId = i,
                    DroneType = "type" + i,
                    Registration = "registration",
                    DroneName = "Phantom" + i,
                    TotalFlightTime = 0
                };

                for (int j = 1; j <= 10; j++)
                {
                    DroneFlight flight = new DroneFlight
                    {
                        FlightId = j,
                        PilotId = 1,
                        Pilot = pilot,
                        ProjectId = 1,
                        Project = project,
                        DroneId = i,
                        Drone = drone,
                        Date = DateTime.Now
                    };

                    drone.DroneFlights.Add(flight);
                }

                drones.Add(drone);
            }
            return drones;
        }

        private List<DroneFlight> GetFlights()
        {
            // Initialise a list of DroneFlight objects to back the DbSet with.
            // Arrange
            List<DroneFlight> flights = new List<DroneFlight>();

            Pilot pilot = new Pilot
            {
                PilotId = 1,
                PilotName = "Pilot1"
            };

            Drone drone = new Drone
            {
                DroneId = 1,
                DroneType = "type1",
                Registration = "registration"
            };

            Project project = new Project
            {
                ProjectId = 1,
                ProjectCode = "Project1"
            };

            for (int i = 1; i <= 10; i++)
            {
                DroneFlight flight = new DroneFlight
                {
                    FlightId = i,
                    ProjectId = 1,
                    Project = project,
                    PilotId = 1,
                    Pilot = pilot,
                    DroneId = 1,
                    Drone = drone,
                    Date = DateTime.Now,
                };

                flights.Add(flight);
            }
            return flights;
        }

        private Mock<DbSet<DroneFlight>> CreateMockSet(List<DroneFlight> flights)
        {
            var queryable = flights.AsQueryable();
            var mockSet = new Mock<DbSet<DroneFlight>>();

            mockSet.As<IQueryable<DroneFlight>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<DroneFlight>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<DroneFlight>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<DroneFlight>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            return mockSet;
        }

        private Mock<DbSet<Drone>> CreateMockSet(List<Drone> drones)
        {
            var queryable = drones.AsQueryable();
            var mockSet = new Mock<DbSet<Drone>>();

            mockSet.As<IQueryable<Drone>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<Drone>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<Drone>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<Drone>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            return mockSet;
        }
    }
}