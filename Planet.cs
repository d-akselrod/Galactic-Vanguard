using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace Galactic_Vanguard
{
    class Planet : Entity
    {
        private int radius;
        private Color color;
        private float angle;

        public static List<Texture2D> images = new List<Texture2D>();
        private static int imgIdx = 0;
        private Texture2D image;

        public Planet() : base()
        {
            collisionEnabled = false;
            radius = rng.Next(200, 400);
            collisionEnabled = false;

            velocity = new Vector2(0,0.4f);
            rec = new Rectangle(rng.Next(Space.rec.Left - radius, Space.rec.Right - radius), Space.rec.Top - 2 * radius, radius * 2, radius * 2);
            position = rec.Location.ToVector2();
            angle = (float)(rng.Next(0, 360)*Math.PI/180);
            image = images[imgIdx % images.Count];
            imgIdx += 1;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(image, new Rectangle(rec.X + rec.Width / 2, rec.Y + rec.Height / 2, rec.Width, rec.Height), null, Color.White, (float)angle, new Vector2(image.Width / 2, image.Height / 2), SpriteEffects.None, 0f);
        }
    }
}
