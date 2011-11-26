using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace vortexWin.Engine
{
    public class Animation:AnimatedSprite
    {
        int frameTime=1000;
        int lastTime = -1;
        bool paused = true;

        public int FrameCount = 0;

        public Animation() : base() { Start(); }

        public Animation(bool start):base()
        {
            Paused = start;
        }

        public bool Paused
        {
            get { return paused; }
            set {
                if (value) Stop();
                else Start();
                }
        }


        public int FrameTime
        {
            get { return frameTime; }
            set { frameTime = value; }
        }

        public void Start()
        {
            lastTime = -1;
            paused = false;
        }

        public void Stop()
        {
            paused = true;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (lastTime == -1) lastTime = (int)gameTime.TotalGameTime.TotalMilliseconds;
            if (!paused)
                if ((gameTime.TotalGameTime.TotalMilliseconds - lastTime) > frameTime)
                {
                    CurrentSprite = CurrentSprite + 1;
                    if (CurrentSprite > FrameCount && FrameCount > 0)
                    {
                        Visible = false;
                        Stop();
                    }
                    lastTime = (int)gameTime.TotalGameTime.TotalMilliseconds;
                }
            
            base.Update(gameTime);
        }

    }
}
