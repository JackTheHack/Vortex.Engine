using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace vortexWin.Engine.Input
{
    public class GamePadEmulator
    {
        public static GamePadState GetState(PlayerIndex player)
        {
            if (GamePad.GetState(player).IsConnected)
                return GamePad.GetState(player);
            else
            {
                KeyboardState keys=Keyboard.GetState();
                MouseInfo mouseState=MouseController.GetState();

                List<Buttons> buttons = new List<Buttons>();

                if(keys.IsKeyDown(Keys.A))
                    addButton(buttons, Buttons.A);
                if(keys.IsKeyDown(Keys.B))
                    addButton(buttons, Buttons.B);
                if(keys.IsKeyDown(Keys.X))
                    addButton(buttons, Buttons.X);
                if(keys.IsKeyDown(Keys.Y))
                    addButton(buttons, Buttons.Y);
                if(keys.IsKeyDown(Keys.Enter))
                    addButton(buttons, Buttons.Start);
                if(keys.IsKeyDown(Keys.Escape))
                    addButton(buttons, Buttons.Back);
                if (keys.IsKeyDown(Keys.Down))
                    addButton(buttons, Buttons.DPadDown);
                if (keys.IsKeyDown(Keys.Up))
                    addButton(buttons, Buttons.DPadUp);
                if (keys.IsKeyDown(Keys.Left))
                    addButton(buttons, Buttons.DPadLeft);
                if (keys.IsKeyDown(Keys.Right))
                    addButton(buttons, Buttons.DPadRight);

                GamePadState state = new GamePadState(
                    new Vector2(mouseState.X, mouseState.Y),
                    new Vector2(0, 0),
                    0,0,
                    buttons.ToArray());

                return state;
                
            }
        }

        private static void addButton(List<Buttons> buttons, Buttons btn)
        {
            buttons.Add(btn);
        }
    }
}
