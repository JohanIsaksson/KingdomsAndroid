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
        /// kollar om knapparna ska vara aktiva eller ej
        /// </summary>
        /// <param name="game1"></param>
        public void EnableButtons(Game1 game1)
        {
            // Current player, tile and selected soldier
            Player player = game1.Playermanager.Players[game1.Playermanager.CurrentPlayerID];
            Soldier soldier = player.Soldiers[player.currentUnit];
            Tile tile = game1.Tilemanager.tileAt(soldier.Pos);

            if (soldier.moved == true)
                Move.active = false;
            else
                Move.active = true;

            if (soldier.fight == true && soldier.fought==false)
                Attack.active = true;
            else
                Attack.active = false;

            Occupy.active = false;
            foreach (Player p in game1.Playermanager.Players)
            {
                if (tile.Type == p.housetype || 
                    tile.Type == 25 ||
                    (tile.Type == p.castletype && soldier.type == 1) ||
                    (tile.Type == 43 && soldier.type == 1))
                {
                    Occupy.active = true;
                    break;
                }
            }
                

        }
        

        /// <summary>
        /// uppdaterar knapparna, kollar om man klickat på dem
        /// </summary>
        /// <param name="GT"></param>
        /// <param name="game1"></param>
        public void Update(GameTime GT, Game1 game1)
        {

            EnableButtons(game1);
                        
            Move.Update();
            Attack.Update();
            Occupy.Update();
            Finish.Update();

            // Current player, tile and selected soldier
            Player player = game1.Playermanager.Players[game1.Playermanager.CurrentPlayerID];
            Soldier soldier = player.Soldiers[player.currentUnit];
            Tile tile = game1.Tilemanager.tileAt(soldier.Pos);

            if (visible == true)
            { 
            
                if (Move.state == TouchButton.ButtonState.Clicked)
                {
                    player.MoveArea = soldier.CalculateMoveRange(game1);
                    player.Pstate = Player.state.SelectMove;
                    player.HideCastleMenu();
                    visible = false;
                }                
                else if (Attack.state == TouchButton.ButtonState.Clicked)
                {
                    soldier.moved = true;
                    player.AttackArea = soldier.CalculateAttackRange(game1);
                    player.Pstate = Player.state.SelectFight;

                    player.HideCastleMenu();
                }
                else if (Occupy.state == TouchButton.ButtonState.Clicked)
                {
                    foreach (Player p in game1.Playermanager.Players)
                    {
                        if (tile.Type == p.housetype || tile.Type == 25)
                        {
                            tile.SetTile(player.housetype, (int)UnitPos.X, (int)UnitPos.Y);
                            break;
                        }
                        else if (tile.Type == 33 || tile.Type == 43)
                        {
                            tile.SetTile(player.castletype, (int)UnitPos.X, (int)UnitPos.Y);
                            player.CheckWin();
                            break;
                        }
                    }
                    
                    soldier.used = true;
                    soldier.moved = true;
                    soldier.fought = true;
                    player.currentUnit = -1;
                    player.Pstate = Player.state.SelectUnit;
                    player.HideUnitMenu();
                }
                else if (Finish.state == TouchButton.ButtonState.Clicked)
                {
                    if (soldier.fought == true || soldier.moved==true)
                    {
                    soldier.used = true;
                    soldier.fought = true;
                    soldier.moved = true;
                    }
                    player.currentUnit = -1;
                    player.Pstate = Player.state.SelectUnit;
                    player.HideUnitMenu();
                }
            }            
        }

        /// <summary>
        /// Draws the unit menu onto the sprite batch.
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
