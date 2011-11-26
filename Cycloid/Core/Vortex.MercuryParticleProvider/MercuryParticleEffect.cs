using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vortex.Engine.Interfaces;
using ProjectMercury;
using Microsoft.Xna.Framework;

namespace Vortex.MercuryParticleProvider
{
    public class MercuryParticleEffect : IParticleEffect
    {
        ParticleEffect effect;

        public MercuryParticleEffect()
        {

        }

        public MercuryParticleEffect(ParticleEffect effect)
        {
            this.effect = effect;
        }

        public ParticleEffect ParticleEffect { get { return effect; } }

        public void Trigger(ref Vector2 position)
        {
            effect.Trigger(ref position);
        }

        public void LoadFromFile(Game game, string path)
        {
            effect = game.Content.Load<ParticleEffect>(path);
            effect.LoadContent(game.Content);
            effect.Initialise();
        }
    }
}
