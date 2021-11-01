using Microsoft.VisualStudio.TestTools.UnitTesting;
using DroneWebApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DroneWebApp.Models;
using System.Web.Mvc;
using Moq;
using System.Data.Entity;
using Xunit;
using Intuit.Ipp.Data;

namespace DroneWebApp.Controllers.Tests
{
    [TestClass()]
    public class DroneFlightsControllerTests
    {
        [TestMethod()]
        public void IndexTest_ActionExecutes_ReturnsViewForIndex()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DroneFlightsController controller = new DroneFlightsController(mockContext.Object);

            // Create a mock DbSet
            List<DroneFlight> flights = GetFlights();
            var mockSet = CreateMockSet(flights);
            // Set up the DroneFlights property so it returns the mocked DbSet
            mockContext.Setup(o => o.DroneFlights).Returns(() => mockSet.Object);

            var result = controller.Index() as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod()]
        public void IndexTest_ViewData()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DroneFlightsController controller = new DroneFlightsController(mockContext.Object);

            // Create a mock DbSet
            List<DroneFlight> flights = GetFlights();
            var mockSet = CreateMockSet(flights);
            // Set up the DroneFlights property so it returns the mocked DbSet
            mockContext.Setup(o => o.DroneFlights).Returns(() => mockSet.Object);

            var result = controller.Index() as ViewResult;
            var resultModel = result.ViewData.Model;
            List<DroneFlight> flightsResult = (List<DroneFlight>)resultModel;

