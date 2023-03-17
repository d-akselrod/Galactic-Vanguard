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
        public static Texture2D reloadBarImg;
        public static Texture2D extraAmmoImg;
        
        public static SpriteFont font;

        public static Texture2D ammoImg;
        public static Texture2D healthImg;
        public static Texture2D shieldImg;
        public static Texture2D engineImg;
        public static Texture2D gunImg;
        public static Texture2D emptyImg;

        private Rectangle ammoRec;
        private Rectangle healthRec;
        private Rectangle shieldRec;
        private Rectangle engineRec;
        private Rectangle gunRec;

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
        private Stats stats;

        public HUD()
        {
            timer = new GameTimer();
            ammoRecs = new List<Rectangle>();
            loadedAmmo = 20;

            for (int i = 0; i <80; i++)
            {
                ammoRecs.Add(new Rectangle(GameEnvironment.rec.Left - 20 - 25*(i%10), 630 - 60*((int)(i/10)), 10, 45));
            }

            reloadRec = new Rectangle(20, GameEnvironment.rec.Bottom - 30, GameEnvironment.rec.Left - 40, 20);
            extraAmmoRec = new Rectangle(20, GameEnvironment.rec.Bottom - 116, 60, 70);

            reloadMsg = "PRESS R TO RELOAD";
            reloadMsgColor = Color.White;

            healthRec = new Rectangle(GameEnvironment.rec.Right + 20, GameEnvironment.rec.Bottom - 80, 50, 35);
            shieldRec = new Rectangle(GameEnvironment.rec.Right + 82, GameEnvironment.rec.Bottom - 80, 50, 35);
            gunRec = new Rectangle(GameEnvironment.rec.Right + 144, GameEnvironment.rec.Bottom - 80, 50, 35);
            ammoRec = new Rectangle(GameEnvironment.rec.Right + 206, GameEnvironment.rec.Bottom - 80, 50, 35);
            engineRec = new Rectangle(GameEnvironment.rec.Right + 268, GameEnvironment.rec.Bottom - 80, 50, 35);
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
            DrawUpgrades();
            DrawStats();
            DrawHPBars();

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

            void DrawUpgrades()
            {
                spriteBatch.Draw(healthImg, healthRec, Color.Crimson);
                spriteBatch.Draw(shieldImg, shieldRec, Color.DeepSkyBlue);
                spriteBatch.Draw(gunImg, gunRec, Color.Green);
                spriteBatch.Draw(ammoImg, ammoRec, Color.Yellow);
                spriteBatch.Draw(engineImg, engineRec, Color.WhiteSmoke);

                for (int i = 0; i < Stats.healthLvl; i++)
                {
                    spriteBatch.Draw(healthImg, new Rectangle(healthRec.Left, healthRec.Top - 40 * (i + 1), healthRec.Width, healthRec.Height), Color.Crimson);
                }

                for (int i = 0; i < Stats.shieldLvl; i++)
                {
                    spriteBatch.Draw(shieldImg, new Rectangle(shieldRec.Left, shieldRec.Top - 40 * (i + 1), shieldRec.Width, shieldRec.Height), Color.DeepSkyBlue);
                }

                for (int i = 0; i < Stats.gunLvl; i++)
                {
                    spriteBatch.Draw(gunImg, new Rectangle(gunRec.Left, gunRec.Top - 40 * (i + 1), gunRec.Width, gunRec.Height), Color.Green);
                }

                for (int i = 0; i < Stats.ammoLvl; i++)
                {
                    spriteBatch.Draw(ammoImg, new Rectangle(ammoRec.Left, ammoRec.Top - 40 * (i + 1), ammoRec.Width, ammoRec.Height), Color.Yellow);
                }

                for (int i = 0; i < Stats.engineLvl; i++)
                {
                    spriteBatch.Draw(engineImg, new Rectangle(engineRec.Left, engineRec.Top - 40 * (i + 1), engineRec.Width, engineRec.Height), Color.WhiteSmoke);
                }

                //Draw Empty Upgrades
                for (int i = Stats.healthLvl; i < Stats.maxLevel; i++)
                {
                    spriteBatch.Draw(emptyImg, new Rectangle(healthRec.Left, healthRec.Top - 40*(i+1), healthRec.Width, healthRec.Height), Color.White * 0.2f);
                }

                for (int i = Stats.shieldLvl; i < Stats.maxLevel; i++)
                {
                    spriteBatch.Draw(emptyImg, new Rectangle(shieldRec.Left, shieldRec.Top - 40 * (i + 1), shieldRec.Width, shieldRec.Height), Color.White * 0.2f);
                }

                for (int i = Stats.gunLvl; i < Stats.maxLevel; i++)
                {
                    spriteBatch.Draw(emptyImg, new Rectangle(gunRec.Left, gunRec.Top - 40 * (i + 1), gunRec.Width, gunRec.Height), Color.White * 0.2f);
                }

                for (int i = Stats.ammoLvl; i < Stats.maxLevel; i++)
                {
                    spriteBatch.Draw(emptyImg, new Rectangle(ammoRec.Left, ammoRec.Top - 40 * (i + 1), ammoRec.Width, ammoRec.Height), Color.White * 0.2f);
                }

                for (int i = Stats.engineLvl; i < Stats.maxLevel; i++)
                {
                    spriteBatch.Draw(emptyImg, new Rectangle(engineRec.Left, engineRec.Top - 40 * (i + 1), engineRec.Width, engineRec.Height), Color.White * 0.2f);
                }
            }
        
            void DrawStats()
            {
                if (Stats.skillPoints >=  0 && Stats.level != Stats.maxLevel * 5)
                {
                    spriteBatch.DrawString(font, "SKILL POINTS: " + Stats.skillPoints.ToString(), new Vector2(GameEnvironment.rec.Right + 60, GameEnvironment.rec.Bottom - 30), Color.White);
                }
                else if (Stats.level == Stats.maxLevel * 5)
                {
                    spriteBatch.DrawString(font, "MAX UPGRADES", new Vector2(GameEnvironment.rec.Right + 100, GameEnvironment.rec.Bottom - 30), Color.White);
                }
                //spriteBatch.DrawString(font, reloadMsg, new Vector2(90, extraAmmoRec.Y + 40), reloadMsgColor);
            }

            void DrawHPBars()
            {
                //GraphicsHelper.DrawRec(spriteBatch, new Rectangle(extraAmmoRec.Left, 100, 20, 480), Color.Black);
                //GraphicsHelper.DrawRec(spriteBatch, new Rectangle(extraAmmoRec.Left + 3, 103, 13, 474), Color.Crimson);

                GraphicsHelper.DrawRec(spriteBatch, extraAmmoRec.Left, 580, 20, 128 + 45 * Stats.healthLvl + 6, Color.White);
                GraphicsHelper.DrawRec(spriteBatch, extraAmmoRec.Left + 3, 577, 14, 128 + 45*Stats.healthLvl, Color.Crimson);

                if(XWing.health > 0)
                {
                    GraphicsHelper.DrawRec(spriteBatch, new Rectangle(extraAmmoRec.Left + 3, 577 - (128 + 45 * Stats.healthLvl), 14, (int)(((XWing.maxHealth - XWing.health) / XWing.maxHealth) * (45 * Stats.healthLvl + 128))), Color.Black);
                }
                else
                {
                    GraphicsHelper.DrawRec(spriteBatch, new Rectangle(extraAmmoRec.Left + 3, 577 - (128 + 45 * Stats.healthLvl), 14, (45 * Stats.healthLvl + 128)), Color.Black);
                }

                GraphicsHelper.DrawRec(spriteBatch, extraAmmoRec.Right - 20, 580, 20, 128 + 45 * Stats.shieldLvl + 6, Color.White);
                GraphicsHelper.DrawRec(spriteBatch, extraAmmoRec.Right - 17, 577, 14, 128 + 45 * Stats.shieldLvl, Color.SteelBlue);
                GraphicsHelper.DrawRec(spriteBatch, new Rectangle(extraAmmoRec.Right - 17, 577 - (128 + 45 * Stats.shieldLvl), 14, (int)(((XWing.maxShield - XWing.shield) / XWing.maxShield) * (45 * Stats.shieldLvl + 128))), Color.Black);
            }
        }
    }
}
