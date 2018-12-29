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

            graphics.PreferredBackBufferWidth = 832;
            graphics.PreferredBackBufferHeight = 512;
            graphics.ApplyChanges();
            
            Camera = new Camera(GraphicsDevice.Viewport);

            //IsFixedTimeStep = false;
            //graphics.SynchronizeWithVerticalRetrace = false;

            state = GameState.MainMenu;

            mainmenu = new MainMenu(this);
            mainmenu.Initialize(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            pausemenu = new PauseMenu(this);
            pausemenu.Initialize(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            newgame = new NewGame(this);

            Tilemanager = new TileManager(Content);
            Tilemanager.initialize();

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
            // Update camera position
            Vector2 cameraMovement = Vector2.Zero;
            TouchCollection touchCollection = TouchPanel.GetState();
            foreach (TouchLocation touch in touchCollection)
            {
                if ((touch.State == TouchLocationState.Moved) || (touch.State == TouchLocationState.Pressed))
                {
                    TouchLocation prevLocation;

                    // Sometimes TryGetPreviousLocation can fail. Bail out early if this happened
                    // or if the last state didn't move
                    if (touch.TryGetPreviousLocation(out prevLocation))
                    {
                        // get your delta
                        var delta = touch.Position - prevLocation.Position;

                        cameraMovement = -delta;
                    }
                    break;
                    // Everything is fine
                    //state = ButtonState.Clicked;
                }
            }
            position += cameraMovement/Camera.Zoom;

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
                    Tilemanager.LoadMap(mapname);
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
                    Camera.Update(position, 64 * 32, 64 * 32);
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

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

            base.Draw(gameTime);
        }
    }
}
