using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneWebApp.Models.PointcloudControlTool
{
    public class Polygon
    {
        public List<PointCloudXYZ> V { get; set; } // vertices of the 3D polygon
        public List<int> Index { get; set; } // vertices index
        public int N { get { return V.Count; } } // number of vertices

        public Polygon(List<PointCloudXYZ> p)
        {
            V = new List<PointCloudXYZ>();
            Index = new List<int>();

            for (int i=0; i<p.Count; i++)
            {
                V.Add(p[i]);
                Index.Add(i);
            }
        }
    }
}