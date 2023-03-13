using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Galactic_Vanguard
{
    public class GraphicsHelper
    {
        public static Rectangle GetCentralRectangle(int screenWidth, int y, int imgWidth, int imgHeight)
        {
            return new Rectangle(screenWidth / 2 - imgWidth / 2, y, imgWidth, imgHeight);
        }

        public static Rectangle DeflateRectangle(Rectangle rec, float scale)
        {
            return new Rectangle((int)(rec.X + (rec.Width - rec.Width * scale) / 2), (int)(rec.Y + (rec.Height - rec.Height * scale) / 2), (int)(rec.Width * scale), (int)(rec.Height * scale));
        }
    }
}
