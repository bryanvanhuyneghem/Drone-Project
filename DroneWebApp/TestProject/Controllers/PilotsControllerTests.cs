using Microsoft.VisualStudio.TestTools.UnitTesting;
using DroneWebApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DroneWebApp.Models;
using Moq;
using System.Data.Entity;
using System.Web.Mvc;

namespace DroneWebApp.Controllers.Tests
{
    [TestClass()]
    public class PilotsControllerTests
    {
        [TestMethod()]
        public void Index_ActionExecutes_ReturnsViewForIndex()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            PilotsController controller = new PilotsController(mockContext.Object);

            // Create a mock DbSet
            List<Pilot> pilots = GetPilots();
            var mockSet = CreateMockSet(pilots);
            // Set up the Pilots property so it returns the mocked DbSet
            mockContext.Setup(o => o.Pilots).Returns(() => mockSet.Object);

            var result = controller.Index() as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod()]
        public void IndexTest_ViewData()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            PilotsController controller = new PilotsController(mockContext.Object);

            // Create a mock DbSet
            List<Pilot> pilots = GetPilots();
            var mockSet = CreateMockSet(pilots);
            // Set up the Pilots property so it returns the mocked DbSet
            mockContext.Setup(o => o.Pilots).Returns(() => mockSet.Object);

            var result = controller.Index() as ViewResult;
            var resultModel = result.ViewData.Model;
            List<Pilot> pilotsResult = (List<Pilot>)resultModel;

