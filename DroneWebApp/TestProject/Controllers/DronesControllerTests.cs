using Microsoft.VisualStudio.TestTools.UnitTesting;
using DroneWebApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using DroneWebApp.Models;
using System.Data.Entity;
using System.Web.Mvc;

namespace DroneWebApp.Controllers.Tests
{
    [TestClass()]
    public class DronesControllerTests
    {
        [TestMethod()]
        public void IndexTest_ActionExecutes_ReturnsViewForIndex()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DronesController controller = new DronesController(mockContext.Object);

            // Create a mock DbSet
            List<Drone> drones = GetDrones();
            var mockSet = CreateMockSet(drones);
            // Set up the Drones property so it returns the mocked DbSet
            mockContext.Setup(o => o.Drones).Returns(() => mockSet.Object);

            var result = controller.Index() as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod()]
        public void IndexTest_ViewData()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DronesController controller = new DronesController(mockContext.Object);

            // Create a mock DbSet
            List<Drone> drones = GetDrones();
            var mockSet = CreateMockSet(drones);
            // Set up the Drones property so it returns the mocked DbSet
            mockContext.Setup(o => o.Drones).Returns(() => mockSet.Object);

            var result = controller.Index() as ViewResult;
            var resultModel = result.ViewData.Model;
            List<Drone> dronesResult = (List<Drone>)resultModel;

