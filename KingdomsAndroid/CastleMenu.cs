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
    public class CastleMenu
    {
        UnitMenuButton shop;
        UnitMenuButton turn;
        public Vector2 Pos { get; set; }
        
        public bool visible { get; set; }

        public CastleMenu(Game1 game)
        {
            shop = new UnitMenuButton(game);
            turn = new UnitMenuButton(game);    
        }

        public void Initialize(Vector2 position,Player player)
        {
            Pos = position;

            if (position.X >= (25 - 3))
                Pos = new Vector2((position.X - 3), (Pos.Y));
            else
                Pos = new Vector2((position.X + 1), (Pos.Y));

            if (position.Y >= (15 - 3))
            {
                if (player.unitmenu.visible == false)
                    Pos = new Vector2(Pos.X, position.Y - 3);
                else
                    Pos = new Vector2(Pos.X, position.Y-3-4);
            }
            else
            {
                if (player.unitmenu.visible == false)
                    Pos = new Vector2(Pos.X, position.Y);
                else
                    Pos = new Vector2(Pos.X,Pos.Y-2);
            }

            shop.Position = new Vector2((Pos.X) * 32, Pos.Y * 32);
            shop.Text = "Shop";
            turn.Position = new Vector2((Pos.X) * 32, (Pos.Y + 1) * 32);
            turn.Text = "End Turn";  
        
        
        
        
        
        
        }

        public void Update(Game1 game)
        {
            shop.Update();
            turn.Update();
            

            if (shop.state == TouchButton.ButtonState.Clicked)
            {
                game.Playermanager.Players[game.Playermanager.CurrentPlayerID].shop = new Shop(game.Playermanager.Players[game.Playermanager.CurrentPlayerID], game);
                game.Playermanager.Players[game.Playermanager.CurrentPlayerID].shop.Initialize(game.Playermanager.Players[game.Playermanager.CurrentPlayerID]);
                game.Playermanager.Players[game.Playermanager.CurrentPlayerID].shop.Gold(game.Playermanager.Players[game.Playermanager.CurrentPlayerID]);
                game.Playermanager.Players[game.Playermanager.CurrentPlayerID].currentUnit = -1;
                game.Playermanager.Players[game.Playermanager.CurrentPlayerID].Pstate = Player.state.Shop;
                game.Playermanager.Players[game.Playermanager.CurrentPlayerID].HideUnitMenu();
            }
            else if (turn.state == TouchButton.ButtonState.Clicked)
            {
                game.Playermanager.Players[game.Playermanager.CurrentPlayerID].currentUnit = -1;
                game.Playermanager.Players[game.Playermanager.CurrentPlayerID].HideUnitMenu();
                game.Playermanager.Players[game.Playermanager.CurrentPlayerID].EndRound();
                game.Playermanager.NewRound();

            }
        
        }

        public void Draw(SpriteBatch SB)
        {
            if (visible == true)
            {
                shop.Draw(SB);
                turn.Draw(SB);
            }
        
        }

    }
}
