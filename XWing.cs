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
        private double rotation;
        private double rotationalVelocity;
        private int bulletSpeed;
        private Rectangle gameRec;

        private int fireTimer = 0;

        private List<Bullet> bullets;
        private EnvironmentListener bulletListener;

        public XWing(int bulletSpeed, Color color, Rectangle gameRec, EnvironmentListener bulletListener)
        {
            this.color = color;
            this.bulletSpeed = bulletSpeed;
            this.gameRec = gameRec;
            rotation = 0;

            this.bulletListener = bulletListener;

            rec = GraphicsHelper.GetCentralRectangle(1280, 600, 80, 80);
            position = rec.Location.ToVector2();

            velocity = new Vector2(1, 0);

            bullets = new List<Bullet>();
        }

        public void SetBulletSpeed(int bulletSpeed)
        {
            this.bulletSpeed = bulletSpeed;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(image, new Rectangle(rec.X + rec.Width / 2, rec.Y + rec.Height / 2, rec.Width, rec.Height), null, color, (float)rotation, new Vector2(image.Width / 2, image.Height / 2), SpriteEffects.None, 0f);

            foreach (Bullet bullet in bullets)
            {
                bullet.Draw(spritebatch);
            }
        }

        public override void Update()
        {
            InputController input = InputController.GetInput();
            MovementControl(input.currKeyboard);
            BulletControl(input.currKeyboard);

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
                velocity.X = -2;
            }
            else if (keyboard.IsKeyDown(Keys.D) && rec.Right < gameRec.Right - 50)
            {
                velocity.X = 2;
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
                rotationalVelocity = -1 * Math.PI / 180;
            }
            else if (keyboard.IsKeyDown(Keys.Right))
            {
                rotationalVelocity = 1 * Math.PI / 180;
            }
            else
            {
                rotationalVelocity = 0;
            }
        }

        private void BulletControl(KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.Space) && fireTimer >= 120)
            {
                bulletListener.AddBullet(new Bullet(new Rectangle((int)(rec.Center.X - rec.Width / 2 * 0.8 * Math.Cos(rotation)), (int)(rec.Center.Y - rec.Height / 2 * 0.8 * Math.Sin(rotation)), 4, 36), bulletSpeed, rotation, Color.SkyBlue));
                bulletListener.AddBullet(new Bullet(new Rectangle((int)(rec.Center.X + rec.Width / 2 * 0.8 * Math.Cos(rotation)), (int)(rec.Center.Y + rec.Height / 2 * 0.8 * Math.Sin(rotation)), 4, 36), bulletSpeed, rotation, Color.SkyBlue));

                fireTimer = 0;
            }

            fireTimer += 1;

            foreach (Bullet bullet in bullets)
            {
                bullet.Update();

                if (bullet.GetRec().Y < -100 || bullet.GetRec().Y >= 1300 ||
                    bullet.GetRec().X >= 1300 || bullet.GetRec().X <= -100)
                {
                    bullets.Remove(bullet);
                    break;
                }
            }
        }
    }
}
