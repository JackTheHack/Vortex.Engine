using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace vortexWin.Helpers
{
    public class VectorMath
    {
        private static float d(Vector2 u,Vector2 v)
        {
            Vector2 vect = u - v;
            return vect.Length();
        }

        public static bool IsProjective(Vector2 point, Vector2 p0, Vector2 p1)
        {
            Vector2 p1p0 = new Vector2(p1.X -p0.X, p1.Y - p0.Y);
            Vector2 p0p1 = Vector2.Negate(p1p0);
            Vector2 p1R = new Vector2(p1.X - point.X, p1.Y - point.Y);
            Vector2 p0R = new Vector2(p0.X - point.X, p0.Y - point.Y);

            double A = p1p0.LengthSquared();
            double B = p1R.LengthSquared();
            double C = p0R.LengthSquared();

            if (B > (A + C)) return false;
            if (C > (A + B)) return false;
            return true;
        }

        /// <summary>
        /// Расстояние до отрезка
        /// </summary>
        /// <param name="point">Точка</param>
        /// <param name="p0">Начало сегмента</param>
        /// <param name="p1">Конец сегмента</param>
        /// <returns></returns>
        public static double DistToSegment(Vector2 point,Vector2 p0,Vector2 p1)
        {
            
            Vector2 v = new Vector2(p1.X - p0.X, p1.Y - p0.Y);
            Vector2 w = new Vector2(point.X - p0.X, point.Y - p0.Y);

            float c1=Vector2.Dot(w,v);
            float c2 = Vector2.Dot(v, v);
            if (c1 <= 0)
                return d(point, p0);
            if (c2 <= c1)
                return d(point, p1);
            float b = c1 / c2;
            v=Vector2.Multiply(v,b);
            Vector2 Pb = new Vector2(p0.X + v.X, p0.Y + v.Y);
            return d(point, Pb);
        }

        public static double AngleDiff(double newugol, double ugolval)
        {
            double val1 = newugol-ugolval;
            while (Math.Abs(val1) > Math.PI)
                val1 -= Math.Sign(val1) * Math.PI * 2;
            return val1;
        }



    }
}
