using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace KingdomsAndroid
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphics { get; set; }
        SpriteBatch spriteBatch;
        public Camera Camera { get; set; }

        public enum GameState
        {
            TitleScreen,
            MainMenu,
            NewGame,
            LoadGame,
            SaveGame,
            Running,
            Paused,
            Exit,
        }
        public GameState state { get; set; }

        public TileManager Tilemanager { get; set; }
        public MainMenu mainmenu { get; set; }
        public NewGame newgame { get; set; }
        public PauseMenu pausemenu { get; set; }
        public PlayerManager Playermanager { get; set; }

        int tSec;
        int tTicks;
        int MaxTicks;

        public string mapname { get; set; }

        SpriteFont FpsFont;

        string SaveDir = "Saves\\";

        public static int screenH = 512;
        public static int screenW = 832;

        private Viewport gameView, bottomHUD, sideHUD;



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

            screenW = GraphicsDevice.PresentationParameters.BackBufferWidth;
            screenH = GraphicsDevice.PresentationParameters.BackBufferHeight;

            gameView = new Viewport();
            gameView.X = 32;
            gameView.Y = 0;
            gameView.Width = screenW - 128;
            gameView.Height = screenH - 64;
            gameView.MinDepth = 0;
            gameView.MaxDepth = 1;

            sideHUD = new Viewport();
            sideHUD.X = screenW - 128+32;
            sideHUD.Y = 0;
            sideHUD.Width = 128;
            sideHUD.Height = screenH - 64;
            sideHUD.MinDepth = 0;
            sideHUD.MaxDepth = 1;

            bottomHUD = new Viewport();
            bottomHUD.X = 32;
            bottomHUD.Y = screenH - 64;
            bottomHUD.Width = screenW;
            bottomHUD.Height = 64;
            bottomHUD.MinDepth = 0;
            bottomHUD.MaxDepth = 1;

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

            graphics.PreferredBackBufferWidth = screenW;
            graphics.PreferredBackBufferHeight = screenH;
            graphics.ApplyChanges();

            Camera = new Camera(gameView);

            //IsFixedTimeStep = false;
            //graphics.SynchronizeWithVerticalRetrace = false;

            state = GameState.MainMenu;

            mainmenu = new MainMenu(this);
            mainmenu.Initialize(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            pausemenu = new PauseMenu(this);
            pausemenu.Initialize(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            newgame = new NewGame(this);

            Tilemanager = new TileManager(this);

            Playermanager = new PlayerManager(this, 2, 25);

            FpsFont = Content.Load<SpriteFont>("MenuFont");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        Vector2 position = Vector2.Zero;

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            TouchManager.Instance.Update(Camera);

            // Update camera position
            //Vector2 cameraMovement = Vector2.Zero;            
            //position += cameraMovement/Camera.Zoom;            

            switch (state)
            {
                case GameState.TitleScreen:
                    break;

                case GameState.MainMenu:
                    mainmenu.Update();
                    Camera.Zoom = 1f;
                    Camera.Update(Vector2.Zero, 64 * 32, 64 * 32);
                    position = Camera.Position;
                    break;

                case GameState.NewGame:

                    newgame.Update(gameTime, this);
                    Camera.Update(Vector2.Zero, 64 * 32, 64 * 32);
                    position = Camera.Position;
                    break;

                case GameState.LoadGame:
                    //Tilemanager.LoadMap(mapname);
                    Playermanager.player[Playermanager.playing].NewRound();
                    Playermanager.player[Playermanager.playing].NewRound();
                    state = GameState.Running;

                    break;

                case GameState.SaveGame:
                    Playermanager.SaveGame(SaveDir);

                    break;

                case GameState.Running:
                    Playermanager.Update(gameTime, this);
                    Tilemanager.Update();
                    Camera.Zoom = 2f;
                    position -= TouchManager.Instance.SwipeDirection / Camera.Zoom;
                    Camera.Update(position,
                                  Tilemanager.MapBounds.Width,
                                  Tilemanager.MapBounds.Height);
                    position = Camera.Position;
                    //Camera.Update(GraphicsDevice.Viewport);

                    /*if (Esc.IsKeyDown(Keys.Escape) == true)
                    {
                        state = GameState.Paused;
                    }
                    if ((Playermanager.player[Playermanager.playing].end == true && Playermanager.player[Playermanager.playing].hud.newBool == false) || (Playermanager.player[Playermanager.notplaying].end == true && Playermanager.player[Playermanager.notplaying].hud.newBool == false))
                    {
                        state = GameState.MainMenu;

                    }*/

                    break;


                case GameState.Paused:
                    pausemenu.Update();

                    break;

                case GameState.Exit:
                    this.Exit();
                    break;
            }

            // FPS räknare---------------------------------
            //if (tSec == DateTime.Now.Second)
            //{
            //    tTicks += 1;
            //}
            //else
            //{
            //    MaxTicks = tTicks;
            //    tTicks = 0;
            //    tSec = DateTime.Now.Second;
            //}
            //--------------------------------------------

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            GraphicsDevice.Viewport = gameView;

            spriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null,
                null,
                null,
                Camera.Transform);

            switch (state)
            {
                case GameState.TitleScreen:
                    break;
                case GameState.MainMenu:
                    mainmenu.Draw(spriteBatch);
                    break;
                case GameState.NewGame:
                    newgame.Draw(spriteBatch);
                    break;
                case GameState.Running:
                    Tilemanager.Draw(spriteBatch);
                    Playermanager.Draw(spriteBatch);
                    break;
                case GameState.Paused:
                    Tilemanager.Draw(spriteBatch);
                    Playermanager.Draw(spriteBatch);
                    pausemenu.Draw(spriteBatch);
                    break;
            }
            //spriteBatch.DrawString(FpsFont, Convert.ToString(MaxTicks), new Vector2(750, 50), Color.White);

            spriteBatch.End();

            // 
            GraphicsDevice.Viewport = sideHUD;
            spriteBatch.Begin();
            Texture2D rect = new Texture2D(graphics.GraphicsDevice, sideHUD.Width, sideHUD.Height);

            Color[] data = new Color[sideHUD.Width * sideHUD.Height];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Chocolate;
            rect.SetData(data);
            
            spriteBatch.Draw(rect, Vector2.Zero, Color.White);

            spriteBatch.End();


            //
            GraphicsDevice.Viewport = bottomHUD;
            spriteBatch.Begin();
            rect = new Texture2D(graphics.GraphicsDevice, bottomHUD.Width, bottomHUD.Height);

            data = new Color[bottomHUD.Width * bottomHUD.Height];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Blue;
            rect.SetData(data);
            
            spriteBatch.Draw(rect, Vector2.Zero, Color.White);

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
