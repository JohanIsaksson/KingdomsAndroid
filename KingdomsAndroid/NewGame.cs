using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace KingdomsAndroid
{
    /// <summary>
    /// skapar en ruta där man kan ändra inställningar och starta ett nytt spel
    /// </summary>
    public class NewGame
    {
        Game1 game;
        Texture2D Newbox;
        Texture2D BG;
        Rectangle BGrect;
        
        Texture2D musb;
        Vector2 musp;

        SliderBar moneybar;
        string money="";
        int cash;
        SpriteFont text;


        SliderBar unitbar;
        string units="";
        int soldater;

        MapList maplist;
        string mapname;







      
        TouchButton Start;
        TouchButton mainmenu;
        Vector2 NewBoxpos;
        int Xmid;
        

        /// <summary>
        /// initialiserar knappar och annat så allt hamnar i förhållande till new game-rutan
        /// laddar texturer och klasser
        /// </summary>
        /// <param name="game1"></param>
        public NewGame(Game1 game1)
        {
            game = game1;
            Newbox = game.Content.Load<Texture2D>("NewGame");
            NewBoxpos = new Vector2((game.graphics.PreferredBackBufferWidth / 2) - (Newbox.Width / 2), (game.graphics.PreferredBackBufferHeight / 2) - (Newbox.Height / 2));
            Xmid=(game.graphics.PreferredBackBufferWidth/2);

            moneybar = new SliderBar(game, Newbox.Width - 64, 2141, new Vector2(NewBoxpos.X + 32, NewBoxpos.Y + 64));
            text = game.Content.Load<SpriteFont>("UnitFont");
            unitbar = new SliderBar(game, Newbox.Width - 64, 27, new Vector2(NewBoxpos.X + 32, NewBoxpos.Y + 64+64));
            musb = game.Content.Load<Texture2D>("mus2");

            maplist = new MapList(game,new Vector2(NewBoxpos.X+32,NewBoxpos.Y+180));
            maplist.LoadInfo("Maps\\");

            BG = game.Content.Load<Texture2D>("grassBack");
            
            Start = new MenuButton(game);
            Start.Position = new Vector2((Newbox.Width-256-16),(Newbox.Height-64-8));
            Start.Text = "   START";

            mainmenu = new MenuButton(game);
            mainmenu.Position = new Vector2((NewBoxpos.X+32+16), (Newbox.Height-64-8));
            mainmenu.Text = "Main Menu";

            BGrect = new Rectangle(0, 0, 1920, 1080);




        }

        
        /// <summary>
        /// uppdaterar knappars bilder och positioner så att de målas upp ordentligt
        /// </summary>
        /// <param name="GT"></param>
        public void Update(GameTime GT,Game1 game1)
        {
            game = game1;

            Start.Update();
            mainmenu.Update();

            NewBoxpos = new Vector2((game.graphics.PreferredBackBufferWidth / 2) - (Newbox.Width / 2), (game.graphics.PreferredBackBufferHeight / 2) - (Newbox.Height / 2));

            Start.Position=new Vector2((NewBoxpos.X+Newbox.Width-256-8),(NewBoxpos.Y+Newbox.Height-64-8));
            mainmenu.Position=new Vector2((NewBoxpos.X+8), (NewBoxpos.Y+Newbox.Height-64-8));            


            MouseState mus = Mouse.GetState();
            musp = new Vector2(mus.X, mus.Y);

            if (Start.state == TouchButton.ButtonState.Clicked)
            {
                var playerList = game.Playermanager.Players;
                playerList[0].Initialize(soldater, "Blue", 1, 0, cash);
                playerList[1].Initialize(soldater, "Red", 2, 1, cash);
                game.mapname = maplist.map + ".map";      

                game.state = Game1.GameState.LoadGame;
            }
            else if (mainmenu.state == TouchButton.ButtonState.Clicked)
                game.state = Game1.GameState.MainMenu;

            moneybar.Update(GT);
            money = "Gold: " + Convert.ToString(moneybar.getBarAttribute);
            cash = moneybar.getBarAttribute;

            unitbar.Update(GT);
            units = "Max Soldiers: " + Convert.ToString(unitbar.getBarAttribute);
            soldater = unitbar.getBarAttribute;

            maplist.Update();    
        
        }

        /// <summary>
        /// målar upp knappar, textrutor och andra element på skärmen
        /// </summary>
        /// <param name="SB"></param>
        public void Draw(SpriteBatch SB)
        {
            SB.Draw(BG, new Vector2(-240, 0), BGrect, Color.White);
            SB.Draw(Newbox, NewBoxpos, new Color(255, 255, 255, 185));

            moneybar.Draw(SB);
            SB.DrawString(text, money, new Vector2(NewBoxpos.X + 32, NewBoxpos.Y + 64 + 16),Color.Gold);

            unitbar.Draw(SB);
            SB.DrawString(text, units, new Vector2(NewBoxpos.X + 32, NewBoxpos.Y + 64 + 64 + 16), Color.LawnGreen);

            maplist.Draw(SB);

            Start.Draw(SB);
            mainmenu.Draw(SB);
            
            SB.Draw(musb, musp, Color.White);
        }
    


}
    
}
