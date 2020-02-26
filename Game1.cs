using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace OakHeart
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private enum GameState { MainMenu, Game, Pause };
        GameState _state = GameState.MainMenu;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D loadingleft, loadingright, rectangle;
        private SpriteFont KronaFont;
        private float menuposition;
        private bool loadingdone = false, menuanimationdone = false;
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
            rectangle = new Texture2D(GraphicsDevice, 1, 1);
            rectangle.SetData(new[] { Color.White });
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
            if (_state == GameState.MainMenu)
            {
                IsMouseVisible = true;
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                {

                }
                // TODO: Add your update logic here
                if (loadingdone == true && menuposition < 500 && menuanimationdone == false)
                {
                    menuposition *= 1.02f;
                    menuposition += 3;
                }
                if (menuposition >= 500 && menuanimationdone == false)
                {
                    menuanimationdone = true;
                    menuposition = 550;
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            if (_state == GameState.MainMenu)
            {
                if (menuanimationdone == true)
                {
                    var mouseState = Mouse.GetState();
                    var mousePosition = new Point(mouseState.X, mouseState.Y);
                    Rectangle PlayButton = new Rectangle(360 - (int)menuposition - (int)KronaFont.MeasureString("Play").X / 2, 202 - (int)KronaFont.MeasureString("Play").Y / 2, (int)KronaFont.MeasureString("Play").X + 80, (int)KronaFont.MeasureString("Play").Y);
                    Rectangle SettingsButton = new Rectangle(360 + (int)menuposition - (int)KronaFont.MeasureString("Settings").X / 2, 302 - (int)KronaFont.MeasureString("Settings").Y / 2, (int)KronaFont.MeasureString("Settings").X + 80, (int)KronaFont.MeasureString("Settings").Y);
                    Rectangle QuitButton = new Rectangle(360 - (int)KronaFont.MeasureString("Quit").X / 2, 402 + (int)menuposition - (int)KronaFont.MeasureString("Quit").Y / 2, (int)KronaFont.MeasureString("Quit").X + 80, (int)KronaFont.MeasureString("Quit").Y);

                    if (PlayButton.Contains(mousePosition))
                    {
                        spriteBatch.Draw(rectangle, PlayButton, new Color(0, 0, 0, 0.1f));
                        if (mouseState.LeftButton == ButtonState.Pressed)
                        {
                            _state = GameState.Game;
                        }

                    }
                    if (SettingsButton.Contains(mousePosition))
                    {
                        spriteBatch.Draw(rectangle, SettingsButton, new Color(0, 0, 0, 0.1f));
                    }
                    if (QuitButton.Contains(mousePosition))
                    {
                        spriteBatch.Draw(rectangle, QuitButton, new Color(0, 0, 0, 0.1f));
                        if (mouseState.LeftButton == ButtonState.Pressed)
                        {
                            Exit();
                        }
                    }
                    spriteBatch.DrawString(KronaFont, "Play", new Vector2(400 - menuposition, 200) - KronaFont.MeasureString("Play") / 2, Color.White);
                    spriteBatch.DrawString(KronaFont, "Settings", new Vector2(400 + menuposition, 300) - KronaFont.MeasureString("Settings") / 2, Color.White);
                    spriteBatch.DrawString(KronaFont, "Quit", new Vector2(400, 400 + menuposition) - KronaFont.MeasureString("Quit") / 2, Color.White);
                }

                if (menuanimationdone == false)
                {
                    spriteBatch.Draw(loadingleft, new Rectangle((int)menuposition * -1, 0, 800, 480), Color.White);
                    spriteBatch.Draw(loadingright, new Rectangle((int)menuposition, 0, 800, 480), Color.White);
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
