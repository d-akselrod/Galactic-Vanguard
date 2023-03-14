using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using Galactic_Warfare;

namespace Galactic_Vanguard
{
    public class MainGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private int FPS;

        private Rectangle screenRec;
        private Rectangle gameRec;
        private GameState gameState;
        private GameTimer gameTimer;
        private InputController input;
        private MusicController music;
        
        private ScrollingScreen leftSpace;
        private ScrollingScreen rightScreen;

        private Rectangle leftSide;
        private Rectangle rightSide;
        private Rectangle leftBorder;
        private Rectangle rightBorder;

        //Titles
        private Texture2D titleImg;
        private Rectangle titleRec;

        //General Sprites
        private Texture2D blankImg;

        //Background Images
        private Texture2D launchBgImg;
        private Texture2D menuBgImg;
        private Texture2D inGameBgNorm;
        private Texture2D inGameBgRev;
        private Texture2D[] hyperSpace;
        private int currImg = 0;

        //Buttons
        private Button playBtn;
        private Button startBtn;

        private SpriteFont font;

        //Game Entities
        private Space space;

        //Entity Sprites
        private Texture2D[] planetImgs;
        private Texture2D meteorImg;
        private Texture2D cometImg;
        private Texture2D xWingImg;
        private Texture2D xWingBulletImg;
        private Texture2D explosionImg;

        //Game Vars
        private int bulletSpeed;
        private int meteorFreq;
        private int planetFreq;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.SynchronizeWithVerticalRetrace = true;

            SamplerState samplerState = new SamplerState();
            samplerState.Filter = TextureFilter.Linear;
            samplerState.AddressU = TextureAddressMode.Wrap;
            samplerState.AddressV = TextureAddressMode.Wrap;
            samplerState.MaxAnisotropy = 16;
            graphics.GraphicsDevice.SamplerStates[0] = samplerState;

            FPS = 120;
            TargetElapsedTime = TimeSpan.FromSeconds(1d / FPS);
            screenRec = new Rectangle(0, 0, 1280, 720);
            gameRec = GraphicsHelper.GetCentralRectangle(screenRec.Width, 0, 600, 720);
            input = new InputController();
            music = new MusicController();
            gameState = new GameState();
            gameTimer = new GameTimer();

            Viewport gameView = new Viewport();
            gameView.X = gameRec.X;
            gameView.Y = gameRec.Y;
            gameView.Width = gameRec.Width;
            gameView.Height = gameRec.Height;

            GraphicsDevice.Viewport = gameView;

            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            LoadSprites();
            LoadBackgrounds();
            LoadButtons();
            LoadTitles();
            LoadFonts();
            LoadMusic();

            void LoadSprites()
            {
                blankImg = Content.Load<Texture2D>("Images/Sprites/blankImg");
                Meteor.image = Content.Load<Texture2D>("Images/Sprites/meteorImg");
                Comet.image = Content.Load<Texture2D>("Images/Sprites/cometImg");
                XWing.image = Content.Load<Texture2D>("Images/Sprites/XWingImg");
                Bullet.image = Content.Load<Texture2D>("Images/Sprites/XWingBulletImg");
                SpaceJunk.image = Content.Load<Texture2D>("Images/Sprites/spaceJunkImg");
                Explosion.spriteSheet = Content.Load<Texture2D>("Images/Spritesheets/explosionAnim");

                Planet.images.Add(Content.Load<Texture2D>("Images/Sprites/planet1Img"));
                Planet.images.Add(Content.Load<Texture2D>("Images/Sprites/planet2Img"));
                Planet.images.Add(Content.Load<Texture2D>("Images/Sprites/planet3Img"));
                Planet.images.Add(Content.Load<Texture2D>("Images/Sprites/planet4Img"));
            }

            void LoadTitles()
            {
                titleImg = Content.Load<Texture2D>("Images/Titles/titleImg");
                titleRec = GraphicsHelper.GetCentralRectangle(screenRec.Width, 320, 900, 240);
            }

            void LoadBackgrounds()
            {
                launchBgImg = Content.Load<Texture2D>("Images/Backgrounds/launchBgImg");
                menuBgImg = Content.Load<Texture2D>("Images/Backgrounds/menuBgImg");
                inGameBgNorm = Content.Load<Texture2D>("Images/Backgrounds/spaceImg");
                inGameBgRev = Content.Load<Texture2D>("Images/Backgrounds/spaceImgRev");

                hyperSpace = new Texture2D[101];

                for(int i = 0; i <= 100; i ++)
                {
                    hyperSpace[i] = Content.Load<Texture2D>("Videos/Hyperspace/warps_" + i.ToString("D3"));
                }
            }

