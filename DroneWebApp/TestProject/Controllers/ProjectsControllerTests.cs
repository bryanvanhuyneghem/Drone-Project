using Microsoft.VisualStudio.TestTools.UnitTesting;
using DroneWebApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DroneWebApp.Models;
using System.Data.Entity;
using Moq;
using System.Web.Mvc;

namespace DroneWebApp.Controllers.Tests
{
    [TestClass()]
    public class ProjectsControllerTests
    {
        [TestMethod()]
        public void Index_ActionExecutes_ReturnsViewForIndex()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            ProjectsController controller = new ProjectsController(mockContext.Object);

            // Create a mock DbSet
            List<Project> projects = GetProjects();
            var mockSet = CreateMockSet(projects);
            // Set up the Projects property so it returns the mocked DbSet
            mockContext.Setup(o => o.Projects).Returns(() => mockSet.Object);

            var result = controller.Index() as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod()]
        public void IndexTest_ViewData()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            ProjectsController controller = new ProjectsController(mockContext.Object);

            // Create a mock DbSet
            List<Project> projects = GetProjects();
            var mockSet = CreateMockSet(projects);
            // Set up the Projects property so it returns the mocked DbSet
            mockContext.Setup(o => o.Projects).Returns(() => mockSet.Object);

            var result = controller.Index() as ViewResult;
            var resultModel = result.ViewData.Model;
            List<Project> projectsResult = (List<Project>)resultModel;

