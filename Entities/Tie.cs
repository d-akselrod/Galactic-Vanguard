using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galactic_Vanguard.Entities
{
    public class Tie : Entity
    {
        public static Texture2D image;
        public bool isAlive;
        private GameTimer timer;

        private Point nextPos;
        private Gun gun;

        public Tie(EnvironmentListener bulletListener)
        {
            isAlive = true;
            timer = new GameTimer();
            nextPos = new Point();
            velocity = new Vector2(1,0);
            rec = GraphicsHelper.GetCentralRectangle(1280, -100, 90, 100);
            gun = new Gun(bulletListener);
        }

        public Tie(bool isAlive)
        {
            this.isAlive = isAlive;
            rec = new Rectangle(-100, -100, 0, 0);
        }

        public void Update(Point xWingPos)
        {
            timer.Update();
            UpdatePosition();
            TurnToXWing(xWingPos);
            Shoot();

            rec.Location += velocity.ToPoint();
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            if(isAlive)
            {
                spritebatch.Draw(image, new Rectangle(rec.X + rec.Width / 2, rec.Y + rec.Height / 2, rec.Width, rec.Height), null, Color.White, (float)angle, new Vector2(image.Width / 2, image.Height / 2), SpriteEffects.None, 0f);
            }
        }

        public void UpdatePosition()
        {
            if(rec.Top < 30)
            {
                rec.Y += 1;
            }
            if(timer.GetFramesPassed() % 120*3 == 0)
            {
                nextPos.X = rng.Next(GameEnvironment.rec.Left + 50, GameEnvironment.rec.Right - 50 - rec.Width);
            }

            if(rec.X < nextPos.X)
            {
                velocity = new Vector2(1, 0);

                if(rec.X > nextPos.X)
                {
                    rec.X = nextPos.X;
                    velocity.X = 0;
                }
            }
            else if(rec.X > nextPos.X)
            {
                velocity = new Vector2(-1, 0);

                if (rec.X < nextPos.X)
                {
                    rec.X = nextPos.X;
                    velocity.X = 0;
                }
            }
            else
            {
                velocity.X = 0;
            }
        }

        public void TurnToXWing(Point xWingCentre)
        {
            Point dist = rec.Center - xWingCentre;

            if(dist.X < 0)
            {
                angle = (float)Math.Tan((float)dist.X / (float)-dist.Y);
            }
            if (dist.X > 0)
            {
                angle = (float)Math.Tan((float)dist.X / (float)-dist.Y);
            }      
        }
    
        public void Shoot()
        {
            if (timer.GetFramesPassed() % 120 == 0)
            {
                gun.TieShoot(rec, angle);
            }
        }
    }
}
