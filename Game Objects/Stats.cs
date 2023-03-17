using Galactic_Warfare;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galactic_Vanguard
{
    public class Stats
    {
        public static int skillPoints;
        public static int maxLevel;
        public static int level;

        public static int shieldLvl;
        public static int healthLvl;
        public static int gunLvl;
        public static int ammoLvl;
        public static int engineLvl;

        public Stats()
        {
            skillPoints = 0;
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
                skillPoints -= 1;
                level += 1;
                XWing.maxShield += 20;
            }
        }

        public void UpgradeHealth()
        {
            if(healthLvl < maxLevel)
            {
                healthLvl += 1;
                skillPoints -= 1;
                level += 1;
                XWing.health += 20;
                XWing.maxHealth += 20;
            }
        }

        public void UpgradeGun()
        {
            if(gunLvl < maxLevel)
            {
                gunLvl += 1;
                skillPoints -= 1;
                level += 1;
                Gun.magSize += 5;
            }
        }

        public void UpgradeAmmo()
        {
            if(ammoLvl < maxLevel)
            {
                ammoLvl += 1;
                skillPoints -= 1;
                level += 1;
                Gun.reloadTime -= 20;
            }
        }

        public void UpgradeEngine()
        {
            if(engineLvl < maxLevel)
            {
                engineLvl += 1;
                skillPoints -= 1;
                level += 1;

                XWing.linearSpeed += 0.15f;
                XWing.rotationalSpeed += 0.05f;
            }
        }
    }
}
