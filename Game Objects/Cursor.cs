using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galactic_Vanguard.Game_Objects
{
    class Cursor
    {
        public static Texture2D img;
        private Rectangle rec;
        public Color color;
        public bool visible;

        public Cursor(Rectangle rec)
        {
            this.rec = rec;
            visible = true;
            color = Color.White;
        }

        public void Update(Point mousePos)
        {
            rec.Location = mousePos;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(img, rec, color);
        }
    }
}
