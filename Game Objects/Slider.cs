using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galactic_Vanguard.Game_Objects
{
    public class Slider
    {
        private Texture2D barImg;
        private Texture2D adjustorImg;

        private Rectangle barRec;
        private Rectangle adjustorRec;

        private bool held;

        public Slider(Texture2D barImg, Texture2D adjustorImg, Rectangle barRec)
        {
            this.barImg = barImg;
            this.adjustorImg = adjustorImg;
            this.barRec = barRec;

            adjustorRec = new Rectangle((int)(barRec.Center.X - (barRec.Height + 16) / 2), (int)(barRec.Center.Y - (barRec.Height + 16) / 2), barRec.Height + 16, barRec.Height + 16);
        }

        public void Update()
        {
            Point mousePos = InputController.currMouse.Position;

            if (Collision.BoxPoint(adjustorRec, InputController.currMouse.Position) && InputController.currMouse.LeftButton == ButtonState.Pressed && mousePos.X >= barRec.Left && mousePos.X <= barRec.Right)
            {
                held = true;
            }
            else if(InputController.currMouse.LeftButton == ButtonState.Released)
            {
                held = false;
            }

            if (held)
            {                
                if (mousePos.X >= barRec.Left && mousePos.X <= barRec.Right)
                {
                    adjustorRec.X = mousePos.X - adjustorRec.Width / 2;
                }
                else if(mousePos.X < barRec.Left)
                {
                    adjustorRec.X = barRec.Left - adjustorRec.Width / 2;
                }
                else if (mousePos.X > barRec.Right)
                {
                    adjustorRec.X = barRec.Right - adjustorRec.Width / 2;
                }
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(barImg, barRec, Color.White);
            spritebatch.Draw(adjustorImg, adjustorRec, Color.White);
        }

        public float GetValue()
        {
            float value = ((float)adjustorRec.Center.X - (float)barRec.Left) / (float)barRec.Width;

            if (value > 1)
            {
                return 1;
            }
            else if (value < 0)
            {
                return 0;
            }
            else
            {
                return value;
            }
        }
    }
}
