using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    public class StarField
    {
        private Random random;
        public Vector2 EmitterLocation { get; set; }
        private List<StarParticle> particles;
        private List<Texture2D> textures;
        private float SCALE;

        public StarField()
        {

        }

        public StarField(List<Texture2D> textures, Vector2 location, float scale)
        {
            EmitterLocation = location;
            this.textures = textures;
            this.particles = new List<StarParticle>();
            random = new Random();
            SCALE = scale;
        }

        public void Update()
        {
            int total = 500;

            if (particles.Count() < total)
            {
                for (int i = particles.Count; i < total; i++)
                {
                    particles.Add(GenerateNewParticle());
                }
            }

            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                //if (particles[particle].Position.X >= 1920)   //left
                if (particles[particle].Position.X <= -10)     //right
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        public StarParticle GenerateNewParticle()
        {
            Texture2D texture = textures[random.Next(textures.Count)];
            //Vector2 position = new Vector2(0 - random.Next(5000), random.Next(1080));     //left
            Vector2 position = new Vector2(1920 + random.Next(5000), random.Next(1080));   //right
            //Vector2 velocity = new Vector2(random.Next(3, 10), 0);        //left
            Vector2 velocity = new Vector2(-random.Next(3, 10), 0);          //right
            float angle = 0;
            float angularVelocity = (0.1f * (float)(random.NextDouble() * 2 - 1) * GameValues.Instance.SCALEX);

            Color color = new Color(255, 255, 255);
            float size = (float)0.1 + (float)random.NextDouble();
            int ttl = 100;

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