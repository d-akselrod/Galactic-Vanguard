using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galactic_Vanguard
{
    public class Meteor : Entity
    {
        private float angle;
        private int radius;
        private int angleDirection;
        private float angularVelocity;

        public static Texture2D image;

        public Meteor() : base()
        {
            radius = rng.Next(14, 30);
            angle = rng.Next(0,360)* rng.Next(0, 360);

            rec = new Rectangle(rng.Next(Space.rec.Left + 20, Space.rec.Right - 20 - 2 * radius), Space.rec.Top - 2 * radius, radius * 2, radius * 2);
            velocity = new Vector2(rng.Next(-5,5)/10f, 2);
            position = rec.Location.ToVector2();
            
            if(Convert.ToInt32(rng.Next(0, 2)) == 0)
            {
                angleDirection = -1;
            }
            else
            {
                angleDirection = 1;
            }

            angularVelocity = angleDirection * (float)(Math.PI / (180));

            collisionEnabled = true;
        }

        public override void Update()
        {
            position += velocity;
            rec.Location = position.ToPoint();

            angle += angularVelocity;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, new Rectangle(rec.X + rec.Width / 2, rec.Y + rec.Height / 2, rec.Width, rec.Height), null, Color.White, (float)angle, new Vector2(image.Width / 2, image.Height / 2), SpriteEffects.None, 0f);

        }
    }
}
