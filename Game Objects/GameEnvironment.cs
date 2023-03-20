using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Galactic_Vanguard.Entities;
using Galactic_Vanguard.Game_Objects;
using Galactic_Warfare;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Galactic_Vanguard
{
    public class GameEnvironment
    {
        public static Rectangle rec;
        private GameTimer gameTimer;
        private ScrollingScreen gameBg;
        private Viewport spaceView;

        private XWing xwing;
        private Tie tie;
        public List<Entity> spaceEntities;

        private EnvironmentListener bulletListener;

        SoundEffect gunSnd;

        private int cometFreq;
        private int meteorFreq;
        private int planetFreq;
        private int tieFreq;
        private int junkFreq;
        private int upgradeFreq;
        private int ammoKitFreq;

        public static int score;

        public GameEnvironment(Texture2D spaceBgImgNorm, Texture2D spaceBgImgRev)
        {
            gameTimer = new GameTimer();
            spaceEntities = new List<Entity>();
            bulletListener = new EnvironmentListener(this);
            spaceView = new Viewport(rec);

            meteorFreq = (int)1.5 * 120;
            planetFreq = 30 * 120;
            cometFreq = 80;
            junkFreq = 4 * 120;
            upgradeFreq = 120 * 15;
            ammoKitFreq = 120 * 20;
            tieFreq = 120*10;

            gameBg = new ScrollingScreen(spaceBgImgNorm, spaceBgImgRev, rec);
            xwing = new XWing(5, Color.White, rec, bulletListener);
            tie = new Tie(false);
            spaceEntities.Add(xwing);
        }

        public void Update(GameTime gameTime)
        {
            gameTimer.Update();
            gameBg.Update();

            if(tie.isAlive == false)
            {
                MeteorControl();
                JunkControl();
            }

            XWingControl();
            
            CometControl();
            PlanetControl();   
            UpgradeControl();
            AmmoKitControl();
            TieControl();
            UpdateEntities();
            CollisionControl();
            MemoryControl();
            IncreaseDifficulty();
            ScoreTimer();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Viewport = spaceView;
            gameBg.Draw(spriteBatch);

            foreach (Entity spaceEntity in spaceEntities)
            {
                spaceEntity.Draw(spriteBatch);
            }

            if (XWing.alive)
            {
                xwing.Draw(spriteBatch);
            }

            if(tie.isAlive)
            {
                tie.Draw(spriteBatch);
            }

            spriteBatch.GraphicsDevice.Viewport = new Viewport(0, 0, 1280, 720);
        }

        public void AddEntity(Entity entity)
        {
            spaceEntities.Add(entity);
        }

        public void XWingControl()
        {
            if (XWing.alive)
            {
                xwing.Update();
            }
        }

        private void MeteorControl()
        {
            if (gameTimer.GetFramesPassed() % meteorFreq == 0)
            {
                spaceEntities.Add(new Meteor());
            }
        }

        private void CometControl()
        {
            if (gameTimer.GetFramesPassed() % cometFreq == 0)
            {
                spaceEntities.Add(new Comet());
            }
        }

        private void JunkControl()
        {
            if (gameTimer.GetFramesPassed() % junkFreq == 0)
            {
                spaceEntities.Add(new SpaceJunk());
            }
        }

        private void PlanetControl()
        {
            if ((gameTimer.GetFramesPassed()) % planetFreq == 0)
            {
                spaceEntities.Add(new Planet());
            }
        }

        private void UpgradeControl()
        {
            if ((gameTimer.GetFramesPassed()) % upgradeFreq == 0 && Stats.level != 5 * Stats.maxLevel)
            {
                spaceEntities.Add(new Upgrade());
            }
        }

        private void AmmoKitControl()
        {
            if ((gameTimer.GetFramesPassed()) % ammoKitFreq == 0)
            {
                spaceEntities.Add(new AmmoKit());
            }
        }

        private void TieControl()
        {
            if(tie.isAlive == false)
            {
                if (gameTimer.GetFramesPassed() % tieFreq == 0)
                {
                    tie = new Tie(bulletListener);
                }
            } 
            else
            {
                tie.Update(xwing.GetRec().Center);
            }
        }

        private void UpdateEntities()
        {
            foreach (Entity entity in spaceEntities)
            {
                if (entity.GetType() != typeof(XWing))
                {
                    entity.Update();
                }
            }
        }

        private void XWingDmg(int damage)
        {
            if (XWing.shield >= damage)
            {
                XWing.shield -= damage;
            }
            else
            {
                XWing.health -= damage - XWing.shield;
                XWing.shield = 0;
            }

            if (XWing.health <= 0)
            {
                XWing.alive = false;
                spaceEntities.Remove(xwing);
            }
        }

        private void CollisionControl()
        {
            BulletMeteor();
            BulletJunk();
            MeteorJunk();
            BulletTie();

            if (XWing.alive)
            {
                XWingMeteor();
                XWingJunk();
                XWingUpgrade();
                XWingAmmoKit();
                BulletXWing();
            }


            void XWingMeteor()
            {
                foreach (Meteor meteor in spaceEntities.OfType<Meteor>())
                {
                    if (xwing.Collides(meteor.GetRec()))
                    {
                        spaceEntities.Remove(meteor);
                        spaceEntities.Add(new Explosion(meteor.GetRec().Center, meteor.GetRec().Width, 1f));
                        XWingDmg(20);
                        goto CollisionDetected;
                    }
                }
            CollisionDetected:
                { }
            }

            void XWingJunk()
            {
                foreach (SpaceJunk junk in spaceEntities.OfType<SpaceJunk>())
                {
                    if (xwing.Collides(junk.GetRec()))
                    {
                        spaceEntities.Remove(junk);
                        spaceEntities.Add(new Explosion(junk.GetRec().Center, junk.GetRec().Width, 1f));
                        XWingDmg(20);
                        goto CollisionDetected;
                    }
                }
            CollisionDetected:
                { }  //Play Animation
            }


            void BulletMeteor()
            {
                foreach (Bullet bullet in spaceEntities.OfType<XWingBullet>())
                {
                    foreach (Meteor meteor in spaceEntities.OfType<Meteor>())
                    {
                        if (Collision.BoxBox(bullet.GetRec(), meteor.GetRec()))
                        {
                            spaceEntities.Remove(meteor);
                            spaceEntities.Remove(bullet);
                            spaceEntities.Add(new Explosion(meteor.GetRec().Center, meteor.GetRec().Width, 1f));
                            score += 5;
                            goto CollisionDetected;
                        }
                    }
                }

            CollisionDetected:
                { }
            }

            void BulletJunk()
            {
                foreach (Bullet bullet in spaceEntities.OfType<XWingBullet>())
                {
                    foreach (SpaceJunk junk in spaceEntities.OfType<SpaceJunk>())
                    {
                        if (Collision.BoxBox(bullet.GetRec(), junk.GetRec()))
                        {
                            spaceEntities.Add(new Explosion(new Point(bullet.GetRec().Center.X, bullet.GetRec().Top), 20, 0f));
                            spaceEntities.Remove(bullet);
                            goto CollisionDetected;
                        }
                    }
                }

            CollisionDetected:
                { }
            }

            void MeteorJunk()
            {
                foreach (SpaceJunk junk in spaceEntities.OfType<SpaceJunk>())
                {
                    foreach (Meteor meteor in spaceEntities.OfType<Meteor>())
                    {
                        if (Collision.BoxBox(junk.GetRec(), meteor.GetRec()))
                        {
                            spaceEntities.Remove(meteor);
                            spaceEntities.Remove(junk);
                            spaceEntities.Add(new Explosion(GraphicsHelper.GetMidpoint(meteor.GetRec().Center, junk.GetRec().Center), meteor.GetRec().Width + junk.GetRec().Width, 1f));
                            goto CollisionDetected;
                        }
                    }
                }

            CollisionDetected:
                { }
            }

            void XWingUpgrade()
            {
                foreach (Upgrade upgrade in spaceEntities.OfType<Upgrade>())
                {
                    if (xwing.Collides(upgrade.GetRec()))
                    {
                        Stats.skillPoints += 1;
                        spaceEntities.Remove(upgrade);
                        goto CollisionDetected;
                    }
                }
            CollisionDetected:
                { }

            }

            void XWingAmmoKit()
            {
                foreach (AmmoKit kit in spaceEntities.OfType<AmmoKit>())
                {
                    if (xwing.Collides(kit.GetRec()))
                    {
                        spaceEntities.Remove(kit);
                        XWing.externalAmmo += Gun.magSize;
                        if(XWing.externalAmmo > 999)
                        {
                            XWing.externalAmmo = 999;
                        }

                        HUD.externalAmmo = XWing.externalAmmo;
                        goto CollisionDetected;
                    }
                }
            CollisionDetected:
                { }
            }

            void BulletTie()
            {
                foreach (Bullet bullet in spaceEntities.OfType<XWingBullet>())
                {
                    if (tie.Collides(bullet.GetRec()))
                    {
                        spaceEntities.Add(new Explosion(tie.GetRec().Center, tie.GetRec().Width + tie.GetRec().Width, 1f));
                        tie = new Tie(false);
                        score += 15;
                        goto CollisionDetected;
                    }
                }
            CollisionDetected:
                { }
            }

            void BulletXWing()
            {
                foreach (Bullet bullet in spaceEntities.OfType<TieBullet>())
                {
                    if (xwing.Collides(bullet.GetRec()))
                    {
                        spaceEntities.Add(new Explosion(bullet.GetRec().Center + new Point(0,20), 10, 1f));
                        spaceEntities.Remove(bullet);
                        XWingDmg(10);
                        goto CollisionDetected;
                    }
                }
            CollisionDetected:
                { }
            }
        }
    
        private void MemoryControl()
        {
            try
            {
                foreach (Entity entity in spaceEntities)
                {
                    switch (entity.GetType())
                    {
                        case Type t when t == typeof(Meteor):
                            if (entity.GetPosition().Y > rec.Bottom)
                            {
                                spaceEntities.Remove(entity);
                            }

                            break;
                        case Type t when t == typeof(Bullet):
                            if (entity.GetPosition().Y < -100)
                            {
                                spaceEntities.Remove(entity);
                            }
                            break;
                        case Type t when t == typeof(Planet):
                            if (entity.GetPosition().Y > rec.Bottom)
                            {
                                spaceEntities.Remove(entity);
                            }
                            break;
                        case Type t when t == typeof(Comet):
                            if(entity.GetRec().Left > rec.Right || entity.GetRec().Right < rec.Left)
                            {
                                spaceEntities.Remove(entity);
                            }
                            break;
                        case Type t when t == typeof(Explosion):
                            if (((Explosion)entity).IsAnimating() == false)
                            {
                                spaceEntities.Remove(entity);
                            }
                            break;
                        case Type t when t == typeof(AmmoKit):
                            if (entity.GetRec().Top > GameEnvironment.rec.Bottom)
                            {
                                spaceEntities.Remove(entity);
                            }
                            break;
                    }
                }
            }
            catch { } 
        }
    
        private void IncreaseDifficulty()
        {
            if(gameTimer.GetFramesPassed() < 5*60*120 && gameTimer.GetFramesPassed() % (120*15) == 0)
            {
                meteorFreq -= 2;
                cometFreq -= 2;
                junkFreq -= 3;
            }
        }
    
        private void ScoreTimer()
        {
            if(gameTimer.GetFramesPassed() % 60 == 0)
            {
                score += 1;
            }
        }
    }
}
