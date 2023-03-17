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

        public Tie()
        {
            isAlive = true;
            timer = new GameTimer();
            nextPos = new Point();
            velocity = new Vector2(1,0);
            rec = GraphicsHelper.GetCentralRectangle(1280, -100, 90, 100);
        }

        public Tie(bool isAlive)
        {
            this.isAlive = isAlive;
        }

        public override void Update()
        {
            timer.Update();
            UpdatePosition();

            rec.Location += velocity.ToPoint();
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(image, rec, Color.White);
        }

        public void UpdatePosition()
        {
            if(rec.Top < 30)
            {
                rec.Y += 1;
            }
            if(timer.GetFramesPassed() % 120*5 == 0)
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
    }
}
