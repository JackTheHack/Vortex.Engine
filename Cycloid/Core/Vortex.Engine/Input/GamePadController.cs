using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using vortexWin.Xbox360;
using vortexWin.Engine.Xbox;

namespace vortexWin.Engine.Input
{
    public class GamePadController
    {
        static GamePadState? lastState;
        static PlayerIndex? playerIndex = null;

        public static event Action<Buttons> KeyDown;
        public static event Action<PlayerIndex> StartPressed;
        public static event Action<Gamer> PlayerSignOut;

        static GamePadController()
        {
            SignedInGamer.SignedOut += new EventHandler<SignedOutEventArgs>(SignedInGamer_SignedOut);
        }

        static void SignedInGamer_SignedOut(object sender, SignedOutEventArgs e)
        {
            if (Index.HasValue && e.Gamer.PlayerIndex == Index.Value)
            {
                OnPlayerSignOut(e.Gamer);
            }
        }

        private static void OnPlayerSignOut(SignedInGamer signedInGamer)
        {
            if (PlayerSignOut != null)
            {
                PlayerSignOut(signedInGamer);
            }
        }

        public static void ShowKeyboardInput(string title, string defaultText, object state, AsyncCallback callback)
        {
            if (Index.HasValue)
            {
                GuideManager.Instance.ShowInput(
                                title, string.Empty, defaultText,
                                callback, state);
            }
        }

        public static void Update(GameTime gameTime)
        {
            if (!Index.HasValue)
            {
                CheckStartButton();
            }else
            {
                UpdateKeyInputState();
            }
            
        }

        private static void UpdateKeyInputState()
        {
            if (!lastState.HasValue)
            {
                if (GamePadEmulator.GetState(Index.Value).IsConnected)
                    lastState = (GamePadState?)GamePadEmulator.GetState(Index.Value);
            }
            else
            {
                GamePadState currentState = GamePadEmulator.GetState(Index.Value);
                Buttons[] controllerButtons = new Buttons[]{
                    Buttons.A,
                    Buttons.B,
                    Buttons.X,
                    Buttons.Y,
                    Buttons.Start,
                    Buttons.Back,
                    Buttons.DPadRight,
                    Buttons.DPadLeft,
                    Buttons.DPadUp,
                    Buttons.DPadDown,
                    Buttons.LeftThumbstickDown,
                    Buttons.LeftThumbstickUp,
                    Buttons.LeftThumbstickLeft,
                    Buttons.LeftThumbstickRight
                };

                foreach (Buttons button in controllerButtons)
                {
                    if (currentState.IsButtonDown(button) && lastState.Value.IsButtonUp(button))
                        OnKeyDown(button);
                }

                lastState = currentState;
            }
        }

        private static void CheckStartButton()
        {
            foreach (PlayerIndex index in EnumHelper.GetValues(typeof(PlayerIndex)))
            {
                if (GamePadEmulator.GetState(index).IsConnected)
                {
                    if (GamePadEmulator.GetState(index).IsButtonDown(Buttons.Start))
                    {
                        OnStartPressed(index);
                    }
                }
            }
        }

        private static void OnStartPressed(PlayerIndex index)
        {
            if (StartPressed != null)
                StartPressed(index);
        }

        public static void Vibrate(int vibrationTime, float amount)
        {
            if (Index.HasValue)
            {
                if (GamePad.GetCapabilities(Index.Value).IsConnected)
                {
                    GamePad.SetVibration(Index.Value, amount, amount);
                    Timer.Add(vibrationTime, (Action)delegate()
                    {
                        GamePad.SetVibration(Index.Value, 0.0f, 0.0f);
                    });
                }
            }
        }

        public static void SetPlayerIndex(PlayerIndex player)
        {
                playerIndex = player;
        }

        public static PlayerIndex? Index
        {
            get
            {
                return playerIndex;
            }
        }

        public static Vector2 Thumbstick
        {
            get {
                GamePadState gamePad = GamePad.GetState(Index.Value);
                if (!gamePad.IsConnected) return new Vector2(0, 0);
                return gamePad.ThumbSticks.Left;
            }
        }

        public static float[] Triggers
        {
            get
            {
                GamePadState gamePad = GamePad.GetState(Index.Value);                
                if (!gamePad.IsConnected) return new float[2]{0.0f,0.0f};
                return new float[]{gamePad.Triggers.Left,gamePad.Triggers.Right};

                
            }
        }       

        public static bool IsConnected
        {
            get
            {
                return GamePad.GetState(Index.Value).IsConnected;
            }
        }

        private static void OnKeyDown(Buttons button)
        {
            if (KeyDown != null)
                KeyDown(button);
        }

        public static void ClearPlayer()
        {
            playerIndex = null;
        }

        public static void Reset()
        {
            KeyDown = null;
        }

        public static GamePadState GetState(PlayerIndex playerIndex)
        {
            return GamePadEmulator.GetState(playerIndex);
        }
              
        public static GamePadState GetState()
        {
            if (playerIndex.HasValue)
            {
                return GamePadEmulator.GetState(Index.Value);
            }
            else
            {
                throw new InvalidOperationException("Player must be signed in first");
            }
        }
    }
}
