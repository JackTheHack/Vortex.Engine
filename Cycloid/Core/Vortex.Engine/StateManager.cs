
#define WINDOWS

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using vortexWin.Engine.Input;
using System.Diagnostics;
using vortexWin.Engine.Sound;



namespace vortexWin.Engine
{
    public class StateManager
    {
        static bool loaded = false;

        public static GameInstance game;
        static GameScreen current=null;
        static GameScreen previous = null;

        static ShaderEffect effectPost;

        static public bool EnablePostEffect { get; set; }
        static public GameScreen CurrentScreen { get { return current; } }



        static RenderTarget2D renderTarget, renderTarget2;
        static Texture2D prevScene = null, currentScene = null;

        static TransitionEffect transition;

        static StateManager()
        {
            EnablePostEffect = false;
        }

        static void KeyboardObserver_KeyDown(Microsoft.Xna.Framework.Input.Keys obj)
        {
            if (current != null)
                current.OnKeyDown(obj);
        }

        public static bool Paused
        {
            get
            {
                if (current != null)
                    return current.Paused;
                else return true;
            }
        }

        public static bool IsPostEffect
        {
            get { return effectPost == null; }
        }

        public static void SetView(GameScreen view)
        {
            Debug.WriteLine("StateManager.SetView " + view.ToString());

                    if (current != null)
                        current.Paused = true;


                    if (view != null)
                    {
                        view.Init(game);
                        view.previusScreen = current;
                        view.LoadContent(game.Content);
                        view.StateChange += new EventHandler<GameStateArgs>(view_StateChange);
                    }

                previous = current;
                current = view;

                view.SetState(GameState.Starting);
                
                view.Paused = false;
                view.Visible = true;
        }

        static void view_StateChange(object sender, GameStateArgs e)
        {
            switch (e.State)
            {
                case GameState.Closed:
                    e.Instance.UnloadContent();
                    break;
                case GameState.Ending:
                    e.Instance.Paused = true;
                    break;
                case GameState.Starting:
                    if (previous != null)
                    {
                        previous.SetState(GameState.Ending);
                        transition = new TransitionEffect();
                        transition.LoadContent(game.Content);
                        transition.Complete += new Action<object>(transition_Complete);
                    }
                    else current.SetState(GameState.Working);
                    break;
            }
        }

        static void transition_Complete(object obj)
        {
            current.SetState(GameState.Working);
            current.Paused = false;
            previous.SetState(GameState.Closed);   
        }

        static public void SetState(GameState state)
        {
            if (current != null)
            {
                current.SetState(state);
                current.OnStateChange();
            }
        }

        static public void Pause(Boolean value)
        {
            if (current != null)
                current.Paused = value;
        }

        static public void Hide(Boolean value)
        {
            if (current != null)
                current.Visible = value;
        }

        static public void SetPostEffect(ShaderEffect effect)
        {
            if(effectPost!=null)
            {
                effectPost.Ending = true;
                effectPost.EffectEnd += (Action<ShaderEffect, GameTime>)
                    delegate(ShaderEffect eff, GameTime gameTime) { 
                        effectPost = effect; 
                    };
            }else{
                effectPost=effect;
            }
        }

        static public void Unload()
        {
            Debug.WriteLine("StateManager.Unload");
            if (transition != null)
            {
                transition.Unload();
                transition = null;
            }

            renderTarget.Dispose();
            renderTarget2.Dispose();
            prevScene.Dispose();
            currentScene.Dispose();

            currentScene = null;
            prevScene = null;

            renderTarget = null;
            renderTarget2 = null;

            current.UnloadContent();
            current = null;

            if (previous != null)
            {
                previous.UnloadContent();
                previous = null;
            }

            //Views.Clear();

            game.Dispose();
            game = null;
        }

       
        public static RenderTarget2D CreateRenderTarget(GraphicsDevice device)
        {
            bool mipmap = true;
            #if WINDOWS 
            mipmap = false;
            #endif
            RenderTarget2D result=
                new RenderTarget2D(device, 
                GameInstance.ScreenWidth, GameInstance.ScreenHeight, mipmap, 
                SurfaceFormat.Color,DepthFormat.None, 0,RenderTargetUsage.DiscardContents);
            return result;


        }
        

