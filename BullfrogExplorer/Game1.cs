using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using BullfrogExplorer.Loaders;
using System;
using BullfrogExplorer.Data;
using BullfrogExplorer.Engines;
using static BullfrogExplorer.Engines.Tiles;



/* NOTES
 * 
 *  I'm not a developer and I'm learning as I'm coding.
 * 
 * 
 * 
 * I tried to be as logical as possible
 *  - Data:
 *          It's just some data, nothing is really computed here except creating some list
 *          
 *  - Loaders:
 *          Each game data file has its loader there, each class open one or more data file, read it and put it in some lists (I love lists).
 *          
 *  - Engines directory:
 *          That's for when something is done with something. Playing a animation, building/drawing/updating a menu etc
 * 
 *  All the application logic is in the Games1.cs file, obviously, that's not very good but anyway, it's there.
 *  
 * 
 * 
 */



namespace BullfrogExplorer
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {

        private bool debug = false;

        #region ***** VARIABLES *****

         enum GameState
        {
            intro,
            menu,
            playing
            
        }

        enum ScreenState
        {
            backgrounds,
            sprites,
            animations,
            tiles,
            about,
            credits,
            texts
        }

        struct FooterElement
        {
            public string name;
            public string content;

            public FooterElement(string n, string c)
            {
                this.name = n;
                this.content = c;
            }
        }
        List<FooterElement> footerList = new List<FooterElement>();

        string filename = "";
        TimeSpan oldTimeSpan;

        List<Texts> texts;
        Guests peons = new Guests();

        int tileIndex = 1;
        Texture2D newTile;

        List<Text.Sentence> aboutList = new List<Text.Sentence>();

        Text aboutPage;
        Text text;
        Color backGroundColor = Color.Black;
       
        Map map;

        Menu menu;

        GameState gameState;
        ScreenState screenState;

        Texture2D mousePointer;

        SpriteFont _font;
        
        KeyboardState keyboardState;
        KeyboardState oldKeyboardState;
        MouseState mouseState;
        MouseState oldMouseState;
        Texture2D Tile;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpritesSheet mblk0Sprites = new SpritesSheet();
        SpritesSheet mspr0Sprites = new SpritesSheet();

        List<SpritesSheet> spritesSheets = new List<SpritesSheet>();
        List<Background> backgrounds = new List<Background>();

        List<Animations> animations = new List<Animations>();

        Background back = new Background();

        Animation testsprite;

        Matrix Scale;

        #endregion

        public Game1()
        {
            #region *** CONSTRUCTOR ***

            #region ** CONFIG **
            Config config = new Config();
            config.LoadFile(CONST.CONFIGFILE);
 
            #endregion

            Window.Title =
                CONST.GAME_NAME +
                " Version " + CONST.GAME_RELEASE_VERSION_HIGH +
                "." + CONST.GAME_RELEASE_VERSION_LOW +
                "." + CONST.GAME_RELEASE_VERSION_MINOR +
                " (" + CONST.GAME_RELEASE_NAME + ")";

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferMultiSampling = true;
            graphics.PreferMultiSampling = false;
            graphics.PreferredBackBufferWidth = Settings.SCREEN_WIDTH; 
            graphics.PreferredBackBufferHeight= Settings.SCREEN_HEIGHT; 

            // En vrai dans Settings mais bon
            float scaleX = graphics.PreferredBackBufferWidth / CONST.TARGETWIDTH;
            float scaleY = graphics.PreferredBackBufferHeight / CONST.TARGETHEIGHT;
            Scale = Matrix.CreateScale(new Vector3(scaleX, scaleY, 1));
            graphics.IsFullScreen = Settings.Fullscreen;

            Content.RootDirectory = "Content";
        
            #endregion
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.Window.Position = Settings.Position;


            if (Settings.playIntro)
            {
                gameState = GameState.intro;
                Settings.showMouse = false;
            }
            else
            {
                gameState = GameState.menu;
                Settings.showMouse = true;
            }

            #region **** INITIALIZATION ****
            // TODO: Add your initialization logic here

            // I guess I'll put them here
            screenState = ScreenState.backgrounds;

            Settings.showTiles = false;
            Settings.backpict = 0;
            
            Settings.showToolbar = false;

            Settings.introFadeDelay = .035;
            Settings.introMyAlphaValue = 0;
            Settings.introMyFadeIncrement = 0.01f;

            base.Initialize();
            #endregion

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            _font = Content.Load<SpriteFont>("Main");

            #region ** LOADCONTENT MUSICS ** (NOT YET WORKING)
            /* MUSIC */

            List<Musics> musics = new List<Musics>();

            foreach (DataFiles.MusicsFile musicsFile in DataFiles.musicsFiles)
            {
                Musics currentMusicsFile = new Musics();
                Table currentTab = new Table(2);

                currentMusicsFile.Initialize(musicsFile.index, musicsFile.name);
                

                currentTab.LoadFile(Settings.datapath + musicsFile.tabFile);
                currentMusicsFile.LoadFile(Settings.datapath + musicsFile.datFile, currentTab);
                musics.Add(currentMusicsFile);

            }
            #endregion

            #region ** LOADCONTENT SOUNDS ** (NOT YET WORKING)
                /* SOUNDS */
           /*     Table snd = new Table(1);
            string path = @"C: \Users\mik\source\repos\ThemePark Data\";
            path = @"C:\Program Files (x86)\GOG Galaxy\Games\Theme Park\GAME\DATA\";
            string file = @"SNDS0-0.TAB";
            snd.LoadFile(Settings.datapath + file);
            Sounds sounds = new Sounds();
            file = @"SNDS0-0.DAT";
            sounds.LoadFile(Settings.datapath + file, snd);*/
            #endregion

            #region ** LOADCONTENT ANIMATIONS **
            /* ANIMATIONS */
            foreach (DataFiles.AnimationsFiles animationsFile in DataFiles.animationsFiles)
            {
                Animations currentAnimations = new Animations();
                currentAnimations.LoadFiles(Settings.datapath + animationsFile.StartFile, Settings.datapath + animationsFile.FramesFile,
                Settings.datapath + animationsFile.ElementsFile, Settings.datapath + animationsFile.LabelsFile);
                animations.Add(currentAnimations);
            }


            testsprite = new Animation(GraphicsDevice, animations.Find(x => x.name.Contains(Settings.AnimationSerie)), Settings.currentanim);

            #endregion

            #region ** LOADCONTENT TEXTS **
            texts = new List<Texts>();

            foreach (DataFiles.TextsFile textsFile in DataFiles.textsFiles)
            {
                Texts currentTexts = new Texts(textsFile.index, textsFile.name);
                currentTexts.LoadFile(Settings.datapath + textsFile.file);
                texts.Add(currentTexts);
            }
            #endregion

            #region ** LOADCONTENT PALETTES ** 
            /* PALETTES */

            List<Palette> palettes = new List<Palette>();

            foreach (DataFiles.PaletteFile paletteFile in DataFiles.paletteFiles)
            {
                Palette currentPalette = new Palette();
                currentPalette.Initialize(paletteFile.index, paletteFile.name);
                currentPalette.LoadFile(Settings.datapath + paletteFile.file);

                palettes.Add(currentPalette);

            }

            Palette Palette = new Palette();
            Palette.Initialize(99, "WHITE");
            Palette.Generate();
            palettes.Add(Palette);
            #endregion

            #region ** LOADCONTENT BACKGROUND **
            /* BACKGROUNDS */


            foreach (DataFiles.BackgroundFile backgroundFile in DataFiles.backgroundFiles)
            {
                Background currentBackground = new Background();
                currentBackground.SetGraphicsDevice(GraphicsDevice);
                currentBackground.LoadFile(Settings.datapath + backgroundFile.file, palettes.Find(x => x.name.Contains(backgroundFile.palette)));
                
                backgrounds.Add(currentBackground);

            }

            #endregion
            
            #region ** LOADCONTENT SPRITES SHEETS **
            /* SPRITES SHEET */

            foreach (DataFiles.SpritesFile spritesFile in DataFiles.spritesFiles)
            {
                SpritesSheet currentSpritesSheet = new SpritesSheet();
                Table currentTab = new Table(0);

                currentSpritesSheet.Initialize(spritesFile.index, spritesFile.name);
                currentSpritesSheet.SetGraphicsDevice(GraphicsDevice);

                currentTab.LoadFile(Settings.datapath + spritesFile.tabFile);
                currentSpritesSheet.LoadFile(Settings.datapath + spritesFile.datFile, currentTab, 
                    palettes.Find(x => x.name.Contains(spritesFile.paletteName)));
                spritesSheets.Add(currentSpritesSheet);

            }

            text = new Text(spritesSheets.Find(x => x.name.Contains("MFONT2")));
            #endregion

            #region ** LOADCONTENT VIDEO ** (NOT YET WORKING ANYWAY)
            foreach (DataFiles.VideoFile videoFile in DataFiles.videoFiles)
            {
                Video currentVideo = new Video();
                currentVideo.LoadFile(Settings.datapath + videoFile.file);

            }
            #endregion
            
            mousePointer = spritesSheets.Find(x => x.name.Contains(Settings.mousePointerSpritesheet)).spriteElements[Settings.mousePointerIndex].sprite;

            #region ** MAP **
            //name, height, width, tile height, tile width
            //Attention, c'est la largeur "utilisable" de la tile, pas la largeur du sprite
            //Dans le cas de TP, ils utilisent un sytème relou de recouvrement...
            //Name, width, height,TileWidth, TileHeight, Tile offset 
            map = new Map("park", 50, 50, 24, 16, 8);
            map.Generate(6);
            map.SetViewport(0, 0);
            #endregion

            #region ** MAINMENU **
            /* MENU */

            List<Menu.MenuElement> menuList = new List<Menu.MenuElement>();
            string[] lines = System.IO.File.ReadAllLines(CONST.MAINMENUFILE);
                        
            int c = 0;
            foreach (string line in lines)
            {
                string[] s = line.Split(',');
                menuList.Add(new Menu.MenuElement(c++, Menu.MenuState.normal, s[0], s[1], (Keys)Enum.Parse(typeof(Keys), s[2], true)));
            }

            //menuList.Find(x => x.Label.Contains("HELP")).State = Menu.MenuState.grey;
            //menuList.Find(x => x.Label.Contains("CREDITS")).State = Menu.MenuState.grey;
            //menuList.Find(x => x.Label.Contains("ABOUT")).State = Menu.MenuState.grey;

            menu = new Menu(spritesSheets.Find(x => x.name.Contains("MFONT2")), menuList, new Vector2(90,85));
            #endregion

            #region ** ABOUT **

            lines = System.IO.File.ReadAllLines(CONST.ABOUTFILE);

            
            foreach (string line in lines)
            {
                aboutList.Add(new Text.Sentence(line, Color.White, "center"));

            }

            aboutPage = new Text(spritesSheets.Find(x => x.name.Contains("MFONT2")));
            aboutPage.SetSpriteBatch(spriteBatch);

            #endregion

            #region ** FOOTER **

            string[] f_lines = System.IO.File.ReadAllLines(CONST.FOOTERFILE);
            foreach (string f_line in f_lines)
            {
                string[] f_s = f_line.Split('=');
                footerList.Add(new FooterElement(f_s[0], f_s[1]));
            }
            #endregion

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }



        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState(); 
            mouseState = Mouse.GetState();

            #region ** GAMEPAD **
            // TODO oldstate
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                if (gameState == GameState.menu) Exit(); else gameState = GameState.menu;
            }

            #endregion
          
            #region ** KEYBOARD KEY STATES **

            
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !oldKeyboardState.IsKeyDown(Keys.Escape))
            {
                if (gameState == GameState.menu) Exit(); else gameState = GameState.menu;
            }

            if ((keyboardState.IsKeyDown(Keys.LeftAlt) && keyboardState.IsKeyDown(Keys.Enter)) && !oldKeyboardState.IsKeyDown(Keys.Enter))
            {
                graphics.ToggleFullScreen();
            }


            if (keyboardState.IsKeyDown(Keys.F10) && !oldKeyboardState.IsKeyDown(Keys.F10))
            {
                tileIndex--;
                if (tileIndex < 1) tileIndex = 41;
            }

            if (keyboardState.IsKeyDown(Keys.F11) && !oldKeyboardState.IsKeyDown(Keys.F11))
            {
                tileIndex++;
                if (tileIndex > 41) tileIndex = 1;
            }

            if (keyboardState.IsKeyDown(Keys.F12) && !oldKeyboardState.IsKeyDown(Keys.F12))
            {
                Guests.currentGuestIndex++;
                if (Guests.currentGuestIndex > 7) Guests.currentGuestIndex = 1;
            }

            #endregion
            
            #region ** MOUSE-KEYBOARD STATES **

            if (gameState == GameState.menu)
            {

                #region * MENU UPDATE *
                switch (menu.Update(gameTime, mouseState, keyboardState))
                {
                    case "CREDITS":
                        gameState = GameState.playing;
                        screenState = ScreenState.credits;
                        Settings.showFooter = false;
                        oldTimeSpan = gameTime.TotalGameTime;
                        break;
                    case "ABOUT":
                        gameState = GameState.playing;
                        screenState = ScreenState.about;
                        Settings.showFooter = false;
                        break;
                    case "BACKGROUNDS":
                        gameState = GameState.playing;
                        screenState = ScreenState.backgrounds;
                        Settings.showFooter = true;
                        break;
                    case "SPRITES":
                        gameState = GameState.playing;
                        screenState = ScreenState.sprites;
                        Settings.showFooter = true;
                        break;
                    case "ANIMATIONS":
                        gameState = GameState.playing;
                        screenState = ScreenState.animations;
                        Settings.showFooter = true;
                        Settings.playAnim = true;
                        break;
                    case "TILES":
                        gameState = GameState.playing;
                        screenState = ScreenState.tiles;
                        Settings.showFooter = true;
                        break;
                    case "TEXTS":
                        gameState = GameState.playing;
                        screenState = ScreenState.texts;
                        Settings.showFooter = true;
                        break;

                    case "EXIT":
                        Exit();
                        break;
                }
                #endregion
            }
            else
            {
                switch (gameState)
                {
                    case GameState.playing:

                        #region Show Footer
                        if (keyboardState.IsKeyDown(Keys.F) && !oldKeyboardState.IsKeyDown(Keys.F))
                        {
                            Settings.showFooter = !Settings.showFooter;
                        }
                        #endregion

                        switch (screenState)
                        {

                            #region ScreenState.about
                            case ScreenState.about:

                                if ((mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton != ButtonState.Pressed) ||
                                    (mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton != ButtonState.Pressed) ||
                                    ((keyboardState != oldKeyboardState) && keyboardState.GetPressedKeys().Length > 0)
                                    )
                                {
                                    gameState = GameState.menu;
                                    Settings.showMouse = true;
                                }
                                break;
                            #endregion

                            #region ScreenState.credits
                            case ScreenState.credits:
                                if ((mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton != ButtonState.Pressed) ||
                                    (mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton != ButtonState.Pressed) ||
                                    ((keyboardState != oldKeyboardState) && keyboardState.GetPressedKeys().Length > 0)
                                    )
                                {
                                    if (screenState == ScreenState.credits)
                                    {
                                        Indexes.credits.currentIndex = Indexes.credits.currentIndex + Indexes.credits.offset + 1;
                                    }
                                }

                                /* Elapsed time */
                                if (gameTime.TotalGameTime - oldTimeSpan > Indexes.credits.delay)
                                {
                                    Indexes.credits.currentIndex = Indexes.credits.currentIndex + Indexes.credits.offset + 1;
                                    oldTimeSpan = gameTime.TotalGameTime;
                                }

                                break;
                            #endregion

                            #region ScreenState.background
                            case ScreenState.backgrounds:

                                if ((mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton != ButtonState.Pressed) ||
                                    keyboardState.IsKeyDown(Keys.X) && !oldKeyboardState.IsKeyDown(Keys.X))
                                {
                                    Settings.backpict++;
                                    if (Settings.backpict > backgrounds.Count-1) { Settings.backpict = 0; };
                                }
                                if ((mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton != ButtonState.Pressed) ||
                                        keyboardState.IsKeyDown(Keys.W) && !oldKeyboardState.IsKeyDown(Keys.W))
                                {
                                    Settings.backpict--;
                                    if (Settings.backpict < 0) { Settings.backpict = backgrounds.Count-1; };
                                }


                             break;
                            #endregion

                            #region ScreenState.sprites
                            case ScreenState.sprites:

                                if ((mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton != ButtonState.Pressed) ||
                                    keyboardState.IsKeyDown(Keys.X) && !oldKeyboardState.IsKeyDown(Keys.X))
                                {
                                    Settings.spritesheet++;
                                    if (spritesSheets[Settings.spritesheet].name == "MFONT2") Settings.spritesheet++;

                                    if (Settings.spritesheet > 12) { Settings.spritesheet = 0; };
                                    Settings.firstsprite = 0;

                                }
                                
                                if ((mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton != ButtonState.Pressed) ||
                                       keyboardState.IsKeyDown(Keys.W) && !oldKeyboardState.IsKeyDown(Keys.W))
                                {
                                    Settings.spritesheet--;

                                    if (Settings.spritesheet < 0) { 
                                        Settings.spritesheet = 12;
                                    };
                                    if (spritesSheets[Settings.spritesheet].name == "MFONT2") Settings.spritesheet--;
                                    Settings.firstsprite = 0;

                                }

                                if (keyboardState.IsKeyDown(Keys.V) && !oldKeyboardState.IsKeyDown(Keys.V))
                                {
                                    Settings.firstsprite++;
                                    if (Settings.firstsprite > spritesSheets[Settings.spritesheet].spriteElements.Count) { Settings.firstsprite = 0; };


                                    while (Settings.firstsprite < spritesSheets[Settings.spritesheet].spriteElements.Count &&
                                        spritesSheets[Settings.spritesheet].spriteElements[Settings.firstsprite].sprite.Width < 2  && 
                                        spritesSheets[Settings.spritesheet].spriteElements[Settings.firstsprite].sprite.Height < 2)
                                        Settings.firstsprite++;

                                }

                                if (keyboardState.IsKeyDown(Keys.C) && !oldKeyboardState.IsKeyDown(Keys.C))
                                {
                                    Settings.firstsprite--;
                                    if (Settings.firstsprite < 0) { Settings.firstsprite = spritesSheets[Settings.spritesheet].spriteElements.Count-1; };
                                    
                                    while (Settings.firstsprite > 0 &&
                                        spritesSheets[Settings.spritesheet].spriteElements[Settings.firstsprite].sprite.Width < 2 &&
                                        spritesSheets[Settings.spritesheet].spriteElements[Settings.firstsprite].sprite.Height < 2)
                                        Settings.firstsprite--;

                                }

                                if (keyboardState.IsKeyDown(Keys.N) && !oldKeyboardState.IsKeyDown(Keys.N))
                                {
                                    Settings.firstsprite = Settings.firstsprite + 25;
                                    if (Settings.firstsprite > 3000) { Settings.firstsprite = 0; };
                                }

                                if (keyboardState.IsKeyDown(Keys.B) && !oldKeyboardState.IsKeyDown(Keys.B))
                                {
                                    Settings.firstsprite = Settings.firstsprite - 25;
                                    if (Settings.firstsprite < 0) { Settings.firstsprite = 3000; };
                                }

                                if (keyboardState.IsKeyDown(Keys.I) && !oldKeyboardState.IsKeyDown(Keys.I))
                                {
                                    Settings.spriteIndex = !Settings.spriteIndex;
                                   
                                }

                                ////   VIEWPORT
                                if (keyboardState.IsKeyDown(Keys.PageUp))
                                {
                                    spritesSheets[Settings.spritesheet].viewport.Y -= 5;
                                }
                                if (keyboardState.IsKeyDown(Keys.PageDown))
                                {
                                    spritesSheets[Settings.spritesheet].viewport.Y += 5;
                                }
                                if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Settings.movement.Up))
                                {
                                    spritesSheets[Settings.spritesheet].viewport.Y--;
                                }
                                if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Settings.movement.Down))
                                {
                                    spritesSheets[Settings.spritesheet].viewport.Y++;
                                }
                                if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Settings.movement.Left))
                                {
                                   spritesSheets[Settings.spritesheet].viewport.X--;
                                }
                                if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Settings.movement.Right))
                                {
                                    spritesSheets[Settings.spritesheet].viewport.X++;
                                }
                                if (keyboardState.IsKeyDown(Keys.Home)) spritesSheets[Settings.spritesheet].viewport = new Point(0, 0);



                                break;

                            #endregion

                            #region ScreenState.texts
                            case ScreenState.texts:

                                if (keyboardState.IsKeyDown(Keys.Left))
                                {
                                    texts.Find(bf => bf.name.Contains(Settings.Lang)).position.X += 20;
                                }
                                if (keyboardState.IsKeyDown(Keys.Right))
                                {
                                    texts.Find(bf => bf.name.Contains(Settings.Lang)).position.X -= 20;
                                }
                                if (keyboardState.IsKeyDown(Keys.Home)) texts.Find(bf => bf.name.Contains(Settings.Lang)).position = new Vector2(0, 0);

                                if ((mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton != ButtonState.Pressed) ||
                                    keyboardState.IsKeyDown(Keys.W) && !oldKeyboardState.IsKeyDown(Keys.W))
                                {
                                    if (texts[texts.Find(bf => bf.name.Contains(Settings.Lang)).index].index > 0)
                                    Settings.Lang=texts[texts.Find(bf => bf.name.Contains(Settings.Lang)).index-1].name;
                                    else Settings.Lang = texts[texts.Count-1].name;
                                }
                                if ((mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton != ButtonState.Pressed) ||
                                        keyboardState.IsKeyDown(Keys.X) && !oldKeyboardState.IsKeyDown(Keys.X))
                                {
                                    if (texts[texts.Find(bf => bf.name.Contains(Settings.Lang)).index].index + 1 < texts.Count)
                                        Settings.Lang = texts[texts.Find(bf => bf.name.Contains(Settings.Lang)).index + 1].name;
                                    else Settings.Lang = texts[0].name;
                                }

                                break;
                            #endregion

                            #region ScreenState.animations
                            case ScreenState.animations:

                                if ((mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton != ButtonState.Pressed) ||
                                    keyboardState.IsKeyDown(Keys.X) && !oldKeyboardState.IsKeyDown(Keys.X))
                                {
                                    //Not sure what's the best way to do that.
                                    bool done=false;
                                    while (testsprite.spritesFrames.Count == 0 || !done)
                                    {

                                        

                                        Settings.currentanim++;
                                        if (Settings.currentanim > animations.Find(x => x.name.Contains(Settings.AnimationSerie)).spritesAnimations.Count) { Settings.currentanim = 1; };
                                        testsprite.NewAnimation(animations.Find(x => x.name.Contains(Settings.AnimationSerie)), Settings.currentanim);
                                        testsprite.PrintFrames();
                                        done = true;
                                    }
                                }
                                if ((mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton != ButtonState.Pressed) ||
                                     keyboardState.IsKeyDown(Keys.W) && !oldKeyboardState.IsKeyDown(Keys.W))
                                {
                                    bool done = false;
                                    while (testsprite.spritesFrames.Count == 0 || !done)
                                    {
                                        Settings.currentanim--;
                                        if (Settings.currentanim < 1) { Settings.currentanim = animations.Find(x => x.name.Contains(Settings.AnimationSerie)).spritesAnimations.Count; };
                                        testsprite.NewAnimation(animations.Find(x => x.name.Contains(Settings.AnimationSerie)), Settings.currentanim);
                                        
                                        done = true;
                                    }
                                }
                                if (keyboardState.IsKeyDown(Keys.Add) && !oldKeyboardState.IsKeyDown(Keys.Add))
                                {
                                    testsprite.Faster();
                                }
                                if (keyboardState.IsKeyDown(Keys.Subtract) && !oldKeyboardState.IsKeyDown(Keys.Subtract))
                                {
                                    testsprite.Slower();
                                }

                                if (keyboardState.IsKeyDown(Keys.P) && !oldKeyboardState.IsKeyDown(Keys.P))
                                {
                                    testsprite.TPlay();
                                }
                                if (keyboardState.IsKeyDown(Keys.N) && !oldKeyboardState.IsKeyDown(Keys.N))
                                {
                                    testsprite.Next();
                                }

                                if (keyboardState.IsKeyDown(Keys.B) && !oldKeyboardState.IsKeyDown(Keys.B))
                                {
                                    testsprite.Previous();
                                }

                                if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Settings.movement.Up))
                                {
                                    testsprite.Location.Y--;
                                }
                                if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Settings.movement.Down))
                                {
                                    testsprite.Location.Y++;
                                }
                                if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Settings.movement.Left))
                                {
                                    testsprite.Location.X--;
                                }
                                if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Settings.movement.Right))
                                {
                                    testsprite.Location.X++;
                                }
                                if (keyboardState.IsKeyDown(Keys.Insert) && !oldKeyboardState.IsKeyDown(Keys.Insert))
                                {
                                    testsprite.AddGuest(1);
                                }
                                if (keyboardState.IsKeyDown(Keys.Delete) && !oldKeyboardState.IsKeyDown(Keys.Delete))
                                {
                                    testsprite.RemGuest(0);
                                }
                                if (keyboardState.IsKeyDown(Keys.I) && !oldKeyboardState.IsKeyDown(Keys.I))
                                {
                                    testsprite.ToggleIndex();
                                }
                                if (keyboardState.IsKeyDown(Keys.F) && !oldKeyboardState.IsKeyDown(Keys.F))
                                {
                                    testsprite.ToggleElem();
                                }

                                break;
                            #endregion

                            #region ScreenState.Tiles
                            case ScreenState.tiles:
                                if (keyboardState.IsKeyDown(Keys.Up))
                                {
                                    /**************** TILES ************/
                                    if (map.viewport.Y + map.viewport.Height / Settings.PIXEL_RATIO < (map.Height - 1) * map.TileHeight) map.viewport.Y++;
                                }
                                if (keyboardState.IsKeyDown(Keys.Down))
                                {
                                    /**************** TILES ************/
                                    if (map.viewport.Y > 0) map.viewport.Y--;
                                }
                                if (keyboardState.IsKeyDown(Keys.Left))
                                {
                                    /**************** TILES ************/
                                    if (map.viewport.X + map.viewport.Width / Settings.PIXEL_RATIO < (map.Width - 1) * map.TileWidth) map.viewport.X++;
                                    //Console.WriteLine("map.viewport.X=" + map.viewport.X + " map.viewport.Width=" + map.viewport.Width + " map.=" + (map.Width * map.TileWidth));
                                }
                                if (keyboardState.IsKeyDown(Keys.Right))
                                {
                                    /**************** TILES ************/
                                    if (map.viewport.X > 0) map.viewport.X--;
                                }
                                break;
                            #endregion
                                


                        }
                        break;

                }
 

                /**************** TILES ************/
                //if (gameState == GameState.playing && screenState == ScreenState.tiles) map.UpdateTile(mouseState.X / Settings.PIXEL_RATIO, mouseState.Y / Settings.PIXEL_RATIO, tileIndex);


            }

            if (keyboardState.IsKeyDown(Keys.H) && !oldKeyboardState.IsKeyDown(Keys.H))
            {
                if (Settings.mousePointerIndex != 1)
                {
                    Settings.mousePointerIndex = 1;
                }
                else
                {
                    Settings.mousePointerIndex = 5;
                }
            }
            

            oldMouseState = Mouse.GetState();

            oldKeyboardState = Keyboard.GetState();
            #endregion
            
            #region * INTRO UPDATES * 
            if (gameState == GameState.intro)
            {

                //Decrement the delay by the number of seconds that have elapsed since
                //the last time that the Update method was called
                Settings.introFadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;

                //If the Fade delays has dropped below zero, then it is time to 
                //fade in/fade out the image a little bit more.
                if (Settings.introFadeDelay <= 0)
                {
                    //Reset the Fade delay
                    Settings.introFadeDelay = .035;

                    Settings.introMyAlphaValue += Settings.introMyFadeIncrement;
                    if (Settings.introMyAlphaValue >= 1 || Settings.introMyAlphaValue <= 0)
                    {
                        Settings.introMyFadeIncrement *= -1;
                        gameState = GameState.menu;
                        Settings.showMouse = true;
                    }
                }
            }
            #endregion

            #region * ANIMATIONS UPDATE *
            ///// ANIMATIONS

            if (gameState == GameState.playing && screenState == ScreenState.animations) testsprite.Update(gameTime);

            #endregion

            #region * TILES UPDATE *
            /**************** TILES ************/
            if (gameState == GameState.playing && screenState == ScreenState.tiles)
            {
                map.Update(gameTime);
                newTile = spritesSheets.Find(x => x.name == "MBLK").spriteElements[tileIndex].sprite;
            }
            #endregion

            mousePointer = spritesSheets.Find(x => x.name.Contains(Settings.mousePointerSpritesheet)).spriteElements[Settings.mousePointerIndex].sprite;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backGroundColor);
            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

            spriteBatch.Begin(SpriteSortMode.Deferred,
                  BlendState.AlphaBlend,
                  SamplerState.PointClamp,
                  null, null, null, Scale);

            GraphicsDevice.BlendState = BlendState.NonPremultiplied;


            #region ** gameState INTRO **
            if (gameState == GameState.intro)
            {

                spriteBatch.Draw(backgrounds.Find(x => x.name.Contains("MMENU-0")).picture, new Vector2(0, 0), Color.White * Settings.introMyAlphaValue);

            }
            #endregion

            #region ** gameState MENU **
            else if (gameState == GameState.menu)
            {
                // --------------- MENU

                spriteBatch.Draw(backgrounds.Find(x => x.name.Contains("MMENU-1")).picture, new Vector2(0, 0), Color.White);
                menu.Draw(spriteBatch);
            }
            #endregion
            else

            #region ** gameState PLAYING ** (sub screenState)
            if (gameState == GameState.playing)
            {

                switch (screenState)
                {
                    case ScreenState.credits:
                        #region - DRAW CREDITS -
                        spriteBatch.Draw(backgrounds.Find(bf => bf.name.Contains(Indexes.credits.background)).picture, new Vector2(0, 0), Color.White);
                        
                        text.SetPosition(Indexes.credits.position);

                        int c_c = 0;
                        if (texts.Find(tf => tf.name == Settings.Lang).sentences.Find(sf => sf.index == Indexes.credits.currentIndex + c_c).text == "*")
                        {
                            Indexes.credits.currentIndex = Indexes.credits.index;
                            gameState = GameState.menu;
                        }
                        else
                        {
                            while (texts.Find(tf => tf.name == Settings.Lang).sentences.Find(sf => sf.index == Indexes.credits.currentIndex + c_c).text != "+" && c_c < 99)
                            {

                                string t = texts.Find(tf => tf.name == Settings.Lang).sentences.Find(sf => sf.index == Indexes.credits.currentIndex + c_c).text;
                                text.SetPosition((CONST.TARGETWIDTH - text.SizeOf(t).X) / 2, text.position.Y);
                                text.WriteLine(spriteBatch, t, Color.White);
                                c_c++;
                            }
                            Indexes.credits.offset = c_c;
                        }
                        break;
                        #endregion

                    case ScreenState.about:
                        #region - DRAW ABOUT -
                        spriteBatch.Draw(backgrounds.Find(bf => bf.name.Contains("MMENU-1")).picture, new Vector2(0, 0), Color.White);
                        if (screenState == ScreenState.about)
                        {
                            spriteBatch.End();
                            spriteBatch.Begin(SpriteSortMode.Deferred,
                              BlendState.AlphaBlend,
                                SamplerState.PointClamp,
                                null, null, null, null);

                            //spriteBatch.DrawString(_font, "eéééé", new Vector2(0,0), Color.Black);

                            Vector2 aboutPosition = new Vector2(Settings.aboutText.X * Settings.PIXEL_RATIO, Settings.aboutText.Y * Settings.PIXEL_RATIO);
                            foreach (Text.Sentence sentence in aboutList)
                            {
                                if (sentence.Text.Length > 0)
                                {
                                    string t = sentence.Text.Replace("[GAME_NAME]", CONST.GAME_NAME);
                                    t = t.Replace("[GAME_RELEASE_VERSION_HIGH]", CONST.GAME_RELEASE_VERSION_HIGH.ToString());
                                    t = t.Replace("[GAME_RELEASE_VERSION_LOW]", CONST.GAME_RELEASE_VERSION_LOW.ToString());
                                    t = t.Replace("[GAME_RELEASE_VERSION_MINOR]", CONST.GAME_RELEASE_VERSION_MINOR.ToString());
                                    t = t.Replace("[GAME_RELEASE_NAME]", CONST.GAME_RELEASE_NAME);
                                    t = t.Replace("[GAME_COPYRIGHT]", CONST.GAME_COPYRIGHT);

                                    
                                    Vector2 v2 = _font.MeasureString(t);
                                    aboutPosition.X = CONST.TARGETWIDTH * 4 / 2 - v2.X / 2;
                                    spriteBatch.DrawString(_font, t, new Vector2(aboutPosition.X + 2, aboutPosition.Y + 2), Color.Black);
                                    spriteBatch.DrawString(_font, t, aboutPosition, Color.White);
                                    aboutPosition.Y = aboutPosition.Y + v2.Y;
                                }
                                else
                                {
                                    aboutPosition.Y = aboutPosition.Y + _font.LineSpacing;
                                }
                            }

                            spriteBatch.End();
                            spriteBatch.Begin(SpriteSortMode.Deferred,
                                  BlendState.AlphaBlend,
                                    SamplerState.PointClamp,
                                    null, null, null, Scale);
                        }
                        break;
                    #endregion

                    case ScreenState.texts:
                        #region - DRAW TEXTS -
                        if (screenState == ScreenState.texts)
                        {
                            spriteBatch.End();
                            spriteBatch.Begin(SpriteSortMode.Deferred,
                              BlendState.AlphaBlend,
                                SamplerState.PointClamp,
                                null, null, null, null);

                            filename = Settings.Lang;

                            bool Binary = false;

                            Vector2 txtPosition = texts.Find(bf => bf.name.Contains(Settings.Lang)).position;
                            float maxX = 0;
                            foreach (Texts.Sentence sentence in texts.Find(bf => bf.name.Contains(Settings.Lang)).sentences)
                            {
                                if (sentence.text.Length > 0)
                                {
                                    Vector2 v2 = _font.MeasureString(sentence.text);
                                    if (txtPosition.Y + v2.Y +40> CONST.TARGETHEIGHT*4)
                                    {
                                        if (txtPosition.X + v2.X < CONST.TARGETWIDTH*4)
                                        {
                                            txtPosition.X = txtPosition.X + maxX + 40;
                                            txtPosition.Y = texts.Find(bf => bf.name.Contains(Settings.Lang)).position.Y;
                                            maxX = 0;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    spriteBatch.DrawString(_font, sentence.index.ToString() + " - " + sentence.text, new Vector2(txtPosition.X + 2, txtPosition.Y + 2), Color.Black);
                                    if (Binary)
                                    {
                                        spriteBatch.DrawString(_font, sentence.index.ToString() + " - " + sentence.text, txtPosition, Color.DarkGray);
                                        Binary = !Binary;
                                    }
                                    else
                                    {
                                        spriteBatch.DrawString(_font, sentence.index.ToString() + " - " + sentence.text, txtPosition, Color.LightGray);
                                        Binary = !Binary;
                                    }


                                    txtPosition.Y = txtPosition.Y + v2.Y;
                                    if (v2.X > maxX) maxX = v2.X;
                                }
                                else
                                {
                                    txtPosition.Y = txtPosition.Y + _font.LineSpacing;
                                }
                            }

                            spriteBatch.End();
                            spriteBatch.Begin(SpriteSortMode.Deferred,
                                  BlendState.AlphaBlend,
                                    SamplerState.PointClamp,
                                    null, null, null, Scale);
                        }
                        break;
                    #endregion

                    case ScreenState.tiles:
                        #region ** DRAW screenState TILES **
                        /******************** TILES *************************/
                        map.Draw(spriteBatch, spritesSheets.Find(F => F.name == "MBLK"), _font);
                        break;
                    #endregion

                    case ScreenState.backgrounds:
                        #region ** DRAW screenState BACKGROUNDS **
                        // --------------- BACKGROUND
                        spriteBatch.Draw(backgrounds[Settings.backpict].picture, new Vector2(0, 0), Color.White);
                        filename = backgrounds[Settings.backpict].name;
                        break;
                    #endregion

                    case ScreenState.sprites:
                        #region ** screenState SPRITES **                
                        float x = spritesSheets[Settings.spritesheet].viewport.X;
                        float y = spritesSheets[Settings.spritesheet].viewport.Y;
                        int height = 0;
                        int spacer = 5;

                        for (int i = 0; i < spritesSheets[Settings.spritesheet].spriteElements.Count; i++)
                        {
                            if (spritesSheets[Settings.spritesheet].spriteElements.Count > i + Settings.firstsprite)
                            {
                                if (spritesSheets[Settings.spritesheet].spriteElements[i + Settings.firstsprite].sprite != null &&
                                    spritesSheets[Settings.spritesheet].spriteElements[i + Settings.firstsprite].sprite.Width > 1 &&
                                    spritesSheets[Settings.spritesheet].spriteElements[i + Settings.firstsprite].sprite.Height > 1
                                    )
                                {
                                    if (x + spritesSheets[Settings.spritesheet].spriteElements[i + Settings.firstsprite].sprite.Width - spritesSheets[Settings.spritesheet].viewport.X > Settings.SCREEN_WIDTH / 4)
                                    {
                                        x = spritesSheets[Settings.spritesheet].viewport.X;
                                        y = y + height + spacer;
                                        height = 0;
                                    }

                                    if (Settings.spriteIndex)
                                    {
                                        spriteBatch.DrawString(_font, spritesSheets[Settings.spritesheet].spriteElements[i + Settings.firstsprite].index.ToString(),
                                        new Vector2(x + 1, y + 1), Color.Black, 0f, new Vector2(0, 0), 0.25f, 0, 0f);

                                        spriteBatch.DrawString(_font, spritesSheets[Settings.spritesheet].spriteElements[i + Settings.firstsprite].index.ToString(),
                                        new Vector2(x, y), Color.White, 0f, new Vector2(0, 0), 0.25f, 0, 0f);
                                        x = x + 5;
                                    }
                                    spriteBatch.Draw(spritesSheets[Settings.spritesheet].spriteElements[i + Settings.firstsprite].sprite,
                                    new Vector2(x, y), Color.White);

                                    x = x + spritesSheets[Settings.spritesheet].spriteElements[i + Settings.firstsprite].sprite.Width + 1;
                                    if (spritesSheets[Settings.spritesheet].spriteElements[i + Settings.firstsprite].sprite.Height > height)
                                        height = spritesSheets[Settings.spritesheet].spriteElements[i + Settings.firstsprite].sprite.Height;
                                }
                            }

                            if (y > Settings.SCREEN_HEIGHT / 4)
                            {
                                break;
                            }
                        }
                        filename = spritesSheets[Settings.spritesheet].filename;
                        break;
                    #endregion

                    case ScreenState.animations:
                        #region ** DRAW screenState ANIMATIONS **
                        
                        testsprite.Draw(spriteBatch, spritesSheets.Find(F => F.name.Contains("MSPR")), _font);
                        filename = animations.Find(xa => xa.name.Contains(Settings.AnimationSerie)).name + " - " + Settings.currentanim + " - " + animations.Find(xb => xb.name.Contains(Settings.AnimationSerie)).AnimationsLabels[Settings.currentanim - 1].label;
                        break;
                        #endregion
                }

                #region *TOOLBAR * (Don't ask don't remember)
                if (Settings.showToolbar)
                {
                    int toolbarX = 0;
                    for (int i = 1; i < 10; i = i + 2)
                    {
                        spriteBatch.Draw(spritesSheets.Find(x => x.name == "MPANEL").spriteElements[i].sprite,
                            new Vector2(
                                toolbarX,
                                Settings.SCREEN_HEIGHT / Settings.PIXEL_RATIO - spritesSheets.Find(x => x.name == "MPANEL").spriteElements[i].sprite.Height
                            ),
                            Color.White);
                        toolbarX = toolbarX + spritesSheets.Find(x => x.name == "MPANEL").spriteElements[i].sprite.Width;
                    }

                    int toolbarTilesX = toolbarX;
                    for (int i = 0; i < 6; i++)
                    {
                        spriteBatch.Draw(spritesSheets.Find(x => x.name == "MBLK").spriteElements[i + tileIndex].sprite,
                            new Vector2(4 + toolbarTilesX, Settings.SCREEN_HEIGHT / Settings.PIXEL_RATIO - 20), Color.White);
                        toolbarTilesX = toolbarTilesX +
                            spritesSheets.Find(x => x.name == "MBLK").spriteElements[i + tileIndex].sprite.Width;
                    }
                }
                #endregion

                /************* END ZONE FACTOR ZOOM x4 ******************/
                spriteBatch.End();



                spriteBatch.Begin(SpriteSortMode.Deferred,
                      BlendState.AlphaBlend,
                      SamplerState.PointClamp,
                      null, null, null, null);


                if (Settings.showFooter)
                {
                    switch (screenState)
                    {
                        case ScreenState.backgrounds:

                            #region - FOOTER BACKGROUNDS -
                            spriteBatch.DrawString(_font, filename,
                                                new Vector2(6, 200 * 4 - 42), Color.Black);

                            spriteBatch.DrawString(_font, filename,
                                new Vector2(4, 200 * 4 - 44), Color.White);

                            spriteBatch.DrawString(_font, footerList.Find(x => x.name.Contains("BACKGROUND")).content,
                                new Vector2(6, 200 * 4 - 20), Color.Black);
                            spriteBatch.DrawString(_font, footerList.Find(x => x.name.Contains("BACKGROUND")).content,
                                new Vector2(4, 200 * 4 - 22), Color.White);
                            break;
                            #endregion

                        case ScreenState.animations:

                            #region - FOOTER ANIMATIONS - 
                            spriteBatch.DrawString(_font, filename,
                                                new Vector2(6, 200 * 4 - 42), Color.Black);

                            spriteBatch.DrawString(_font, filename,
                                new Vector2(4, 200 * 4 - 44), Color.White);
                            spriteBatch.DrawString(_font, footerList.Find(x => x.name.Contains("ANIMATIONS")).content,
                                new Vector2(6, 200 * 4 - 20), Color.Black);
                            spriteBatch.DrawString(_font, footerList.Find(x => x.name.Contains("ANIMATIONS")).content,
                            new Vector2(4, 200 * 4 - 22), Color.White);
                            break;
                            #endregion

                        case ScreenState.sprites:

                            #region - FOOTER SPRITES - 
                            spriteBatch.DrawString(_font, filename,
                                                new Vector2(6, 200 * 4 - 42), Color.Black);

                            spriteBatch.DrawString(_font, filename,
                                new Vector2(4, 200 * 4 - 44), Color.White);
                            spriteBatch.DrawString(_font, footerList.Find(x => x.name.Contains("SPRITES")).content,
                                new Vector2(6, 200 * 4 - 20), Color.Black);
                            spriteBatch.DrawString(_font, footerList.Find(x => x.name.Contains("SPRITES")).content,
                                new Vector2(4, 200 * 4 - 22), Color.White);
                            break;
                            #endregion

                        case ScreenState.texts:

                            #region - FOOTER TEXTS - 
                            float posX = 0;
                            foreach (Texts t in texts) {

                                

                                spriteBatch.DrawString(_font, t.name,
                                                    new Vector2(6 + posX, 200 * 4 - 42), Color.Black);

                                Color c = Color.DarkGray;
                                
                                if (filename == t.name) c = Color.White;

                                spriteBatch.DrawString(_font, t.name,
                                    new Vector2(4 + posX, 200 * 4 - 44), c);
                                posX = posX +4 + _font.MeasureString(t.name).X;

                            }

                            spriteBatch.DrawString(_font, footerList.Find(x => x.name.Contains("TEXTS")).content,
                                new Vector2(6, 200 * 4 - 20), Color.Black);

                            spriteBatch.DrawString(_font, footerList.Find(x => x.name.Contains("TEXTS")).content,
                                new Vector2(4, 200 * 4 - 22), Color.White);
                            break;
                            #endregion

                          
                    }
                }


            }
            #endregion

            #region MOUSE POINTER
            if (Settings.showMouse)
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred,
                      BlendState.AlphaBlend,
                      SamplerState.PointClamp,
                      null, null, null, Scale);

                /***************** MOUSE POINTER ******************************/
                spriteBatch.Draw(mousePointer, new Vector2(mouseState.X / Settings.PIXEL_RATIO, mouseState.Y / Settings.PIXEL_RATIO), Color.White);

            }
            
            spriteBatch.End();

            #endregion


            base.Draw(gameTime);
        }
    }
}
