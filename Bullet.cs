using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Galactic_Vanguard
{
    public class Bullet : Entity
    {
        public static Texture2D image;

        private double speed;
        private double angle;
        private Color color;

        public Bullet(Rectangle rec, double speed, double angle, Color color) : base()
        {
            this.rec = rec;
            this.speed = speed;
            this.angle = angle;
            this.color = color;

            position = rec.Location.ToVector2();

            velocity.X = (float)(speed * Math.Sin(angle));
            velocity.Y = (float)(speed * Math.Cos(angle));
        }

        public override void Update()
        {
            position.X += velocity.X;
            position.Y -= velocity.Y;

            rec.Location = position.ToPoint();
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(image, rec, null, color, (float)angle, new Vector2(image.Width / 2, image.Height / 2), SpriteEffects.None, 0f);
        }
    }
}
