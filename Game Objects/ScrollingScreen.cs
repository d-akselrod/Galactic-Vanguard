using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Galactic_Vanguard
{
    public class ScrollingScreen
    {
        private Texture2D imageImg;
        private Texture2D flippedImg;
        private Rectangle[] imageRecs;
        private Rectangle rec;

        private int scrollSpeed;

        public ScrollingScreen(Texture2D imageImg, Texture2D flippedImg, Rectangle rec)
        {
            this.imageImg = imageImg;
            this.flippedImg = flippedImg;
            this.rec = rec;

            scrollSpeed = 1;
            imageRecs = new Rectangle[4];


            for (int i = 0; i < imageRecs.Length; i++)
            {
                imageRecs[i] = new Rectangle(rec.X, rec.Height * -i, rec.Width, rec.Height);
            }
        }

        public void Update()
        {
            for (int i = 0; i < imageRecs.Length; i++)
            {
                if (imageRecs[i].Top >= rec.Bottom)
                {
                    imageRecs[i].Y = imageRecs[i].Height * -3;
                }
                imageRecs[i].Y += scrollSpeed;

            }
        }

        public void Update2(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (int i = 0; i < imageRecs.Length; i++)
            {
                float yPos = imageRecs[i].Y + (scrollSpeed * deltaTime);
                float interpolatedYPos = MathHelper.SmoothStep(imageRecs[i].Y, yPos, 5f);
                imageRecs[i].Y = (int)interpolatedYPos;

                if (imageRecs[i].Top >= rec.Bottom)
                {
                    imageRecs[i].Y = imageRecs[i].Height * -3;
                }
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            for (int i = 0; i < imageRecs.Length; i++)
            {
                if (i % 2 == 0)
                {
                    spritebatch.Draw(imageImg, imageRecs[i], Color.White);
                }
                else
                {
                    spritebatch.Draw(flippedImg, imageRecs[i], Color.White);
                }
            }
        }

        public void SetScrollSpeed(int scrollSpeed)
        {
            this.scrollSpeed = scrollSpeed;
        }
    }
}
