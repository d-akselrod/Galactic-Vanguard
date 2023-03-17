using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Galactic_Vanguard.Entities
{
    public class AmmoKit : Entity
    {
        public static Texture2D image;

        private int baseLine;

        public AmmoKit() : base()
        {
            int radius = 15;
            baseLine = rng.Next(GameEnvironment.rec.Left + 150, GameEnvironment.rec.Right - 150 - 2 * 10);

            rec = new Rectangle(0, GameEnvironment.rec.Top - 25, 40, 25);
            velocity = new Vector2(0, 1f);
            position = rec.Location.ToVector2();
        }

        public override void Update()
        {
            position.Y += velocity.Y;
            position.X = (float)(80 * Math.Sin(position.Y / 40) + baseLine);

            rec.Location = position.ToPoint();
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(image, rec, Color.White);
        }
    }
}
