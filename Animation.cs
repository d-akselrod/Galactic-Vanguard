using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galactic_Vanguard
{
    public class Animation
    {
        public const int NO_IDLE = -1;

        public const int ANIMATE_FOREVER = -1;

        public const int ANIMATE_ONCE = 1;

        public const SpriteEffects FLIP_NONE = SpriteEffects.None;

        public const SpriteEffects FLIP_HORIZONTAL = SpriteEffects.FlipHorizontally;

        public const SpriteEffects FLIP_VERTICAL = SpriteEffects.FlipVertically;

        private Texture2D img;

        private int numFramesWide;

        private int numFramesHigh;

        private int totalFrameCount;

        private int idleFrameNum;

        private int smoothRate;

        private int smoothCount;

        private int repeatCount;

        private int repeatBase;

        private int startFrameBase;

        private Rectangle srcRec;

        public int curFrame;

        public int frameWidth;

        public int frameHeight;

        public bool isAnimating;

        public Rectangle destRec;

        private bool started;

        public Animation(Texture2D img, int numFramesWide, int numFramesHigh, int totalFrameCount, int startingFrameNum, int idleFrameNum, int repeatCount, int smoothRate, Vector2 pos, float scale, bool startNow)
        {
            this.img = img;
            this.numFramesWide = numFramesWide;
            this.numFramesHigh = numFramesHigh;
            this.totalFrameCount = totalFrameCount;
            curFrame = (curFrame = MathHelper.Clamp(startingFrameNum, 0, totalFrameCount - 1));
            startFrameBase = curFrame;
            this.idleFrameNum = MathHelper.Clamp(idleFrameNum, -1, totalFrameCount - 1);
            this.repeatCount = repeatCount;
            repeatBase = repeatCount;
            this.smoothRate = smoothRate;
            isAnimating = startNow;
            smoothCount = smoothRate;
            frameWidth = img.Width / numFramesWide;
            frameHeight = img.Height / numFramesHigh;
            destRec = new Rectangle((int)pos.X, (int)pos.Y, (int)((float)frameWidth * scale), (int)((float)frameHeight * scale));
            srcRec = GetSourceRectangle();
        }

        public void Update(GameTime gameTime)
        {
            if (isAnimating)
            {
                if (!started)
                {
                    ResetAnimation();
                    started = true;
                }

                if (smoothCount == 0)
                {
                    srcRec = GetSourceRectangle();
                    SetNextFrame();
                    smoothCount = smoothRate;
                }

                smoothCount--;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Color color, SpriteEffects flipType)
        {
            if (isAnimating || (!isAnimating && idleFrameNum != -1))
            {
                spriteBatch.Draw(img, destRec, srcRec, color, 0f, Vector2.Zero, flipType, 0f);
            }
        }

        public Rectangle GetSourceRectangle()
        {
            int x = curFrame % numFramesWide * frameWidth;
            int y = curFrame / numFramesWide * frameHeight;
            return new Rectangle(x, y, frameWidth, frameHeight);
        }

        private void SetNextFrame()
        {
            curFrame++;
            if (curFrame != totalFrameCount)
            {
                return;
            }

            if (repeatCount == -1)
            {
                curFrame = 0;
                return;
            }

            repeatCount--;
            curFrame = 0;
            if (repeatCount == 0)
            {
                if (idleFrameNum == -1)
                {
                    curFrame = MathHelper.Clamp(startFrameBase, 0, totalFrameCount - 1);
                }
                else
                {
                    curFrame = MathHelper.Clamp(idleFrameNum, -1, totalFrameCount - 1);
                }

                isAnimating = false;
                srcRec = GetSourceRectangle();
                started = false;
            }
        }

        private void ResetAnimation()
        {
            curFrame = MathHelper.Clamp(startFrameBase, 0, totalFrameCount - 1);
            repeatCount = repeatBase;
            smoothCount = smoothRate;
            srcRec = GetSourceRectangle();
        }
    }
}
