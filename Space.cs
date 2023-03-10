using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galactic_Warfare;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Galactic_Vanguard
{
    public class Space
    {
        public static Rectangle rec;
        private GameTimer gameTimer;
        private ScrollingScreen gameBg;

        private XWing xwing;
        public List<Entity> spaceEntities;

        private EnvironmentListener bulletListener;

        private int meteorFreq;
        private int planetFreq;
        private int tieFreq;

        private int bulletSpeed;

        public Space(Rectangle gameRec, Texture2D meteorImg, Texture2D planetImg, Texture2D spaceBgImgNorm, Texture2D spaceBgImgRev, Texture2D XWingImg, Texture2D XWingBulletImg)
        {
            Meteor.image = meteorImg;
            Planet.image = planetImg;
            XWing.image = XWingImg;
            Bullet.image = XWingBulletImg;
            rec = gameRec;

            gameTimer = new GameTimer();
            spaceEntities = new List<Entity>();
            bulletListener = new EnvironmentListener(this);

            bulletSpeed = 5;

            meteorFreq = 30;
            planetFreq = 8 * 120;

            gameBg = new ScrollingScreen(spaceBgImgNorm, spaceBgImgRev, gameRec);
            xwing = new XWing(5, Color.White, gameRec, bulletListener);
        }

        public void Update()
        {
            gameTimer.Update();
            gameBg.Update();

            XWingControl();
            MeteorControl();
            PlanetControl();
            UpdateEntities();
            CollisionControl();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            gameBg.Draw(spriteBatch);

            foreach (Entity spaceEntity in spaceEntities)
            {
                spaceEntity.Draw(spriteBatch);
            }

            xwing.Draw(spriteBatch);
        }

        public void AddEntity(Entity entity)
        {
            spaceEntities.Add(entity);
            Console.WriteLine("Added");
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
            foreach(Entity entity in spaceEntities)
            {
                switch(entity.GetType())
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
                }
            }
        }
    }
}
