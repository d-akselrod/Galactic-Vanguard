using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galactic_Vanguard
{
    public class Gun
    {
        public static SoundEffect soundEffect;

        private Bullet round;
        private EnvironmentListener bulletListener;
        private int bulletSpeed;
        public int fireTimer;
        private int magSize;
        private int loadedAmmo;

        public Gun(EnvironmentListener bulletListener)
        {
            this.bulletListener = bulletListener;
            fireTimer = 0;
            bulletSpeed = 5;
        }

        public void Update()
        {
            fireTimer -= 1;
        }

        public void Shoot(Rectangle xWingRec, float xWingRotation)
        {
            bulletListener.AddBullet(new Bullet(new Rectangle((int)(xWingRec.Center.X - xWingRec.Width / 2 * 0.8 * Math.Cos(xWingRotation)), (int)(xWingRec.Center.Y - xWingRec.Height / 2 * 0.8 * Math.Sin(xWingRotation)), 4, 36), bulletSpeed, xWingRotation, Color.SkyBlue));
            bulletListener.AddBullet(new Bullet(new Rectangle((int)(xWingRec.Center.X + xWingRec.Width / 2 * 0.8 * Math.Cos(xWingRotation)), (int)(xWingRec.Center.Y + xWingRec.Height / 2 * 0.8 * Math.Sin(xWingRotation)), 4, 36), bulletSpeed, xWingRotation, Color.SkyBlue));

            fireTimer = 10;
            soundEffect.CreateInstance().Play();
        }
    }
}