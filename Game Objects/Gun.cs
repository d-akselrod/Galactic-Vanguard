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
        public static SoundEffect shootSfx;
        public static SoundEffect reloadSfx;
        private SoundEffectInstance reloadSfxInst;

        private Bullet round;
        private EnvironmentListener bulletListener;
        private int bulletSpeed;

        public int fireTimer;

        public int magSize;
        public int loadedAmmo;
        public int reloadTime;
        public bool isReloading;

        int reloadTimer;

        public Gun(EnvironmentListener bulletListener)
        {
            this.bulletListener = bulletListener;
            fireTimer = 0;
            bulletSpeed = 5;

            magSize = 20;
            loadedAmmo = magSize;
            reloadTime = 240;

            reloadSfxInst = reloadSfx.CreateInstance();
            reloadSfxInst.Volume = 1f;
        }

        public void Update()
        {
            fireTimer -= 1; 

            if (isReloading == true)
            {
                reloadTimer -= 1;
                HUD.reloadPercentage = (float)(((float)reloadTime - (float)reloadTimer) / (float)reloadTime);
                if (reloadTimer <= 0)
                {
                    isReloading = false;
                    HUD.isReloading = isReloading;
                    HUD.loadedAmmo = loadedAmmo;
                }
            }
        }

        public void Shoot(Rectangle xWingRec, float xWingRotation)
        {
            bulletListener.AddBullet(new Bullet(new Rectangle((int)(xWingRec.Center.X - xWingRec.Width / 2 * 0.8 * Math.Cos(xWingRotation)), (int)(xWingRec.Center.Y - xWingRec.Height / 2 * 0.8 * Math.Sin(xWingRotation)), 4, 36), bulletSpeed, xWingRotation, Color.SkyBlue));
            bulletListener.AddBullet(new Bullet(new Rectangle((int)(xWingRec.Center.X + xWingRec.Width / 2 * 0.8 * Math.Cos(xWingRotation)), (int)(xWingRec.Center.Y + xWingRec.Height / 2 * 0.8 * Math.Sin(xWingRotation)), 4, 36), bulletSpeed, xWingRotation, Color.SkyBlue));

            fireTimer = 30;
            loadedAmmo -= 2;
            shootSfx.CreateInstance().Play();

            HUD.loadedAmmo = loadedAmmo;
        }

        public void Reload(ref int extraAmmo)
        {
            isReloading = true;
            HUD.isReloading = isReloading;
            reloadTimer = reloadTime;
            reloadSfxInst.Play();

            while (extraAmmo > 0)
            {
                if (loadedAmmo < magSize)
                {
                    loadedAmmo += 1;
                    extraAmmo -= 1;
                }
                else
                {
                    break;
                }
            }
            HUD.externalAmmo = extraAmmo;
        }
    }
}