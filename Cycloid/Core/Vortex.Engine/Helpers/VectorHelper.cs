using System;
using Microsoft.Xna.Framework;

namespace vortexWin
{
    public class VectorHelper
    {
        public static double GetAngle(Vector2 v1, Vector2 v2)
        {
            double cosa = 
                (v1.X * v2.X + v1.Y * v2.Y) / 
                (v1.Length() * v2.Length());
            if (cosa > 1)
                cosa = 1;
            if (cosa < -1)
                cosa = -1;
            double alpha = Math.Acos(cosa);
            return alpha;
        }

        public static Vector2 GetNormal(Vector2 src)
        {
            Vector2 result = new Vector2();
            result.Y = src.X / src.Length();
            result.X = -src.Y /src.Length();
            return result;
        }

        public static Vector2 RotateVector(Vector2 src,double alpha)
        {
            Vector3 temp= new Vector3(src,0);
            Matrix rotationMatrix = Matrix.CreateRotationZ((float)alpha);
            temp = Vector3.Transform(temp, rotationMatrix);
            return new Vector2(temp.X,temp.Y);
        }

        public static int ComparePointPosition(Vector2 vector,Vector2 point)
        {
            float A = vector.X;
            float B = -vector.Y;

            float position = A * point.X + B * point.Y;
            if (position != 0)
                return (int)(position / position);
            else return 0;
        }

        

    }
}
