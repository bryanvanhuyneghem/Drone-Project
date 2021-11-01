using Microsoft.VisualStudio.TestTools.UnitTesting;
using DroneWebApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DroneWebApp.Models;
using Moq;
using System.Web.Mvc;
using System.Data.Entity;

namespace DroneWebApp.Controllers.Tests
{
    [TestClass()]
    public class MapControllerTests
    {
        [TestMethod()]
        public void ViewMapTest_ActionExecutes_ReturnsViewForViewMap()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            MapController controller = new MapController(mockContext.Object);

            // Create a mock DbSet
            List<DroneFlight> flights = GetFlights();
            var mockSet = CreateMockSet(flights);
            // Set up the DroneFlights property so it returns the mocked DbSet
            mockContext.Setup(o => o.DroneFlights).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockSet.Setup(s => s.Find(It.IsAny<object[]>())).Returns((object[] input) => flights.SingleOrDefault(x => x.FlightId == (int)input.First()));

            var result = controller.ViewMap(3) as ViewResult;
            Assert.AreEqual("ViewMap", result.ViewName);
        }

        [TestMethod()]
        public void ViewMapTest_ActionExecutes_ReturnsViewForViewMapWithNullId()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            MapController controller = new MapController(mockContext.Object);

            // Create a mock DbSet
            List<DroneFlight> flights = GetFlights();
            var mockSet = CreateMockSet(flights);
            // Set up the DroneFlights property so it returns the mocked DbSet
            mockContext.Setup(o => o.DroneFlights).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockSet.Setup(s => s.Find(It.IsAny<object[]>())).Returns((object[] input) => flights.SingleOrDefault(x => x.FlightId == (int)input.First()));

            var result = controller.ViewMap(null) as ViewResult;
            Assert.AreEqual("ViewMap", result.ViewName);
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
                    hasDroneLog = true,
                    QualityReport = new QualityReport { QualityReportId = i },
                    TFW = new TFW { TFWId = i }
                };

                List<CTRLPoint> ctrlPoints = new List<CTRLPoint>();
                List<GroundControlPoint> gcps = new List<GroundControlPoint>();
                List<RawImage> images = new List<RawImage>();

                for (int j = 1; j <= 5; j++)
                {
                    CTRLPoint ctrl = new CTRLPoint { FlightId = i, DroneFlight = flight, CTRLId = j, CTRLName = "ctrl-" + j, X = 1, Y = 2, Z = 3 };
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
    }
}