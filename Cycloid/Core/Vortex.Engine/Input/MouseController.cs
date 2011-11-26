#define XBOX

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

#if WINDOWS_VER
using MultiInput;
#endif

namespace vortexWin.Engine.Input
{
    public enum MouseButton{Left,Right,Middle,Other,None}

    public class MouseInfo
    {
        public int X;
        public int Y;
        public bool Pressed;
        public MouseButton Button=MouseButton.None;
        public int Scroll;

        #if WINDOWS_VER
        public MouseInfo(RawMouse mouse)
        {
            this.X = mouse.X;
            this.Y = mouse.Y;
        }
        #endif

        #if XBOX
        public MouseInfo(MouseState state)
        {
            this.X = state.X;
            this.Y = state.Y;
            bool leftpressed=state.LeftButton==ButtonState.Pressed;
            bool rightpressed=state.RightButton==ButtonState.Pressed;
            bool middlepressed=state.MiddleButton==ButtonState.Pressed;
            this.Pressed = (leftpressed || rightpressed || middlepressed);
            this.Button = MouseButton.None;
            if (leftpressed)
                this.Button = MouseButton.Left;
            if (rightpressed)
                this.Button = MouseButton.Right;
            if (middlepressed)
                this.Button = MouseButton.Middle;
            this.Scroll = state.ScrollWheelValue;
            
        }
        #endif

    }

    public class MouseNotExists : Exception
    {
        public MouseNotExists()
            :base("Mice index out of bounds.Mouse is not installed or plugged in"){}
    }

    public class MouseController
    {

        #if WINDOWS_VER
        static MultiInput.MultiInputController multimouse;
        #endif

        public static bool MultiInput
        {
            get { 
                #if WINDOWS_VER
                return true;
                #endif

                #if XBOX
                return false;
                #endif
            }
        }

        public static int MouseCount
        {
            get
            {
                #if WINDOWS_VER
                return multimouse.MiceCount;
                #endif

                #if XBOX
                return 1;
                #endif
            }
        }

        public static void Init(GameInstance game)
        {                
            #if WINDOWS_VER
            multimouse = new MultiInput.MultiInputController(game.Window.Handle);
            #endif
        }

        public static MouseInfo GetState()
        {
            #if XBOX
            return new MouseInfo(Mouse.GetState());
            #endif

            #if WINDOWS_VER
            return new MouseInfo(multimouse.Mice[0]);
            #endif
        }



        public static MouseInfo GetState(int index)
        {
            
            #if WINDOWS_VER
            if (index >= multimouse.MiceCount)
                throw new MouseNotExists();
            return new MouseInfo(multimouse.Mice[index]);
            #else
            throw new Exception("Multipal mouse is implemented only for Windows");
            #endif


        }




        public static void Unload()
        {
            #if WINDOWS_VER
                multimouse.Dispose();
                multimouse=null;
            #endif
        }

        public static void SetPosition(Microsoft.Xna.Framework.Vector2 position)
        {
            #if WINDOWS_VER
            
            #else
            Mouse.SetPosition((int)position.X, (int)position.Y);

            #endif

        }
    }

    
}
