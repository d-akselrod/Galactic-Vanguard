using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Galactic_Vanguard.Game_Objects
{
    public class XWingBullet : Bullet
    {
        public XWingBullet(Rectangle rec, double speed, float angle) : base(rec, speed, angle)
        {
            this.rec = rec;
            this.speed = speed;
            this.angle = angle;

            color = Color.White;

            position = rec.Location.ToVector2();

            velocity.X = (float)(speed * Math.Sin(angle));
            velocity.Y = -(float)(speed * Math.Cos(angle));
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);
        }
    }
}
