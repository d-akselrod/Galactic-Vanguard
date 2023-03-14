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
    public class Space
    {
        public static Rectangle rec;
        private GameTimer gameTimer;
        private ScrollingScreen gameBg;
        private Viewport spaceView;
        private GameTime gameTime;

        private XWing xwing;
        public List<Entity> spaceEntities;

        private EnvironmentListener bulletListener;

        SoundEffect gunSnd;

        private int cometFreq;
        private int meteorFreq;
        private int planetFreq;
        private int tieFreq;

        private int bulletSpeed;

        public Space(Rectangle gameRec, Texture2D spaceBgImgNorm, Texture2D spaceBgImgRev)
        {
            rec = gameRec;

            gameTimer = new GameTimer();
            spaceEntities = new List<Entity>();
            bulletListener = new EnvironmentListener(this);
            spaceView = new Viewport(rec);

            bulletSpeed = 5;

            meteorFreq = (int)2.5*120;
            planetFreq = 30 * 120;
            cometFreq =  80;

            gameBg = new ScrollingScreen(spaceBgImgNorm, spaceBgImgRev, gameRec);
            xwing = new XWing(5, Color.White, gameRec, bulletListener);
        }

        public void Update(GameTime gameTime)
        {
            this.gameTime = gameTime;
            gameTimer.Update();
            gameBg.Update();

            XWingControl();
            MeteorControl();
            CometControl();
            PlanetControl();
            
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
            spriteBatch.GraphicsDevice.Viewport = new Viewport(0,0,1280,720);
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

        private void XWingGunControl()
        {

        }

        private void CollisionControl()
        {
            BulletMeteor();

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
                            spaceEntities.Add(new Explosion(meteor.GetRec().Center, meteor.GetRec().Width, gameTime));
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
                            if (entity.GetPosition().ToPoint().Y > rec.Bottom)
                            {
                                spaceEntities.Remove(entity);
                            }

                            break;
                        case Type t when t == typeof(Bullet):
                            if (entity.GetPosition().ToPoint().Y < -100)
                            {
                                spaceEntities.Remove(entity);
                            }
                            break;
                        case Type t when t == typeof(Planet):
                            if (entity.GetPosition().ToPoint().Y > rec.Bottom)
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

                    }
                }
            }
            catch { } 
        }
    }
}
