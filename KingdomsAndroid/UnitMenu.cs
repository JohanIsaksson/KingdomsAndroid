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
        TouchButton Move;
        TouchButton Attack;
        TouchButton Occupy;
        TouchButton Finish;
        public Vector2 Pos { get; set; }
        Vector2 UnitPos;

        

        public bool visible { get; set; }
        
        /// <summary>
        /// laddar knapparna
        /// </summary>
        /// <param name="game"></param>
        public UnitMenu(Game1 game, Vector2 unitpos, Player player)
        {
            Texture2D unitMenuButtons = game.Content.Load<Texture2D>("soldierbuttons");

            // Retrieve sub texture
            Rectangle r = new Rectangle(1*32, 0, 32, 32);
            //Texture2D t = new Texture2D(game.GraphicsDevice, 32, 32);
            //Color[] data = new Color[32*32];
            //unitMenuButtons.GetData(0, r, data, 0, data.Length);
            //t.SetData(data);
            Move = new TouchButton(game, new Vector2(unitpos.X, unitpos.Y - 1)*32, unitMenuButtons, r);

            r = new Rectangle(0, 0, 32, 32);
            //t = new Texture2D(game.GraphicsDevice, 32, 32);
            //unitMenuButtons.GetData(0, r, data, 0, data.Length);
            //t.SetData(data);
            Attack = new TouchButton(game, new Vector2(unitpos.X + 1, unitpos.Y) * 32, unitMenuButtons, r);

            r = new Rectangle(3*32, 0, 32, 32);
            //t = new Texture2D(game.GraphicsDevice, 32, 32);
            //unitMenuButtons.GetData(0, r, data, 0, data.Length);
            //t.SetData(data);
            Occupy = new TouchButton(game, new Vector2(unitpos.X - 1, unitpos.Y) * 32, unitMenuButtons, r);

            r = new Rectangle(1*32, 1*32, 32, 32);
            //t = new Texture2D(game.GraphicsDevice, 32, 32);
            //unitMenuButtons.GetData(0, r, data, 0, data.Length);
            //t.SetData(data);
            Finish = new TouchButton(game, new Vector2(unitpos.X, unitpos.Y + 1) * 32, unitMenuButtons, r);
            
        
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


        Move.Position = new Vector2(Pos.X*32,Pos.Y*32);
        Move.Text = "Move";
        Attack.Position = new Vector2(Pos.X * 32, (Pos.Y+1) * 32);
        Attack.Text="Attack";
        Occupy.Position = new Vector2(Pos.X * 32, (Pos.Y+2) * 32);
        Occupy.Text = "Occupy";
        Finish.Position = new Vector2(Pos.X * 32, (Pos.Y+3) * 32);
        Finish.Text = "Finish";

        UnitPos = unitpos;
        }
        
        /// <summary>
        /// kollar om knapparna ska vara aktiva eller ej
        /// </summary>
        /// <param name="game1"></param>
        public void EnableBtn(Game1 game1)
        {

            if (game1.Playermanager.player[game1.Playermanager.playing].soldiers[game1.Playermanager.player[game1.Playermanager.playing].currentUnit].moved == true)
                Move.active = false;
            else
                Move.active = true;

            if (game1.Playermanager.player[game1.Playermanager.playing].soldiers[game1.Playermanager.player[game1.Playermanager.playing].currentUnit].fight == true && game1.Playermanager.player[game1.Playermanager.playing].soldiers[game1.Playermanager.player[game1.Playermanager.playing].currentUnit].fought==false)
                Attack.active = true;
            else
                Attack.active = false;

            if (game1.Tilemanager.Map[(int)UnitPos.X + (int)UnitPos.Y * game1.Tilemanager.MapTileHeight].Type == game1.Playermanager.player[game1.Playermanager.notplaying].housetype || game1.Tilemanager.Map[(int)UnitPos.X + (int)UnitPos.Y * game1.Tilemanager.MapTileHeight].Type == 25)
                Occupy.active = true;
            else if (game1.Tilemanager.Map[(int)UnitPos.X + (int)UnitPos.Y * game1.Tilemanager.MapTileHeight].Type == game1.Playermanager.player[game1.Playermanager.notplaying].castletype && game1.Playermanager.player[game1.Playermanager.playing].soldiers[game1.Playermanager.player[game1.Playermanager.playing].currentUnit].type == 1)
                Occupy.active = true;
            else if (game1.Tilemanager.Map[(int)UnitPos.X + (int)UnitPos.Y * game1.Tilemanager.MapTileHeight].Type == 43 && game1.Playermanager.player[game1.Playermanager.playing].soldiers[game1.Playermanager.player[game1.Playermanager.playing].currentUnit].type == 1)
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

            Player player = game1.Playermanager.player[game1.Playermanager.playing];


            if (visible == true)
            { 
            
            if (Move.state == TouchButton.ButtonState.Clicked)
            {       
                // Calculate range for given soldier
                player.CalculateMoveRange(player.soldiers[player.currentUnit],game1);
                
                //paigame1.Playermanager.player[game1.Playermanager.playing].setRange(game1.Playermanager.player[game1.Playermanager.playing].currentUnit,game1);
                game1.Playermanager.player[game1.Playermanager.playing].Pstate = Player.state.SelectMove;

                game1.Playermanager.player[game1.Playermanager.playing].HideCastleMenu();
                visible = false;
            }                
            else if (Attack.state == TouchButton.ButtonState.Clicked)
            {
                game1.Playermanager.player[game1.Playermanager.playing].soldiers[game1.Playermanager.player[game1.Playermanager.playing].currentUnit].moved = true;
                game1.Playermanager.player[game1.Playermanager.playing].Pstate = Player.state.SelectFight;

                game1.Playermanager.player[game1.Playermanager.playing].HideCastleMenu();
            }
            else if (Occupy.state == TouchButton.ButtonState.Clicked)
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
                game1.Playermanager.player[game1.Playermanager.playing].soldiers[game1.Playermanager.player[game1.Playermanager.playing].currentUnit].used = true;
                game1.Playermanager.player[game1.Playermanager.playing].soldiers[game1.Playermanager.player[game1.Playermanager.playing].currentUnit].moved = true;
                game1.Playermanager.player[game1.Playermanager.playing].soldiers[game1.Playermanager.player[game1.Playermanager.playing].currentUnit].fought = true;
                game1.Playermanager.player[game1.Playermanager.playing].currentUnit = -1;
                game1.Playermanager.player[game1.Playermanager.playing].Pstate = Player.state.SelectUnit;
                game1.Playermanager.player[game1.Playermanager.playing].HideUnitMenu();
            }
            else if (Finish.state == TouchButton.ButtonState.Clicked)
            {
                if (game1.Playermanager.player[game1.Playermanager.playing].soldiers[game1.Playermanager.player[game1.Playermanager.playing].currentUnit].fought == true || game1.Playermanager.player[game1.Playermanager.playing].soldiers[game1.Playermanager.player[game1.Playermanager.playing].currentUnit].moved==true)
                {
                game1.Playermanager.player[game1.Playermanager.playing].soldiers[game1.Playermanager.player[game1.Playermanager.playing].currentUnit].used = true;
                game1.Playermanager.player[game1.Playermanager.playing].soldiers[game1.Playermanager.player[game1.Playermanager.playing].currentUnit].fought = true;
                game1.Playermanager.player[game1.Playermanager.playing].soldiers[game1.Playermanager.player[game1.Playermanager.playing].currentUnit].moved = true;
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
