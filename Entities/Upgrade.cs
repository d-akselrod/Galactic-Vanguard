using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galactic_Vanguard.Entities
{
    public class Upgrade : Entity
    {
        public static Texture2D image;

        private int baseLine;
        public Upgrade()
        {
            int radius = 15;
            baseLine = rng.Next(GameEnvironment.rec.Left + 150, GameEnvironment.rec.Right - 150 - 2 * 10);

            rec = new Rectangle(0, GameEnvironment.rec.Top - 2 * radius, radius * 2, radius * 2);
            velocity = new Vector2(0, 0.4f);
            position = rec.Location.ToVector2();
        }
        public override void Update()
        {
            position.Y += velocity.Y;
            position.X = (float)(130 * Math.Sin(position.Y/40) + baseLine);

            rec.Location = position.ToPoint();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, rec, Color.Goldenrod);
        }
    }    
}
