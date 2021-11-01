using DroneWebApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System.Web.Helpers;

[assembly: OwinStartupAttribute(typeof(DroneWebApp.Startup))]
namespace DroneWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app); // Configuration method from App_Start.Startup.Auth
            CreateRolesandUsers();
            AntiForgeryConfig.SuppressXFrameOptionsHeader = true; // When false this gives a error with the download of the logbooks
        }

        // In this method we will create the User and Admin role for login    
        private void CreateRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
    
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin rool    
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                // Here we create a Admin super user who will maintain the website                   
                var user = new ApplicationUser();
                user.UserName = "admin";
                user.Email = "admin@jdn.com";

                string userPWD = "password";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin    
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
            }

            // Creating User role     
            if (!roleManager.RoleExists("User"))
            {
                var role = new IdentityRole();
                role.Name = "User";
                roleManager.Create(role);

            }
        }
    }
}
