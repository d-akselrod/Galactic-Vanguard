using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galactic_Vanguard
{
    class SpaceJunk : Entity
    {
        public static Texture2D image;

        public SpaceJunk() : base()
        {
            int radius = rng.Next(40, 70);
            rec = new Rectangle(0, 0, radius, radius);
            angle = rng.Next(0, 360);
            position = new Vector2(rng.Next(Space.rec.Left + 10, Space.rec.Right - 10 - rec.Width), -rec.Height);
            velocity = new Vector2(0, 1);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(image, new Rectangle(rec.X + rec.Width / 2, rec.Y + rec.Height / 2, rec.Width, rec.Height), null, Color.White, angle, new Vector2(image.Width / 2, image.Height / 2), SpriteEffects.None, 0f);
        }
    }
}
