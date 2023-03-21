using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galactic_Vanguard
{
    class Control
    {
        private string name;
        private Keys bind;
        private Rectangle bindRec;
        private Color boxColor;
        private bool editing;

        public static SpriteFont font;

        public Control(string name, Keys bind, int idx)
        {
            this.name = name;
            this.bind = bind;
            boxColor = Color.Gray * 0.5f;

            bindRec = new Rectangle(GameEnvironment.rec.Left + 360, 135 + 50 * idx, 140, 40);
        }

        public void Update(ref Hashtable controls)
        {
            ChangeBind(ref controls);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, name, new Vector2(GameEnvironment.rec.Left + 100, bindRec.Top + 10), Color.White);

            GraphicsHelper.DrawRec(spriteBatch, bindRec, boxColor);
            spriteBatch.DrawString(font, bind.ToString(), new Vector2(GameEnvironment.rec.Left + 380, bindRec.Top + 10), Color.White);
        }

        public void ChangeBind(ref Hashtable controls)
        {
            if(InputController.Click(bindRec))
            {
                boxColor = Color.White * 0.5f;
                editing = true;
            }

            if(editing)
            {
                if(InputController.currKeyboard.GetPressedKeys().Length == 1)
                {
                    Keys newBind = InputController.currKeyboard.GetPressedKeys()[0];
                    if (controls.ContainsValue(newBind) == false && (newBind < (Keys)48 || newBind > (Keys)57))
                    {
                        bind = newBind;

                        controls[name] = bind;
                        boxColor = Color.Gray * 0.5f;
                    }
                    
                }
            }

            if(InputController.Misclick(bindRec))
            {
                editing = false;
                boxColor = Color.Gray * 0.5f;
            }
        }
    }
}
