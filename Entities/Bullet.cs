using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Galactic_Vanguard
{
    public abstract class Bullet : Entity
    {
        public static Texture2D image;

        protected double speed;

        public Bullet(Rectangle rec, double speed, float angle) : base()
        {
            this.rec = rec;
            this.speed = speed;
            this.angle = angle;

            position = rec.Location.ToVector2();

            velocity.X = (float)(speed * Math.Sin(angle));
            velocity.Y = -(float)(speed * Math.Cos(angle));
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(image, rec, null, color, (float)angle, new Vector2(image.Width / 2, image.Height / 2), SpriteEffects.None, 0f);
            Rectangle r = new Rectangle();
        }
    }
}