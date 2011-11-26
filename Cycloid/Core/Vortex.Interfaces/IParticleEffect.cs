using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Vortex.Engine.Interfaces
{
    public interface IParticleEffect
    {
        void Trigger(ref Vector2 position);

        void LoadFromFile(Game game, string path);
    }
}