            for (int i = 0; i < projectsResult.Count; i++)
            {
                Assert.AreEqual(true, ProjectEquals(projects[i], projectsResult[i]));
            }
        }

        [TestMethod()]
        public void DetailsTest_ActionExecutes_ReturnsViewForDetails()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            ProjectsController controller = new ProjectsController(mockContext.Object);

            // Create a mock DbSet
            List<Project> projects = GetProjects();
            var mockSet = CreateMockSet(projects);
            // Set up the Projects property so it returns the mocked DbSet
            mockContext.Setup(o => o.Projects).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockSet.Setup(set => set.Find(It.IsAny<object[]>())).Returns((object[] input) => projects.SingleOrDefault(x => x.ProjectId == (int)input.First()));

            var result = controller.Details(3) as ViewResult;
            Assert.AreEqual("Details", result.ViewName);
        }

        [TestMethod()]
        public void DetailsTest_ViewData()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            ProjectsController controller = new ProjectsController(mockContext.Object);

            // Create a mock DbSet
            List<Project> projects = GetProjects();
            var mockSet = CreateMockSet(projects);
            // Set up the Projects property so it returns the mocked DbSet
            mockContext.Setup(o => o.Projects).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockSet.Setup(set => set.Find(It.IsAny<object[]>())).Returns((object[] input) => projects.SingleOrDefault(x => x.ProjectId == (int)input.First()));

            var result = controller.Details(3) as ViewResult;
            Project project = (Project)result.Model;
            Assert.AreEqual(true, ProjectEquals(projects.FirstOrDefault(p => p.ProjectId == 3), project));
        }

        [TestMethod()]
        public void CreateTest1_ActionExecutes_ReturnsViewForCreate()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            ProjectsController controller = new ProjectsController(mockContext.Object);

            // Create a mock DbSet
            List<Project> projects = GetProjects();
            var mockSet = CreateMockSet(projects);
            // Set up the Projects property so it returns the mocked DbSet
            mockContext.Setup(o => o.Projects).Returns(() => mockSet.Object);

            var result = controller.Create() as ViewResult;
            Assert.AreEqual("Create", result.ViewName);
        }

        [TestMethod()]
        public void CreateTest2_ActionExecutes_ReturnsViewForCreate()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            ProjectsController controller = new ProjectsController(mockContext.Object);

            // Create a mock DbSet
            List<Project> projects = GetProjects();
            var mockSet = CreateMockSet(projects);
            // Set up the Pilots property so it returns the mocked DbSet
            mockContext.Setup(o => o.Projects).Returns(() => mockSet.Object);
            // Set up the Add method for the mockdd DbSet
            mockSet.Setup(x => x.Add(It.IsAny<Project>())).Callback<Project>(p => projects.Add(p));

            Project project = new Project
            {
                ProjectId = 11,
                ProjectCode = "Project11"
            };

            var result = controller.Create(project) as RedirectToRouteResult;

            mockSet.Verify(x => x.Add(It.IsAny<Project>()), Times.Once);
            mockContext.Verify(x => x.SaveChanges(), Times.Once);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
        }

        [TestMethod()]
        public void CreateTest2_EntityAddedToContext()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            ProjectsController controller = new ProjectsController(mockContext.Object);

            // Create a mock DbSet
            List<Project> projects = GetProjects();
            var mockSet = CreateMockSet(projects);
            // Set up the Projects property so it returns the mocked DbSet
            mockContext.Setup(o => o.Projects).Returns(() => mockSet.Object);
            // Set up the Add method for the mockdd DbSet
            mockSet.Setup(x => x.Add(It.IsAny<Project>())).Callback<Project>(p => projects.Add(p));

            Project project = new Project
            {
                ProjectId = 11,
                ProjectCode = "Project11"
            };

            var result = controller.Create(project) as RedirectToRouteResult;

            mockSet.Verify(x => x.Add(It.IsAny<Project>()), Times.Once);
            mockContext.Verify(x => x.SaveChanges(), Times.Once);
            Assert.AreEqual(11, projects.Count);
            Assert.IsTrue(projects.Any(p => p.ProjectId == 11));
        }

        [TestMethod()]
        public void EditTest1_ActionExecutes_ReturnsViewForEdit()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            ProjectsController controller = new ProjectsController(mockContext.Object);

            // Create a mock DbSet
            List<Project> projects = GetProjects();
            var mockSet = CreateMockSet(projects);
            // Set up the Projects property so it returns the mocked DbSet
            mockContext.Setup(o => o.Projects).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockSet.Setup(set => set.Find(It.IsAny<object[]>())).Returns((object[] input) => projects.SingleOrDefault(x => x.ProjectId == (int)input.First()));

            var result = controller.Edit(3) as ViewResult;
            Assert.AreEqual("Edit", result.ViewName);
        }

        [TestMethod()]
        public void EditTest1_ViewData()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            ProjectsController controller = new ProjectsController(mockContext.Object);

            // Create a mock DbSet
            List<Project> projects = GetProjects();
            var mockSet = CreateMockSet(projects);
            // Set up the Projects property so it returns the mocked DbSet
            mockContext.Setup(o => o.Projects).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockSet.Setup(set => set.Find(It.IsAny<object[]>())).Returns((object[] input) => projects.SingleOrDefault(x => x.ProjectId == (int)input.First()));

            var result = controller.Edit(3) as ViewResult;
            Project project = (Project)result.Model;
            Assert.AreEqual(true, ProjectEquals(projects.FirstOrDefault(p => p.ProjectId == 3), project));
        }

        [TestMethod()]
        public void EditTest2_ActionExecutes_ReturnsViewForEdit()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            ProjectsController controller = new ProjectsController(mockContext.Object);

            // Create a mock DbSet
            List<Project> projects = GetProjects();
            var mockSet = CreateMockSet(projects);
            // Set up the Projects property so it returns the mocked DbSet
            mockContext.Setup(o => o.Projects).Returns(() => mockSet.Object);

            var result = controller.Edit(projects[3]) as RedirectToRouteResult;

            mockContext.Verify(x => x.SaveChanges(), Times.Once);
            //Assert.AreEqual(EntityState.Modified, mockContext.Object.Entry(projects[3]).State);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
        }

        [TestMethod()]
        public void DeleteTest_ActionExecutes_ReturnsViewForDelete()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            ProjectsController controller = new ProjectsController(mockContext.Object);

            // Create a mock DbSet
            List<Project> projects = GetProjects();
            var mockSet = CreateMockSet(projects);
            // Set up the Projects property so it returns the mocked DbSet
            mockContext.Setup(o => o.Projects).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockSet.Setup(set => set.Find(It.IsAny<object[]>())).Returns((object[] input) => projects.SingleOrDefault(x => x.ProjectId == (int)input.First()));

            var result = controller.Delete(3) as ViewResult;
            Assert.AreEqual("Delete", result.ViewName); ;
        }

        [TestMethod()]
        public void DeleteTest_ViewData()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            ProjectsController controller = new ProjectsController(mockContext.Object);

            // Create a mock DbSet
            List<Project> projects = GetProjects();
            var mockSet = CreateMockSet(projects);
            // Set up the Projects property so it returns the mocked DbSet
            mockContext.Setup(o => o.Projects).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockSet.Setup(set => set.Find(It.IsAny<object[]>())).Returns((object[] input) => projects.SingleOrDefault(x => x.ProjectId == (int)input.First()));

            var result = controller.Delete(3) as ViewResult;
            Project project = (Project)result.Model;
            Assert.AreEqual(true, ProjectEquals(projects.FirstOrDefault(p => p.ProjectId == 3), project));
        }

        [TestMethod()]
        public void DeleteConfirmedTest_ActionExecutes_ReturnsViewForDelete()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            ProjectsController controller = new ProjectsController(mockContext.Object);

            // Create a mock DbSet
            List<Project> projects = GetProjects();
            List<Drone> drones = new List<Drone>();
            drones.Add(projects[0].DroneFlights.ToList()[0].Drone);
            var mockSetDrones = CreateMockSet(drones);
            var mockSetProjects = CreateMockSet(projects);
            // Set up the Projects property so it returns the mocked DbSet
            mockContext.Setup(o => o.Projects).Returns(() => mockSetProjects.Object);
            // Set up the Find method for the mocked DbSet
            mockContext.Setup(c => c.Projects.Find(It.IsAny<object[]>())).Returns((object[] input) => projects.SingleOrDefault(x => x.ProjectId == (int)input.First()));
            // Set up the Remove method for the mocked context
            mockContext.Setup(m => m.Projects.Remove(It.IsAny<Project>())).Callback<Project>((entity) => projects.Remove(entity));
            // Set up Drones property so it return all drones
            mockContext.Setup(c => c.Drones).Returns(() => mockSetDrones.Object);

            var result = controller.DeleteConfirmed(3) as RedirectToRouteResult;

            mockContext.Verify(x => x.Projects.Find(3), Times.Once);
            mockContext.Verify(x => x.Projects.Remove(It.IsAny<Project>()), Times.Once);
            mockContext.Verify(x => x.SaveChanges(), Times.Exactly(3)); // SaveChanges is called once in the controller method and 2 times in the Helper class
            Assert.AreEqual("Index", result.RouteValues["Action"]);
        }

        [TestMethod()]
        public void DeleteConfirmedTest_EntityRemovedFromContext()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            ProjectsController controller = new ProjectsController(mockContext.Object);

            // Create a mock DbSet
            List<Project> projects = GetProjects();
            List<Drone> drones = new List<Drone>();
            drones.Add(projects[0].DroneFlights.ToList()[0].Drone);
            var mockSetDrones = CreateMockSet(drones);
            var mockSet = CreateMockSet(projects);
            // Set up the Projects property so it returns the mocked DbSet
            mockContext.Setup(o => o.Projects).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockContext.Setup(c => c.Projects.Find(It.IsAny<object[]>())).Returns((object[] input) => projects.SingleOrDefault(x => x.ProjectId == (int)input.First()));
            // Set up the Remove method for the mocked context
            mockContext.Setup(m => m.Projects.Remove(It.IsAny<Project>())).Callback<Project>((entity) => projects.Remove(entity));
            // Set up Drones property so it return all drones
            mockContext.Setup(c => c.Drones).Returns(() => mockSetDrones.Object);

            var result = controller.DeleteConfirmed(3) as RedirectToRouteResult;

            mockContext.Verify(x => x.Projects.Find(3), Times.Once);
            mockContext.Verify(x => x.Projects.Remove(It.IsAny<Project>()), Times.Once);
            mockContext.Verify(x => x.SaveChanges(), Times.Exactly(3)); // SaveChanges is called once in the controller method and 2 times in the Helper class
            Assert.AreEqual(9, projects.Count);
            Assert.IsFalse(projects.Any(p => p.ProjectId == 3));
        }

        [TestMethod()]
        public void DroneFlights_ActionExecutes_ReturnsViewForDroneFlights()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            ProjectsController controller = new ProjectsController(mockContext.Object);

            // Create a mock DbSet
            List<Project> projects = GetProjects();
            var mockSet = CreateMockSet(projects);
            // Set up the Projects property so it returns the mocked DbSet
            mockContext.Setup(o => o.Projects).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockSet.Setup(set => set.Find(It.IsAny<object[]>())).Returns((object[] input) => projects.SingleOrDefault(x => x.ProjectId == (int)input.First()));

            var result = controller.DroneFlights(3) as ViewResult;
            Assert.AreEqual("DroneFlights", result.ViewName);
        }

        [TestMethod()]
        public void DroneFlights_ViewData()
        {
            // Create mock context
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            ProjectsController controller = new ProjectsController(mockContext.Object);

            // Create a mock DbSet
            List<Project> projects = GetProjects();
            var mockSet = CreateMockSet(projects);
            // Set up the Projects property so it returns the mocked DbSet
            mockContext.Setup(o => o.Projects).Returns(() => mockSet.Object);
            // Set up the Find method for the mocked DbSet
            mockSet.Setup(set => set.Find(It.IsAny<object[]>())).Returns((object[] input) => projects.SingleOrDefault(x => x.ProjectId == (int)input.First()));

            var result = controller.DroneFlights(3) as ViewResult;
            List<DroneFlight> flights = (List<DroneFlight>)result.Model;
            List<DroneFlight> generated = projects[2].DroneFlights.ToList();

            for (int i = 0; i < flights.Count; i++)
            {
                Assert.AreEqual(true, DroneFlightEquals(generated[i], flights[i]));
            }
        }

        private List<Project> GetProjects()
        {
            List<Project> projects = new List<Project>();

            Pilot pilot = new Pilot
            {
                PilotId = 1,
                PilotName = "Pilot"
            };

            Drone drone = new Drone
            {
                DroneId = 1,
                DroneType = "type",
                Registration = "registration",
            };

            for (int i = 1; i <= 10; i++)
            {
                Project project = new Project
                {
                    ProjectId = i,
                    ProjectCode = "Project" + i,
                    SiteRefCode = "refCode",
                    VerticalRef = i
                };

                for (int j = 1; j <= 10; j++)
                {
                    DroneFlight flight = new DroneFlight
                    {
                        FlightId = j,
                        PilotId = 1,
                        Pilot = pilot,
                        ProjectId = i,
                        Project = project,
                        DroneId = 1,
                        Drone = drone,
                        Date = DateTime.Now
                    };

                    project.DroneFlights.Add(flight);
                }

                projects.Add(project);
            }
            return projects;
        }

        private bool ProjectEquals(Project p1, Project p2)
        {
            if (p1.ProjectId != p2.ProjectId)
            {
                return false;
            }
            if (!string.Equals(p1.ProjectCode, p2.ProjectCode))
            {
                return false;
            }
            if (!string.Equals(p1.SiteRefCode, p2.SiteRefCode))
            {
                return false;
            }
            if (p1.VerticalRef != p2.VerticalRef)
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