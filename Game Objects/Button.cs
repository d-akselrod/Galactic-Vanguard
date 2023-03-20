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
        private Color originalColor;

        public Button(Texture2D img, Rectangle rec)
        {
            this.img = img;
            this.rec = rec;
            color = Color.White;
            originalColor = color;
        }

        public Button(Texture2D img, Rectangle rec, Color color)
        {
            this.img = img;
            this.rec = rec;
            this.originalColor = color;
        }

        public void Update()
        {
            if (Collision.BoxPoint(rec, InputController.currMouse.Position))
            {
                color = originalColor * 0.6f;

                if (InputController.currMouse.LeftButton == ButtonState.Pressed && InputController.prevMouse.LeftButton == ButtonState.Released)
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
                color = originalColor;
                pressed = false;
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(img, rec, color);
        }
    }
}