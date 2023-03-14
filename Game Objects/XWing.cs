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

        private int fireTimer = 0;

        private List<Bullet> bullets;
        private EnvironmentListener bulletListener;

        public XWing(int bulletSpeed, Color color, Rectangle gameRec, EnvironmentListener bulletListener)
        {
            this.color = color;
            this.bulletSpeed = bulletSpeed;
            this.gameRec = gameRec;
            rotation = 0;
            gun = new Gun(bulletListener);

            rec = GraphicsHelper.GetCentralRectangle(1280, 600, 80, 80);
            position = rec.Location.ToVector2();

            velocity = new Vector2(1, 0);
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
            InputController input = InputController.GetInput();
            MovementControl(input.currKeyboard);
            GunControl(input.currKeyboard);

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
                rotationalVelocity = (float)(-1 * Math.PI / 180);
            }
            else if (keyboard.IsKeyDown(Keys.Right))
            {
                rotationalVelocity = (float)(1 * Math.PI / 180);
            }
            else
            {
                rotationalVelocity = 0;
            }
        }

        private void GunControl(KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.Space) && gun.fireTimer <= 0)
            {
                gun.Shoot(rec, rotation);
            }
            gun.Update();
        }
    }
}