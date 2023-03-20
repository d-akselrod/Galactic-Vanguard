using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Galactic_Vanguard.Game_Objects
{
    public class TieBullet : Bullet
    {
        public TieBullet(Rectangle rec, double speed, float angle) : base(rec, speed, angle)
        {
            this.rec = rec;
            this.speed = speed;
            this.angle = angle;

            color = Color.DarkOrange;

            position = rec.Location.ToVector2();

            velocity.X = -(float)(speed * Math.Sin(angle));
            velocity.Y = (float)(speed * Math.Cos(angle));
        }

        public override void Update()
        {
            position += velocity;
            rec.Location = position.ToPoint();
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);
        }
    }
}
