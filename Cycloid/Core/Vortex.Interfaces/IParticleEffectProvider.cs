using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Vortex.Engine.Interfaces
{
    public interface IParticleEffectProvider : IDisposable
    {
        void Initialize(Game game);

        void Add(IParticleEffect effect, string key);

        void Remove(string key);

        bool Contains(string key);

        IParticleEffect this[string key] {get;}

        void Render(Game game);

        void Update(GameTime gameTime);

        IParticleEffect CreateParticle(Game game, string path);
    }
}
