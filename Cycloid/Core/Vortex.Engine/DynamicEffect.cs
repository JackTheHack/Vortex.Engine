using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vortexWin.Engine;
using Microsoft.Xna.Framework;

namespace vortexWin.Engine
{
    public class DynamicEffect:ShaderEffect
    {
        protected float m_Timer = 0;

        public override void PreDraw()
        {
            base.PreDraw();
            if (effect.Parameters["fTimer"] != null)
                effect.Parameters["fTimer"].SetValue(m_Timer);
            else throw new Exception("Effect must have m_Timer property to work properly");
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            m_Timer += (float)gameTime.ElapsedGameTime.Milliseconds / 500;          
        }


    }
}
