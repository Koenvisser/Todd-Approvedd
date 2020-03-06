using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace OakHeart
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        enum GameState { MainMenu, Settings,LevelSelect, Game, Pause };
        GameState _state = GameState.MainMenu;
        GameState _pausedstate = GameState.MainMenu;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D loadingleft, loadingright, rectangle, circle, pause1, pause2;
        private SpriteFont KronaFont, LevelSelectFont, PacificoFont;
        private float menuposition, volume;
        private bool loadingdone = false, menuanimationdone = false, PlayButtonClicked = false, SettingsButtonClicked = false, ConfirmButtonClicked = false, CancelButtonClicked = false, QuitButtonClicked = false, LevelButtonClicked = false, DragSlider = false, escdown = false, ResetButtonClicked, MainMenuButtonClicked, ResumeButtonClicked, fullscreen = false, fullscreensliderclick = false, BackButtonClicked = false, ResetGame = false;
        private int LevelCompleted, SliderPosition, ElapsedTime;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.ApplyChanges();
            base.Initialize();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            loadingleft = Content.Load<Texture2D>("images/left");
            loadingright = Content.Load<Texture2D>("images/right");
            pause1 = Content.Load<Texture2D>("images/pause1");
            pause2 = Content.Load<Texture2D>("images/pause2");
            KronaFont = Content.Load<SpriteFont>("fonts/Krona");
            LevelSelectFont = Content.Load<SpriteFont>("fonts/Krona2");
            PacificoFont = Content.Load<SpriteFont>("fonts/Pacifico");
            rectangle = new Texture2D(GraphicsDevice, 1, 1);
            circle = Content.Load<Texture2D>("images/circle");
            rectangle.SetData(new[] { Color.White });
            string settingsline;
            StreamReader settingsfile = new StreamReader(Directory.GetCurrentDirectory().Replace(@"bin\Windows\x86\Debug", "Content") + @"\settings.txt");
            bool success1 = false, success2 = false;
            while ((settingsline = settingsfile.ReadLine()) != null)
            {
                if (settingsline.Contains("volume "))
                {
                    success1 = float.TryParse(settingsline.Replace("volume ", ""), out volume);
                    SliderPosition = (int)(graphics.PreferredBackBufferWidth / 2 - 173 + 300 * volume);                  
                }
                else if (settingsline.Contains("fullscreen "))
                {
                    success2 = bool.TryParse(settingsline.Replace("fullscreen ", ""), out fullscreen);
                    if (fullscreen == true)
                    {
                        graphics.IsFullScreen = true;
                        graphics.ApplyChanges();
                    }
                }
            }
            settingsfile.Close();
            if (success1 == false || success2 == false)
            {
                File.WriteAllText(Directory.GetCurrentDirectory().Replace(@"bin\Windows\x86\Debug", "Content") + @"\settings.txt", "volume 1\nfullscreen True\n");
            }
            // TODO: use this.Content to load your game content here
            loadingdone = true;
            IsMouseVisible = true;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();
            spriteBatch.Dispose();
            rectangle.Dispose();
            // TODO: Unload any non ContentManager content here
        }
        private void LoadSave()
        {
            bool success = Int32.TryParse(File.ReadAllText(Directory.GetCurrentDirectory().Replace(@"bin\Windows\x86\Debug", "Content") + @"\save.txt"), out LevelCompleted);
            if (success == false)
            {
                File.WriteAllText(Directory.GetCurrentDirectory().Replace(@"bin\Windows\x86\Debug", "Content") + @"\save.txt", "0");
            }
        }

        private void Pause() {
            if (_state != GameState.Pause && _state != GameState.Settings)
            {
                _pausedstate = _state;
                _state = GameState.Pause;
                IsMouseVisible = true;
            }
            else if (_state == GameState.Settings)
            {
                _state = GameState.Pause;
            }
            else
            {
                _state = _pausedstate;
                if (_state == GameState.Game)
                { IsMouseVisible = false; }
            }
        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                
                    if (escdown == false)
                {
                    Pause();
                    escdown = true;
                }
            }
            else { escdown = false; }
            if (_state == GameState.MainMenu)
            {
                // TODO: Add your update logic here
                if (loadingdone == true && menuposition < graphics.PreferredBackBufferWidth * 0.6f && menuanimationdone == false && ElapsedTime > 500)
                {
                    menuposition *= 1.02f;
                    menuposition += 3;
                }
                else if (ElapsedTime <= 500)
                {
                    ElapsedTime += gameTime.ElapsedGameTime.Milliseconds;
                }
                if (menuposition >= graphics.PreferredBackBufferWidth * 0.6f && menuanimationdone == false)
                {
                    menuanimationdone = true;
                    menuposition = graphics.PreferredBackBufferWidth * 0.6f;
                }
                if (menuanimationdone == true && menuposition > 0)
                {
                    menuposition *= 0.96f;
                    menuposition -= 2;
                }
                else if (menuanimationdone == true && menuposition < 0)
                { menuposition = 0; }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        protected override void Draw(GameTime gameTime)
        {
            int width = graphics.PreferredBackBufferWidth;
            int height = graphics.PreferredBackBufferHeight;
            var mouseState = Mouse.GetState();
            var mousePosition = new Point(mouseState.X, mouseState.Y);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            if (_state == GameState.MainMenu || ((_state == GameState.Pause || _state == GameState.Settings) && _pausedstate == GameState.MainMenu))
            {
                if (menuanimationdone == true)
                {

                    Rectangle PlayButton = new Rectangle(width / 2 - (int)(KronaFont.MeasureString("Play").X * 0.6f), (int)(height * .5f + menuposition), (int)(KronaFont.MeasureString("Play").X * 1.2f), (int)KronaFont.MeasureString("Play").Y);
                    if (PlayButton.Contains(mousePosition))
                    {
                        if (mouseState.LeftButton == ButtonState.Pressed)
                        {
                            PlayButtonClicked = true;
                            spriteBatch.Draw(rectangle, PlayButton, new Color(0, 0, 0, 0.3f));
                        }
                        else if (PlayButtonClicked == true)
                        {
                            _state = GameState.LevelSelect;
                            LoadSave();
                            PlayButtonClicked = false;
                        }
                        else
                        {
                            spriteBatch.Draw(rectangle, PlayButton, new Color(0, 0, 0, 0.1f));
                        }

                    }
                    else { PlayButtonClicked = false; }

                    spriteBatch.DrawString(KronaFont, "Play", new Vector2(graphics.PreferredBackBufferWidth / 2 - KronaFont.MeasureString("Play").X / 2, graphics.PreferredBackBufferHeight * .5f + menuposition), Color.White);
                }

                if (menuanimationdone == false)
                {
                    spriteBatch.Draw(loadingleft, new Rectangle((int)menuposition * -1, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    spriteBatch.Draw(loadingright, new Rectangle((int)menuposition, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                }
            }
            
            else if (_state == GameState.LevelSelect || ((_state == GameState.Pause || _state == GameState.Settings) && _pausedstate == GameState.LevelSelect))
            {
                int i = 0;
                Color LevelColor = Color.ForestGreen;
                Vector2 LevelSelectPosition = new Vector2();
                while (i <= LevelCompleted && i <= 4)
                {
                    if (i == 0)
                    { LevelSelectPosition = new Vector2(.5f * graphics.PreferredBackBufferWidth, .5f * graphics.PreferredBackBufferHeight); }
                    else if (i == 1)
                    { LevelSelectPosition = new Vector2(0, 0); }
                    else if (i == 2)
                    { LevelSelectPosition = new Vector2(0, 0); }
                    else if (i == 3)
                    { LevelSelectPosition = new Vector2(0, 0); }
                    else
                    { LevelSelectPosition = new Vector2(0, 0); }
                    if (i >= LevelCompleted)
                    {
                        LevelColor = Color.Red;
                    }
                    i++;
                    Rectangle LevelButton = new Rectangle((int)LevelSelectPosition.X - 31, (int)LevelSelectPosition.Y - 31, 62, 62);
                    if (LevelButton.Contains(mousePosition))
                    {
                        if (mouseState.LeftButton == ButtonState.Pressed)
                        {
                            spriteBatch.Draw(rectangle, new Rectangle((int)LevelSelectPosition.X - 38, (int)LevelSelectPosition.Y - 38, 76, 76), new Color(0, 0, 0, 0.6f));
                            LevelButtonClicked = true;
                        }
                        else if (LevelButtonClicked == true)
                        {
                            _state = GameState.Game;
                            IsMouseVisible = false;
                            // level = i
                            LevelButtonClicked = false;
                        }
                        else
                        {
                            spriteBatch.Draw(rectangle, new Rectangle((int)LevelSelectPosition.X - 38, (int)LevelSelectPosition.Y - 38, 76, 76), new Color(0, 0, 0, 0.3f));
                        }
                    }
                    else
                    {
                        spriteBatch.Draw(rectangle, new Rectangle((int)LevelSelectPosition.X - 38, (int)LevelSelectPosition.Y - 38, 76, 76), new Color(0, 0, 0, 0.15f));
                    }
                    spriteBatch.Draw(rectangle, new Rectangle((int)LevelSelectPosition.X - 48, (int)LevelSelectPosition.Y - 48, 10, 96), LevelColor);
                    spriteBatch.Draw(rectangle, new Rectangle((int)LevelSelectPosition.X + 38, (int)LevelSelectPosition.Y - 48, 10, 96), LevelColor);
                    spriteBatch.Draw(rectangle, new Rectangle((int)LevelSelectPosition.X - 48, (int)LevelSelectPosition.Y - 48, 96, 10), LevelColor);
                    spriteBatch.Draw(rectangle, new Rectangle((int)LevelSelectPosition.X - 48, (int)LevelSelectPosition.Y + 38, 96, 10), LevelColor);
                    spriteBatch.DrawString(LevelSelectFont, i.ToString(), LevelSelectPosition - LevelSelectFont.MeasureString(i.ToString()) / 2, Color.White);

                }
            }
            else if (_state == GameState.Game || ((_state == GameState.Pause || _state == GameState.Settings) && _pausedstate == GameState.Game))
            {

            }
            if (_state == GameState.Pause || _state == GameState.Settings)
            {
                spriteBatch.Draw(rectangle, new Rectangle(0, 0, width, height), new Color(0, .1f, 0, 0.4f));
                spriteBatch.Draw(rectangle, new Rectangle(width / 2 - 250, height / 2 - 375, 500, 750), new Color(0, .2f, 0, 0.9f));
                spriteBatch.Draw(pause1, new Rectangle(width / 2 - 300, height / 2 - 400, 120, 800), Color.White);
                spriteBatch.Draw(pause2, new Rectangle(width / 2 - 250, height / 2 - 430, 550, 120), Color.White);
                spriteBatch.Draw(pause1, new Rectangle(width / 2 + 200, height / 2 - 400, 120, 800), Color.White);
                spriteBatch.Draw(pause2, new Rectangle(width / 2 - 250, height / 2 + 300, 550, 120), Color.White);
            }
            if (_state == GameState.Settings)
            {
                if (ResetGame == false)
                {
                    int volumeheight = 280;
                    int fullscreenheight = 450;
                    spriteBatch.DrawString(PacificoFont, "Volume", new Vector2(width / 2, volumeheight) - PacificoFont.MeasureString("Volume") / 2, Color.White);
                    spriteBatch.DrawString(PacificoFont, "Fullscreen", new Vector2(width / 2, fullscreenheight) - PacificoFont.MeasureString("Fullscreen") / 2, Color.White);
                    int sliderwidth = width / 2;
                    Rectangle SlideBar = new Rectangle(sliderwidth - 150, volumeheight + 70, 300, 26);
                    int FullscreenSliderPos = 0;
                    Color FullscreenColor = new Color();
                    if (fullscreen == true)
                    {
                        FullscreenSliderPos = width / 2 + 27;
                        FullscreenColor = Color.Green;
                    }
                    else
                    {
                        FullscreenSliderPos = width / 2 - 73;
                        FullscreenColor = new Color(200, 200, 200);
                    }
                    Rectangle FullscreenSlider = new Rectangle(FullscreenSliderPos, fullscreenheight + 70, 46, 46);
                    Rectangle FullscreenRec = new Rectangle(width / 2 - 50, fullscreenheight + 70, 100, 46);
                    Rectangle FullscreenRight = new Rectangle(width / 2 + 27, fullscreenheight + 70, 46, 46);
                    Rectangle FullscreenLeft = new Rectangle(width / 2 - 73, fullscreenheight + 70, 46, 46);
                    if (FullscreenRec.Contains(mousePosition) || FullscreenSlider.Contains(mousePosition) || FullscreenLeft.Contains(mousePosition) || FullscreenRight.Contains(mousePosition))
                    {
                        if (mouseState.LeftButton == ButtonState.Pressed)
                        {
                            fullscreensliderclick = true;
                        }
                        else if (fullscreensliderclick == true)
                        {
                            fullscreen = !fullscreen;
                            graphics.IsFullScreen = fullscreen;
                            graphics.ApplyChanges();
                            fullscreensliderclick = false;
                            File.WriteAllText(Directory.GetCurrentDirectory().Replace(@"bin\Windows\x86\Debug", "Content") + @"\settings.txt", "volume " + volume + "\nfullscreen " + fullscreen + "\n");
                        }
                    }
                    else
                    {
                        fullscreensliderclick = false;
                    }

                    Color DragColor = Color.White;
                    if ((SlideBar.Contains(mousePosition) || (DragSlider == true)) && mouseState.LeftButton == ButtonState.Pressed)
                    {
                        DragColor = new Color(230, 230, 230);
                        SliderPosition = mousePosition.X - 15;
                        if (SliderPosition < sliderwidth - 173)
                        {
                            SliderPosition = sliderwidth - 173;
                        }
                        if (SliderPosition > sliderwidth + 127)
                        {
                            SliderPosition = sliderwidth + 127;
                        }
                        volume = SliderPosition - (sliderwidth - 173);
                        volume /= 300;
                        DragSlider = true;
                    }
                    else if (mouseState.LeftButton != ButtonState.Pressed && DragSlider == true)
                    {
                        DragSlider = false;
                        File.WriteAllText(Directory.GetCurrentDirectory().Replace(@"bin\Windows\x86\Debug", "Content") + @"\settings.txt", "volume " + volume + "\nfullscreen " + fullscreen + "\n");
                    }
                    spriteBatch.Draw(rectangle, SlideBar, new Color(200, 200, 200));
                    spriteBatch.Draw(circle, new Rectangle(width / 2 + 137, volumeheight + 70, 26, 26), new Color(200, 200, 200));
                    spriteBatch.Draw(circle, new Rectangle(width / 2 - 163, volumeheight + 70, 26, 26), Color.Green);
                    spriteBatch.Draw(rectangle, new Rectangle(sliderwidth - 150, volumeheight + 70, SliderPosition + 173 - sliderwidth, 26), Color.Green);
                    spriteBatch.Draw(rectangle, FullscreenRec, FullscreenColor);
                    spriteBatch.Draw(circle, FullscreenRight, FullscreenColor);
                    spriteBatch.Draw(circle, FullscreenLeft, FullscreenColor);
                    spriteBatch.Draw(circle, FullscreenSlider, Color.White);
                    spriteBatch.Draw(circle, new Rectangle(SliderPosition, volumeheight + 60, 46, 46), DragColor);
                    Rectangle BackButton = new Rectangle((int)(width - PacificoFont.MeasureString("Back").X) / 2 - 10, (int)(height - PacificoFont.MeasureString("Back").Y) / 2 + 270, (int)PacificoFont.MeasureString("Back").X + 20, (int)PacificoFont.MeasureString("Back").Y);
                    Rectangle ResetButton = new Rectangle((int)(width - PacificoFont.MeasureString("Reset Game").X) / 2 - 10, (int)(height - PacificoFont.MeasureString("Reset Game").Y) / 2 + 130, (int)PacificoFont.MeasureString("Reset Game").X + 20, (int)PacificoFont.MeasureString("Reset Game").Y);

                    if (BackButton.Contains(mousePosition))
                    {
                        if (mouseState.LeftButton == ButtonState.Pressed)
                        {
                            BackButtonClicked = true;
                            spriteBatch.Draw(rectangle, BackButton, new Color(0, 0, 0, 0.3f));
                        }
                        else if (BackButtonClicked == true)
                        {
                            _state = GameState.Pause;
                            BackButtonClicked = false;
                        }
                        else
                        {
                            spriteBatch.Draw(rectangle, BackButton, new Color(0, 0, 0, 0.1f));
                        }

                    }
                    else { BackButtonClicked = false; }
                    if (ResetButton.Contains(mousePosition))
                    {
                        if (mouseState.LeftButton == ButtonState.Pressed)
                        {
                            ResetButtonClicked = true;
                            spriteBatch.Draw(rectangle, ResetButton, new Color(0, 0, 0, 0.3f));
                        }
                        else if (ResetButtonClicked == true)
                        {
                            ResetGame = true;
                            ResetButtonClicked = false;
                        }
                        else
                        {
                            spriteBatch.Draw(rectangle, ResetButton, new Color(0, 0, 0, 0.1f));
                        }

                    }
                    else { ResetButtonClicked = false; }
                    spriteBatch.DrawString(PacificoFont, "Reset Game", new Vector2((width - PacificoFont.MeasureString("Reset Game").X) / 2, (height - PacificoFont.MeasureString("Reset Game").Y) / 2 + 130), Color.White);
                    spriteBatch.DrawString(PacificoFont, "Back", new Vector2((width - PacificoFont.MeasureString("Back").X) / 2, (height - PacificoFont.MeasureString("Back").Y) / 2 + 270), Color.White);
                }
                else {
                    spriteBatch.DrawString(PacificoFont, "Are you sure", new Vector2((width - PacificoFont.MeasureString("Are you sure").X) / 2, (height - PacificoFont.MeasureString("Are you sure").Y) / 2 - 270), Color.White);
                    spriteBatch.DrawString(PacificoFont, "you want to", new Vector2((width - PacificoFont.MeasureString("you want to").X) / 2, (height - PacificoFont.MeasureString("you want to").Y) / 2 - 270 + PacificoFont.MeasureString("Are you sure").Y), Color.White);
                    spriteBatch.DrawString(PacificoFont, "reset the game?", new Vector2((width - PacificoFont.MeasureString("reset the game?").X) / 2, (height - PacificoFont.MeasureString("reset the game?").Y) / 2 - 270 + PacificoFont.MeasureString("Are you sure").Y + PacificoFont.MeasureString("you want to").Y), Color.White);
                    Rectangle ConfirmButton = new Rectangle((width - (int)PacificoFont.MeasureString("Yes").X) / 2, (height - (int)PacificoFont.MeasureString("Yes").Y) / 2 + 260, (int)PacificoFont.MeasureString("Yes").X,(int)PacificoFont.MeasureString("Yes").Y);
                    Rectangle CancelButton = new Rectangle((width - (int)PacificoFont.MeasureString("No").X) / 2, (height - (int)PacificoFont.MeasureString("No").Y) / 2 + 130, (int)PacificoFont.MeasureString("Yes").X, (int)PacificoFont.MeasureString("No").Y);
                    if (ConfirmButton.Contains(mousePosition))
                    {
                        if (mouseState.LeftButton == ButtonState.Pressed)
                        {
                            ConfirmButtonClicked = true;
                            spriteBatch.Draw(rectangle, ConfirmButton, new Color(0, 0, 0, 0.3f));
                        }
                        else if (ConfirmButtonClicked == true)
                        {
                            File.WriteAllText(Directory.GetCurrentDirectory().Replace(@"bin\Windows\x86\Debug", "Content") + @"\save.txt", "0");
                            _state = GameState.MainMenu;
                            ConfirmButtonClicked = false;
                            ResetGame = false;
                        }
                        else
                        {
                            spriteBatch.Draw(rectangle, ConfirmButton, new Color(0, 0, 0, 0.2f));
                        }
                    }
                    else
                    {
                        ConfirmButtonClicked = false;
                    }
                    if (CancelButton.Contains(mousePosition))
                    {
                        if (mouseState.LeftButton == ButtonState.Pressed)
                        {
                            CancelButtonClicked = true;
                            spriteBatch.Draw(rectangle, CancelButton, new Color(0, 0, 0, 0.3f));
                        }
                        else if (CancelButtonClicked == true)
                        {
                            ResetGame = false;
                            CancelButtonClicked = false;
                        }
                        else
                        {
                            spriteBatch.Draw(rectangle, CancelButton, new Color(0, 0, 0, 0.2f));
                        }
                    }
                    else
                    {
                        CancelButtonClicked = false;
                    }
                    spriteBatch.DrawString(PacificoFont, "Yes", new Vector2((width - PacificoFont.MeasureString("Yes").X) / 2, (height - PacificoFont.MeasureString("Yes").Y) / 2 + 260), Color.White);
                    spriteBatch.DrawString(PacificoFont, "No", new Vector2((width - PacificoFont.MeasureString("No").X) / 2, (height - PacificoFont.MeasureString("No").Y) / 2 + 130), Color.White);
                }
            }
            if (_state == GameState.Pause)
            {
                Rectangle ResumeButton = new Rectangle((int)(width - PacificoFont.MeasureString("Resume").X) / 2 - 10, (int)(height - PacificoFont.MeasureString("Resume").Y) / 2 - 210, (int)PacificoFont.MeasureString("Resume").X + 20, (int)PacificoFont.MeasureString("Resume").Y);
                Rectangle QuitButton = new Rectangle((int)(width - PacificoFont.MeasureString("Quit").X) / 2 - 10, (int)(height - PacificoFont.MeasureString("Quit").Y) / 2 + 210, (int)PacificoFont.MeasureString("Quit").X + 20, (int)PacificoFont.MeasureString("Quit").Y);
                Rectangle SettingsButton = new Rectangle((int)(width - PacificoFont.MeasureString("Settings").X) / 2 - 10, (int)(height - PacificoFont.MeasureString("Settings").Y) / 2 + 70, (int)PacificoFont.MeasureString("Settings").X + 20, (int)PacificoFont.MeasureString("Settings").Y);
                Rectangle BackButton = new Rectangle((int)(width - PacificoFont.MeasureString("Main Menu").X) / 2 - 10, (int)(height - PacificoFont.MeasureString("Main Menu").Y) / 2 - 70, (int)PacificoFont.MeasureString("Main Menu").X + 20, (int)PacificoFont.MeasureString("Main Menu").Y);

                if (QuitButton.Contains(mousePosition))
                {
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        QuitButtonClicked = true;
                        spriteBatch.Draw(rectangle, QuitButton, new Color(0, 0, 0, 0.3f));
                    }
                    else if (QuitButtonClicked == true)
                    {
                        Exit();
                    }
                    else
                    {
                        spriteBatch.Draw(rectangle, QuitButton, new Color(0, 0, 0, 0.2f));
                    }
                }
                else
                {
                    QuitButtonClicked = false;
                }
                if (SettingsButton.Contains(mousePosition))
                {
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        SettingsButtonClicked = true;
                        spriteBatch.Draw(rectangle, SettingsButton, new Color(0, 0, 0, 0.3f));
                    }
                    else if (SettingsButtonClicked == true)
                    {
                        _state = GameState.Settings;
                        SettingsButtonClicked = false;
                    }
                    else
                    {
                        spriteBatch.Draw(rectangle, SettingsButton, new Color(0, 0, 0, 0.2f));
                    }
                }
                else
                {
                    SettingsButtonClicked = false;
                }
                if (BackButton.Contains(mousePosition) && _pausedstate != GameState.MainMenu)
                {
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        MainMenuButtonClicked = true;
                        spriteBatch.Draw(rectangle, BackButton, new Color(0, 0, 0, 0.3f));
                    }
                    else if (MainMenuButtonClicked == true)
                    {
                        _state = GameState.MainMenu;
                        MainMenuButtonClicked = false;
                    }
                    else
                    {
                        spriteBatch.Draw(rectangle, BackButton, new Color(0, 0, 0, 0.2f));
                    }
                }
                else
                {
                    MainMenuButtonClicked = false;
                }
                if (ResumeButton.Contains(mousePosition))
                {
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        ResumeButtonClicked = true;
                        spriteBatch.Draw(rectangle, ResumeButton, new Color(0, 0, 0, 0.3f));
                    }
                    else if (ResumeButtonClicked == true)
                    {
                        Pause();
                        ResumeButtonClicked = false;
                    }
                    else
                    {
                        spriteBatch.Draw(rectangle, ResumeButton, new Color(0, 0, 0, 0.2f));
                    }
                }
                else
                {
                    ResumeButtonClicked = false;
                }
                spriteBatch.DrawString(PacificoFont, "Quit", new Vector2((width - PacificoFont.MeasureString("Quit").X) / 2, (height - PacificoFont.MeasureString("Quit").Y) / 2 + 210), Color.White);
                spriteBatch.DrawString(PacificoFont, "Resume", new Vector2((width - PacificoFont.MeasureString("Resume").X) / 2, (height - PacificoFont.MeasureString("Resume").Y) / 2 - 210), Color.White);
                spriteBatch.DrawString(PacificoFont, "Settings", new Vector2((width - PacificoFont.MeasureString("Settings").X) / 2, (height - PacificoFont.MeasureString("Settings").Y) / 2 + 70), Color.White);
                if (_pausedstate != GameState.MainMenu)
                {
                    spriteBatch.DrawString(PacificoFont, "Main Menu", new Vector2((width - PacificoFont.MeasureString("Main Menu").X) / 2, (height - PacificoFont.MeasureString("Main Menu").Y) / 2 - 70), Color.White);
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
