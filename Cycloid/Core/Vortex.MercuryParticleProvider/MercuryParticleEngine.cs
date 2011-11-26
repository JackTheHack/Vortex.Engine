using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vortex.Engine.Interfaces;
using Microsoft.Xna.Framework;
using ProjectMercury;
using Microsoft.Xna.Framework.Graphics;
using ProjectMercury.Renderers;

namespace Vortex.MercuryParticleProvider
{
    public class MercuryParticleEngine : IParticleEffectProvider
    {
        ParticleEffectManager particleEngine;

        protected Dictionary<string, MercuryParticleEffect> particles = new Dictionary<string, MercuryParticleEffect>();

        public void Initialize(Game game)
        {
            particleEngine = new ParticleEffectManager(new SpriteBatchRenderer());
            particleEngine.Renderer.GraphicsDeviceService = game.Services.GetService(typeof(IGraphicsDeviceService)) as IGraphicsDeviceService;
            particleEngine.Renderer.LoadContent(game.Content);
        }

        public void Add(IParticleEffect effect, string key)
        {
            var mercuryEffect = effect as MercuryParticleEffect;
            
            if(mercuryEffect == null) throw new ArgumentException("Effect must be Mercury particle effect");

            particles.Add(key, mercuryEffect);

            particleEngine.Add(mercuryEffect.ParticleEffect);
        }

        public void Remove(string key)
        {
            if (Contains(key))
            {
                particleEngine.Remove(particles[key].ParticleEffect);

                particles.Remove(key);
            }
        }

        public bool Contains(string key)
        {
            return particles.ContainsKey(key);
        }

        public IParticleEffect this[string key]
        {
            get { return particles[key]; }
        }

        public void Render(Game game)
        {
            foreach (MercuryParticleEffect effect in particles.Values)
                particleEngine.Renderer.RenderEffect(effect.ParticleEffect);
        }

        public void Update(GameTime gameTime)
        {
            particleEngine.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f, false);
        }

        public void Dispose()
        {
            
        }

        public IParticleEffect CreateParticle(Game game, string path)
        {
           var result = new MercuryParticleEffect();
           result.LoadFromFile(game, path);
           return result;
        }
    }
}
