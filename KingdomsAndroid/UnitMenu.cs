using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

//menyn för varje soldat
namespace KingdomsAndroid
{
    public class UnitMenu
    {
        UnitMenuButton Move;
        UnitMenuButton Attack;
        UnitMenuButton Occupy;
        UnitMenuButton Finish;
        public Vector2 Pos { get; set; }
        Vector2 UnitPos;

        

        public bool visible { get; set; }
        
        /// <summary>
        /// laddar knapparna
        /// </summary>
        /// <param name="game"></param>
        public UnitMenu(Game1 game)
        {
            Move = new UnitMenuButton(game.Content);
            Attack = new UnitMenuButton(game.Content);
            Occupy = new UnitMenuButton(game.Content);
            Finish = new UnitMenuButton(game.Content);
            
        
        }

        /// <summary>
        /// säger till var menyn ska målas upp
        /// initialiserar knappen på sin nya position
        /// </summary>
        /// <param name="unitpos"></param>
        public void Initialize(Vector2 unitpos,Player player)
        {
            Pos = new Vector2((unitpos.X + 1), (unitpos.Y));

            if (unitpos.X>=(25-3))
                Pos = new Vector2((unitpos.X - 3), (Pos.Y));
            else
                Pos = new Vector2((unitpos.X +1), (Pos.Y));
            if (unitpos.Y >= (15 - 3))
                Pos = new Vector2(Pos.X, unitpos.Y - 3);
            else
                Pos = new Vector2(Pos.X, unitpos.Y);


        Move.Initialize(new Vector2(Pos.X*32,Pos.Y*32));
        Move.Text = "Move";
        Attack.Initialize(new Vector2(Pos.X * 32, (Pos.Y+1) * 32));
        Attack.Text="Attack";
        Occupy.Initialize(new Vector2(Pos.X * 32, (Pos.Y+2) * 32));
        Occupy.Text = "Occupy";
        Finish.Initialize(new Vector2(Pos.X * 32, (Pos.Y+3) * 32));
        Finish.Text = "Finish";

        UnitPos = unitpos;
        }

        /// <summary>
        /// kollar om knapparna ska vara aktiva eller ej
        /// </summary>
        /// <param name="game1"></param>
        public void EnableBtn(Game1 game1)
        {

            if (game1.Playermanager.player[game1.Playermanager.playing].soldat[game1.Playermanager.player[game1.Playermanager.playing].currentUnit].moved == true)
                Move.active = false;
            else
                Move.active = true;

            if (game1.Playermanager.player[game1.Playermanager.playing].soldat[game1.Playermanager.player[game1.Playermanager.playing].currentUnit].fight == true && game1.Playermanager.player[game1.Playermanager.playing].soldat[game1.Playermanager.player[game1.Playermanager.playing].currentUnit].fought==false)
                Attack.active = true;
            else
                Attack.active = false;

            if (game1.Tilemanager.Map[(int)UnitPos.X + (int)UnitPos.Y * game1.Tilemanager.MapTileHeight].Type == game1.Playermanager.player[game1.Playermanager.notplaying].housetype || game1.Tilemanager.Map[(int)UnitPos.X + (int)UnitPos.Y * game1.Tilemanager.MapTileHeight].Type == 25)
                Occupy.active = true;
            else if (game1.Tilemanager.Map[(int)UnitPos.X + (int)UnitPos.Y * game1.Tilemanager.MapTileHeight].Type == game1.Playermanager.player[game1.Playermanager.notplaying].castletype && game1.Playermanager.player[game1.Playermanager.playing].soldat[game1.Playermanager.player[game1.Playermanager.playing].currentUnit].type == 1)
                Occupy.active = true;
            else if (game1.Tilemanager.Map[(int)UnitPos.X + (int)UnitPos.Y * game1.Tilemanager.MapTileHeight].Type == 43 && game1.Playermanager.player[game1.Playermanager.playing].soldat[game1.Playermanager.player[game1.Playermanager.playing].currentUnit].type == 1)
                Occupy.active = true;
            else
                Occupy.active = false;

        }
        