            for (int i=0 ; i<pilotsResult.Count; i++)
            {
                Assert.AreEqual(true, PilotEquals(pilots[i], pilotsResult[i]));
            }
        }

        [TestMethod()]
        public void DetailsTest_ActionExecutes_ReturnsViewForDetails()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            PilotsController controller = new PilotsController(mockContext.Object);

            // Create a mock DbSet
            List<Pilot> pilots = GetPilots();
            var mockSet = CreateMockSet(pilots);
            // Set up the Pilots property so it returns the mocked DbSet
            mockContext.Setup(o => o.Pilots).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockSet.Setup(set => set.Find(It.IsAny<object[]>())).Returns((object[] input) => pilots.SingleOrDefault(x => x.PilotId == (int)input.First()));

            var result = controller.Details(3) as ViewResult;
            Assert.AreEqual("Details", result.ViewName);
        }

        [TestMethod()]
        public void DetailsTest_ViewData()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            PilotsController controller = new PilotsController(mockContext.Object);

            // Create a mock DbSet
            List<Pilot> pilots = GetPilots();
            var mockSet = CreateMockSet(pilots);
            // Set up the Pilots property so it returns the mocked DbSet
            mockContext.Setup(o => o.Pilots).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockSet.Setup(set => set.Find(It.IsAny<object[]>())).Returns((object[] input) => pilots.SingleOrDefault(x => x.PilotId == (int)input.First()));

            var result = controller.Details(3) as ViewResult;
            Pilot pilot = (Pilot)result.Model;
            Assert.AreEqual(true, PilotEquals(pilots.FirstOrDefault(p => p.PilotId == 3), pilot));
        }

        [TestMethod()]
        public void CreateTest1_ActionExecutes_ReturnsViewForCreate()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            PilotsController controller = new PilotsController(mockContext.Object);

            // Create a mock DbSet
            List<Pilot> pilots = GetPilots();
            var mockSet = CreateMockSet(pilots);
            // Set up the Pilots property so it returns the mocked DbSet
            mockContext.Setup(o => o.Pilots).Returns(() => mockSet.Object);

            var result = controller.Create() as ViewResult;
            Assert.AreEqual("Create", result.ViewName);
        }

        [TestMethod()]
        public void CreateTest2_ActionExecutes_ReturnsViewForCreate()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            PilotsController controller = new PilotsController(mockContext.Object);

            // Create a mock DbSet
            List<Pilot> pilots = GetPilots();
            var mockSet = CreateMockSet(pilots);
            // Set up the Pilots property so it returns the mocked DbSet
            mockContext.Setup(o => o.Pilots).Returns(() => mockSet.Object);
            // Set up the Add method for the mockdd DbSet
            mockSet.Setup(x => x.Add(It.IsAny<Pilot>())).Callback<Pilot>(p => pilots.Add(p));

            Pilot pilot = new Pilot
            {
                PilotId = 11,
                PilotName = "Pilot11"
            };

            var result = controller.Create(pilot) as RedirectToRouteResult;

            mockSet.Verify(x => x.Add(It.IsAny<Pilot>()), Times.Once);
            mockContext.Verify(x => x.SaveChanges(), Times.Once);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
        }

        [TestMethod()]
        public void CreateTest2_EntityAddedToContext()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            PilotsController controller = new PilotsController(mockContext.Object);

            // Create a mock DbSet
            List<Pilot> pilots = GetPilots();
            var mockSet = CreateMockSet(pilots);
            // Set up the Pilots property so it returns the mocked DbSet
            mockContext.Setup(o => o.Pilots).Returns(() => mockSet.Object);
            // Set up the Add method for the mockdd DbSet
            mockSet.Setup(x => x.Add(It.IsAny<Pilot>())).Callback<Pilot>(p => pilots.Add(p));

            Pilot pilot = new Pilot
            {
                PilotId = 11,
                PilotName = "Pilot11"
            };

            var result = controller.Create(pilot) as RedirectToRouteResult;

            mockSet.Verify(x => x.Add(It.IsAny<Pilot>()), Times.Once);
            mockContext.Verify(x => x.SaveChanges(), Times.Once);
            Assert.AreEqual(11, pilots.Count);
            Assert.IsTrue(pilots.Any(p => p.PilotId == 11));
        }

        [TestMethod()]
        public void EditTest1_ActionExecutes_ReturnsViewForEdit()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            PilotsController controller = new PilotsController(mockContext.Object);

            // Create a mock DbSet
            List<Pilot> pilots = GetPilots();
            var mockSet = CreateMockSet(pilots);
            // Set up the Pilots property so it returns the mocked DbSet
            mockContext.Setup(o => o.Pilots).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockSet.Setup(set => set.Find(It.IsAny<object[]>())).Returns((object[] input) => pilots.SingleOrDefault(x => x.PilotId == (int)input.First()));

            var result = controller.Edit(3) as ViewResult;
            Assert.AreEqual("Edit", result.ViewName);
        }

        [TestMethod()]
        public void EditTest1_ViewData()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            PilotsController controller = new PilotsController(mockContext.Object);

            // Create a mock DbSet
            List<Pilot> pilots = GetPilots();
            var mockSet = CreateMockSet(pilots);
            // Set up the Pilots property so it returns the mocked DbSet
            mockContext.Setup(o => o.Pilots).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockSet.Setup(set => set.Find(It.IsAny<object[]>())).Returns((object[] input) => pilots.SingleOrDefault(x => x.PilotId == (int)input.First()));

            var result = controller.Edit(3) as ViewResult;
            Pilot pilot = (Pilot)result.Model;
            Assert.AreEqual(true, PilotEquals(GetPilots().FirstOrDefault(p => p.PilotId == 3), pilot));
        }

        [TestMethod()]
        public void EditTest2_ActionExecutes_ReturnsViewForEdit()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            PilotsController controller = new PilotsController(mockContext.Object);

            // Create a mock DbSet
            List<Pilot> pilots = GetPilots();
            var mockSet = CreateMockSet(pilots);
            // Set up the Pilots property so it returns the mocked DbSet
            mockContext.Setup(o => o.Pilots).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockSet.Setup(set => set.Find(It.IsAny<object[]>())).Returns((object[] input) => pilots.SingleOrDefault(x => x.PilotId == (int)input.First()));

            var result = controller.Edit(pilots[3]) as RedirectToRouteResult;

            mockContext.Verify(x => x.SaveChanges(), Times.Once);
            //Assert.AreEqual(EntityState.Modified, mockContext.Object.Entry(GetPilots()[3]).State);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
        }

        [TestMethod()]
        public void DeleteTest_ActionExecutes_ReturnsViewForDelete()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            PilotsController controller = new PilotsController(mockContext.Object);

            // Create a mock DbSet
            List<Pilot> pilots = GetPilots();
            var mockSet = CreateMockSet(pilots);
            // Set up the Pilots property so it returns the mocked DbSet
            mockContext.Setup(o => o.Pilots).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockSet.Setup(set => set.Find(It.IsAny<object[]>())).Returns((object[] input) => pilots.SingleOrDefault(x => x.PilotId == (int)input.First()));

            var result = controller.Delete(3) as ViewResult;
            Assert.AreEqual("Delete", result.ViewName); ;
        }

        [TestMethod()]
        public void DeleteTest_ViewData()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            PilotsController controller = new PilotsController(mockContext.Object);

            // Create a mock DbSet
            List<Pilot> pilots = GetPilots();
            var mockSet = CreateMockSet(pilots);
            // Set up the Pilots property so it returns the mocked DbSet
            mockContext.Setup(o => o.Pilots).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockSet.Setup(set => set.Find(It.IsAny<object[]>())).Returns((object[] input) => pilots.SingleOrDefault(x => x.PilotId == (int)input.First()));

            var result = controller.Delete(3) as ViewResult;
            Pilot pilot = (Pilot)result.Model;
            Assert.AreEqual(true, PilotEquals(GetPilots().FirstOrDefault(p => p.PilotId == 3), pilot));
        }

        [TestMethod()]
        public void DeleteConfirmedTest_ActionExecutes_ReturnsViewForDelete()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            PilotsController controller = new PilotsController(mockContext.Object);

            // Create a mock DbSet
            List<Pilot> pilots = GetPilots();
            var mockSet = CreateMockSet(pilots);
            // Set up the Pilots property so it returns the mocked DbSet
            mockContext.Setup(o => o.Pilots).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockContext.Setup(c => c.Pilots.Find(It.IsAny<object[]>())).Returns((object[] input) => pilots.SingleOrDefault(x => x.PilotId == (int)input.First()));
            // Set up the Remove method for the mocked context
            mockContext.Setup(m => m.Pilots.Remove(It.IsAny<Pilot>())).Callback<Pilot>((entity) => pilots.Remove(entity));

            var result = controller.DeleteConfirmed(3) as RedirectToRouteResult;

            mockContext.Verify(x => x.Pilots.Find(3), Times.Once);
            mockContext.Verify(x => x.Pilots.Remove(It.IsAny<Pilot>()), Times.Once);
            mockContext.Verify(x => x.SaveChanges(), Times.Once);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
        }

        [TestMethod()]
        public void DeleteConfirmedTest_EntityRemovedFromContext()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            PilotsController controller = new PilotsController(mockContext.Object);

            // Create a mock DbSet
            List<Pilot> pilots = GetPilots();
            var mockSet = CreateMockSet(pilots);
            // Set up the Pilots property so it returns the mocked DbSet
            mockContext.Setup(o => o.Pilots).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockContext.Setup(c => c.Pilots.Find(It.IsAny<object[]>())).Returns((object[] input) => pilots.SingleOrDefault(x => x.PilotId == (int)input.First()));
            // Set up the Remove method for the mocked context
            mockContext.Setup(m => m.Pilots.Remove(It.IsAny<Pilot>())).Callback<Pilot>((entity) => pilots.Remove(entity));

            var result = controller.DeleteConfirmed(3) as RedirectToRouteResult;

            mockContext.Verify(x => x.Pilots.Find(3), Times.Once);
            mockContext.Verify(x => x.Pilots.Remove(It.IsAny<Pilot>()), Times.Once);
            mockContext.Verify(x => x.SaveChanges(), Times.Once);
            Assert.AreEqual(9, pilots.Count);
            Assert.IsFalse(pilots.Any(p => p.PilotId == 3));
        }

        [TestMethod()]
        public void DroneFlights_ActionExecutes_ReturnsViewForDroneFlights()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            PilotsController controller = new PilotsController(mockContext.Object);

            // Create a mock DbSet
            List<Pilot> pilots = GetPilots();
            var mockSet = CreateMockSet(pilots);
            // Set up the Pilots property so it returns the mocked DbSet
            mockContext.Setup(o => o.Pilots).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockSet.Setup(set => set.Find(It.IsAny<object[]>())).Returns((object[] input) => pilots.SingleOrDefault(x => x.PilotId == (int)input.First()));

            var result = controller.DroneFlights(3) as ViewResult;
            Assert.AreEqual("DroneFlights", result.ViewName);
        }

        [TestMethod()]
        public void DroneFlights_ViewData()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            PilotsController controller = new PilotsController(mockContext.Object);

            // Create a mock DbSet
            List<Pilot> pilots = GetPilots();
            var mockSet = CreateMockSet(pilots);
            // Set up the Pilots property so it returns the mocked DbSet
            mockContext.Setup(o => o.Pilots).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockSet.Setup(set => set.Find(It.IsAny<object[]>())).Returns((object[] input) => pilots.SingleOrDefault(x => x.PilotId == (int)input.First()));

            var result = controller.DroneFlights(3) as ViewResult;
            List<DroneFlight> flights = (List<DroneFlight>)result.Model;
            List<DroneFlight> generated = pilots[2].DroneFlights.ToList();

            for (int i=0; i<flights.Count; i++)
            {                
                Assert.AreEqual(true, DroneFlightEquals(generated[i], flights[i]));
            }
        }

        private List<Pilot> GetPilots()
        {
            Project project = new Project
            {
                ProjectId = 1,
                ProjectCode = "P1"
            };

            Drone drone = new Drone
            {
                DroneId = 1,
                DroneType = "droneType",
                Registration = "registration"
            };

            List<Pilot> pilots = new List<Pilot>();
            for (int i=1; i<=10; i++)
            {
                Pilot pilot = new Pilot
                {
                    PilotId = i,
                    PilotName = "Pilot" + i,
                    Country = "België",
                    City = "Gent",
                    Street = "Straat",
                    ZIP = 9000,
                    Phone = "0474000000",
                    LicenseNo = i,
                    Email = "pilot@jdn.com"
                };

                for (int j=1; j<=10; j++)
                {
                    DroneFlight flight = new DroneFlight
                    {
                        FlightId = j,
                        Project = project,
                        ProjectId = 1,
                        Drone = drone,
                        DroneId = 1,
                        Pilot = pilot,
                        PilotId = i,
                        Date = DateTime.Now
                    };

                    pilot.DroneFlights.Add(flight);
                }

                pilots.Add(pilot);
            }
            return pilots;
        }

        private bool PilotEquals(Pilot p1, Pilot p2)
        {
            if (p1.PilotId != p2.PilotId)
            {
                return false;
            }
            if (!string.Equals(p1.PilotName, p2.PilotName))
            {
                return false;
            }
            if (!string.Equals(p1.Country, p2.Country))
            {
                return false;
            }
            if (!string.Equals(p1.City, p2.City))
            {
                return false;
            }
            if (!string.Equals(p1.Street, p2.Street))
            {
                return false;
            }
            if (p1.ZIP != p2.ZIP)
            {
                return false;
            }
            if (!string.Equals(p1.Phone, p2.Phone))
            {
                return false;
            }
            if (p1.LicenseNo != p2.LicenseNo)
            {
                return false;
            }
            if (!string.Equals(p1.Email, p2.Email))
            {
                return false;
            }
            if (!string.Equals(p1.EmergencyPhone, p2.EmergencyPhone))
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
    }
}