using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace vortexWin.Engine
{
    public class TransitionEffect
    {
        public Effect effectPost;
        double m_Timer =  Math.PI/2;
        bool isPaused = false;

        public int Speed { get; set; }
        public bool Completed { get; set; }

        Texture2D tex1, tex2;

        Rectangle drawRegion;

        public event Action<object> Complete;

        public TransitionEffect()
        {
            drawRegion = new Rectangle(0, 0, GameInstance.ScreenWidth, GameInstance.ScreenHeight);
            Speed = 250;
        }

        public TransitionEffect(Rectangle region)
        {
            drawRegion = region;
            Speed = 250;
        }

        public void Reset()
        {
            m_Timer = Math.PI / 2;
            Completed = false;
        }


        public void LoadContent(ContentManager Content )
        {
            effectPost = Content.Load<Effect>(@"effects\FrameTransition");
        }

        public void SetTextures(Texture2D tex1, Texture2D tex2)
        {
            this.tex1 = tex1;
            this.tex2 = tex2;
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            if (!Completed)
            {
                // Render the scene, using our fade transition to fade between the two scenes we have
                // Apply the post process shader
                float fadeBetweenScenes = ((float)Math.Sin(m_Timer) * 0.5f) + 0.5f;

                effectPost.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                effectPost.Parameters["fFadeAmount"].SetValue(fadeBetweenScenes);
                effectPost.Parameters["fSmoothSize"].SetValue(0.095f);
                effectPost.Parameters["ColorMap2"].SetValue(tex2);
                {
                    effectPost.CurrentTechnique.Passes[0].Apply();
                    {
                        spriteBatch.Draw(tex1, drawRegion, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                    }
                }
            }
            else
            {
                spriteBatch.Draw(tex2, drawRegion, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            }
        }

        public void Update(GameTime time)
        {
            if (!isPaused)
            {
                m_Timer -= (double)time.ElapsedGameTime.Milliseconds / Speed;
                if (m_Timer < -Math.PI / 2)

                {
                    m_Timer = -Math.PI / 2;
                    if (Complete != null)                    
                        Complete(this);
                    Completed = true;
                }
            }
        }

        public void Unload()
        {
            effectPost.Dispose();
            effectPost = null;
        }

        public void Pause()
        {
            isPaused = true;
        }

        public void Start()
        {
            isPaused = false;
        }
    }
}
