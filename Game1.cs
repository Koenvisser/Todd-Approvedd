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
        private enum GameState { MainMenu, Settings,LevelSelect, Game, Pause };
        GameState _state = GameState.MainMenu;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D loadingleft, loadingright, rectangle, circle, pause1, pause2;
        private SpriteFont KronaFont, LevelSelectFont, PacificoFont;
        private float menuposition, volume;
        private bool loadingdone = false, menuanimationdone = false, PlayButtonClicked = false, SettingsButtonClicked = false, QuitButtonClicked = false, LevelButtonClicked = false, DragSlider = false, escdown = false, MainMenuButtonClicked, ResumeButtonClicked, fullscreen = false, fullscreensliderclick = false;
        private int LevelCompleted, SliderPosition;
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
            SliderPosition = (int)(graphics.PreferredBackBufferWidth * 0.75f - 250);
            string settingsline;
            System.IO.StreamReader settingsfile = new System.IO.StreamReader(Directory.GetCurrentDirectory().Replace(@"bin\Windows\x86\Debug", "Content") + @"\settings.txt");
            bool success1 = true, success2 = true;
            while ((settingsline = settingsfile.ReadLine()) != null)
            {
                if (settingsline.Contains("volume "))
                {
                    success1 = float.TryParse(settingsline.Replace("volume ", ""), out volume);
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
                File.WriteAllText(Directory.GetCurrentDirectory().Replace(@"bin\Windows\x86\Debug", "Content") + @"\settings.txt", "volume 1\nfullscreen true\n");
            }
            // TODO: use this.Content to load your game content here
            loadingdone = true;
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
                    if (_state == GameState.MainMenu)
                    {
                        Exit();
                    }
                    else if (_state == GameState.Settings || _state == GameState.LevelSelect)
                    {
                        PlayButtonClicked = false;
                        SettingsButtonClicked = false;
                        _state = GameState.MainMenu;
                    }
                    else if (_state == GameState.Game)
                    {
                        _state = GameState.Pause;
                    }
                    else if (_state == GameState.Pause)
                    {
                        _state = GameState.Game;
                    }
                    escdown = true;
                }
            }
            else { escdown = false; }
            if (_state == GameState.MainMenu)
            {
                IsMouseVisible = true;
                
                // TODO: Add your update logic here
                if (loadingdone == true && menuposition < graphics.PreferredBackBufferWidth * 0.6f && menuanimationdone == false)
                {
                    menuposition *= 1.02f;
                    menuposition += 3;
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
            if (_state == GameState.MainMenu)
            {
                if (menuanimationdone == true)
                {

                    Rectangle PlayButton = new Rectangle(width / 2 - (int)(KronaFont.MeasureString("Play").X * 0.6f + menuposition), (int)(height * .35f), (int)(KronaFont.MeasureString("Play").X * 1.2f), (int)KronaFont.MeasureString("Play").Y);
                    Rectangle SettingsButton = new Rectangle(width / 2 - (int)(KronaFont.MeasureString("Settings").X * 0.6f - menuposition), (int)(height * .4f + KronaFont.MeasureString("Play").Y), (int)(KronaFont.MeasureString("Settings").X * 1.2f), (int)KronaFont.MeasureString("Settings").Y);
                    Rectangle QuitButton = new Rectangle(width / 2 - (int)(KronaFont.MeasureString("Quit").X * 0.6f), (int)(height * .45f + KronaFont.MeasureString("Play").Y + KronaFont.MeasureString("Settings").Y + menuposition), (int)(KronaFont.MeasureString("Quit").X * 1.2f), (int)KronaFont.MeasureString("Quit").Y);

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
                            spriteBatch.Draw(rectangle, SettingsButton, new Color(0, 0, 0, 0.1f));
                        }
                    }
                    else { SettingsButtonClicked = false; }

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
                            QuitButtonClicked = false;
                        }
                        else
                        {
                            spriteBatch.Draw(rectangle, QuitButton, new Color(0, 0, 0, 0.1f));
                        }
                    }
                    else { QuitButtonClicked = false; }
                    spriteBatch.DrawString(KronaFont, "Play", new Vector2(graphics.PreferredBackBufferWidth / 2 - menuposition - KronaFont.MeasureString("Play").X / 2, graphics.PreferredBackBufferHeight * .35f), Color.White);
                    spriteBatch.DrawString(KronaFont, "Settings", new Vector2(graphics.PreferredBackBufferWidth / 2 + menuposition - KronaFont.MeasureString("Settings").X / 2, graphics.PreferredBackBufferHeight * .4f + KronaFont.MeasureString("Play").Y), Color.White);
                    spriteBatch.DrawString(KronaFont, "Quit", new Vector2(graphics.PreferredBackBufferWidth / 2 - KronaFont.MeasureString("Quit").X / 2, graphics.PreferredBackBufferHeight * .45f + KronaFont.MeasureString("Play").Y + KronaFont.MeasureString("Settings").Y + menuposition), Color.White);
                }

                if (menuanimationdone == false)
                {
                    spriteBatch.Draw(loadingleft, new Rectangle((int)menuposition * -1, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    spriteBatch.Draw(loadingright, new Rectangle((int)menuposition, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                }
            }
            else if (_state == GameState.Settings)
            {
                int volumeheight = 100;
                int fullscreenheight = 250 + (int)KronaFont.MeasureString("Volume").Y;
                spriteBatch.DrawString(KronaFont, "Volume", new Vector2(GraphicsDevice.Viewport.Width * 0.25f, volumeheight) - KronaFont.MeasureString("Volume") / 2, Color.White);
                spriteBatch.DrawString(KronaFont, "Fullscreen", new Vector2(GraphicsDevice.Viewport.Width * 0.25f, fullscreenheight) - KronaFont.MeasureString("Fullscreen") / 2, Color.White);
                int sliderwidth = (int)(GraphicsDevice.Viewport.Width * 0.75f);
                Rectangle SlideBar = new Rectangle(sliderwidth - 250, volumeheight, (int)(graphics.PreferredBackBufferWidth * .3f), 26);
                int FullscreenSliderPos = 0;
                Color FullscreenColor = new Color();
                if (fullscreen == true)
                {
                    FullscreenSliderPos = (int)(GraphicsDevice.Viewport.Width * 0.65f) + 77;
                    FullscreenColor = Color.Green;
                }
                else {
                    FullscreenSliderPos = (int)(GraphicsDevice.Viewport.Width * 0.65f) - 23;
                    FullscreenColor = Color.Red;
                }
                Rectangle FullscreenSlider = new Rectangle(FullscreenSliderPos, fullscreenheight - 9, 46, 46);
                Rectangle FullscreenRec = new Rectangle((int)(GraphicsDevice.Viewport.Width * 0.65f), fullscreenheight, 100, 26);
                if (FullscreenRec.Contains(mousePosition) || FullscreenSlider.Contains(mousePosition))
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
                    }
                }
                else {
                    fullscreensliderclick = false;
                }

                Color DragColor = Color.White;
                if ((SlideBar.Contains(mousePosition) || (DragSlider == true)) && mouseState.LeftButton == ButtonState.Pressed)
                {
                    DragColor = new Color(230, 230, 230);
                    SliderPosition = mousePosition.X - 15;
                    if (SliderPosition < sliderwidth - 250)
                    {
                        SliderPosition = sliderwidth - 250;
                    }
                    if (SliderPosition > sliderwidth - 296 + (int)(graphics.PreferredBackBufferWidth * .3f))
                    {
                        SliderPosition = sliderwidth - 296 + (int)(graphics.PreferredBackBufferWidth * .3f);
                    }
                    volume = SliderPosition - (sliderwidth - 250);
                    volume /= 270;
                    DragSlider = true;
                }
                else if (mouseState.LeftButton != ButtonState.Pressed && DragSlider == true)
                { DragSlider = false; }
                spriteBatch.Draw(rectangle, SlideBar, new Color(200, 200, 200));
                spriteBatch.Draw(rectangle, new Rectangle(sliderwidth - 250, volumeheight, SliderPosition + 265 - sliderwidth, 26), Color.Gray);
                spriteBatch.Draw(rectangle, FullscreenRec, FullscreenColor);
                spriteBatch.Draw(circle, FullscreenSlider, Color.White);
                spriteBatch.Draw(circle, new Rectangle(SliderPosition, volumeheight - 10, 46, 46), DragColor);

            }
            else if (_state == GameState.LevelSelect)
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
            else if (_state == GameState.Game || _state == GameState.Pause)
            {

            }
            if (_state == GameState.Pause)
            {
                spriteBatch.Draw(rectangle, new Rectangle(0, 0, width, height), new Color(0, 0, 0, 0.2f));
                spriteBatch.Draw(rectangle, new Rectangle(width / 2 - 250,height / 2 - 375,500,750), new Color(0, 0, 0, 0.35f));
                spriteBatch.Draw(pause1, new Rectangle(width / 2 - 300,height / 2 - 400,120,800), Color.White);
                spriteBatch.Draw(pause2, new Rectangle(width / 2 - 250, height / 2 - 430, 550, 120), Color.White);
                spriteBatch.Draw(pause1, new Rectangle(width / 2 + 200, height / 2 - 400, 120, 800), Color.White);
                spriteBatch.Draw(pause2, new Rectangle(width / 2 - 250, height / 2 + 300, 550, 120), Color.White);
                Rectangle MainMenuButton = new Rectangle((int)(width - LevelSelectFont.MeasureString("Main Menu").X) / 2 - 10, (int)(height - LevelSelectFont.MeasureString("Main Menu").Y) / 2 + 200, (int)LevelSelectFont.MeasureString("Main Menu").X + 20, (int)LevelSelectFont.MeasureString("Main Menu").Y);
                Rectangle ResumeButton = new Rectangle((int)(width - LevelSelectFont.MeasureString("Resume").X) / 2 - 10, (int)(height - LevelSelectFont.MeasureString("Resume").Y) / 2 - 200, (int)LevelSelectFont.MeasureString("Resume").X + 20, (int)LevelSelectFont.MeasureString("Resume").Y);
                if (MainMenuButton.Contains(mousePosition))
                {
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        MainMenuButtonClicked = true;
                        spriteBatch.Draw(rectangle, MainMenuButton, new Color(0, 0, 0, 0.5f));
                    }
                    else if (MainMenuButtonClicked == true)
                    {
                        _state = GameState.MainMenu;
                        MainMenuButtonClicked = false;
                    }
                    else
                    {
                        spriteBatch.Draw(rectangle, MainMenuButton, new Color(0, 0, 0, 0.4f));
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
                        spriteBatch.Draw(rectangle, ResumeButton, new Color(0, 0, 0, 0.5f));
                    }
                    else if (ResumeButtonClicked == true)
                    {
                        _state = GameState.Game;
                        ResumeButtonClicked = false;
                    }
                    else
                    {
                        spriteBatch.Draw(rectangle, ResumeButton, new Color(0, 0, 0, 0.4f));
                    }
                }
                else
                {
                    ResumeButtonClicked = false;
                }
                spriteBatch.DrawString(PacificoFont, "Main Menu", new Vector2((width - PacificoFont.MeasureString("Main Menu").X) / 2, (height - PacificoFont.MeasureString("Main Menu").Y) / 2 + 200), Color.White);
                spriteBatch.DrawString(PacificoFont, "Resume", new Vector2((width - PacificoFont.MeasureString("Resume").X) / 2, (height - PacificoFont.MeasureString("Resume").Y) / 2 - 200), Color.White);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