        static public void Draw(GameTime gameTime)
        {
            //lock (Views)
            //{
                if (previous != null && previous.State == GameState.Ending)
                {
                  
                    #region draw previous scene

                    previous.PreDraw(gameTime);

                    game.graphics.GraphicsDevice.SetRenderTarget(renderTarget);
                    game.graphics.GraphicsDevice.Clear(Color.Black);
                    game.graphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                    previous.Draw(gameTime);
                    game.graphics.GraphicsDevice.SetRenderTarget(null);
                    prevScene = renderTarget;

                    #endregion

                    #region draw new scene

                    current.PreDraw(gameTime);

                    game.graphics.GraphicsDevice.SetRenderTarget(renderTarget2);
                    game.graphics.GraphicsDevice.Clear(Color.White);
                    game.graphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                    current.Draw(gameTime);
                    game.graphics.GraphicsDevice.SetRenderTarget(null);
                    currentScene = renderTarget2;
                    #endregion
                    
                    game.GraphicsDevice.Clear(Color.Green);
                    game.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

                    transition.SetTextures(prevScene, currentScene);

                    if(effectPost!=null)
                    effectPost.Draw((Action)delegate()
                    {
                        transition.Draw(game.graphics, game.SpriteBatch);
                    });
                    else transition.Draw(game.graphics, game.SpriteBatch);

                    game.SpriteBatch.End();
                }
                else
                {

                    if (!current.Visible)
                        return;

                    //render optimization
                    bool usePostEffect = (effectPost != null && EnablePostEffect);
                    
                    current.PreDraw(gameTime);

                    if(usePostEffect)
                        game.GraphicsDevice.SetRenderTarget(renderTarget);
                        
                    current.Draw(gameTime);

                    if (usePostEffect)
                    {
                        game.GraphicsDevice.SetRenderTarget(null);
                        currentScene = renderTarget;

                        #region Post processing effects

                        game.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                        if (effectPost != null && EnablePostEffect)
                            effectPost.Draw(game.SpriteBatch,currentScene);                           
                        else game.SpriteBatch.Draw(currentScene, new Rectangle(0, 0, GameInstance.ScreenWidth, GameInstance.ScreenHeight), Color.White);

                        game.SpriteBatch.End();
                        #endregion
                    }
                }
           // }
        }

        static public void Update(GameTime gameTime)
        {
            Timer.Update(gameTime);

            if (previous!=null && previous.State == GameState.Ending)
                transition.Update(gameTime);


            if(current!=null)
            if (!current.Paused)
                current.Update(gameTime);


            if (effectPost != null)
                effectPost.Update(gameTime);           

        }

        public static void Init(GameInstance gameInstance, GameScreen startView)
        {
            Debug.WriteLine("StateManager.Init");
            game = gameInstance;
            //game.GraphicsDevice.RenderState.MultiSampleAntiAlias = true;
            //game.GraphicsDevice.PresentationParameters.MultiSampleType = MultiSampleType.FourSamples;
            game.Activated += new EventHandler<EventArgs>(game_Activated);
            KeyboardObserver.Reset();
            KeyboardObserver.KeyDown += new Action<Microsoft.Xna.Framework.Input.Keys>(KeyboardObserver_KeyDown);
            GamePadController.Reset();
            GamePadController.KeyDown += new Action<Microsoft.Xna.Framework.Input.Buttons>(GamePadController_KeyDown);

            SetView(startView);
        }

        static void GamePadController_KeyDown(Microsoft.Xna.Framework.Input.Buttons obj)
        {
            if (current != null)
                current.OnKeyDown(obj);
        }

        static void game_Activated(object sender, EventArgs e)
        {
            Debug.WriteLine("StateManager.game_Activated");
            if (!loaded)
            {
                renderTarget = CreateRenderTarget(game.graphics.GraphicsDevice);
                renderTarget2 = CreateRenderTarget(game.graphics.GraphicsDevice);

                prevScene = CreateScreenTex();
                currentScene = CreateScreenTex();

                loaded = true;
            }
        }

        public static Texture2D CreateScreenTex()
        {
            bool mipmaps = true;
            #if WINDOWS
            mipmaps = false;
            #endif

            return new Texture2D(
                    game.graphics.GraphicsDevice,
                    GameInstance.ScreenWidth, GameInstance.ScreenHeight, mipmaps, SurfaceFormat.Color);
        }

        public static void Exit()
        {
            Debug.WriteLine("StateManager.Exit");
            FontHelper.Unload();
            MouseController.Unload();
            KeyboardObserver.KeyDown -= KeyboardObserver_KeyDown;

            if (current != null)
                current.UnloadContent();

            game.Exit();
            game.Dispose();

            Unload();

            GC.Collect();
        }

        public static void SetView(vortexWin.Engine.GameScreen next, vortexWin.Engine.GameScreen screenprev)
        {
            previous = screenprev;
            SetView(next);
            next.previusScreen = screenprev;
        }

        public static void ClearPostEffect()
        {
            effectPost = null;
        }
    }
}
