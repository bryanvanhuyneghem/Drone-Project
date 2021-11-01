using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneWebApp.Models.PointcloudControlTool
{
    public class Vector
    {
        PointCloudXYZ p1; // vector begin point
        PointCloudXYZ p2; // vector end point

        public double X { get { return (double)p2.X - (double)p1.X; } } // vector x axis projection value
        public double Y { get { return (double)p2.Y - (double)p1.Y; } } // vector y axis projection value
        public double Z { get { return (double)p2.Z - (double)p1.Z; } } // vector z axis projection value

        public Vector(PointCloudXYZ p1, PointCloudXYZ p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }

        // vector cross product
        public static Vector operator *(Vector u, Vector v)
        {
            double x = u.Y * v.Z - u.Z * v.Y;
            double y = u.Z * v.X - u.X * v.Z;
            double z = u.X * v.Y - u.Y * v.X;

            PointCloudXYZ point1 = v.p1;
            PointCloudXYZ point2 = new PointCloudXYZ
            {
                X = point1.X + x,
                Y = point1.Y + y,
                Z = point1.Z + z
            };
                
            return new Vector(point1, point2);
        }
    }
}