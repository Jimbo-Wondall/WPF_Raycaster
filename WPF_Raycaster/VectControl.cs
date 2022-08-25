using System;
using System.Windows;

namespace WPF_Raycaster
{
    public static class VectControl
    {
        public static Vector Rotate(Vector vect, double angle)
        {
            vect = new Vector
            (
                (vect.X * Math.Cos(angle)) - vect.Y * Math.Sin(angle),
                (vect.X * Math.Sin(angle)) + vect.Y * Math.Cos(angle)
            );
            return vect;
        }
        public static Vector Rotate(double x, double y, double angle)
        {
            Vector vect = new Vector
            (
                (x * Math.Cos(angle)) - y * Math.Sin(angle),
                (x * Math.Sin(angle)) + y * Math.Cos(angle)
            );
            return vect;
        }
        public static Vector FromRad(double radians)
        {
            Vector vect = new Vector((float)Math.Cos(radians), (float)Math.Sin(radians));
            return vect;
        }
        public static Vector FromDeg(double degrees)
        {
            Vector vect = FromRad(degrees * (Math.PI / 180));
            return vect;
        }
        public static double GetAngle(Vector vect)
        {
            double angle = Math.Atan2(vect.Y, vect.X); ;
            return angle;
        }
    }
}
