using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galactic_Vanguard
{
    public class Stats
    {
        public static int points;
        public static int maxLevel;

        public static int shieldLvl;
        public static int healthLvl;
        public static int gunLvl;
        public static int ammoLvl;
        public static int engineLvl;

        public Stats()
        {
            points = 100;
            maxLevel = 8;

            shieldLvl = 0;
            healthLvl = 0;
            gunLvl = 0;
            ammoLvl = 0;
            engineLvl = 0;
        }

        public void UpgradeShield()
        {
            if(shieldLvl < maxLevel)
            {
                shieldLvl += 1;
                points -= 1;
            }
        }

        public void UpgradeHealth()
        {
            if(healthLvl < maxLevel)
            {
                healthLvl += 1;
                points -= 1;
            }
        }

        public void UpgradeGun()
        {
            if(gunLvl < maxLevel)
            {
                gunLvl += 1;
                points -= 1;
            }
        }

        public void UpgradeAmmo()
        {
            if(ammoLvl < maxLevel)
            {
                ammoLvl += 1;
                points -= 1;
            }
        }

        public void UpgradeEngine()
        {
            if(engineLvl < maxLevel)
            {
                engineLvl += 1;
                points -= 1;
            }
        }
    }
}
