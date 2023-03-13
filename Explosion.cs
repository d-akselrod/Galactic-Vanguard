using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Linq;

namespace Galactic_Vanguard
{
    
    class Explosion : Entity
    {
        public static Texture2D spriteSheet;
        private Animation anim;
        private GameTime gameTime;

        public Explosion(Point centre, int radius, GameTime gameTime) : base()
        {
            rec = new Rectangle(centre.X - radius, centre.Y - radius, radius*2, radius*2);
            anim = new Animation(spriteSheet, 8, 8, 64, 1, -1, Animation.ANIMATE_ONCE, 1, rec.Location.ToVector2(), 1, true);
        }

        public override void Update()
        {
            anim.destRec = rec;
            anim.Update(gameTime);
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            anim.Draw(spritebatch, color, SpriteEffects.None);
        }
    }
}
