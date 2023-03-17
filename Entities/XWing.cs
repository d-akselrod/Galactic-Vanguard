using Galactic_Vanguard;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Galactic_Warfare
{
    public class XWing : Entity
    {
        public static Texture2D image;
        private float rotation;
        private float rotationalVelocity;
        private int bulletSpeed;
        private Rectangle gameRec;
        private Gun gun;
        public static int externalAmmo;

        public static double health;
        public static double maxHealth;
        public static double shield;
        public static double maxShield;

        public static float linearSpeed;
        public static float rotationalSpeed;

        public static bool alive;

        private int fireTimer = 0;

        private List<Bullet> bullets;
        private EnvironmentListener bulletListener;

        public List<Rectangle> collisionRecs;
        private GameTimer timer;

        public Stats stats;

        public XWing(int bulletSpeed, Color color, Rectangle gameRec, EnvironmentListener bulletListener) : base()
        {
            this.color = color;
            this.bulletSpeed = bulletSpeed;
            this.gameRec = gameRec;
            rotation = 0;
            gun = new Gun(bulletListener);
            externalAmmo = 100;
            stats = new Stats();
            timer = new GameTimer();

            maxHealth = 200 * (Stats.healthLvl + 1);
            health = maxHealth;

            maxShield = 200 * (Stats.healthLvl + 1);
            shield = maxShield;

            alive = true;

            linearSpeed = 1;
            rotationalSpeed = 1;

            rec = GraphicsHelper.GetCentralRectangle(1280, 600, 80, 80);
            position = rec.Location.ToVector2();

            collisionRecs = new List<Rectangle>();
            collisionRecs.Add(new Rectangle(rec.Left, (int)(rec.Bottom - rec.Height * 0.6), (int)(rec.Width * 0.15), (int)(rec.Height * 0.6)));
            collisionRecs.Add(new Rectangle((int)(rec.Right - rec.Width * 0.15), (int)(rec.Bottom - rec.Height * 0.6), (int)(rec.Width * 0.15), (int)(rec.Height * 0.6)));
            collisionRecs.Add(new Rectangle((int)(rec.Center.X - rec.Width * 0.08), rec.Top, (int)(rec.Width * 0.16), rec.Height));
            collisionRecs.Add(new Rectangle(rec.Left, (int)(rec.Top + rec.Height * 0.7), (int)(rec.Width), (int)(rec.Height * 0.3)));

            velocity = new Vector2(1, 0);
            HUD.externalAmmo = externalAmmo;
        }

        public void SetBulletSpeed(int bulletSpeed)
        {
            this.bulletSpeed = bulletSpeed;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(image, new Rectangle(rec.X + rec.Width / 2, rec.Y + rec.Height / 2, rec.Width, rec.Height), null, color, (float)rotation, new Vector2(image.Width / 2, image.Height / 2), SpriteEffects.None, 0f);
        }

        public override void Update()
        {
            MovementControl(InputController.currKeyboard);
            GunControl(InputController.currKeyboard);
            StatsControl(InputController.currKeyboard, InputController.prevKeyboard);
            timer.Update();
            RegenShield();

            position += velocity;
            rec.Location = position.ToPoint();
            rotation += rotationalVelocity;
        }

        private void MovementControl(KeyboardState keyboard)
        {
            if ((keyboard.IsKeyDown(Keys.D) && keyboard.IsKeyDown(Keys.A))
                || (!keyboard.IsKeyDown(Keys.D) && !keyboard.IsKeyDown(Keys.A)))
            {
                velocity.X = 0;
            }
            else if (keyboard.IsKeyDown(Keys.A) && rec.Left > gameRec.Left + 50)
            {
                velocity.X = -linearSpeed;
            }
            else if (keyboard.IsKeyDown(Keys.D) && rec.Right < gameRec.Right - 50)
            {
                velocity.X = linearSpeed;
            }
            else
            {
                velocity.X = 0;
            }

            if ((keyboard.IsKeyDown(Keys.Right) && keyboard.IsKeyDown(Keys.Left))
                || (!keyboard.IsKeyDown(Keys.Right) && !keyboard.IsKeyDown(Keys.Left)))
            {
                rotationalVelocity = 0;
            }
            else if (keyboard.IsKeyDown(Keys.Left))
            {
                rotationalVelocity = rotationalSpeed * ((float)(-1 * Math.PI / 180));
            }
            else if (keyboard.IsKeyDown(Keys.Right))
            {
                rotationalVelocity = rotationalSpeed * (float)(1 * Math.PI / 180);
            }
            else
            {
                rotationalVelocity = 0;
            }
        }

        private void GunControl(KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.Space) && gun.fireTimer <= 0 && gun.loadedAmmo > 0 && gun.isReloading == false)
            {
                gun.Shoot(rec, rotation);
            }

            if(keyboard.IsKeyDown(Keys.R) && gun.isReloading == false && gun.loadedAmmo < Gun.magSize && externalAmmo > 0)
            {
                gun.Reload(ref externalAmmo);
            }

            gun.Update();
        }

        private void StatsControl(KeyboardState currKeyboard, KeyboardState prevKeyboard)
        {
            if (Stats.skillPoints > 0)
            {
                if (currKeyboard.IsKeyDown(Keys.D1) && currKeyboard != prevKeyboard)
                {
                    stats.UpgradeHealth();
                }
                if (currKeyboard.IsKeyDown(Keys.D2) && currKeyboard != prevKeyboard)
                {
                    stats.UpgradeShield();
                }
                if (currKeyboard.IsKeyDown(Keys.D3) && currKeyboard != prevKeyboard)
                {
                    stats.UpgradeGun();
                }
                if (currKeyboard.IsKeyDown(Keys.D4) && currKeyboard != prevKeyboard)
                {
                    stats.UpgradeAmmo();
                }
                if (currKeyboard.IsKeyDown(Keys.D5) && currKeyboard != prevKeyboard)
                {
                    stats.UpgradeEngine();
                }
            }  
        }

        public bool Collides(Rectangle rec2)
        {
            collisionRecs[0] = (new Rectangle(rec.Left, (int)(rec.Bottom - rec.Height * 0.6), (int)(rec.Width * 0.15), (int)(rec.Height * 0.6)));
            collisionRecs[1] = (new Rectangle((int)(rec.Right - rec.Width * 0.15), (int)(rec.Bottom - rec.Height * 0.6), (int)(rec.Width * 0.15), (int)(rec.Height * 0.6)));
            collisionRecs[2] = (new Rectangle((int)(rec.Center.X - rec.Width * 0.08), rec.Top, (int)(rec.Width * 0.16), rec.Height));
            collisionRecs[3] = (new Rectangle(rec.Left, (int)(rec.Top + rec.Height * 0.7), (int)(rec.Width), (int)(rec.Height * 0.3)));

            foreach (Rectangle rec1 in collisionRecs)
            {
                if (rec2.Intersects(rec1))
                {
                    return true;
                }
            }
            return false;
        }
    
        public void RegenShield()
        {
            if(timer.GetFramesPassed() % 12 == 0)
            {
                if(XWing.shield < XWing.maxShield)
                {
                    shield += 0.1;

                    if (XWing.shield > XWing.maxShield)
                    {
                        XWing.shield = XWing.maxShield;
                    }
                }
            }
        }
    }
}