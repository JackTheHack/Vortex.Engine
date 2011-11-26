using System;
using Microsoft.Xna.Framework;

namespace vortexWin.Helpers
{
    public class RandomHelper
    {
        public static Random rand = new Random((int)DateTime.Now.Ticks);

        public static double NextDouble()
        {
            return rand.NextDouble();
        }

        public static int Next()
        {
            return rand.Next();
        }

        public static int Next(int min,int max)
        {
            return rand.Next(min,max);
        }

        public static Vector2 RandomSpeedVector(float speed)
        {
            Vector2 speedVector = new Vector2(
                (float)(RandomHelper.NextDouble() * 2 - 1),
                (float)(RandomHelper.NextDouble() * 2 - 1));
            speedVector.Normalize();
            speedVector = Vector2.Multiply(speedVector, speed);
            return speedVector;
        }
    }
}
