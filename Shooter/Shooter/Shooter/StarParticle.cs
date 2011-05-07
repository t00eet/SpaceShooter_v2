using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceShooter
{
    public class Particle
    {
        public Texture2D Texture;
        public Vector2 Position;
        public Vector2 Velocity;
        public float Angle;
        public float AngularVelocity;
        public Color Color;
        public float Size;
        public int TTL;
        public float SCALE;

        public Particle(Texture2D texture, Vector2 position, Vector2 velocity, float angle, float angularVelocity, Color color, float size, int ttl, float scale)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
            Angle = angle;
            AngularVelocity = angularVelocity;
            Color = color;
            Size = size;
            TTL = ttl;
            SCALE = scale;
        }

        public void Update()
        {
            TTL--;
            Position += Velocity;
            Angle += AngularVelocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            //spriteBatch.Draw(Texture, Position, sourceRectangle, Color, Angle, origin, Size, SpriteEffects.None, 0f);
            spriteBatch.Draw(Texture, Position, sourceRectangle, Color, 0.0f, Vector2.Zero, SCALE, SpriteEffects.None, 0);
        }
        
    }
}