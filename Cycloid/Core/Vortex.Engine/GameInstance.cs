using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using vortexWin.Helpers;
using vortexWin.Engine.Input;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.GamerServices;
using vortexWin.Engine.Xbox;
using Vortex.Engine.Interfaces;
using Vortex.MercuryParticleProvider;



namespace vortexWin.Engine
{
    public class GameInstance:Game
    {      
        IParticleEffectProvider particlesystem = new MercuryParticleEngine();

        public IParticleEffectProvider Particles
        {get { return particlesystem; }}

        public GraphicsDeviceManager graphics;

        public static int ScreenWidth = 1280;
        public static int ScreenHeight = 720;
        public static Vector2 Resolution { get { return new Vector2(ScreenWidth, ScreenHeight); } }

        //public FormCollection gui;
        //ServiceHelper serviceHelper;

        private SpriteBatch spriteBatch;
        public bool Fullscreen=false;

        public SpriteBatch SpriteBatch{
            get { return spriteBatch; }
        }

        public Primitives.PrimitiveBatch primitivesBatch;
        //protected Random rnd;


        //RenderTarget2D renderTarget;
        RenderTarget2D extraTarget;

        public T CreateShader<T>(string name)
            where T:ShaderEffect,new()
        {
            T result = new T();
            result.LoadContent(this, name);
            return result;
        }

        public Texture2D DrawToTexture(Action draw)
        {
            GraphicsDevice.SetRenderTarget(extraTarget);
            draw();
            GraphicsDevice.SetRenderTarget(null);
            return extraTarget;
        }

        public GameInstance(GameScreen startView):base()
        {
            this.IsFixedTimeStep = true;

#if TRIAL
            Guide.SimulateTrialMode = true;
#endif

            graphics = new GraphicsDeviceManager(this);           

            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.SynchronizeWithVerticalRetrace = false;
            
            graphics.DeviceCreated += new EventHandler<EventArgs>(
                delegate(object obj,EventArgs e){
                    graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
                    Debug.WriteLine("<GameInstance.cs> Device created");
                    Debug.WriteLine(string.Format("Device screen resolution:{0}x{1}",
                        graphics.GraphicsDevice.DisplayMode.Width,
                        graphics.GraphicsDevice.DisplayMode.Height));
                    Debug.WriteLine(string.Format("Device title safe area:{0},{1},{2},{3}",
                        graphics.GraphicsDevice.DisplayMode.TitleSafeArea.Left,
                        graphics.GraphicsDevice.DisplayMode.TitleSafeArea.Top,
                        graphics.GraphicsDevice.DisplayMode.TitleSafeArea.Width,
                        graphics.GraphicsDevice.DisplayMode.TitleSafeArea.Height));
                    StateManager.Init(this, startView);
                });

            Content.RootDirectory = "Content";

            Components.Add(new Microsoft.Xna.Framework.GamerServices.GamerServicesComponent(this));

        }


        protected override void Initialize()
        {
            Debug.WriteLine("GameInstance.Initialize");

            base.Initialize();

            MouseController.Init(this);

            InitDevice();

            //gui = new xWinFormsLib.FormCollection();

            graphics.DeviceReset += new EventHandler<EventArgs>(graphics_DeviceReset);
            graphics.GraphicsDevice.DeviceLost +=
                new EventHandler<EventArgs>(GraphicsDevice_DeviceLost);
        }

        void GraphicsDevice_DeviceLost(object sender, EventArgs e)
        {
            Debug.WriteLine("<GameInstance.cs> Device lost");
            graphics = new GraphicsDeviceManager(this);
            InitDevice();
        }

        private void InitDevice()
        {
            Debug.WriteLine("GameInstance.InitDevice");
            FontHelper.LoadFonts(Content);

            //serviceHelper = new ServiceHelper();
            //serviceHelper.Initialize(graphics.GraphicsDevice, Content);

            if (Fullscreen)
            {
                Debug.WriteLine("Toggle fullscreen");
                graphics.ToggleFullScreen();

            }

            spriteBatch = new SpriteBatch(GraphicsDevice);
            primitivesBatch = new Primitives.PrimitiveBatch(GraphicsDevice);


            //renderTarget = StateManager.CreateRenderTarget(graphics.GraphicsDevice);
            extraTarget = StateManager.CreateRenderTarget(graphics.GraphicsDevice);

        }

        void graphics_DeviceReset(object sender, EventArgs e)
        {
            Debug.WriteLine("<GameInstance.cs> Device reset");
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            
                StateManager.Draw(gameTime);

                particlesystem.Render(this);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            try
            {
                particlesystem.Update(gameTime);
            }
            catch (Exception e)
            {
            }

            if (!IsActive)
                return;

            FrameRateCounter.Update(gameTime);

            KeyboardState currentKeyboardState = Keyboard.GetState();
            if (currentKeyboardState.IsKeyDown(Keys.PageUp))
                Thread.Sleep(10);

            if (currentKeyboardState.IsKeyDown(Keys.PageDown))
                return;

            KeyboardObserver.Update(gameTime);
            GamePadController.Update(gameTime);

            GuideManager.Instance.Update(gameTime);

            StateManager.Update(gameTime);
        }

        protected override void UnloadContent()
        {
            Debug.WriteLine("GameInstance.UnloadContent");

            base.UnloadContent();

            primitivesBatch.Dispose();
            primitivesBatch = null;

            SpriteBatch.Dispose();
            spriteBatch = null;
        }

        protected override void LoadContent()
        {
            particlesystem.Initialize(this);
            
            Debug.WriteLine("Particle system initialization complete");
            base.LoadContent();
        }

        public IGraphicsDeviceService GraphicsService
        {
            get { 
                return (IGraphicsDeviceService)Services.GetService(typeof(IGraphicsDeviceService));
            }
        }

        public void TriggerParticle(string p, Vector2 position)
        {
            try
            {
                particlesystem[p].Trigger(ref position);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        internal void RemoveParticle(string p)
        {
            try
            {
                particlesystem.Remove(p);
            }
            catch (Exception e)
            {
            }
        }

        internal void AddParticle(IParticleEffect effect, string p)
        {
            try
            {
                particlesystem.Add(effect, p);
            }
            catch (Exception e)
            {
            }
        }
    }
}
