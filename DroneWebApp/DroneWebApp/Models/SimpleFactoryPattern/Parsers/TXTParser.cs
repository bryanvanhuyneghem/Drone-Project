using DroneWebApp.Models.SimpleFactoryPattern.Parsers;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace DroneWebApp.Models.SimpleFactoryPattern.Parsers
{
    public class TXTParser : IParser
    {
        public bool Parse(string path, int flightId, DroneDBEntities db)
        {
            return true;
        }
    }
}