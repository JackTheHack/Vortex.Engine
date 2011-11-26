using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace vortexWin.Engine.Input
{
    public class PolarAngleController
    {
        public static double GetAngle(Vector2 mousecenter)
        {
            if(GamePadController.IsConnected)
            {
                Vector2 thumbstick = GamePadController.Thumbstick;
                if (thumbstick.Length() > 0.6f)
                {
                    double newugol = Math.Atan2(
                    -thumbstick.Y,
                    thumbstick.X);
                    return newugol;
                }
                else
                {
                    return double.NaN;
                }
            }else{
                MouseInfo mouse = MouseController.GetState();
                double newugol = Math.Atan2(
                    mouse.Y-mousecenter.Y,
                    mouse.X-mousecenter.X
                    );
                return newugol;
            }
        }
    }
}
