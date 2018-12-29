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
            shop = new UnitMenuButton(game.Content);
            turn = new UnitMenuButton(game.Content);    
    
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

            shop.Initialize(new Vector2((Pos.X) * 32, Pos.Y * 32));
            shop.Text = "Shop";
            turn.Initialize(new Vector2((Pos.X) * 32, (Pos.Y + 1) * 32));
            turn.Text = "End Turn";  
        
        
        
        
        
        
        }

        public void Update(Game1 game)
        {
            shop.Update();
            turn.Update();
            

            if (shop.state == UnitMenuButton.BState.pressed)
            {
                game.Playermanager.player[game.Playermanager.playing].shop = new Shop(game.Playermanager.player[game.Playermanager.playing], game);
                game.Playermanager.player[game.Playermanager.playing].shop.Initialize(game.Playermanager.player[game.Playermanager.playing]);
                game.Playermanager.player[game.Playermanager.playing].shop.Gold(game.Playermanager.player[game.Playermanager.playing]);
                game.Playermanager.player[game.Playermanager.playing].currentUnit = -1;
                game.Playermanager.player[game.Playermanager.playing].Pstate = Player.state.Shop;
                game.Playermanager.player[game.Playermanager.playing].HideUnitMenu();
            }
            else if (turn.state == UnitMenuButton.BState.pressed)
            {
                game.Playermanager.player[game.Playermanager.playing].currentUnit = -1;
                game.Playermanager.player[game.Playermanager.playing].HideUnitMenu();
                game.Playermanager.player[game.Playermanager.playing].EndRound();
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