            for (int i = 0; i < flightsResult.Count; i++)
            {
                Assert.AreEqual(true, DroneFlightEquals(flights[i], flightsResult[i]));
            }
        }

        [TestMethod()]
        public void DetailsTest_ActionExecutes_ReturnsViewForDetails()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DroneFlightsController controller = new DroneFlightsController(mockContext.Object);

            // Create a mock DbSet
            List<DroneFlight> flights = GetFlights();
            var mockSet = CreateMockSet(flights);
            // Set up the DroneFlights property so it returns the mocked DbSet
            mockContext.Setup(o => o.DroneFlights).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockSet.Setup(set => set.Find(It.IsAny<object[]>())).Returns((object[] input) => flights.SingleOrDefault(x => x.FlightId == (int)input.First()));

            var result = controller.Details(3) as ViewResult;
            Assert.AreEqual("Details", result.ViewName);
        }

        [TestMethod()]
        public void DetailsTest_ViewData()
        {
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DroneFlightsController controller = new DroneFlightsController(mockContext.Object);

            // Create mock context
            //Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();

            // Create a mock DbSet
            List<DroneFlight> flights = GetFlights();
            var mockSet = CreateMockSet(flights);
            // Set up the DroneFlights property so it returns the mocked DbSet
            mockContext.Setup(o => o.DroneFlights).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockSet.Setup(set => set.Find(It.IsAny<object[]>())).Returns((object[] input) => flights.SingleOrDefault(x => x.FlightId == (int)input.First()));

            var result = controller.Details(3) as ViewResult;
            DroneFlight flight = (DroneFlight)result.Model;
            Assert.AreEqual(true, DroneFlightEquals(flights.FirstOrDefault(df => df.FlightId == 3), flight));
        }

        [TestMethod()]
        public void CreateTest1_ActionExecutes_ReturnsViewForCreate()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DroneFlightsController controller = new DroneFlightsController(mockContext.Object);

            // Create a mock DbSet
            List<DroneFlight> flights = GetFlights();
            List<Pilot> pilots = GetPilots();
            List<Drone> drones = GetDrones();
            List<Project> projects = GetProjects();
            var mockSetFlights = CreateMockSet(flights);
            var mockSetPilots = CreateMockSet(pilots);
            var mockSetDrones = CreateMockSet(drones);
            var mockSetProjects = CreateMockSet(projects);
            // Set up the DroneFlights property so it returns the mocked DbSet
            mockContext.Setup(o => o.DroneFlights).Returns(() => mockSetFlights.Object);
            // Set up the Pilots property so it returns the mocked DbSet
            mockContext.Setup(o => o.Pilots).Returns(() => mockSetPilots.Object);
            // Set up the Drones property so it returns the mocked DbSet
            mockContext.Setup(o => o.Drones).Returns(() => mockSetDrones.Object);
            // Set up the Projects property so it returns the mocked DbSet
            mockContext.Setup(o => o.Projects).Returns(() => mockSetProjects.Object);

            var result = controller.Create(1, 1, 1) as ViewResult;
            Assert.AreEqual("Create", result.ViewName);
        }

        [TestMethod()]
        public void CreateTest2_ActionExecutes_ReturnsViewForCreate()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DroneFlightsController controller = new DroneFlightsController(mockContext.Object);

            // Create a mock DbSet
            List<DroneFlight> flights = GetFlights();
            var mockSet = CreateMockSet(flights);
            // Set up the DroneFlights property so it returns the mocked DbSet
            mockContext.Setup(o => o.DroneFlights).Returns(() => mockSet.Object);
            // Set up the Add method for the mocked DbSet
            mockSet.Setup(x => x.Add(It.IsAny<DroneFlight>())).Callback<DroneFlight>(df => flights.Add(df));

            DroneFlight flight = new DroneFlight
            {
                FlightId = 11,
                Pilot = new Pilot { PilotId = 2, PilotName = "Pilot2" },
                PilotId = 2,
                Project = new Project { ProjectId = 2, ProjectCode = "Project2" },
                ProjectId = 2,
                Drone = new Drone { DroneId = 2, DroneType = "type", Registration = "reg" },
                Date = DateTime.Now
            };

            var result = controller.Create(flight) as RedirectToRouteResult;

            mockSet.Verify(x => x.Add(It.IsAny<DroneFlight>()), Times.Once);
            mockContext.Verify(x => x.SaveChanges(), Times.Once);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
        }

        [TestMethod()]
        public void CreateTest2_EntityAddedToContext()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DroneFlightsController controller = new DroneFlightsController(mockContext.Object);

            // Create a mock DbSet
            List<DroneFlight> flights = GetFlights();
            var mockSet = CreateMockSet(flights);
            // Set up the DroneFlights property so it returns the mocked DbSet
            mockContext.Setup(o => o.DroneFlights).Returns(() => mockSet.Object);
            // Set up the Add method for the mockdd DbSet
            mockSet.Setup(x => x.Add(It.IsAny<DroneFlight>())).Callback<DroneFlight>(df => flights.Add(df));

            DroneFlight flight = new DroneFlight
            {
                FlightId = 11,
                Pilot = new Pilot { PilotId = 2, PilotName = "Pilot2" },
                PilotId = 2,
                Project = new Project { ProjectId = 2, ProjectCode = "Project2" },
                ProjectId = 2,
                Drone = new Drone { DroneId = 2, DroneType = "type", Registration = "reg" },
                Date = DateTime.Now
            };

            var result = controller.Create(flight) as RedirectToRouteResult;

            mockSet.Verify(x => x.Add(It.IsAny<DroneFlight>()), Times.Once);
            mockContext.Verify(x => x.SaveChanges(), Times.Once);
            Assert.AreEqual(11, flights.Count);
            Assert.IsTrue(flights.Any(df => df.FlightId == 11));
        }

        [TestMethod()]
        public void EditTest1_ActionExecutes_ReturnsViewForEdit()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DroneFlightsController controller = new DroneFlightsController(mockContext.Object);

            // Create a mock DbSet
            List<DroneFlight> flights = GetFlights();
            List<Pilot> pilots = GetPilots();
            List<Drone> drones = GetDrones();
            List<Project> projects = GetProjects();
            var mockSetFlights = CreateMockSet(flights);
            var mockSetPilots = CreateMockSet(pilots);
            var mockSetDrones = CreateMockSet(drones);
            var mockSetProjects = CreateMockSet(projects);
            // Set up the DroneFlights property so it returns the mocked DbSet
            mockContext.Setup(o => o.DroneFlights).Returns(() => mockSetFlights.Object);
            // Set up the Pilots property so it returns the mocked DbSet
            mockContext.Setup(o => o.Pilots).Returns(() => mockSetPilots.Object);
            // Set up the Drones property so it returns the mocked DbSet
            mockContext.Setup(o => o.Drones).Returns(() => mockSetDrones.Object);
            // Set up the Projects property so it returns the mocked DbSet
            mockContext.Setup(o => o.Projects).Returns(() => mockSetProjects.Object);
            // Set up the Find method for the mocked DbSet
            mockSetFlights.Setup(set => set.Find(It.IsAny<object[]>())).Returns((object[] input) => flights.SingleOrDefault(x => x.FlightId == (int)input.First()));

            var result = controller.Edit(3) as ViewResult;
            Assert.AreEqual("Edit", result.ViewName);
        }

        [TestMethod()]
        public void EditTest1_ViewData()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DroneFlightsController controller = new DroneFlightsController(mockContext.Object);

            // Create a mock DbSet
            List<DroneFlight> flights = GetFlights();
            List<Pilot> pilots = GetPilots();
            List<Drone> drones = GetDrones();
            List<Project> projects = GetProjects();
            var mockSetFlights = CreateMockSet(flights);
            var mockSetPilots = CreateMockSet(pilots);
            var mockSetDrones = CreateMockSet(drones);
            var mockSetProjects = CreateMockSet(projects);
            // Set up the DroneFlights property so it returns the mocked DbSet
            mockContext.Setup(o => o.DroneFlights).Returns(() => mockSetFlights.Object);
            // Set up the Pilots property so it returns the mocked DbSet
            mockContext.Setup(o => o.Pilots).Returns(() => mockSetPilots.Object);
            // Set up the Drones property so it returns the mocked DbSet
            mockContext.Setup(o => o.Drones).Returns(() => mockSetDrones.Object);
            // Set up the Projects property so it returns the mocked DbSet
            mockContext.Setup(o => o.Projects).Returns(() => mockSetProjects.Object);
            // Set up the Find method for the mocked DbSet
            mockSetFlights.Setup(set => set.Find(It.IsAny<object[]>())).Returns((object[] input) => flights.SingleOrDefault(x => x.FlightId == (int)input.First()));

            var result = controller.Edit(3) as ViewResult;
            DroneFlight flight = (DroneFlight)result.Model;
            Assert.AreEqual(true, DroneFlightEquals(flights.FirstOrDefault(df => df.FlightId == 3), flight));
        }

        [TestMethod()]
        public void EditTest2_ActionExecutes_ReturnsViewForEdit()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DroneFlightsController controller = new DroneFlightsController(mockContext.Object);

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
            mockContext.Setup(c => c.DroneFlights.Find(It.IsAny<object[]>())).Returns((object[] input) => flights.SingleOrDefault(x => x.FlightId == (int)input.First()));

            var result = controller.Edit(flights[3]) as RedirectToRouteResult;

            mockContext.Verify(x => x.SaveChanges(), Times.Exactly(21)); // SaveChanges is called once in the controller method and 2 times per drone in the Helper class
            //Assert.AreEqual(EntityState.Modified, mockContext.Object.Entry(flights[3]).State);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
        }

        [TestMethod()]
        public void DeleteTest_ActionExecutes_ReturnsViewForDelete()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DroneFlightsController controller = new DroneFlightsController(mockContext.Object);

            // Create a mock DbSet
            List<DroneFlight> flights = GetFlights();
            var mockSet = CreateMockSet(flights);
            // Set up the DroneFlights property so it returns the mocked DbSet
            mockContext.Setup(o => o.DroneFlights).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockSet.Setup(set => set.Find(It.IsAny<object[]>())).Returns((object[] input) => flights.SingleOrDefault(x => x.FlightId == (int)input.First()));

            var result = controller.Delete(3) as ViewResult;
            Assert.AreEqual("Delete", result.ViewName); ;
        }

        [TestMethod()]
        public void DeleteTest_ViewData()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DroneFlightsController controller = new DroneFlightsController(mockContext.Object);

            // Create a mock DbSet
            List<DroneFlight> flights = GetFlights();
            var mockSet = CreateMockSet(flights);
            // Set up the DroneFlights property so it returns the mocked DbSet
            mockContext.Setup(o => o.DroneFlights).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockSet.Setup(set => set.Find(It.IsAny<object[]>())).Returns((object[] input) => flights.SingleOrDefault(x => x.FlightId == (int)input.First()));

            var result = controller.Delete(3) as ViewResult;
            DroneFlight flight = (DroneFlight)result.Model;
            Assert.AreEqual(true, DroneFlightEquals(flights.FirstOrDefault(df => df.FlightId == 3), flight));
        }

        [TestMethod()]
        public void DeleteConfirmedTest_ActionExecutes_ReturnsViewForDelete()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DroneFlightsController controller = new DroneFlightsController(mockContext.Object);

            // Create a mock DbSet
            List<DroneFlight> flights = GetFlights();
            List<Pilot> pilots = GetPilots();
            List<Drone> drones = GetDrones();
            List<Project> projects = GetProjects();
            var mockSetFlights = CreateMockSet(flights);
            var mockSetPilots = CreateMockSet(pilots);
            var mockSetDrones = CreateMockSet(drones);
            var mockSetProjects = CreateMockSet(projects);
            // Set up the DroneFlights property so it returns the mocked DbSet
            mockContext.Setup(o => o.DroneFlights).Returns(() => mockSetFlights.Object);
            // Set up the Pilots property so it returns the mocked DbSet
            mockContext.Setup(o => o.Pilots).Returns(() => mockSetPilots.Object);
            // Set up the Drones property so it returns the mocked DbSet
            mockContext.Setup(o => o.Drones).Returns(() => mockSetDrones.Object);
            // Set up the Projects property so it returns the mocked DbSet
            mockContext.Setup(o => o.Projects).Returns(() => mockSetProjects.Object);
            // Set up the Find method for the mocked DbSet
            mockContext.Setup(c => c.DroneFlights.Find(It.IsAny<object[]>())).Returns((object[] input) => flights.SingleOrDefault(x => x.FlightId == (int)input.First()));
            // Set up the Remove method for the mocked context
            mockContext.Setup(m => m.DroneFlights.Remove(It.IsAny<DroneFlight>())).Callback<DroneFlight>((entity) => flights.Remove(entity));

            var result = controller.DeleteConfirmed(3) as RedirectToRouteResult;

            mockContext.Verify(x => x.DroneFlights.Find(3), Times.Once);
            mockContext.Verify(x => x.DroneFlights.Remove(It.IsAny<DroneFlight>()), Times.Once);
            mockContext.Verify(x => x.SaveChanges(), Times.Exactly(21)); // SaveChanges is called once in the controller method and 2 times per drone in the Helper class
            Assert.AreEqual("Index", result.RouteValues["Action"]);
        }

        [TestMethod()]
        public void DeleteConfirmedTest_EntityRemovedFromContext()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DroneFlightsController controller = new DroneFlightsController(mockContext.Object);

            // Create a mock DbSet
            List<DroneFlight> flights = GetFlights();
            List<Pilot> pilots = GetPilots();
            List<Drone> drones = GetDrones();
            List<Project> projects = GetProjects();
            var mockSetFlights = CreateMockSet(flights);
            var mockSetPilots = CreateMockSet(pilots);
            var mockSetDrones = CreateMockSet(drones);
            var mockSetProjects = CreateMockSet(projects);
            // Set up the DroneFlights property so it returns the mocked DbSet
            mockContext.Setup(o => o.DroneFlights).Returns(() => mockSetFlights.Object);
            // Set up the Pilots property so it returns the mocked DbSet
            mockContext.Setup(o => o.Pilots).Returns(() => mockSetPilots.Object);
            // Set up the Drones property so it returns the mocked DbSet
            mockContext.Setup(o => o.Drones).Returns(() => mockSetDrones.Object);
            // Set up the Projects property so it returns the mocked DbSet
            mockContext.Setup(o => o.Projects).Returns(() => mockSetProjects.Object);
            // Set up the Find method for the mocked DbSet
            mockContext.Setup(c => c.DroneFlights.Find(It.IsAny<object[]>())).Returns((object[] input) => flights.SingleOrDefault(x => x.FlightId == (int)input.First()));
            // Set up the Remove method for the mocked context
            mockContext.Setup(m => m.DroneFlights.Remove(It.IsAny<DroneFlight>())).Callback<DroneFlight>((entity) => flights.Remove(entity));

            var result = controller.DeleteConfirmed(3) as RedirectToRouteResult;

            mockContext.Verify(x => x.DroneFlights.Find(3), Times.Once);
            mockContext.Verify(x => x.DroneFlights.Remove(It.IsAny<DroneFlight>()), Times.Once);
            mockContext.Verify(x => x.SaveChanges(), Times.Exactly(21)); // SaveChanges is called once in the controller method and 2 times per drone in the Helper class
            Assert.AreEqual(9, flights.Count);
            Assert.IsFalse(flights.Any(d => d.DroneId == 3));
        }

        [TestMethod()]
        public void QualityReportTest_ActionExecutes_ReturnsViewForQualityReport()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DroneFlightsController controller = new DroneFlightsController(mockContext.Object);

            // Create a mock DbSet
            List<DroneFlight> flights = GetFlights();
            var mockSet = CreateMockSet(flights);
            // Set up the DroneFlights property so it returns the mocked DbSet
            mockContext.Setup(o => o.DroneFlights).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockContext.Setup(c => c.DroneFlights.Find(It.IsAny<object[]>())).Returns((object[] input) => flights.SingleOrDefault(x => x.FlightId == (int)input.First()));

            var result = controller.QualityReport(3) as ViewResult;
            Assert.AreEqual("QualityReport", result.ViewName);
        }

        [TestMethod()]
        public void QualityReportTest_ViewData()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DroneFlightsController controller = new DroneFlightsController(mockContext.Object);

            // Create a mock DbSet
            List<DroneFlight> flights = GetFlights();
            var mockSet = CreateMockSet(flights);
            // Set up the DroneFlights property so it returns the mocked DbSet
            mockContext.Setup(o => o.DroneFlights).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockContext.Setup(c => c.DroneFlights.Find(It.IsAny<object[]>())).Returns((object[] input) => flights.SingleOrDefault(x => x.FlightId == (int)input.First()));

            var result = controller.QualityReport(3) as ViewResult;
            QualityReport report = (QualityReport)result.Model;
            Assert.AreEqual(flights.SingleOrDefault(df => df.FlightId == 3).QualityReport, report);
        }

        [TestMethod()]
        public void CTRLPointsTest_ActionExecutes_ReturnsViewForCTRLPoints()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DroneFlightsController controller = new DroneFlightsController(mockContext.Object);

            // Create a mock DbSet
            List<DroneFlight> flights = GetFlights();
            var mockSet = CreateMockSet(flights);
            // Set up the DroneFlights property so it returns the mocked DbSet
            mockContext.Setup(o => o.DroneFlights).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockContext.Setup(c => c.DroneFlights.Find(It.IsAny<object[]>())).Returns((object[] input) => flights.SingleOrDefault(x => x.FlightId == (int)input.First()));

            var result = controller.CTRLPoints(3) as ViewResult;
            Assert.AreEqual("CTRLPoints", result.ViewName);
        }

        [TestMethod()]
        public void CTRLPointsTest_ViewData()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DroneFlightsController controller = new DroneFlightsController(mockContext.Object);

            // Create a mock DbSet
            List<DroneFlight> flights = GetFlights();
            var mockSet = CreateMockSet(flights);
            // Set up the DroneFlights property so it returns the mocked DbSet
            mockContext.Setup(o => o.DroneFlights).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockContext.Setup(c => c.DroneFlights.Find(It.IsAny<object[]>())).Returns((object[] input) => flights.SingleOrDefault(x => x.FlightId == (int)input.First()));

            var result = controller.CTRLPoints(3) as ViewResult;
            List<CTRLPoint> ctrlPoints = (List<CTRLPoint>)result.Model;
            for (int i=0; i<ctrlPoints.Count; i++)
            {
                Assert.AreEqual(flights.SingleOrDefault(df => df.FlightId == 3).CTRLPoints.ToList()[i], ctrlPoints[i]);
            }
        }

        [TestMethod()]
        public void GCPPointsTest_ActionExecutes_ReturnsViewForGCPPoints()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DroneFlightsController controller = new DroneFlightsController(mockContext.Object);

            // Create a mock DbSet
            List<DroneFlight> flights = GetFlights();
            var mockSet = CreateMockSet(flights);
            // Set up the DroneFlights property so it returns the mocked DbSet
            mockContext.Setup(o => o.DroneFlights).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockContext.Setup(c => c.DroneFlights.Find(It.IsAny<object[]>())).Returns((object[] input) => flights.SingleOrDefault(x => x.FlightId == (int)input.First()));

            var result = controller.GCPPoints(3) as ViewResult;
            Assert.AreEqual("GCPPoints", result.ViewName);
        }

        [TestMethod()]
        public void GCPPointsTest_ViewData()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DroneFlightsController controller = new DroneFlightsController(mockContext.Object);

            // Create a mock DbSet
            List<DroneFlight> flights = GetFlights();
            var mockSet = CreateMockSet(flights);
            // Set up the DroneFlights property so it returns the mocked DbSet
            mockContext.Setup(o => o.DroneFlights).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockContext.Setup(c => c.DroneFlights.Find(It.IsAny<object[]>())).Returns((object[] input) => flights.SingleOrDefault(x => x.FlightId == (int)input.First()));

            var result = controller.GCPPoints(3) as ViewResult;
            List<GroundControlPoint> gcpPoints = (List<GroundControlPoint>)result.Model;
            for (int i = 0; i < gcpPoints.Count; i++)
            {
                Assert.AreEqual(flights.SingleOrDefault(df => df.FlightId == 3).GroundControlPoints.ToList()[i], gcpPoints[i]);
            }
        }

        [TestMethod()]
        public void TFWTest_ActionExecutes_ReturnsViewForTFW()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DroneFlightsController controller = new DroneFlightsController(mockContext.Object);

            // Create a mock DbSet
            List<DroneFlight> flights = GetFlights();
            var mockSet = CreateMockSet(flights);
            // Set up the DroneFlights property so it returns the mocked DbSet
            mockContext.Setup(o => o.DroneFlights).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockContext.Setup(c => c.DroneFlights.Find(It.IsAny<object[]>())).Returns((object[] input) => flights.SingleOrDefault(x => x.FlightId == (int)input.First()));

            var result = controller.TFW(3) as ViewResult;
            Assert.AreEqual("TFW", result.ViewName);
        }

        [TestMethod()]
        public void TFWTest_ViewData()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DroneFlightsController controller = new DroneFlightsController(mockContext.Object);

            // Create a mock DbSet
            List<DroneFlight> flights = GetFlights();
            var mockSet = CreateMockSet(flights);
            // Set up the DroneFlights property so it returns the mocked DbSet
            mockContext.Setup(o => o.DroneFlights).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockContext.Setup(c => c.DroneFlights.Find(It.IsAny<object[]>())).Returns((object[] input) => flights.SingleOrDefault(x => x.FlightId == (int)input.First()));

            var result = controller.TFW(3) as ViewResult;
            TFW tfw = (TFW)result.Model;
            Assert.AreEqual(flights.SingleOrDefault(df => df.FlightId == 3).TFW, tfw);
        }

        [TestMethod()]
        public void RawImagesTest_ActionExecutes_ReturnsViewForRawImages()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DroneFlightsController controller = new DroneFlightsController(mockContext.Object);

            // Create a mock DbSet
            List<DroneFlight> flights = GetFlights();
            var mockSet = CreateMockSet(flights);
            // Set up the DroneFlights property so it returns the mocked DbSet
            mockContext.Setup(o => o.DroneFlights).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockContext.Setup(c => c.DroneFlights.Find(It.IsAny<object[]>())).Returns((object[] input) => flights.SingleOrDefault(x => x.FlightId == (int)input.First()));

            var result = controller.RawImages(3) as ViewResult;
            Assert.AreEqual("RawImages", result.ViewName);
        }

        [TestMethod()]
        public void RawImagesTest_ViewData()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DroneFlightsController controller = new DroneFlightsController(mockContext.Object);

            // Create a mock DbSet
            List<DroneFlight> flights = GetFlights();
            var mockSet = CreateMockSet(flights);
            // Set up the DroneFlights property so it returns the mocked DbSet
            mockContext.Setup(o => o.DroneFlights).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockContext.Setup(c => c.DroneFlights.Find(It.IsAny<object[]>())).Returns((object[] input) => flights.SingleOrDefault(x => x.FlightId == (int)input.First()));

            var result = controller.RawImages(3) as ViewResult;
            List<RawImage> images = (List<RawImage>)result.Model;
            for (int i=0; i<images.Count; i++)
            {
                Assert.AreEqual(flights.SingleOrDefault(df => df.FlightId == 3).RawImages.ToList()[i], images[i]);
            }
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

            for (int i=1; i<=10; i++)
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
                    QualityReport = new QualityReport { QualityReportId = i},
                    TFW = new TFW { TFWId = i}
                };

                List<CTRLPoint> ctrlPoints = new List<CTRLPoint>();
                List<GroundControlPoint> gcps = new List<GroundControlPoint>();
                List<RawImage> images = new List<RawImage>();

                for (int j=1; j<=5; j++)
                {
                    CTRLPoint ctrl = new CTRLPoint { FlightId = i, DroneFlight = flight, CTRLId = j, CTRLName = "ctrl-"+j, X = 1, Y = 2, Z = 3 };
                    GroundControlPoint gcp = new GroundControlPoint { FlightId = i, DroneFlight = flight, GCPId = j, GCPName = "gcp-" + j, X = 1, Y = 2, Z = 3 };
                    RawImage image = new RawImage { FlightId = i, DroneFlight = flight, RawImageKey = j };

                    ctrlPoints.Add(ctrl);
                    gcps.Add(gcp);
                    images.Add(image);
                }

                flight.CTRLPoints = ctrlPoints;
                flight.GroundControlPoints = gcps;
                flight.RawImages = images;
                flights.Add(flight);
            }
            return flights;
        }

        private List<Pilot> GetPilots()
        {
            List<Pilot> pilots = new List<Pilot>();
            for (int i = 1; i <= 10; i++)
            {
                Pilot pilot = new Pilot
                {
                    PilotId = i,
                    PilotName = "Pilot" + i
                };

                pilots.Add(pilot);
            }
            return pilots;
        }

        private List<Drone> GetDrones()
        {
            List<Drone> drones = new List<Drone>();

            for (int i = 1; i <= 10; i++)
            {
                Drone drone = new Drone
                {
                    DroneId = i,
                    DroneType = "type" + i,
                    Registration = "registration"
                };

                drones.Add(drone);
            }
            return drones;
        }

        private List<Project> GetProjects()
        {
            List<Project> projects = new List<Project>();

            for (int i = 1; i <= 10; i++)
            {
                Project project = new Project
                {
                    ProjectId = i,
                    ProjectCode = "Project" + i
                };

                projects.Add(project);
            }
            return projects;
        }

        private bool DroneFlightEquals(DroneFlight df1, DroneFlight df2)
        {
            if (df1.FlightId != df2.FlightId)
            {
                return false;
            }
            if (df1.ProjectId != df2.ProjectId)
            {
                return false;
            }
            if (df1.PilotId != df2.PilotId)
            {
                return false;
            }
            if (df1.DroneId != df2.DroneId)
            {
                return false;
            }
            /*
            if (df1.Date != df2.Date)
            {
                return false;
            }
            */

            return true;
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

        private Mock<DbSet<Pilot>> CreateMockSet(List<Pilot> pilots)
        {
            var queryable = pilots.AsQueryable();
            var mockSet = new Mock<DbSet<Pilot>>();

            mockSet.As<IQueryable<Pilot>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<Pilot>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<Pilot>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<Pilot>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

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

        private Mock<DbSet<Project>> CreateMockSet(List<Project> projects)
        {
            var queryable = projects.AsQueryable();
            var mockSet = new Mock<DbSet<Project>>();

            mockSet.As<IQueryable<Project>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<Project>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<Project>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<Project>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            return mockSet;
        }
    }
}

