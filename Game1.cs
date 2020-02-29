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
        private Texture2D loadingleft, loadingright, rectangle, circle;
        private SpriteFont KronaFont, LevelSelectFont;
        private float menuposition, volume;
        private bool loadingdone = false, menuanimationdone = false, PlayButtonClicked = false, SettingsButtonClicked = false, QuitButtonClicked = false, LevelButtonClicked = false, DragSlider = false;
        private int LevelCompleted, SliderPosition;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
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
            KronaFont = Content.Load<SpriteFont>("fonts/Krona");
            LevelSelectFont = Content.Load<SpriteFont>("fonts/Krona2");
            rectangle = new Texture2D(GraphicsDevice, 1, 1);
            circle = Content.Load<Texture2D>("images/circle");
            rectangle.SetData(new[] { Color.White });
            SliderPosition = GraphicsDevice.Viewport.Width / 4 * 3 - 150;
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

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                if (_state == GameState.MainMenu)
                {
                    Exit();
                }
                else if (_state == GameState.Settings)
                {
                    _state = GameState.MainMenu;
                }
            }
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
            var mouseState = Mouse.GetState();
            var mousePosition = new Point(mouseState.X, mouseState.Y);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            if (_state == GameState.MainMenu)
            {
                if (menuanimationdone == true)
                {

                    Rectangle PlayButton = new Rectangle();
                    Rectangle SettingsButton = new Rectangle();
                    Rectangle QuitButton = new Rectangle();

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
                            bool success = Int32.TryParse(File.ReadAllText(Directory.GetCurrentDirectory().Replace(@"bin\Windows\x86\Debug", "Content") + @"\save.txt"), out LevelCompleted);
                            if (success == false)
                            {
                                File.WriteAllText(Directory.GetCurrentDirectory().Replace(@"bin\Windows\x86\Debug", "Content") + @"\save.txt", "0");
                            }
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
                        }
                        else
                        {
                            spriteBatch.Draw(rectangle, QuitButton, new Color(0, 0, 0, 0.1f));
                        }
                    }
                    else { QuitButtonClicked = false; }
                    spriteBatch.DrawString(KronaFont, "Play", new Vector2(graphics.PreferredBackBufferWidth / 2 - menuposition, 200) - KronaFont.MeasureString("Play") / 2, Color.White);
                    spriteBatch.DrawString(KronaFont, "Settings", new Vector2(graphics.PreferredBackBufferWidth / 2 + menuposition, 300) - KronaFont.MeasureString("Settings") / 2, Color.White);
                    spriteBatch.DrawString(KronaFont, "Quit", new Vector2(graphics.PreferredBackBufferWidth / 2, 400 + menuposition) - KronaFont.MeasureString("Quit") / 2, Color.White);
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
                int fullscreenheight = 150 + (int)KronaFont.MeasureString("Fullscreen").Y; 
                spriteBatch.DrawString(KronaFont, "Volume", new Vector2(GraphicsDevice.Viewport.Width * 0.25f, volumeheight) - KronaFont.MeasureString("Volume") / 2, Color.White);
                spriteBatch.DrawString(KronaFont, "Fullscreen", new Vector2(GraphicsDevice.Viewport.Width * 0.25f, fullscreenheight) - KronaFont.MeasureString("Fullscreen") / 2, Color.White);
                int width = (int)(GraphicsDevice.Viewport.Width * 0.75f);
                Rectangle SlideBar = new Rectangle(width - 150, volumeheight, 300, 14);
                Color DragColor = Color.White;
                if ((SlideBar.Contains(mousePosition) || (DragSlider == true)) && mouseState.LeftButton == ButtonState.Pressed)
                {
                    DragColor = new Color(230,230,230);
                    SliderPosition = mousePosition.X - 15;
                    if (SliderPosition < width - 150)
                    {
                        SliderPosition = width - 150;
                    }
                    if (SliderPosition > width + 120)
                    {
                        SliderPosition = width + 120;
                    }
                    volume = SliderPosition - (width - 150);
                    volume /= 270;
                    DragSlider = true;
                }
                else if (mouseState.LeftButton != ButtonState.Pressed && DragSlider == true)
                { DragSlider = false; }
                spriteBatch.Draw(rectangle, SlideBar, new Color(200,200,200));
                spriteBatch.Draw(rectangle, new Rectangle(width - 150, volumeheight, SliderPosition + 165 - width, 14), Color.Gray);
                spriteBatch.Draw(circle, new Rectangle(SliderPosition, volumeheight - 7, 30, 30), DragColor);

            }
            else if (_state == GameState.LevelSelect)
            {
                int i = 0;
                Color LevelColor = Color.ForestGreen;
                Vector2 LevelSelectPosition = new Vector2();
                while (i <= LevelCompleted && i <= 4)
                {
                    if (i == 0)
                    { LevelSelectPosition = new Vector2(300, 200); }
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
                            spriteBatch.Draw(rectangle, new Rectangle((int)LevelSelectPosition.X - 25, (int)LevelSelectPosition.Y - 25, 50, 50), new Color(0, 0, 0, 0.6f));
                            LevelButtonClicked = true;
                        }
                        else if (LevelButtonClicked == true)
                        {
                            _state = GameState.Game;
                            // level = i
                        }
                        else
                        {
                            spriteBatch.Draw(rectangle, new Rectangle((int)LevelSelectPosition.X - 25, (int)LevelSelectPosition.Y - 25, 50, 50), new Color(0, 0, 0, 0.3f));
                        }
                    }
                    else
                    {
                        spriteBatch.Draw(rectangle, new Rectangle((int)LevelSelectPosition.X - 25, (int)LevelSelectPosition.Y - 25, 50, 50), new Color(0, 0, 0, 0.15f));
                    }
                    spriteBatch.Draw(rectangle, new Rectangle((int)LevelSelectPosition.X - 31, (int)LevelSelectPosition.Y - 31, 6, 62), LevelColor);
                    spriteBatch.Draw(rectangle, new Rectangle((int)LevelSelectPosition.X + 25, (int)LevelSelectPosition.Y - 31, 6, 62), LevelColor);
                    spriteBatch.Draw(rectangle, new Rectangle((int)LevelSelectPosition.X - 31, (int)LevelSelectPosition.Y - 31, 62, 6), LevelColor);
                    spriteBatch.Draw(rectangle, new Rectangle((int)LevelSelectPosition.X - 31, (int)LevelSelectPosition.Y + 25, 62, 6), LevelColor);
                    spriteBatch.DrawString(LevelSelectFont, i.ToString(), LevelSelectPosition - LevelSelectFont.MeasureString(i.ToString()) / 2, Color.White);

                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
