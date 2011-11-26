using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using vortexWin.Engine.Input;
using System.Collections.Generic;
using System.Diagnostics;

namespace vortexWin.Engine
{
    public enum GameState{None,Starting,Working,Ending,Closed}

    public class GameStateArgs:EventArgs
    {
        public Game Game;
        public GameScreen Instance;
        public GameTime Time=null;
        public GameState State;
    }


    public abstract class GameScreen
    {
        internal GameScreen previusScreen = null;        

        /// <summary>
        /// </summary>
        protected GameState currentState;

        protected List<Animation> animations = new List<Animation>();

        public bool Loaded = false;

        public GameState State
        {
            get { return currentState; }
            set { SetState(value); }
        }
        
        private GameInstance gameInstance;
        public GameInstance Game
        {
            get { return gameInstance; }
            set { gameInstance = value; }
        }
        protected StateManager manager;
        public ContentManager Content = null;

        private bool visible = true;
        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        private bool paused = false;
        public bool Paused
        {
            get { return paused; }
            set { paused = value; }
        }


        public virtual void Draw(GameTime gameTime){
            lock (animations)
            {
                Game.SpriteBatch.Begin(SpriteSortMode.Deferred,
                        BlendState.AlphaBlend);

                foreach (Animation anim in animations)
                    anim.Draw(Game.SpriteBatch);

                Game.SpriteBatch.End();
            }
        }

        public virtual void Update(GameTime gameTime){
            lock (animations)
            {
                foreach (Animation anim in animations)
                    anim.Update(gameTime);
            }
        }

        private string GetClassName()
        {
            return this.GetType().Name;
        }

        public virtual void Init(GameInstance game)
        {
            Debug.WriteLine("GameScreen.Init (" + GetClassName() + ")");
            StateChange = null;
            currentState=GameState.Starting;
            Content = game.Content;
            this.gameInstance = game;
            OnStateChange();
        }

        public virtual void OnStateChange( )
        {
            if (StateChange != null)
                StateChange(this, new GameStateArgs() { Instance = this,Game=gameInstance, State = currentState });
        }

        public virtual void OnStateStarting( ) {
            currentState=GameState.Working;
            OnStateChange();
        }

        public virtual void OnStateEnding( ) { 
            currentState=GameState.Closed;
            OnStateChange();
        }

        public virtual void LoadContent(ContentManager contentManager)
        {
            if (!Loaded)
            {
                Debug.WriteLine("GameScreen.LoadContent (" + GetClassName() + ")");
                OnLoadContent(contentManager);
                Loaded = true;
            }
        }

        public virtual void Back()
        {
            Paused = true;
            Visible = false;
            if(previusScreen!=null)
                StateManager.SetView(previusScreen);
        }

        protected virtual void OnLoadContent(ContentManager contentManager)
        {
        }

        public virtual void UnloadContent()
        {
        }

        public void SetState(GameState state)
        {
            currentState = state;
            OnStateChange();
        }

        public void AddAnimation(Animation animation)
        {
            animation.Load(this);
            animations.Add(animation);
        }
     
        public event EventHandler<GameStateArgs> StateChange;

        public virtual void PreDraw(GameTime gameTime){}

        public virtual void OnKeyDown(Microsoft.Xna.Framework.Input.Keys obj){}

        public virtual void OnKeyDown(Microsoft.Xna.Framework.Input.Buttons button) { }
    }
}
