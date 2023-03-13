using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galactic_Vanguard
{
    public class Comet : Entity
    {
        public static Texture2D image;
        private float angle;
        private int speed;
        SpriteEffects flip;

        public Comet() : base()
        {
            angle = (float)(300 * Math.PI / 180);
            flip = SpriteEffects.None;
            rec = new Rectangle(0, rng.Next(200,600), 10, 70);
            speed = 0;

            while(speed == 0)
            {
                speed = rng.Next(-2, 2);
            }

            velocity.X = (float)(speed * Math.Cos(angle));
            velocity.Y = (float)(Math.Abs(speed * Math.Sin(angle)));

            if (velocity.X > 0)
            {
                rec.X = Space.rec.Left - rec.Width;
            }
            if (velocity.X < 0)
            {
                rec.X = Space.rec.Right;
                flip = SpriteEffects.FlipHorizontally;
            }

            position = rec.Location.ToVector2();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(image, new Rectangle(rec.X + rec.Width / 2, rec.Y + rec.Height / 2, rec.Width, rec.Height), null, Color.White, angle, new Vector2(image.Width / 2, image.Height / 2), flip, 0f);
        }
    }
}