        /// <summary>
        /// uppdaterar knapparna, kollar om man klickat på dem
        /// </summary>
        /// <param name="GT"></param>
        /// <param name="game1"></param>
        public void Update(GameTime GT, Game1 game1)
        {

            EnableBtn(game1);
                        
            Move.Update();
            Attack.Update();
            Occupy.Update();
            Finish.Update();




            if (visible == true)
            { 
            
            if (Move.state == UnitMenuButton.BState.pressed)
            {                
                game1.Playermanager.player[game1.Playermanager.playing].setRange(game1.Playermanager.player[game1.Playermanager.playing].currentUnit,game1);
                game1.Playermanager.player[game1.Playermanager.playing].Pstate = Player.state.SelectMove;

                game1.Playermanager.player[game1.Playermanager.playing].HideCastleMenu();
            }                
            else if (Attack.state == UnitMenuButton.BState.pressed)
            {
                game1.Playermanager.player[game1.Playermanager.playing].soldat[game1.Playermanager.player[game1.Playermanager.playing].currentUnit].moved = true;
                game1.Playermanager.player[game1.Playermanager.playing].Pstate = Player.state.SelectFight;

                game1.Playermanager.player[game1.Playermanager.playing].HideCastleMenu();
            }
            else if (Occupy.state == UnitMenuButton.BState.pressed)
            {
                if (game1.Tilemanager.Map[(int)UnitPos.X + (int)UnitPos.Y * game1.Tilemanager.MapTileHeight].Type ==
                        game1.Playermanager.player[game1.Playermanager.notplaying].housetype || game1.Tilemanager.Map[(int)UnitPos.X + (int)UnitPos.Y * game1.Tilemanager.MapTileHeight].Type == 25)
                {
                    game1.Tilemanager.Map[(int)UnitPos.X + (int)UnitPos.Y * game1.Tilemanager.MapTileHeight].SetTile(game1.Playermanager.player[game1.Playermanager.playing].housetype, (int)UnitPos.X, (int)UnitPos.Y);
                }
                else if (game1.Tilemanager.Map[(int)UnitPos.X + (int)UnitPos.Y * game1.Tilemanager.MapTileHeight].Type == 33 ||
                         game1.Tilemanager.Map[(int)UnitPos.X + (int)UnitPos.Y * game1.Tilemanager.MapTileHeight].Type == 43)
                {
                    game1.Tilemanager.Map[(int)UnitPos.X + (int)UnitPos.Y * game1.Tilemanager.MapTileHeight].SetTile(game1.Playermanager.player[game1.Playermanager.playing].castletype, (int)UnitPos.X, (int)UnitPos.Y);
                    game1.Playermanager.player[game1.Playermanager.playing].CheckWin();
                }
                game1.Playermanager.player[game1.Playermanager.playing].soldat[game1.Playermanager.player[game1.Playermanager.playing].currentUnit].used = true;
                game1.Playermanager.player[game1.Playermanager.playing].soldat[game1.Playermanager.player[game1.Playermanager.playing].currentUnit].moved = true;
                game1.Playermanager.player[game1.Playermanager.playing].soldat[game1.Playermanager.player[game1.Playermanager.playing].currentUnit].fought = true;
                game1.Playermanager.player[game1.Playermanager.playing].currentUnit = -1;
                game1.Playermanager.player[game1.Playermanager.playing].Pstate = Player.state.SelectUnit;
                game1.Playermanager.player[game1.Playermanager.playing].HideUnitMenu();
            }
            else if (Finish.state == UnitMenuButton.BState.pressed)
            {
                if (game1.Playermanager.player[game1.Playermanager.playing].soldat[game1.Playermanager.player[game1.Playermanager.playing].currentUnit].fought == true || game1.Playermanager.player[game1.Playermanager.playing].soldat[game1.Playermanager.player[game1.Playermanager.playing].currentUnit].moved==true)
                {
                game1.Playermanager.player[game1.Playermanager.playing].soldat[game1.Playermanager.player[game1.Playermanager.playing].currentUnit].used = true;
                game1.Playermanager.player[game1.Playermanager.playing].soldat[game1.Playermanager.player[game1.Playermanager.playing].currentUnit].fought = true;
                game1.Playermanager.player[game1.Playermanager.playing].soldat[game1.Playermanager.player[game1.Playermanager.playing].currentUnit].moved = true;
                }
                game1.Playermanager.player[game1.Playermanager.playing].currentUnit = -1;
                game1.Playermanager.player[game1.Playermanager.playing].Pstate = Player.state.SelectUnit;
                game1.Playermanager.player[game1.Playermanager.playing].HideUnitMenu();
            }
            }


            
        }

        /// <summary>
        /// Målar upp knapparna
        /// </summary>
        /// <param name="SB"></param>
        public void Draw(SpriteBatch SB)
        {
            if (visible == true)
            {
                Move.Draw(SB);
                Attack.Draw(SB);
                Occupy.Draw(SB);
                Finish.Draw(SB);
            }
        
        
        }
    }
}
