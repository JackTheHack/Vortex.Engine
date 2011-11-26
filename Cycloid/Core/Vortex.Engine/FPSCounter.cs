using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace vortexWin.Engine
{
    public class FrameRateCounter
    {
        static int frameRate = 0;

        public static int FrameRate
        {
            get { return frameRate; }
        }

        static int frameCounter = 0;
        static TimeSpan elapsedTime = TimeSpan.Zero;


        static FrameRateCounter()
        {
        }


        public static void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
        }


        public static void OnDraw()
        {
            frameCounter++;

        }
    }
}
