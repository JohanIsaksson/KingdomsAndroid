using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace KingdomsAndroid
{
    /// <summary>
    /// skapar en pausemeny med 4 knappar
    /// </summary>
    public class PauseMenu
    {
        MenuButton ResumeGame;
        MenuButton SaveGame;
        MenuButton LoadGame;
        MenuButton Exit;

        Texture2D BG;
        Rectangle BGrect;
        Texture2D musb;
        Vector2 musp;

        Game1 game;

        /// <summary>
        /// laddar knappar och texturer
        /// </summary>
        /// <param name="game"></param>
        public PauseMenu(Game1 game1)
        {
            game = game1;
            ResumeGame = new MenuButton(game);
            LoadGame = new MenuButton(game);
            SaveGame = new MenuButton(game);
            Exit = new MenuButton(game);

            musb = game.Content.Load<Texture2D>("mus2");
            BG = game.Content.Load<Texture2D>("Black");

        }

        /// <summary>
        /// placerar knapparna i mitten beroende på hur stor rutan är
        /// </summary>
        /// <param name="W"></param>
        /// <param name="H"></param>
        public void Initialize(int W, int H)
        {
            ResumeGame.Initialize(new Vector2((W / 2) - 128, (H / 2) - 128));
            ResumeGame.Text = "Resume";
            LoadGame.Initialize(new Vector2((W / 2) - 128, (H / 2) - 60));
            LoadGame.Text = "Load Game";
            SaveGame.Initialize(new Vector2((W / 2) - 128, (H / 2) +10));
            SaveGame.Text = "Save Game";
            Exit.Initialize(new Vector2((W / 2) - 128, (H / 2) + 78));
            Exit.Text = "Exit";

            BGrect = new Rectangle(0, 0, 1080, 1920);
        }

        /// <summary>
        /// uppdaterar knapparna och muspositioner
        /// </summary>
        public void Update()
        {
            ResumeGame.Update();
            LoadGame.Update();
            SaveGame.Update();
            Exit.Update();


            MouseState mus = Mouse.GetState();
            musp = new Vector2(mus.X, mus.Y);

            if (ResumeGame.state == MenuButton.ButtonState.Pressed)
                game.state = Game1.GameState.Running;
            else if (LoadGame.state == MenuButton.ButtonState.Pressed)
                game.state = Game1.GameState.LoadGame;
            else if (Exit.state == MenuButton.ButtonState.Pressed)
                game.state = Game1.GameState.MainMenu;


        }

        /// <summary>
        /// målar upp knappar och mus
        /// </summary>
        /// <param name="SB"></param>
        public void Draw(SpriteBatch SB)
        {
            SB.Draw(BG, new Vector2(0, 0), BGrect, new Color(255,255,255,100));

            ResumeGame.Draw(SB);
            LoadGame.Draw(SB);
            SaveGame.Draw(SB);
            Exit.Draw(SB);

            SB.Draw(musb, musp, Color.White);
        }


    }


}

