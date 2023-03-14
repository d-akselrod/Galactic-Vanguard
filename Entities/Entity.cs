using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galactic_Vanguard
{
    public abstract class Entity
    {
        protected Rectangle rec;
        protected Vector2 velocity;
        protected Vector2 position;
        protected bool collisionEnabled;
        protected Color color;
        protected float angle;

        protected Random rng;

        public Entity()
        {
            rng = new Random();
            color = Color.White;
        }

        public virtual void Update()
        {
            position += velocity;
            rec.Location = position.ToPoint();
        }

        public virtual void Draw(SpriteBatch spritebatch)
        {
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public Vector2 GetVelocity()
        {
            return velocity;
        }

        public Rectangle GetRec()
        {
            return rec;
        }
    }
}
