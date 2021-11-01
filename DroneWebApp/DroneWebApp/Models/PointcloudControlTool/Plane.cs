using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneWebApp.Models.PointcloudControlTool
{
    public class Plane
    {
        // plane equation: a*x + b*y + c*z + d = 0

        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }
        public double D { get; set; }

        public Plane(double a, double b, double c, double d)
        {
            A = a;
            B = b;
            C = c;
            D = d;
        }

        public Plane(PointCloudXYZ p1, PointCloudXYZ p2, PointCloudXYZ p3)
        {
            Vector v = new Vector(p1, p2);
            Vector u = new Vector(p1, p3);
            Vector n = u * v; // normal vector

            A = n.X;
            B = n.Y;
            C = n.Z;
            D = -(A * (double)p1.X + B * (double)p1.Y + C * (double)p1.Z);
        }

        public static Plane operator -(Plane plane)
        {
            return new Plane(-plane.A, -plane.B, -plane.C, -plane.D);
        }

        public static double operator *(PointCloudXYZ p, Plane plane)
        {
            return ((double)p.X * plane.A + (double)p.Y * plane.B + (double)p.Z * plane.C + plane.D);
        }
    }
}