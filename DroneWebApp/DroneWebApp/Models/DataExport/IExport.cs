using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DroneWebApp.Models.DataExport
{
    interface IExport
    {
        void CreatePilotLog(int PilotId, DroneDBEntities db, HttpContextBase context);
        void CreateDroneLog(int DroneId, DroneDBEntities db, HttpContextBase context);
    }
}
