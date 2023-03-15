using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public List<Entity> spaceEntities;

        private EnvironmentListener bulletListener;

        SoundEffect gunSnd;

        private int cometFreq;
        private int meteorFreq;
        private int planetFreq;
        private int tieFreq;
        private int junkFreq;

        public GameEnvironment(Texture2D spaceBgImgNorm, Texture2D spaceBgImgRev)
        {
            gameTimer = new GameTimer();
            spaceEntities = new List<Entity>();
            bulletListener = new EnvironmentListener(this);
            spaceView = new Viewport(rec);

            meteorFreq = (int)2.5*120;
            planetFreq = 30 * 120;
            cometFreq =  80;
            junkFreq = 7 * 120;
            
            meteorFreq = (int)1 * 120;
            planetFreq = 30 * 120;
            cometFreq = 80;
            junkFreq = 2 * 120;

            gameBg = new ScrollingScreen(spaceBgImgNorm, spaceBgImgRev, rec);
            xwing = new XWing(5, Color.White, rec, bulletListener);
        }

        public void Update(GameTime gameTime)
        {
            gameTimer.Update();
            gameBg.Update();

            XWingControl();
            MeteorControl();
            CometControl();
            PlanetControl();
            JunkControl();
            
            UpdateEntities();
            CollisionControl();
            MemoryControl();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Viewport = spaceView;
            gameBg.Draw(spriteBatch);

            foreach (Entity spaceEntity in spaceEntities)
            {
                spaceEntity.Draw(spriteBatch);
            }

            xwing.Draw(spriteBatch);
            spriteBatch.GraphicsDevice.Viewport = new Viewport(0, 0, 1280, 720);
        }

        public void AddEntity(Entity entity)
        {
            spaceEntities.Add(entity);
        }

        public void XWingControl()
        {
            xwing.Update();
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
    
        private void UpdateEntities()
        {
            foreach(Entity entity in spaceEntities)
            {
                entity.Update();               
            }
        }

        private void CollisionControl()
        {
            BulletMeteor();
            BulletJunk();
            MeteorJunk();
            XWingMeteor();
            XWingJunk();

            void XWingMeteor()
            {
                foreach (Meteor meteor in spaceEntities.OfType<Meteor>())
                {
                    if (xwing.Collides(meteor.GetRec()))
                    {
                        spaceEntities.Remove(meteor);
                        spaceEntities.Add(new Explosion(meteor.GetRec().Center, meteor.GetRec().Width, 1f));
                        goto CollisionDetected;
                    }
                }
            CollisionDetected:
                {
                    //Play Animation
                }
            }

            void XWingJunk()
            {
                foreach (SpaceJunk junk in spaceEntities.OfType<SpaceJunk>())
                {
                    if (xwing.Collides(junk.GetRec()))
                    {
                        spaceEntities.Remove(junk);
                        spaceEntities.Add(new Explosion(junk.GetRec().Center, junk.GetRec().Width, 1f));
                        goto CollisionDetected;
                    }
                }
            CollisionDetected:
                {
                    //Play Animation
                }
            }

            void BulletMeteor()
            {
                foreach(Bullet bullet in spaceEntities.OfType<Bullet>())
                {
                    foreach (Meteor meteor in spaceEntities.OfType<Meteor>())
                    {
                        if (Collision.BoxBox(bullet.GetRec(), meteor.GetRec()))
                        {
                            spaceEntities.Remove(meteor);
                            spaceEntities.Remove(bullet);
                            spaceEntities.Add(new Explosion(meteor.GetRec().Center, meteor.GetRec().Width, 1f));
                            goto CollisionDetected;
                        }
                    }
                }

                CollisionDetected:
                {
                    //Play Animation
                }
            }

            void BulletJunk()
            {
                foreach (Bullet bullet in spaceEntities.OfType<Bullet>())
                {
                    foreach (SpaceJunk junk in spaceEntities.OfType<SpaceJunk>())
                    {
                        if (Collision.BoxBox(bullet.GetRec(), junk.GetRec()))
                        {
                            spaceEntities.Add(new Explosion(new Point(bullet.GetRec().Center.X, bullet.GetRec().Top), 20,0f));
                            spaceEntities.Remove(bullet);
                            goto CollisionDetected;
                        }
                    }
                }

            CollisionDetected:
                {
                    //Play Animation
                }
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
                {
                    //Play Animation
                }
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
                    }
                }
            }
            catch { } 
        }
    }
}
