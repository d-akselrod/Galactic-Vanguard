using Galactic_Warfare;
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
        public static Texture2D reloadBarImg;
        public static Texture2D extraAmmoImg;
        public static SpriteFont font;

        public static int loadedAmmo;
        public static int externalAmmo;
        private List<Rectangle> ammoRecs;

        private Rectangle reloadRec;
        private Rectangle reloadSrcRec;
        private Rectangle extraAmmoRec;

        public static bool isReloading;
        public static int reloadTime;
        public static int reloadTimer;
        public static float reloadPercentage;

        private string reloadMsg;
        private Color reloadMsgColor;

        private GameTimer timer;

        public HUD()
        {
            timer = new GameTimer();
            ammoRecs = new List<Rectangle>();
            loadedAmmo = 20;

            for (int i = 0; i <50; i++)
            {
                ammoRecs.Add(new Rectangle(GameEnvironment.rec.Left - 20 - 25*(i%10), 630 - 60*((int)(i/10)), 10, 45));
            }

            reloadRec = new Rectangle(20, GameEnvironment.rec.Bottom - 30, GameEnvironment.rec.Left - 40, 20);
            extraAmmoRec = new Rectangle(20, GameEnvironment.rec.Bottom - 116, 60, 70);

            reloadMsg = "PRESS R TO RELOAD";
            reloadMsgColor = Color.White;
        }

        public void Update()
        {
            timer.Update();

            reloadMsgColor = Color.White * (float)(0.5*Math.Sin(timer.GetFramesPassed()/20)+0.5);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawAmmo();
            DrawReloadBar();
            DrawExtraAmmo();
            DrawReloadText();

            void DrawReloadBar()
            {
                if(isReloading)
                {
                    GraphicsHelper.DrawRec(spriteBatch, new Rectangle(20, extraAmmoRec.Y + 80, 310, 24), Color.Black);
                    //GraphicsHelper.DrawRec(spriteBatch, new Rectangle(94, extraAmmoRec.Y + 49, (int)(230*reloadPercentage), 16), Color.LightBlue);
                    spriteBatch.Draw(reloadBarImg, new Rectangle(24, extraAmmoRec.Y + 84, (int)(302 * reloadPercentage), 16), Color.White);
                }
            }

            void DrawAmmo()
            {            
                for(int i = 0; i < loadedAmmo; i++)
                {
                    spriteBatch.Draw(bulletImg, ammoRecs[i], Color.White);
                }             
            }

            void DrawExtraAmmo()
            {
                spriteBatch.Draw(extraAmmoImg, extraAmmoRec, Color.White);
                spriteBatch.DrawString(font, externalAmmo.ToString("d3"), new Vector2(extraAmmoRec.Center.X - font.MeasureString(externalAmmo.ToString(("d3"))).X/2, extraAmmoRec.Y + 15), Color.White);
            }

            void DrawReloadText()
            {
                if(loadedAmmo == 0 && externalAmmo > 0 && isReloading == false)
                {
                    spriteBatch.DrawString(font, reloadMsg, new Vector2(90, extraAmmoRec.Y + 40), reloadMsgColor);
                }
            }
        }
    }
}
