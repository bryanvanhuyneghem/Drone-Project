using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneWebApp.Models.PointcloudControlTool
{
    public class Face
    {
        // a face is a flat surface that forms part of the boundary of an object

        public List<PointCloudXYZ> V { get; set; } // vertices in one face of the 3D polygon
        public List<int> Index { get; set; } // vertices index
        public int N { get { return V.Count; } } // number of vertices

        public Face(List<PointCloudXYZ> p, List<int> index)
        {
            V = new List<PointCloudXYZ>();
            Index = new List<int>();

            for (int i=0; i<p.Count; i++)
            {
                V.Add(p[i]);
                Index.Add(index[i]);
            }
        }
    }
}