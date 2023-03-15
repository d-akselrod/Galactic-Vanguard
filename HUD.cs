using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace Galactic_Vanguard
{
    public class HUD
    {
        public static Texture2D bulletImg;
        public static Texture2D healthImg;

        public static int loadedAmmo;
        public static int externalAmmo;
        public static int health;

        private List<Rectangle> ammoRecs;

        public HUD()
        {
            ammoRecs = new List<Rectangle>();
            loadedAmmo = 20;

            for (int i = 0; i <50; i++)
            {
                ammoRecs.Add(new Rectangle(GameEnvironment.rec.Left - 20 - 25*(i%10), 660 - 60*((int)(i/10)), 10, 45));
            }
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawAmmo();

            void DrawAmmo()
            {
                for(int i = 0; i < loadedAmmo; i++)
                {
                    spriteBatch.Draw(bulletImg, ammoRecs[i], Color.White);
                }             
            }
        }
    }
}
