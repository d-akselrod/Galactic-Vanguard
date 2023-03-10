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
    }
}
