﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections;
using Galactic_Warfare;
using Microsoft.Xna.Framework.Audio;
using Galactic_Vanguard.Entities;
using Galactic_Vanguard.Game_Objects;

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
        private HUD hud;
        private RenderTarget2D renderTarget;
        
        public static string gameTimeStr;

        private Rectangle leftSide;
        private Rectangle rightSide;
        private Rectangle leftBorder;
        private Rectangle rightBorder;

        private Cursor cursor;

        //Titles
        private Texture2D titleImg;
        private Rectangle titleRec;

        //General Sprites
        private Texture2D blankImg;
       

        //Background Images
        private Texture2D launchBgImg;
        private Texture2D menuBgImg;
        private Texture2D menuInfoImg;
        private Texture2D gameOverImg;
        private Texture2D inGameBgNorm;
        private Texture2D inGameBgRev;
        private Texture2D[] hyperSpace;
        private int currImg = 0;

        //Buttons
        private Button playBtn;
        private Button startBtn;
        private Button resumeBtn;
        private Button settingsBtn;
        private Button exitBtn;
        private Button backBtn;
        private Button menuBtn;

        //Sliders
        private Slider volumeSlider;

        //Fonts
        private SpriteFont font;

        //Game Entities
        private GameEnvironment space;

        //Control
        public static Hashtable controls;

        private Control moveRight;
        private Control moveLeft;
        private Control rotateRight;
        private Control rotateLeft;
        private Control shoot;
        private Control reload;

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
            graphics.IsFullScreen = true;

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
            GameEnvironment.rec = gameRec;
            input = new InputController();
            music = new MusicController();
            gameState = new GameState();
            gameTimer = new GameTimer();
            hud = new HUD();

            Viewport gameView = new Viewport();
            gameView.X = gameRec.X;
            gameView.Y = gameRec.Y;
            gameView.Width = gameRec.Width;
            gameView.Height = gameRec.Height;

            GraphicsDevice.Viewport = gameView;

            graphics.ApplyChanges();
            renderTarget = new RenderTarget2D(GraphicsDevice, 1920, 1080);
            cursor = new Cursor(new Rectangle(0,0,30,30));
            IsMouseVisible = false;

            controls = new Hashtable();
            controls.Add("MOVE_LEFT", Keys.A);
            controls.Add("MOVE_RIGHT", Keys.D);
            controls.Add("ROTATE_LEFT", Keys.Left);
            controls.Add("ROTATE_RIGHT", Keys.Right);
            controls.Add("SHOOT", Keys.Space);
            controls.Add("RELOAD", Keys.R);

            moveRight = new Control("MOVE_RIGHT", Keys.D, 0);
            moveLeft = new Control("MOVE_LEFT", Keys.A, 1);
            rotateRight = new Control("ROTATE_RIGHT", Keys.Right, 2);
            rotateLeft = new Control("ROTATE_LEFT", Keys.Left, 3);
            shoot = new Control("SHOOT", Keys.Space, 4);
            reload = new Control("RELOAD", Keys.R, 5);


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
            LoadSFX();
            LoadSliders();

            void LoadSprites()
            {
                blankImg = Content.Load<Texture2D>("Images/Sprites/blankImg");
                GraphicsHelper.blank = blankImg;
                Meteor.image = Content.Load<Texture2D>("Images/Sprites/meteorImg");
                Comet.image = Content.Load<Texture2D>("Images/Sprites/cometImg");
                XWing.image = Content.Load<Texture2D>("Images/Sprites/XWingImg");
                Tie.image = Content.Load<Texture2D>("Images/Sprites/tieImg");
                Bullet.image = Content.Load<Texture2D>("Images/Sprites/XWingBulletImg");
                SpaceJunk.image = Content.Load<Texture2D>("Images/Sprites/spaceJunkImg");
                Upgrade.image = Content.Load<Texture2D>("Images/Sprites/upgradeImg");
                AmmoKit.image = Content.Load<Texture2D>("Images/Sprites/ammoKitImg");
                Explosion.spriteSheet = Content.Load<Texture2D>("Images/Spritesheets/explosionAnim");

                HUD.bulletImg = Content.Load<Texture2D>("Images/Sprites/bulletIconImg");
                HUD.reloadBarImg = Content.Load<Texture2D>("Images/Sprites/reloadBarImg");
                HUD.extraAmmoImg = Content.Load<Texture2D>("Images/Sprites/extraAmmoImg");

                Planet.images.Add(Content.Load<Texture2D>("Images/Sprites/planet1Img"));
                Planet.images.Add(Content.Load<Texture2D>("Images/Sprites/planet2Img"));
                Planet.images.Add(Content.Load<Texture2D>("Images/Sprites/planet3Img"));
                Planet.images.Add(Content.Load<Texture2D>("Images/Sprites/planet4Img"));

                HUD.ammoImg = Content.Load<Texture2D>("Images/Icons/ammoIcon");
                HUD.healthImg = Content.Load<Texture2D>("Images/Icons/healthIcon");
                HUD.shieldImg = Content.Load<Texture2D>("Images/Icons/shieldIcon");
                HUD.engineImg = Content.Load<Texture2D>("Images/Icons/rocketIcon");
                HUD.reloadImg = Content.Load<Texture2D>("Images/Icons/reloadIcon");
                HUD.emptyImg = Content.Load<Texture2D>("Images/Icons/emptyIcon");
                HUD.clockImg = Content.Load<Texture2D>("Images/Sprites/secondClock");

                Cursor.img = Content.Load<Texture2D>("Images/Sprites/cursorImg");
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
                menuInfoImg = Content.Load<Texture2D>("Images/Backgrounds/menuPageImg");
                gameOverImg = Content.Load<Texture2D>("Images/Backgrounds/gameOverImg");

                hyperSpace = new Texture2D[101];

                for(int i = 0; i <= 100; i ++)
                {
                    hyperSpace[i] = Content.Load<Texture2D>("Videos/Hyperspace/warps_" + i.ToString("D3"));
                }

                leftBorder = new Rectangle(gameRec.Left - 3, 0, 6, gameRec.Height);
                rightBorder = new Rectangle(gameRec.Right - 3, 0, 6, gameRec.Height);

                leftSide = new Rectangle(0, 0, gameRec.Left - 3, gameRec.Height);
                rightSide = new Rectangle(gameRec.Right + 3, 0, screenRec.Width - gameRec.Right + 3, gameRec.Height);

            }

            void LoadButtons()
            {
                playBtn = new Button(Content.Load<Texture2D>("Images/Buttons/playBtnImg"), GraphicsHelper.GetCentralRectangle(screenRec.Width, 580, 250,100));
                startBtn = new Button(Content.Load<Texture2D>("Images/Buttons/startBtnImg"), GraphicsHelper.GetCentralRectangle(screenRec.Width, 580, 250, 100));

                resumeBtn = new Button(Content.Load<Texture2D>("Images/Buttons/resumeBtn"), GraphicsHelper.GetCentralRectangle(screenRec.Width, 100, 340, 100), Color.White);
                settingsBtn = new Button(Content.Load<Texture2D>("Images/Buttons/settingsBtn"), GraphicsHelper.GetCentralRectangle(screenRec.Width, 300, 340, 100), Color.White);
                exitBtn = new Button(Content.Load<Texture2D>("Images/Buttons/exitBtn"), GraphicsHelper.GetCentralRectangle(screenRec.Width, 500, 340, 100), Color.White);
                backBtn = new Button(Content.Load<Texture2D>("Images/Buttons/backBtn"), GraphicsHelper.GetCentralRectangle(screenRec.Width, 500, 340, 100), Color.White);
                menuBtn = new Button(Content.Load<Texture2D>("Images/Buttons/menuBtn"), GraphicsHelper.GetCentralRectangle(screenRec.Width, 550, 340, 100), Color.White);
            }

            void LoadSliders()
            {
                volumeSlider = new Slider(Content.Load<Texture2D>("Images/Sprites/volumeBar"), Content.Load<Texture2D>("Images/Sprites/volumeSlider"), GraphicsHelper.GetCentralRectangle(screenRec.Width, 50, 350, 30));
            }

            void LoadSFX()
            {
                Explosion.soundEffect = Content.Load<SoundEffect>("Audio/SFX/explosionSfx");
                Gun.shootSfx = Content.Load<SoundEffect>("Audio/SFX/laserSfx");
                Gun.reloadSfx = Content.Load<SoundEffect>("Audio/SFX/reloadSfx");
            }

            void LoadFonts()
            {
                font = Content.Load<SpriteFont>("Fonts/hudFont");
                Control.font = font;
                HUD.font = font;
            }       
        
            void LoadMusic()
            {
                music.AddSong("LAUNCH", Content.Load<Song>("Audio/Music/launchMusic"));
                music.AddSong("INGAME", Content.Load<Song>("Audio/Music/inGameMusic"));
            }
        }

        protected override void Update(GameTime gameTime)
        {
            input.Update();
            cursor.Update(InputController.currMouse.Position);

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
                case GameState.PAUSE:
                    UpdatePause();
                    break;
                case GameState.SETTINGS:
                    UpdateSettings();
                    break;
                case GameState.ENDGAME:
                    UpdateEndGame();
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
                playBtn.Update();

                if (playBtn.pressed)
                {
                    gameState.SetState(GameState.MENU);
                }
            }

            void UpdateMenu()
            {
                startBtn.Update();

                if(startBtn.pressed)
                {
                    InstansiateParams();
                    gameState.SetState(GameState.INGAME);
                    music.Update(gameState);
                }
            }

            void UpdateInGame()
            {
                gameTimer.Update();
                HyperSpace();
                HUDControl();

                space.Update(gameTime);


                if (InputController.currKeyboard.IsKeyDown(Keys.P))
                {
                    gameState.SetState(GameState.PAUSE);
                }

                CheckGameOver();                
            }

            void UpdatePause()
            {
                resumeBtn.Update();
                settingsBtn.Update();
                exitBtn.Update();

                if(resumeBtn.pressed)
                {
                    gameState.SetState(GameState.INGAME);
                }
                else if(settingsBtn.pressed)
                {
                    gameState.SetState(GameState.SETTINGS);
                }
                else if(exitBtn.pressed)
                {
                    Exit();
                }
            }

            void UpdateSettings()
            {
                backBtn.Update();
                if (backBtn.pressed)
                {
                    gameState.SetState(GameState.PAUSE);
                }

                volumeSlider.Update();
                SoundEffect.MasterVolume = volumeSlider.GetValue();
                MediaPlayer.Volume = volumeSlider.GetValue();

                moveRight.Update(ref controls);
                moveLeft.Update(ref controls);
                rotateRight.Update(ref controls);
                rotateLeft.Update(ref controls);
                shoot.Update(ref controls);
                reload.Update(ref controls);

            }

            void UpdateEndGame()
            {
                menuBtn.Update();
                if(menuBtn.pressed)
                {
                    gameState.SetState(GameState.MENU);
                    InstansiateParams();
                }
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
                    cursor.Draw(spriteBatch);
                    break;
                case GameState.MENU:
                    DrawMenu();
                    cursor.Draw(spriteBatch);
                    break;
                case GameState.INGAME:
                    DrawInGame();
                    break;
                case GameState.PAUSE:
                    DrawInGame();
                    DrawPause();
                    cursor.Draw(spriteBatch);
                    break;
                case GameState.SETTINGS:
                    DrawInGame();
                    DrawSettings();
                    cursor.Draw(spriteBatch);
                    break;
                case GameState.ENDGAME:
                    DrawEndGame();
                    cursor.Draw(spriteBatch);
                    break;

            }

            base.Draw(gameTime);
            spriteBatch.End();

            void DrawLaunch()
            {
                spriteBatch.Draw(launchBgImg, screenRec, Color.White);
                spriteBatch.Draw(titleImg, titleRec, Color.White);
                playBtn.Draw(spriteBatch);
                spriteBatch.DrawString(font, "Daniel Akselrod", new Vector2(1050, 690), Color.White);
            }

            void DrawMenu()
            {
                spriteBatch.Draw(menuBgImg, screenRec, Color.White);
                spriteBatch.Draw(menuInfoImg, screenRec, Color.White);
                startBtn.Draw(spriteBatch);
            }

            void DrawInGame()
            {
                space.Draw(spriteBatch);

                spriteBatch.Draw(hyperSpace[currImg], leftSide, leftSide, Color.DarkSlateGray);
                spriteBatch.Draw(hyperSpace[currImg], rightSide, rightSide, Color.DarkSlateGray);
                spriteBatch.Draw(blankImg, leftBorder, Color.White);
                spriteBatch.Draw(blankImg, rightBorder, Color.White);

                hud.Draw(spriteBatch);
            }

            void DrawPause()
            {
                GraphicsHelper.DrawRec(spriteBatch, gameRec, Color.Black * 0.7f);
                resumeBtn.Draw(spriteBatch);
                settingsBtn.Draw(spriteBatch);
                exitBtn.Draw(spriteBatch);
            }

            void DrawSettings()
            {
                GraphicsHelper.DrawRec(spriteBatch, gameRec, Color.Black * 0.7f);
                backBtn.Draw(spriteBatch);
                volumeSlider.Draw(spriteBatch);

                GraphicsHelper.DrawCentralText(spriteBatch, font, "CONTROLS", 200, Color.White);

                moveRight.Draw(spriteBatch);
                moveLeft.Draw(spriteBatch);
                rotateLeft.Draw(spriteBatch);
                rotateRight.Draw(spriteBatch);
                shoot.Draw(spriteBatch);
                reload.Draw(spriteBatch);

            }

            void DrawEndGame()
            {
                spriteBatch.Draw(gameOverImg, screenRec, Color.White);
                GraphicsHelper.DrawCentralText(spriteBatch, font, Convert.ToString(GameEnvironment.score), 265, Color.Black);
                menuBtn.Draw(spriteBatch);
            }
        }

        private void InstansiateParams()
        {
            space = new GameEnvironment(inGameBgNorm, inGameBgRev);
            GameEnvironment.score = 0;
            music.Update(gameState);

            gameTimer = new GameTimer();
            hud = new HUD();
        }
        
        private void HyperSpace()
        {
            if(gameTimer.GetFramesPassed() % 5 == 0)
            {
                currImg += 1;
                currImg %= 100;
            }
        }
    
        private void HUDControl()
        {
            gameTimeStr = gameTimer.GetTimeFormat();
            hud.Update();
        }

        private void CheckGameOver()
        {
            if(XWing.health <= 0)
            {
                gameState.SetState(GameState.ENDGAME);              
            }
        }
    }
}