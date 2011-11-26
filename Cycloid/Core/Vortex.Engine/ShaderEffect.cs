using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace vortexWin.Engine
{
    public class ShaderEffect
    {
        public bool Immortal = false;
        public double LifeTime = 30000;

        protected bool ending = false;
        public bool Ending
        {
            get { return ending; }
            set { ending = value; }
        }


        protected Effect effect = null;
        private Dictionary<string, object> parameters = new Dictionary<string, object>();

        public Dictionary<string, object> References 
        { 
            get { return parameters; }
        }
        public EffectParameterCollection Parameters 
        { 
            get { return effect.Parameters; }
        }

        public ShaderEffect()
        {
        }

        public virtual void LoadContent(GameInstance game,string filename){
            effect = game.Content.Load<Effect>(filename);
        }

        public virtual void Unload() { effect = null; }
        
        public virtual void Draw(Action draw) {
            PreDraw();
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    draw();
                }
        }

        public virtual void PreDraw(){}

        public virtual void Update(GameTime gameTime) {
            if (LifeTime > 0 && !Immortal)
            {
                LifeTime -= gameTime.ElapsedGameTime.Milliseconds;
                if (LifeTime <= 0)
                    Ending = true;
            }
        }

        public event Action<ShaderEffect, GameTime> EffectEnd;
        protected virtual void OnEffectEnd(ShaderEffect effect, GameTime gameTime)
        {
            if (Immortal)
            {
                Ending = false;
                Reset();
            }

            if (EffectEnd != null)
                EffectEnd(effect, gameTime);
        }

        public virtual void Reset() { }




        public virtual void Draw(SpriteBatch batch,Texture2D currentScene)
        {
            PreDraw();
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                batch.Draw(
                    currentScene, new Rectangle(0, 0, GameInstance.ScreenWidth, GameInstance.ScreenHeight),
                    Color.White);
            }
        }
    }
}