            for (int i = 0; i < dronesResult.Count; i++)
            {
                Assert.AreEqual(true, DroneEquals(drones[i], dronesResult[i]));
            }
        }

        [TestMethod()]
        public void DetailsTest_ActionExecutes_ReturnsViewForDetails()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DronesController controller = new DronesController(mockContext.Object);

            // Create a mock DbSet
            List<Drone> drones = GetDrones();
            var mockSet = CreateMockSet(drones);
            // Set up the Drones property so it returns the mocked DbSet
            mockContext.Setup(o => o.Drones).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockSet.Setup(set => set.Find(It.IsAny<object[]>())).Returns((object[] input) => drones.SingleOrDefault(x => x.DroneId == (int)input.First()));

            var result = controller.Details(3) as ViewResult;
            Assert.AreEqual("Details", result.ViewName);
        }

        [TestMethod()]
        public void DetailsTest_ViewData()
        {
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DronesController controller = new DronesController(mockContext.Object);

            // Create mock context
            //Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();

            // Create a mock DbSet
            List<Drone> drones = GetDrones();
            var mockSet = CreateMockSet(drones);
            // Set up the Drones property so it returns the mocked DbSet
            mockContext.Setup(o => o.Drones).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockSet.Setup(set => set.Find(It.IsAny<object[]>())).Returns((object[] input) => drones.SingleOrDefault(x => x.DroneId == (int)input.First()));

            var result = controller.Details(3) as ViewResult;
            Drone drone = (Drone)result.Model;
            Assert.AreEqual(true, DroneEquals(drones.FirstOrDefault(d => d.DroneId == 3), drone));
        }

        [TestMethod()]
        public void CreateTest1_ActionExecutes_ReturnsViewForCreate()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DronesController controller = new DronesController(mockContext.Object);

            // Create a mock DbSet
            List<Drone> drones = GetDrones();
            var mockSet = CreateMockSet(drones);
            // Set up the Drones property so it returns the mocked DbSet
            mockContext.Setup(o => o.Drones).Returns(() => mockSet.Object);

            var result = controller.Create() as ViewResult;
            Assert.AreEqual("Create", result.ViewName);
        }

        [TestMethod()]
        public void CreateTest2_ActionExecutes_ReturnsViewForCreate()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DronesController controller = new DronesController(mockContext.Object);

            // Create a mock DbSet
            List<Drone> drones = GetDrones();
            var mockSet = CreateMockSet(drones);
            // Set up the Drones property so it returns the mocked DbSet
            mockContext.Setup(o => o.Drones).Returns(() => mockSet.Object);
            // Set up the Add method for the mocked DbSet
            mockSet.Setup(x => x.Add(It.IsAny<Drone>())).Callback<Drone>(d => drones.Add(d));

            Drone drone = new Drone
            {
                DroneId = 11,
                DroneType = "droneType",
                Registration = "reg"
            };

            var result = controller.Create(drone) as RedirectToRouteResult;

            mockSet.Verify(x => x.Add(It.IsAny<Drone>()), Times.Once);
            mockContext.Verify(x => x.SaveChanges(), Times.Once);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
        }

        [TestMethod()]
        public void CreateTest2_EntityAddedToContext()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DronesController controller = new DronesController(mockContext.Object);

            // Create a mock DbSet
            List<Drone> drones = GetDrones();
            var mockSet = CreateMockSet(drones);
            // Set up the Drones property so it returns the mocked DbSet
            mockContext.Setup(o => o.Drones).Returns(() => mockSet.Object);
            // Set up the Add method for the mockdd DbSet
            mockSet.Setup(x => x.Add(It.IsAny<Drone>())).Callback<Drone>(d => drones.Add(d));

            Drone drone = new Drone
            {
                DroneId = 11,
                DroneType = "droneType",
                Registration = "reg"
            };

            var result = controller.Create(drone) as RedirectToRouteResult;

            mockSet.Verify(x => x.Add(It.IsAny<Drone>()), Times.Once);
            mockContext.Verify(x => x.SaveChanges(), Times.Once);
            Assert.AreEqual(11, drones.Count);
            Assert.IsTrue(drones.Any(d => d.DroneId == 11));
        }

        [TestMethod()]
        public void EditTest1_ActionExecutes_ReturnsViewForEdit()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DronesController controller = new DronesController(mockContext.Object);

            // Create a mock DbSet
            List<Drone> drones = GetDrones();
            var mockSet = CreateMockSet(drones);
            // Set up the Drones property so it returns the mocked DbSet
            mockContext.Setup(o => o.Drones).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockSet.Setup(set => set.Find(It.IsAny<object[]>())).Returns((object[] input) => drones.SingleOrDefault(x => x.DroneId == (int)input.First()));

            var result = controller.Edit(3) as ViewResult;
            Assert.AreEqual("Edit", result.ViewName);
        }

        [TestMethod()]
        public void EditTest1_ViewData()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DronesController controller = new DronesController(mockContext.Object);

            // Create a mock DbSet
            List<Drone> drones = GetDrones();
            var mockSet = CreateMockSet(drones);
            // Set up the Drones property so it returns the mocked DbSet
            mockContext.Setup(o => o.Drones).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockSet.Setup(set => set.Find(It.IsAny<object[]>())).Returns((object[] input) => drones.SingleOrDefault(x => x.DroneId == (int)input.First()));

            var result = controller.Edit(3) as ViewResult;
            Drone drone = (Drone)result.Model;
            Assert.AreEqual(true, DroneEquals(drones.FirstOrDefault(d => d.DroneId == 3), drone));
        }

        [TestMethod()]
        public void EditTest2_ActionExecutes_ReturnsViewForEdit()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DronesController controller = new DronesController(mockContext.Object);

            // Create a mock DbSet
            List<Drone> drones = GetDrones();
            var mockSet = CreateMockSet(drones);
            // Set up the Drones property so it returns the mocked DbSet
            mockContext.Setup(o => o.Drones).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockSet.Setup(s => s.Find(It.IsAny<object[]>())).Returns((object[] input) => drones.SingleOrDefault(x => x.DroneId == (int)input.First()));

            List<Drone> test = mockSet.Object.ToList();
            foreach (Drone d in test)
            {
                System.Diagnostics.Debug.WriteLine(d.DroneName);
            }
            
            List<Drone> test1 = mockContext.Object.Drones.ToList();
            foreach (Drone d in test1)
            {
                System.Diagnostics.Debug.WriteLine(d.DroneName);
            }

            var result = controller.Edit(drones[3]) as RedirectToRouteResult;

            mockContext.Verify(x => x.SaveChanges(), Times.Exactly(21)); // SaveChanges is called once in the controller method and 2 times per drone in the Helper class
            //Assert.AreEqual(EntityState.Modified, mockContext.Object.Entry(drones[3]).State);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
        }

        [TestMethod()]
        public void DeleteTest_ActionExecutes_ReturnsViewForDelete()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DronesController controller = new DronesController(mockContext.Object);

            // Create a mock DbSet
            List<Drone> drones = GetDrones();
            var mockSet = CreateMockSet(drones);
            // Set up the Drones property so it returns the mocked DbSet
            mockContext.Setup(o => o.Drones).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockSet.Setup(set => set.Find(It.IsAny<object[]>())).Returns((object[] input) => drones.SingleOrDefault(x => x.DroneId == (int)input.First()));

            var result = controller.Delete(3) as ViewResult;
            Assert.AreEqual("Delete", result.ViewName);
        }

        [TestMethod()]
        public void DeleteTest_ViewData()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DronesController controller = new DronesController(mockContext.Object);

            // Create a mock DbSet
            List<Drone> drones = GetDrones();
            var mockSet = CreateMockSet(drones);
            // Set up the Drones property so it returns the mocked DbSet
            mockContext.Setup(o => o.Drones).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockSet.Setup(set => set.Find(It.IsAny<object[]>())).Returns((object[] input) => drones.SingleOrDefault(x => x.DroneId == (int)input.First()));

            var result = controller.Delete(3) as ViewResult;
            Drone drone = (Drone)result.Model;
            Assert.AreEqual(true, DroneEquals(drones.FirstOrDefault(d => d.DroneId == 3), drone));
        }

        [TestMethod()]
        public void DeleteConfirmedTest_ActionExecutes_ReturnsViewForDelete()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DronesController controller = new DronesController(mockContext.Object);

            // Create a mock DbSet
            List<Drone> drones = GetDrones();
            var mockSet = CreateMockSet(drones);
            // Set up the Drones property so it returns the mocked DbSet
            mockContext.Setup(o => o.Drones).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockContext.Setup(c => c.Drones.Find(It.IsAny<object[]>())).Returns((object[] input) => drones.SingleOrDefault(x => x.DroneId == (int)input.First()));
            // Set up the Remove method for the mocked context
            mockContext.Setup(m => m.Drones.Remove(It.IsAny<Drone>())).Callback<Drone>((entity) => drones.Remove(entity));

            var result = controller.DeleteConfirmed(3) as RedirectToRouteResult;

            mockContext.Verify(x => x.Drones.Find(3), Times.Once);
            mockContext.Verify(x => x.Drones.Remove(It.IsAny<Drone>()), Times.Once);
            mockContext.Verify(x => x.SaveChanges(), Times.Once);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
        }

        [TestMethod()]
        public void DeleteConfirmedTest_EntityRemovedFromContext()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DronesController controller = new DronesController(mockContext.Object);

            // Create a mock DbSet
            List<Drone> drones = GetDrones();
            var mockSet = CreateMockSet(drones);
            // Set up the Drones property so it returns the mocked DbSet
            mockContext.Setup(o => o.Drones).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockContext.Setup(c => c.Drones.Find(It.IsAny<object[]>())).Returns((object[] input) => drones.SingleOrDefault(x => x.DroneId == (int)input.First()));
            // Set up the Remove method for the mocked context
            mockContext.Setup(m => m.Drones.Remove(It.IsAny<Drone>())).Callback<Drone>((entity) => drones.Remove(entity));

            var result = controller.DeleteConfirmed(3) as RedirectToRouteResult;

            mockContext.Verify(x => x.Drones.Find(3), Times.Once);
            mockContext.Verify(x => x.Drones.Remove(It.IsAny<Drone>()), Times.Once);
            mockContext.Verify(x => x.SaveChanges(), Times.Once);
            Assert.AreEqual(9, drones.Count);
            Assert.IsFalse(drones.Any(d => d.DroneId == 3));
        }

        [TestMethod()]
        public void DroneFlights_ActionExecutes_ReturnsViewForDroneFlights()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DronesController controller = new DronesController(mockContext.Object);

            // Create a mock DbSet
            List<Drone> drones = GetDrones();
            var mockSet = CreateMockSet(drones);
            // Set up the Drones property so it returns the mocked DbSet
            mockContext.Setup(o => o.Drones).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockSet.Setup(set => set.Find(It.IsAny<object[]>())).Returns((object[] input) => drones.SingleOrDefault(x => x.DroneId == (int)input.First()));

            var result = controller.DroneFlights(3) as ViewResult;
            Assert.AreEqual("DroneFlights", result.ViewName);
        }

        [TestMethod()]
        public void DroneFlights_ViewData()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            DronesController controller = new DronesController(mockContext.Object);

            // Create a mock DbSet
            List<Drone> drones = GetDrones();
            var mockSet = CreateMockSet(drones);
            // Set up the Drones property so it returns the mocked DbSet
            mockContext.Setup(o => o.Drones).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockSet.Setup(set => set.Find(It.IsAny<object[]>())).Returns((object[] input) => drones.SingleOrDefault(x => x.DroneId == (int)input.First()));

            var result = controller.DroneFlights(3) as ViewResult;
            List<DroneFlight> flights = (List<DroneFlight>)result.Model;
            List<DroneFlight> generated = drones[2].DroneFlights.ToList();

            for (int i = 0; i < flights.Count; i++)
            {
                Assert.AreEqual(true, DroneFlightEquals(generated[i], flights[i]));
            }
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

            for (int i=1; i<=10; i++)
            {
                Drone drone = new Drone
                {
                    DroneId = i,
                    DroneType = "type" + i,
                    Registration = "registration",
                    DroneName = "Phantom" + i,
                    TotalFlightTime = 0
                };

                for (int j=1; j<=10; j++)
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

        private bool DroneEquals(Drone d1, Drone d2)
        {
            if (d1.DroneId != d2.DroneId)
            {
                return false;
            }
            if (!string.Equals(d1.DroneType, d2.DroneType))
            {
                return false;
            }
            if (!string.Equals(d1.Registration, d2.Registration))
            {
                return false;
            }
            if (!string.Equals(d1.DroneName, d2.DroneName))
            {
                return false;
            }
            return true;
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