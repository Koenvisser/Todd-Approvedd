using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace OakHeart
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        enum GameState { MainMenu, Settings, LevelSelect, Game, Pause, Cutscene };
        GameState _state = GameState.MainMenu;
        GameState _pausedstate = GameState.MainMenu;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Background background;
        InputHelper inputHelper;
        private Texture2D loadingleft, loadingright, rectangle, circle, pause1, pause2, menuleave, placeholder, logo;
        private Texture2D[] levelselecttrees = new Texture2D[4];
        private SpriteFont KronaFont, LevelSelectFont, PacificoFont;
        private float menuposition, volume, timer, bottombarfade, levelselectfade, cutscenetimer, logoposition;
        private float[] angles = new float[30];
        private int[] LevelsProgress = new int[4];
        private bool loadingdone = false, menuanimationdone = false, menuanimationdone2 = false, menusongfadeout = false, soundfadeout = false, PlayButtonClicked = false, SettingsButtonClicked = false, ConfirmButtonClicked = false, CancelButtonClicked = false, QuitButtonClicked = false, DragSlider = false, escdown = false, ResetButtonClicked, MainMenuButtonClicked, ResumeButtonClicked, fullscreen = false, fullscreensliderclick = false, BackButtonClicked = false, ResetGame = false, CutscenePlaying = false;
        private bool[] LevelButtonClicked = new bool[4], hoveringbutton = new bool[4];
        private int LevelCompleted, SliderPosition, ElapsedTime, EasterEgssFound;
        Vector2 lastcollision;
        private SoundEffectInstance backgroundsongmenu;
        private Vector2[] menuleavespos = new Vector2[30];
        List<SoundEffect> soundEffects;
        Player player;
        Camera camera;
        public int levelint;
        public List<Map> levels;
        public Map level;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            soundEffects = new List<SoundEffect>();
        }

        protected static Point screen;
        protected static AssetManager assetManager;
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
            screen = new Point(1920, 1080);
            assetManager = new AssetManager(Content);
            base.Initialize();
            inputHelper = new InputHelper();

        }


        public static Point Screen
        {
            get
            { return screen; }
            set
            { screen = value; }
        }



        public static AssetManager AssetManager
        {
            get
            { return assetManager; }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            placeholder = Content.Load<Texture2D>("images/cutscene/placeholder");
            logo = Content.Load<Texture2D>("images/menu/Logo");
            loadingleft = Content.Load<Texture2D>("images/menu/left");
            loadingright = Content.Load<Texture2D>("images/menu/right");
            menuleave = Content.Load<Texture2D>("images/menu/menuleave");
            pause1 = Content.Load<Texture2D>("images/menu/pause1");
            pause2 = Content.Load<Texture2D>("images/menu/pause2");
            for (int i = 1; i <= 4; i++)
            {
                levelselecttrees[i - 1] = Content.Load<Texture2D>("images/menu/tree" + i);
            }
            KronaFont = Content.Load<SpriteFont>("fonts/Krona");
            LevelSelectFont = Content.Load<SpriteFont>("fonts/Krona2");
            PacificoFont = Content.Load<SpriteFont>("fonts/Pacifico");
            rectangle = new Texture2D(GraphicsDevice, 1, 1);
            circle = Content.Load<Texture2D>("images/menu/circle");
            soundEffects.Add(Content.Load<SoundEffect>("sounds/backgroundmenu"));
            rectangle.SetData(new[] { Color.White });
            backgroundsongmenu = soundEffects[0].CreateInstance();
            backgroundsongmenu.IsLooped = true;
            string settingsline;
            if (!File.Exists(Directory.GetCurrentDirectory() + @"\settings.txt"))
            {
                File.WriteAllText(Directory.GetCurrentDirectory() + @"\settings.txt", "volume 1\nfullscreen True\n");
            }
            StreamReader settingsfile = new StreamReader(Directory.GetCurrentDirectory() + @"\settings.txt");
            bool success1 = false, success2 = false;
            while ((settingsline = settingsfile.ReadLine()) != null)
            {
                if (settingsline.Contains("volume "))
                {
                    success1 = float.TryParse(settingsline.Replace("volume ", ""), out volume);
                    SliderPosition = (int)(graphics.PreferredBackBufferWidth / 2 - 173 + 300 * volume);
                    SoundEffect.MasterVolume = volume;
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
            if (!success1 || !success2)
            {
                File.WriteAllText(Directory.GetCurrentDirectory() + @"\settings.txt", "volume 1\nfullscreen True\n");
            }
            menuleavespos[29] = new Vector2(graphics.PreferredBackBufferWidth / 2 - KronaFont.MeasureString("Play").X * 0.6f - 20, graphics.PreferredBackBufferHeight * .5f - 20);
            LoadSave();
            // TODO: use this.Content to load your game content here
            loadingdone = true;
            IsMouseVisible = true;
            background = new Background(Content, new Vector2(0, 0), "images/game/Level_1_Background");
            player = new Player(new Vector2(0, 600));
            levels = new List<Map>();
            for (int x = 1; x <= 4; x++)
                levels.Add(new Map(x));
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
            int i = 0;
            string saveline;
            if (!File.Exists(Directory.GetCurrentDirectory() + @"\save.txt"))
            {
                File.WriteAllText(Directory.GetCurrentDirectory() + @"\save.txt", "0\n0\n0\n0\n0\n0\n");
            }
            StreamReader savefile = new StreamReader(Directory.GetCurrentDirectory() + @"\save.txt");
            while ((saveline = savefile.ReadLine()) != null)
            {
                bool success = false;
                if (i == 0)
                {
                    success = Int32.TryParse(saveline, out LevelCompleted);
                }
                else if (i == 1)
                {
                    success = Int32.TryParse(saveline, out EasterEgssFound);
                }
                else if (i <= 5)
                {
                    success = Int32.TryParse(saveline, out LevelsProgress[i - 2]);
                }
                if (!success)
                {
                    File.WriteAllText(Directory.GetCurrentDirectory() + @"\save.txt", "0\n0\n0\n0\n0\n0\n");
                }
                i++;
            }
            savefile.Close();
        }

        private void LevelComplete(int level, int fungusclearedpercent)
        {
            string saveline, previoustext = "";
            int i = 0;
            bool success = false, success2 = false;
            int previouslevelcompleted = 0, previousfunguscleared = 0;
            StreamReader savefile = new StreamReader(Directory.GetCurrentDirectory() + @"\save.txt");
            while ((saveline = savefile.ReadLine()) != null)
            {
                if (i == 0)
                {
                    success = Int32.TryParse(saveline, out previouslevelcompleted);
                }
                else if (i == level + 1)
                {
                    success2 = Int32.TryParse(saveline, out previousfunguscleared);
                    if (fungusclearedpercent > previousfunguscleared)
                    {
                        previoustext += fungusclearedpercent + "\n";
                    }
                    else { previoustext += saveline + "\n"; }
                }
                else
                {
                    previoustext += saveline + "\n";
                }
                i++;
            }
            savefile.Close();
            if (level <= previouslevelcompleted)
            {
                if (fungusclearedpercent > previousfunguscleared)
                {
                    File.WriteAllText(Directory.GetCurrentDirectory() + @"\save.txt", previouslevelcompleted + "\n" + previoustext);
                }
                else
                {
                    return;
                }
            }
            else if (!success)
            {
                File.WriteAllText(Directory.GetCurrentDirectory() + @"\save.txt", "0\n0\n0\n0\n0\n0\n");
            }
            else
            {
                File.WriteAllText(Directory.GetCurrentDirectory() + @"\save.txt", level + "\n" + previoustext);
            }
        }

        private void FoundEasterEgg(string eastereggname)
        {
            if (!File.Exists(Directory.GetCurrentDirectory() + @"\eastereggsfound.txt"))
            {
                File.WriteAllText(Directory.GetCurrentDirectory() + @"\eastereggsfound.txt", "\n");
            }
            string eastereggfiletext = "";
            string[] alleastereggsfound = File.ReadAllLines(Directory.GetCurrentDirectory() + @"\eastereggsfound.txt");
            foreach (string x in alleastereggsfound)
            {
                if (x == eastereggname)
                { return; }
                else
                {
                    eastereggfiletext += x + "\n";
                }
            }
            string saveline, previoustext = "", level = "";
            int i = 0, previouseastereggs = 0;
            bool success = false;
            StreamReader savefile = new StreamReader(Directory.GetCurrentDirectory() + @"\save.txt");
            while ((saveline = savefile.ReadLine()) != null)
            {
                if (i == 0)
                {
                    level = saveline;
                }
                else if (i == 1)
                {
                    success = Int32.TryParse(saveline, out previouseastereggs);
                }
                else { previoustext += saveline + "\n"; }
                i++;
            }
            savefile.Close();
            if (!success)
            {
                File.WriteAllText(Directory.GetCurrentDirectory() + @"\save.txt", "0\n0\n0\n0\n0\n0\n0\n");
            }
            else
            {
                previouseastereggs++;
                File.WriteAllText(Directory.GetCurrentDirectory() + @"\save.txt", level + "\n" + previouseastereggs + "\n" + previoustext);
                File.WriteAllText(Directory.GetCurrentDirectory() + @"\eastereggsfound.txt", eastereggfiletext + eastereggname + "\n");
            }
        }

        private void Pause()
        {
            if (_state != GameState.Pause && _state != GameState.Settings)
            {
                if (_state == GameState.Game)
                {
                    if (assetManager.sound != null && assetManager.sound.State == SoundState.Playing)
                    {
                        assetManager.sound.Pause();
                    }
                }
                _pausedstate = _state;
                _state = GameState.Pause;
                IsMouseVisible = true;
                if (_pausedstate == GameState.MainMenu || _pausedstate == GameState.LevelSelect)
                {
                    soundfadeout = true;
                }
            }
            else if (_state == GameState.Settings)
            {
                _state = GameState.Pause;
            }
            else
            {
                _state = _pausedstate;
                soundfadeout = false;
                if (_state == GameState.Game)
                { IsMouseVisible = false;
                    if (assetManager.sound != null && assetManager.sound.State == SoundState.Paused)
                    {
                        assetManager.sound.Resume();
                    }
                }
                else if (_pausedstate == GameState.MainMenu || _pausedstate == GameState.LevelSelect)
                {
                    if (backgroundsongmenu.State == SoundState.Stopped)
                    {
                        backgroundsongmenu.Play();
                    }
                }
            }


        }

        private void PlayCutscene(GameTime gameTime, SpriteBatch spriteBatch)
        {
            float fade = 1;
            if (_state != GameState.Settings && _state != GameState.Pause)
            {
                if (cutscenetimer > 1000)
                {
                    _state = GameState.Cutscene;
                }
                if (AssetManager.sound != null && assetManager.sound.State == SoundState.Paused)
                {
                    assetManager.sound.Resume();
                }
            }
            else if (_state == GameState.MainMenu)
            {
                CutscenePlaying = false;
            }
            else if (assetManager.sound.State == SoundState.Playing)
            {
                assetManager.sound.Pause();
            }
            if (cutscenetimer > 500 && cutscenetimer < 47500)
            {
                spriteBatch.Draw(rectangle, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.Black);
            }
                if (LevelCompleted == 0)
            {
                if (cutscenetimer < 10000)
                {
                    if (cutscenetimer < 1000)
                    {
                        fade = cutscenetimer / 1000;
                    }
                    else if (cutscenetimer > 9000)
                    {
                        fade = (10000 - cutscenetimer) / 1000;
                    }
                    if (AssetManager.sound == null && cutscenetimer < 500)
                    {
                        AssetManager.PlaySound("voicelines/Cutscene1/line1", false);
                    }
                    spriteBatch.Draw(placeholder, new Rectangle(0 - (int)(cutscenetimer / 100), 0 - (int)(cutscenetimer / 100), graphics.PreferredBackBufferWidth + 100, graphics.PreferredBackBufferHeight + 100), Color.White * fade);
                }
                else if (cutscenetimer < 29000)
                {
                    if (cutscenetimer < 11000)
                    {
                        fade = (cutscenetimer - 10000) / 1000;
                    }
                    else if (cutscenetimer > 28000)
                    {
                        fade = (29000 - cutscenetimer) / 1000;
                    }
                    if (AssetManager.sound.State == SoundState.Stopped && cutscenetimer < 10500)
                    {
                        AssetManager.PlaySound("voicelines/Cutscene1/line2", false);
                    }
                    else if (AssetManager.sound.State == SoundState.Stopped && cutscenetimer < 20000 && cutscenetimer > 18000)
                    {
                        AssetManager.PlaySound("voicelines/Cutscene1/dialogue1", false);
                    }
                    spriteBatch.Draw(placeholder, new Rectangle(-50 + (int)((cutscenetimer - 10000) / 290), 50 - (int)((cutscenetimer - 10000) / 290), graphics.PreferredBackBufferWidth + 100, graphics.PreferredBackBufferHeight + 100), Color.White * fade);
                }
                else if (cutscenetimer < 48000)
                {
                    if (cutscenetimer < 30000)
                    {
                        fade = (cutscenetimer - 29000) / 1000;
                    }
                    else if (cutscenetimer > 47000)
                    {
                        fade = (48000 - cutscenetimer) / 1000;
                    }
                    if (AssetManager.sound.State == SoundState.Stopped && cutscenetimer < 29500)
                    {
                        AssetManager.PlaySound("voicelines/Cutscene1/dialogue2", false);
                    }
                    spriteBatch.Draw(placeholder, new Rectangle(50 - (int)((cutscenetimer - 10000) / 290), -50 + (int)((cutscenetimer - 10000) / 290), graphics.PreferredBackBufferWidth + 100, graphics.PreferredBackBufferHeight + 100), Color.White * fade);
                    if (cutscenetimer > 47500)
                    {
                        _state = GameState.Game;
                    }
                }
                else if (cutscenetimer >= 48000)
                {
                    CutscenePlaying = false;
                    _state = GameState.Game;
                }
                if (_state == GameState.Cutscene || cutscenetimer <= 1000 || (cutscenetimer > 47500 && _state == GameState.Game))
                {
                    cutscenetimer += gameTime.ElapsedGameTime.Milliseconds;
                }
            }
             }

                 /// <summary>
                 /// Allows the game to run logic such as updating the world,
                 /// checking for collisions, gathering input, and playing audio.
                 /// </summary>
                 /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.Milliseconds;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {

                if (!escdown)
                {
                    Pause();
                    escdown = true;
                }
            }
            else { escdown = false; }
            if (_state == GameState.MainMenu)
            {
                // TODO: Add your update logic here
                if (loadingdone == true && menuposition < graphics.PreferredBackBufferWidth * 0.6f && !menuanimationdone && ElapsedTime > 500)
                {
                    menuposition *= 1.02f;
                    menuposition += 3;
                }
                else if (ElapsedTime <= 500)
                {
                    ElapsedTime += gameTime.ElapsedGameTime.Milliseconds;
                }
                if (menuposition >= graphics.PreferredBackBufferWidth * 0.6f && !menuanimationdone)
                {
                    menuanimationdone = true;
                    menuposition = graphics.PreferredBackBufferWidth * 0.6f;
                }
                if (menuanimationdone == true && menuposition > 0)
                {
                    menuposition *= 0.96f;
                    menuposition -= 2;
                }
                
                if (menuanimationdone2 == true && timer >= 40)
                {
                    logoposition += gameTime.ElapsedGameTime.Milliseconds * 0.0025f;
                    Console.WriteLine(logoposition);
                    if (logoposition > 1)
                    {
                        logoposition = 0;
                    }
                    Random random = new Random();
                    float angle = random.Next(0, 1000);
                    angle *= .01f;
                    for (int i = 0; i < 29; i++)
                    {
                        angles[i] = angles[i + 1];
                        menuleavespos[i] = menuleavespos[i + 1];
                    }
                    angles[29] = angle;
                    if (menuleavespos[29].X <= graphics.PreferredBackBufferWidth / 2 - KronaFont.MeasureString("Play").X * 0.6f - 20 && menuleavespos[29].Y > graphics.PreferredBackBufferHeight * .5f - 15)
                    {
                        menuleavespos[29].X = graphics.PreferredBackBufferWidth / 2 - KronaFont.MeasureString("Play").X * 0.6f - 20;
                        menuleavespos[29].Y = menuleavespos[28].Y - 25;
                    }
                    if (menuleavespos[29].X > graphics.PreferredBackBufferWidth / 2 - KronaFont.MeasureString("Play").X * 0.6f - 20 && menuleavespos[29].Y >= graphics.PreferredBackBufferHeight * .5f + 40 + KronaFont.MeasureString("Play").Y)
                    {
                        menuleavespos[29].X = menuleavespos[28].X - 20;
                        menuleavespos[29].Y = graphics.PreferredBackBufferHeight * .5f + 40 + KronaFont.MeasureString("Play").Y;
                    }
                    if (menuleavespos[29].X >= graphics.PreferredBackBufferWidth / 2 + KronaFont.MeasureString("Play").X * 0.6f + 15 && menuleavespos[29].Y < graphics.PreferredBackBufferHeight * .5f + 40 + KronaFont.MeasureString("Play").Y)
                    {
                        menuleavespos[29].X = graphics.PreferredBackBufferWidth / 2 + KronaFont.MeasureString("Play").X * 0.6f + 20;
                        menuleavespos[29].Y = menuleavespos[28].Y + 25;
                    }
                    if (menuleavespos[29].X < graphics.PreferredBackBufferWidth / 2 + KronaFont.MeasureString("Play").X * 0.6f + 20 && menuleavespos[29].Y <= graphics.PreferredBackBufferHeight * .5f - 15)
                    {
                        menuleavespos[29].X = menuleavespos[28].X + 20;
                        menuleavespos[29].Y = graphics.PreferredBackBufferHeight * .5f - 20;
                    }
                    if (menuleavespos[29].X <= graphics.PreferredBackBufferWidth / 2 - KronaFont.MeasureString("Play").X * 0.6f - 20 && menuleavespos[29].Y > graphics.PreferredBackBufferHeight * .5f - 15)
                    {
                        menuleavespos[29].X = graphics.PreferredBackBufferWidth / 2 - KronaFont.MeasureString("Play").X * 0.6f - 20;
                        menuleavespos[29].Y = menuleavespos[28].Y - 25;
                    }
                    if (menuleavespos[29].X > graphics.PreferredBackBufferWidth / 2 - KronaFont.MeasureString("Play").X * 0.6f - 20 && menuleavespos[29].Y >= graphics.PreferredBackBufferHeight * .5f + 40 + KronaFont.MeasureString("Play").Y)
                    {
                        menuleavespos[29].X = menuleavespos[28].X - 20;
                        menuleavespos[29].Y = graphics.PreferredBackBufferHeight * .5f + 40 + KronaFont.MeasureString("Play").Y;
                    }
                    timer = 0;
                }
            }
            else if(_state != GameState.MainMenu && _state != GameState.LevelSelect)
            {
                if (menusongfadeout && backgroundsongmenu.Volume >= 0.01f)
                {
                    backgroundsongmenu.Volume -= 0.01f;
                }
                else if (backgroundsongmenu.State == SoundState.Playing && backgroundsongmenu.Volume < 0.01f)
                {
                    backgroundsongmenu.Stop();
                }
            }
            if (_state == GameState.MainMenu || _state == GameState.LevelSelect)
            {
                if (menuanimationdone && menuposition < 0 && !menuanimationdone2)
                {
                    menuposition = 0;
                    menuanimationdone2 = true;
                    timer = 0;
                    SoundEffect.MasterVolume = 0;
                    backgroundsongmenu.Play();
                }
                if (!menusongfadeout && backgroundsongmenu.Volume <= 0.995f)
                {
                    backgroundsongmenu.Volume += 0.005f;
                }
            }
            if (_state == GameState.Game)
            {
                if (player.idletime > 15 && (assetManager.sound == null || (assetManager.sound != null && assetManager.sound.State != SoundState.Playing)))
                {
                    Random random = new Random();
                    int rdm = random.Next(1,2);
                    assetManager.PlaySound("voicelines/Oakheart/tiktik" + rdm, false);
                    FoundEasterEgg("Tiktik");
                }
                if (player.jump == true)
                {
                    Random random = new Random();
                    int soundn = random.Next(0,3);
                    string randomsound = "";
                    if (soundn == 0)
                    {
                        randomsound = "yay";
                    }
                    else if (soundn == 1)
                    {
                        randomsound = "woo";
                    }
                    else if (soundn == 2)
                    {
                        randomsound = "wee";
                    }
                    else {
                        randomsound = "wahoo";
                    }
                    assetManager.PlaySound("voicelines/Oakheart/" + randomsound, false);
                }
                if (player.wallslide)
                { 
                    player.velocity.Y = 50;
                }
                else if (!player.playercol && !player.wallslide) { 
                    player.velocity.Y += 10;
                }
                player.playercol = false;
                player.wallslide = false;
                player.isOnFloor = false;
                player.Update(gameTime);
                HandleInput(gameTime);
                foreach (Platform platform in level.Platform)
                {

                    if (player.CollidesWith(platform))
                    {
                        if (player.position.Y + player.Height - 80 <= platform.position.Y && platform.rot != 90)
                        {
                            player.isOnFloor = true;
                            player.walljumping = false;
                        }
                        if ((platform.rot == 90 || platform.rot == 270) && player.position.X <= platform.position.X)
                        {
                            player.wallslide = true;
                        }
                        if ((platform.rot == 90 || platform.rot == 270) && player.position.X <= platform.position.X && !((lastcollision.X == platform.position.X) && (lastcollision.Y == platform.position.Y)))
                        {
                            player.walljump = true;
                            player.walljumping = false;
                        }
                        lastcollision = platform.position;
                        player.playercol = true;
                        platform.Update(gameTime, true);
                        player.position += player.MTV;
                        platform.playerpos = player.position;
                    }
                    platform.Update(gameTime, false);
                }
                foreach (Enemy enemy in level.Enemy)
                {
                    if (player.CollidesWith(enemy))
                    {

                    }
                }
                player.HandleInput(inputHelper);
                camera = new Camera(player);
                camera.camera(gameTime, levelint);
            }
            if (SoundEffect.MasterVolume <= 0.995f && !soundfadeout)
            {
                SoundEffect.MasterVolume += 0.005f;
            }
            else if (SoundEffect.MasterVolume >= 0.005f && soundfadeout)
            {
                SoundEffect.MasterVolume -= 0.005f;
            }
            base.Update(gameTime);
        }

        protected void HandleInput(GameTime gameTime)
        {
            inputHelper.Update();

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

                    Rectangle PlayButton = new Rectangle(width / 2 - (int)(KronaFont.MeasureString("Play").X * 0.6f), (int)(height * .5f + menuposition), (int)(KronaFont.MeasureString("Play").X * 1.2f), (int)KronaFont.MeasureString("Play").Y + 20);
                    if (PlayButton.Contains(mousePosition) && _state == GameState.MainMenu)
                    {
                        if (mouseState.LeftButton == ButtonState.Pressed)
                        {
                            PlayButtonClicked = true;
                            spriteBatch.Draw(rectangle, PlayButton, new Color(0, 0, 0, 0.3f));
                        }
                        else if (PlayButtonClicked == true)
                        {
                            _state = GameState.LevelSelect;
                            levelselectfade = 1;
                            PlayButtonClicked = false;
                        }
                        else
                        {
                            spriteBatch.Draw(rectangle, PlayButton, new Color(0, 0, 0, 0.1f));
                        }
                    }
                    else { PlayButtonClicked = false; }
                    float logopos = logoposition;
                    if (logopos > 0.5f)
                    {
                        logopos = 1 - logoposition;
                    }
                    spriteBatch.Draw(logo,new Rectangle(width / 2 - 750,100 - (int)menuposition - (int)(logopos * 40), 1500, 225), Color.White);
                    spriteBatch.DrawString(KronaFont, "Play", new Vector2(graphics.PreferredBackBufferWidth / 2 - KronaFont.MeasureString("Play").X / 2, graphics.PreferredBackBufferHeight * .5f + menuposition), Color.White);
                    if (menuanimationdone2 == true)
                    {
                        Color leavecolor = Color.White;
                        for (int i = 0; i < 30; i++)
                        {
                            if (menuleavespos[i].X != 0 && menuleavespos[i].Y != 0)
                            {
                                if (i == 0)
                                {
                                    leavecolor = Color.White * (1 - timer / 40);
                                }
                                else if (i == 29)
                                {
                                    leavecolor = Color.White * (timer / 40);
                                }
                                else
                                {
                                    leavecolor = Color.White;
                                }
                                spriteBatch.Draw(menuleave, menuleavespos[i], new Rectangle(0, 0, menuleave.Width, menuleave.Height), leavecolor, angles[i], new Vector2(menuleave.Width / 2, menuleave.Height / 2), .08f, SpriteEffects.None, 1);
                            }
                        }
                    }
                }

                if (!menuanimationdone)
                {
                    spriteBatch.Draw(loadingleft, new Rectangle((int)menuposition * -1, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    spriteBatch.Draw(loadingright, new Rectangle((int)menuposition, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                }
            }

            else if (_state == GameState.LevelSelect || ((_state == GameState.Pause || _state == GameState.Settings) && _pausedstate == GameState.LevelSelect))
            {
                int backgroundtree = LevelCompleted;
                if (backgroundtree >= 4) { backgroundtree = 3; }
                spriteBatch.Draw(levelselecttrees[backgroundtree],new Rectangle(width / 2 - (1000 / levelselecttrees[backgroundtree].Height * levelselecttrees[backgroundtree].Width) / 2, height - 1000, 1000 / levelselecttrees[backgroundtree].Height * levelselecttrees[backgroundtree].Width, 1000), Color.White * levelselectfade);
                if (levelselectfade <= 0.995f && backgroundtree > 0)
                {
                    spriteBatch.Draw(levelselecttrees[backgroundtree - 1], new Rectangle(width / 2 - (1000 / levelselecttrees[backgroundtree - 1].Height * levelselecttrees[backgroundtree - 1].Width) / 2, height - 1000, 1000 / levelselecttrees[backgroundtree - 1].Height * levelselecttrees[backgroundtree - 1].Width, 1000), Color.White * (1 - levelselectfade));
                    if (_state == GameState.LevelSelect)
                    {
                        levelselectfade += 0.005f;
                    }
                    }
                else if (levelselectfade <= 0.995f && backgroundtree == 0)
                {
                    levelselectfade = 1;
                }
                int percentage = LevelCompleted * 25;
                spriteBatch.Draw(rectangle, new Rectangle(0, 0, width, height / 10), Color.Black * .3f);
                spriteBatch.DrawString(LevelSelectFont, percentage + "% Levels Completed", new Vector2(width / 4, height / 20) - LevelSelectFont.MeasureString(percentage + "% Levels Completed") / 2, Color.White);
                spriteBatch.DrawString(LevelSelectFont, (float)(EasterEgssFound) / 4 * 100 + "% Easter Eggs Found", new Vector2(width / 4 * 3, height / 20) - LevelSelectFont.MeasureString((float)(EasterEgssFound) / 4 * 100 + "% Easter Eggs Found") / 2, Color.White);

                int i = 0;
                Color LevelColor = Color.ForestGreen;
                Vector2[] LevelSelectPosition = new Vector2[4];
                Rectangle[] LevelButton = new Rectangle[4];
                while (i <= LevelCompleted && i <= 3)
                {
                    if (i == 0)
                    { LevelSelectPosition[i] = new Vector2(.5f * graphics.PreferredBackBufferWidth, .5f * graphics.PreferredBackBufferHeight); }
                    else if (i == 1)
                    { LevelSelectPosition[i] = new Vector2(100, 100); }
                    else if (i == 2)
                    { LevelSelectPosition[i] = new Vector2(0, 0); }
                    else if (i == 3)
                    { LevelSelectPosition[i] = new Vector2(0, 0); }
                    if (i >= LevelCompleted)
                    {
                        LevelColor = Color.Red;
                    }
                    i++;
                    LevelButton[i - 1] = new Rectangle((int)LevelSelectPosition[i - 1].X - 48, (int)LevelSelectPosition[i - 1].Y - 48, 96, 96);
                    if (bottombarfade > 0)
                    {
                        spriteBatch.Draw(rectangle, new Rectangle(0, height - height / 10, width, height / 10), Color.Black * .3f * bottombarfade);
                        spriteBatch.DrawString(LevelSelectFont, LevelsProgress[i - 1] + "% Fungus Cleared", new Vector2(width / 2, height - height / 20) - LevelSelectFont.MeasureString(LevelsProgress[i - 1] + "% Fungus Cleared") / 2, Color.White * bottombarfade);
                    }
                    if (LevelButton[i - 1].Contains(mousePosition) && _state == GameState.LevelSelect)
                    {
                        hoveringbutton[i - 1] = true;
                        if (bottombarfade <= .95f)
                        {
                            bottombarfade += .05f;
                        }
                        if (mouseState.LeftButton == ButtonState.Pressed)
                        {
                            spriteBatch.Draw(rectangle, new Rectangle((int)LevelSelectPosition[i - 1].X - 38, (int)LevelSelectPosition[i - 1].Y - 38, 76, 76), new Color(0, 0, 0, 0.6f));
                            LevelButtonClicked[i - 1] = true;
                        }
                        else if (LevelButtonClicked[i - 1] == true)
                        {
                            IsMouseVisible = false;
                            level = levels[i - 1];
                            LevelButtonClicked[i - 1] = false;
                            menusongfadeout = true;
                            levelselectfade = 0;
                            //if (LevelCompleted <= i)
                            //{
                            //    CutscenePlaying = true;
                            //}
                            //else {
                                _state = GameState.Game;
                            //}
                        }
                        else
                        {
                            spriteBatch.Draw(rectangle, new Rectangle((int)LevelSelectPosition[i - 1].X - 38, (int)LevelSelectPosition[i - 1].Y - 38, 76, 76), new Color(0, 0, 0, 0.3f));
                        }
                    }
                    else if(!LevelButton[i - 1].Contains(mousePosition) || _state != GameState.LevelSelect)
                    {
                        hoveringbutton[i - 1] = false;
                        spriteBatch.Draw(rectangle, new Rectangle((int)LevelSelectPosition[i - 1].X - 38, (int)LevelSelectPosition[i - 1].Y - 38, 76, 76), new Color(0, 0, 0, 0.15f));
                        if (bottombarfade >= .05f && _state == GameState.LevelSelect && hoveringbutton[0] == false && hoveringbutton[1] == false && hoveringbutton[2] == false && hoveringbutton[3] == false)
                        {
                            bottombarfade -= .05f;
                        }
                        LevelButtonClicked[i - 1] = false;
                    }
                    spriteBatch.Draw(rectangle, new Rectangle((int)LevelSelectPosition[i - 1].X - 48, (int)LevelSelectPosition[i - 1].Y - 48, 10, 96), LevelColor);
                    spriteBatch.Draw(rectangle, new Rectangle((int)LevelSelectPosition[i - 1].X + 38, (int)LevelSelectPosition[i - 1].Y - 48, 10, 96), LevelColor);
                    spriteBatch.Draw(rectangle, new Rectangle((int)LevelSelectPosition[i - 1].X - 48, (int)LevelSelectPosition[i - 1].Y - 48, 96, 10), LevelColor);
                    spriteBatch.Draw(rectangle, new Rectangle((int)LevelSelectPosition[i - 1].X - 48, (int)LevelSelectPosition[i - 1].Y + 38, 96, 10), LevelColor);
                    spriteBatch.DrawString(LevelSelectFont, i.ToString(), LevelSelectPosition[i - 1] - LevelSelectFont.MeasureString(i.ToString()) / 2, Color.White);

                }
            }
            else if (_state == GameState.Game || ((_state == GameState.Pause || _state == GameState.Settings) && _pausedstate == GameState.Game))
            {
                background.Draw(gameTime, spriteBatch); // draws background
                player.Draw(gameTime, spriteBatch);
                level.Draw(gameTime, spriteBatch); // draws the level
            }
            if (CutscenePlaying == true)
            {
                PlayCutscene(gameTime, spriteBatch);
            }
            if (_state == GameState.Pause || _state == GameState.Settings)
            {
                spriteBatch.Draw(rectangle, new Rectangle(0, 0, width, height), new Color(0, .1f, 0, 0.4f));
                spriteBatch.Draw(rectangle, new Rectangle(width / 2 - 250, height / 2 - 375, 500, 750), new Color(0, .2f, 0, 0.9f));
                spriteBatch.Draw(pause1, new Rectangle(width / 2 - 310, height / 2 - 400, 120, 800), Color.White);
                spriteBatch.Draw(pause2, new Rectangle(width / 2 - 250, height / 2 - 430, 550, 120), Color.White);
                spriteBatch.Draw(pause1, new Rectangle(width / 2 + 190, height / 2 - 400, 120, 800), Color.White);
                spriteBatch.Draw(pause2, new Rectangle(width / 2 - 250, height / 2 + 300, 550, 120), Color.White);
            }
            if (_state == GameState.Settings)
            {
                if (!ResetGame)
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
                            File.WriteAllText(Directory.GetCurrentDirectory() + @"\settings.txt", "volume " + volume + "\nfullscreen " + fullscreen + "\n");
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
                        SoundEffect.MasterVolume = volume;
                    }
                    else if (mouseState.LeftButton != ButtonState.Pressed && DragSlider == true)
                    {
                        DragSlider = false;
                        File.WriteAllText(Directory.GetCurrentDirectory() + @"\settings.txt", "volume " + volume + "\nfullscreen " + fullscreen + "\n");
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
                else
                {
                    spriteBatch.DrawString(PacificoFont, "Are you sure", new Vector2((width - PacificoFont.MeasureString("Are you sure").X) / 2, (height - PacificoFont.MeasureString("Are you sure").Y) / 2 - 270), Color.White);
                    spriteBatch.DrawString(PacificoFont, "you want to", new Vector2((width - PacificoFont.MeasureString("you want to").X) / 2, (height - PacificoFont.MeasureString("you want to").Y) / 2 - 270 + PacificoFont.MeasureString("Are you sure").Y), Color.White);
                    spriteBatch.DrawString(PacificoFont, "reset the game?", new Vector2((width - PacificoFont.MeasureString("reset the game?").X) / 2, (height - PacificoFont.MeasureString("reset the game?").Y) / 2 - 270 + PacificoFont.MeasureString("Are you sure").Y + PacificoFont.MeasureString("you want to").Y), Color.White);
                    Rectangle ConfirmButton = new Rectangle((width - (int)PacificoFont.MeasureString("Yes").X) / 2, (height - (int)PacificoFont.MeasureString("Yes").Y) / 2 + 260, (int)PacificoFont.MeasureString("Yes").X, (int)PacificoFont.MeasureString("Yes").Y);
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
                            File.WriteAllText(Directory.GetCurrentDirectory() + @"\save.txt", "0\n0\n0\n0\n0\n0\n");
                            File.WriteAllText(Directory.GetCurrentDirectory() + @"\eastereggsfound.txt", "\n");
                            _state = GameState.MainMenu;
                            if (backgroundsongmenu.State == SoundState.Stopped)
                            {
                                backgroundsongmenu.Play();
                            }
                            menusongfadeout = false;
                            soundfadeout = false;
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
            else if (_state == GameState.Pause)
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
                        LoadSave();
                        player = new Player(new Vector2(0, 600));
                        _state = GameState.MainMenu;
                        if (backgroundsongmenu.State == SoundState.Stopped)
                        {
                            backgroundsongmenu.Play();
                        }
                        menusongfadeout = false;
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