            void LoadButtons()
            {
                playBtn = new Button(Content.Load<Texture2D>("Images/Buttons/playBtnImg"), GraphicsHelper.GetCentralRectangle(screenRec.Width, 580, 300,100));
                startBtn = new Button(Content.Load<Texture2D>("Images/Buttons/startBtnImg"), GraphicsHelper.GetCentralRectangle(screenRec.Width, 580, 300, 100));
            }

            void LoadSFX()
            {

            }
        
            void LoadFonts()
            {
                font = Content.Load<SpriteFont>("Fonts/font");
            }       
        
            void LoadMusic()
            {
                music.AddSong("LAUNCH", Content.Load<Song>("Audio/Music/launchMusic"));
                //music.AddSong("MENU", Content.Load<Song>("Audio/Music/menuMusic"));
                music.AddSong("INGAME", Content.Load<Song>("Audio/Music/inGameMusic"));
            }
        }

        protected override void Update(GameTime gameTime)
        {
            input.Update();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (gameState.GetState())
            {
                case GameState.FIRSTFRAME:
                    UpdateFirstFrame();
                    break;
                case GameState.LAUNCH:
                    UpdateLaunch();
                    break;
                case GameState.MENU:
                    UpdateMenu();
                    break;
                case GameState.INGAME:
                    UpdateInGame();
                    break;
            }

            base.Update(gameTime);

            void UpdateFirstFrame()
            {
                gameState.SetState(GameState.LAUNCH);
                music.Update(gameState);
            }

            void UpdateLaunch()
            {
                playBtn.Update(input);

                if (playBtn.pressed)
                {
                    gameState.SetState(GameState.MENU);
                }
            }

            void UpdateMenu()
            {
                startBtn.Update(input);

                if(startBtn.pressed)
                {
                    InstansiateParams();
                    gameState.SetState(GameState.INGAME);
                    music.Update(gameState);
                }
            }

            void UpdateInGame()
            {
                TimeControl();
                HyperSpace();

                space.Update(gameTime);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            switch(gameState.GetState())
            {
                case GameState.LAUNCH:
                    DrawLaunch();
                    break;
                case GameState.MENU:
                    DrawMenu();
                    break;
                case GameState.INGAME:
                    DrawInGame();
                    break;
            }
            DrawText();
            base.Draw(gameTime);
            spriteBatch.End();

            void DrawLaunch()
            {
                spriteBatch.Draw(launchBgImg, screenRec, Color.White);
                spriteBatch.Draw(titleImg, titleRec, Color.White);
                playBtn.Draw(spriteBatch);
            }

            void DrawMenu()
            {
                spriteBatch.Draw(menuBgImg, screenRec, Color.White);
                startBtn.Draw(spriteBatch);
            }

            void DrawInGame()
            {
                space.Draw(spriteBatch);

                spriteBatch.Draw(hyperSpace[currImg], leftSide, leftSide, Color.White);
                spriteBatch.Draw(hyperSpace[currImg], rightSide, rightSide, Color.White);
                spriteBatch.Draw(blankImg, leftBorder, Color.White);
                spriteBatch.Draw(blankImg, rightBorder, Color.White);

                //spriteBatch.Draw(blankImg, leftSide, Color.Crimson);
                //spriteBatch.Draw(blankImg, rightSide, Color.DarkSlateGray);

            }
        }

        private void InstansiateParams()
        {
            space = new Space(gameRec,inGameBgNorm, inGameBgRev);
            leftBorder = new Rectangle(gameRec.Left-3, 0, 6, gameRec.Height);
            rightBorder = new Rectangle(gameRec.Right - 3, 0, 6, gameRec.Height);

            leftSide = new Rectangle(0, 0, gameRec.Left - 3, gameRec.Height);
            rightSide = new Rectangle(gameRec.Right + 3, 0, screenRec.Width - gameRec.Right + 3, gameRec.Height);
        }
    
        private void TimeControl()
        {
            gameTimer.Update();
        }

        private void DrawText()
        {
            try
            {
                spriteBatch.DrawString(font, Convert.ToString(gameState.GetState()), new Vector2(0, 0), Color.White);

            }
            catch (Exception e)
            {

            }
        }
        private void HyperSpace()
        {
            if(gameTimer.GetFramesPassed() % 5 == 0)
            {
                currImg += 1;
                currImg %= 100;
            }
        }
    }
}