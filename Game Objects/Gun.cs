using Galactic_Vanguard.Game_Objects;
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

        public static int magSize;
        public static int reloadTime;
        public int loadedAmmo;
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

            HUD.loadedAmmo = loadedAmmo;
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

        public void XWingShoot(Rectangle shipRec, float rotation)
        {
            bulletListener.AddBullet(new XWingBullet(new Rectangle((int)(shipRec.Center.X - shipRec.Width / 2 * 0.8 * Math.Cos(rotation)), (int)(shipRec.Center.Y - shipRec.Height / 2 * 0.8 * Math.Sin(rotation)), 4, 36), bulletSpeed, rotation));
            bulletListener.AddBullet(new XWingBullet(new Rectangle((int)(shipRec.Center.X + shipRec.Width / 2 * 0.8 * Math.Cos(rotation)), (int)(shipRec.Center.Y + shipRec.Height / 2 * 0.8 * Math.Sin(rotation)), 4, 36), bulletSpeed, rotation));

            fireTimer = 30;
            loadedAmmo -= 2;
            shootSfx.CreateInstance().Play();

            HUD.loadedAmmo = loadedAmmo;
        }

        public void TieShoot(Rectangle shipRec, float rotation)
        {
            bulletListener.AddBullet(new TieBullet(new Rectangle((int)(shipRec.Center.X - shipRec.Width / 2 * 0.8 * Math.Cos(rotation)), (int)(shipRec.Center.Y - shipRec.Height / 2 * 0.8 * Math.Sin(rotation)), 4, 36), bulletSpeed, rotation));
            bulletListener.AddBullet(new TieBullet(new Rectangle((int)(shipRec.Center.X + shipRec.Width / 2 * 0.8 * Math.Cos(rotation)), (int)(shipRec.Center.Y + shipRec.Height / 2 * 0.8 * Math.Sin(rotation)), 4, 36), bulletSpeed, rotation));

            shootSfx.CreateInstance().Play();
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