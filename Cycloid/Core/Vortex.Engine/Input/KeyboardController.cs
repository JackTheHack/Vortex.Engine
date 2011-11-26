using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace vortexWin.Engine.Input
{
    public class KeyboardObserver
    {
        static KeyboardState lastState;
        static KeyboardState currentState;

        public static bool Active = true;
        public static event Action<Keys> KeyDown;
        //public static event Action<Keys> KeyUp;

        static KeyboardObserver()
        {
            lastState = Keyboard.GetState();
        }

        public static bool Shift
        {
            get{return lastState.IsKeyDown(Keys.LeftShift)||lastState.IsKeyDown(Keys.RightShift);}
        }

        public static void Update(GameTime gameTime)
        {
            currentState = Keyboard.GetState();
               
            //do something
            Keys[] key1 = currentState.GetPressedKeys();
            Keys[] key2 = lastState.GetPressedKeys();
            
            foreach (Keys key in key1)
            {
                bool contains = false;
                foreach (Keys k in key2)
                    if (k == key)
                        contains = true;

                if (!contains && Active)
                    OnKeyDown(key);

            }

            lastState = currentState;

        }

        private static void OnKeyDown(Keys key)
        {
            if(!StateManager.Paused)
            if (KeyDown != null)
                KeyDown(key);
        }

        internal static void Reset()
        {
            KeyDown = null;
            Active = true;
        }
    }
}
