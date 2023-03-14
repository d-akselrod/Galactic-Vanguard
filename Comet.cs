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

        public Comet() : base()
        {
            rec = new Rectangle(0, rng.Next(-40,720), 6, 40);
            velocity.X = (rng.Next(2) * 2 - 1 )*7;       
            velocity.Y = rng.Next(1,4);


            if (velocity.X > 0)
            {
                rec.X = Space.rec.Left - rec.Width;

                angle = (float)Math.Atan(velocity.Y / velocity.X) + (float)(Math.PI / 2);
            }
            if (velocity.X < 0)
            {
                rec.X = Space.rec.Right;

                angle = -(float)Math.Atan(velocity.Y / Math.Abs(velocity.X)) - (float)(Math.PI / 2);
            }

            position = rec.Location.ToVector2();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(image, new Rectangle(rec.X + rec.Width / 2, rec.Y + rec.Height / 2, rec.Width, rec.Height), null, Color.White * 0.4f, angle, new Vector2(image.Width / 2, image.Height / 2), SpriteEffects.None, 0f);
        }
    }
}
