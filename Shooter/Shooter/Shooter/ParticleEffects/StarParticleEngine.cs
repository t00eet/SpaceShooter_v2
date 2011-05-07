using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    public class ParticleEngine
    {
        private Random random;
        public Vector2 EmitterLocation { get; set; }
        private List<StarParticle> particles;
        private List<Texture2D> textures;
        private float SCALE;

        public ParticleEngine()
        {

        }

        public ParticleEngine(List<Texture2D> textures, Vector2 location, float scale)
        {
            EmitterLocation = location;
            this.textures = textures;
            this.particles = new List<StarParticle>();
            random = new Random();
            SCALE = scale;
        }

        public void Update()
        {
            int total = 10;

            for (int i = 0; i < total; i++)
            {
                particles.Add(GenerateNewParticle());
            }

            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                if (particles[particle].TTL <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        public StarParticle GenerateNewParticle()
        {
            Texture2D texture = textures[random.Next(textures.Count)];
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(
                                    1f * (float)(random.NextDouble() * 2 - 1),
                                    1f * (float)(random.NextDouble() * 2 - 1));
            float angle = 0;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            /*Color color = new Color(
                        (float)random.NextDouble(),
                        (float)random.NextDouble(),
                        (float)random.NextDouble());*/
            Color color = new Color(210, 105, 30);
            float size = 5 + (float)random.NextDouble();
            int ttl = 1 + random.Next(5);

            return new StarParticle(texture, position, velocity, angle, angularVelocity, color, size, ttl, SCALE);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }
            spriteBatch.End();
        }
    }
}