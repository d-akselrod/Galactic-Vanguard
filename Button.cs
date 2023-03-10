using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Galactic_Vanguard
{
    public class Button
    {
        private Texture2D img;
        public bool pressed;
        private Rectangle rec;
        private Color color;
        private double rotation;

        public Button(Texture2D img, Rectangle rec)
        {
            this.img = img;
            this.rec = rec;
            color = Color.White;
        }

        public void Update(InputController input)
        {
            if (Collision.BoxPoint(rec, input.currMouse.Position))
            {
                color = Color.White * 0.85f;

                if (input.currMouse.LeftButton == ButtonState.Pressed && input.prevMouse.LeftButton == ButtonState.Released)
                {
                    pressed = true;
                }
                else
                {
                    pressed = false;
                }
            }
            else
            {
                color = Color.White;
                pressed = false;
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(img, rec, color);
        }
    }
}