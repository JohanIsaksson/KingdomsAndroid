using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace KingdomsAndroid
{
    public class Soldier
    {
        public Vector2 Pos { get; set; }
        public Rectangle Rect {get; set;}
        public Rectangle Bild { get; set; }

        public bool used { get; set; }
        public bool moved { get; set; }
        public bool fought { get; set; }

        public bool fight { get; set; }

        public int type { get; set; }
        public int nr { get; set; }
        public int Hp { get; set; }
        public int HpReg { get; set; }
        public int Damage { get; set; }
        public float hpdmg { get; set; }
        public int Level { get; set; }
        public int Armor { get; set; }        
        int BeginArmor;
        int BonusArmor;
        public int WalkRange { get; set; }
        public int AttackRange { get; set; }
        

        string PlayerColor;
        int Pcolor;
        int Team;
        

        public Soldier(Game1 pgame,string color)
        {
            PlayerColor = color;
            if (color == "Blue")
                Pcolor = 0;
            else if (color == "Red")
                Pcolor = 1;
            else if (color == "Yellow")
                Pcolor = 2;
            else if (color == "Green")
                Pcolor = 3;
            
        }


        public void SetType(int value,int x,int y,int Unr)
        {
            switch (value)
            { 
                case 1: //king
                    Bild = new Rectangle((value - 1) * 32, 0 + (Pcolor * 32), 32, 32);                    
                    BeginArmor = 15;
                    Damage = 50;
                    hpdmg = 0.5f;
                    AttackRange = 1;
                    WalkRange = 4;
                break;

                case 2: //Knight
                Bild = new Rectangle((value - 1) * 32, 0 + (Pcolor * 32), 32, 32);
                BeginArmor = 10;
                Damage = 40;
                hpdmg = 0.4f;
                AttackRange = 1;
                WalkRange = 4;
                break;

                case 3: //archer
                Bild = new Rectangle((value - 1) * 32, 0 + (Pcolor * 32), 32, 32);
                BeginArmor = 5;
                Damage = 40;
                hpdmg = 0.4f;
                AttackRange = 2;
                WalkRange = 4;
                break;

                case 4: //Shieldman
                Bild = new Rectangle((value -1)*32, 0 + (Pcolor * 32), 32, 32);
                BeginArmor = 20;
                Damage = 50;
                hpdmg = 0.5f;
                AttackRange = 1;
                WalkRange = 4;

                break;

                case 5: //Catapult
                Bild = new Rectangle((value -1)*32, 0 + (Pcolor * 32), 32, 32);
                BeginArmor = 0;
                Damage = 60;
                hpdmg = 0.6f;
                AttackRange = 6;
                WalkRange = 3;
                break;

                case 6:
                    Bild = new Rectangle((value -1)*32, 0 + (Pcolor * 32), 32, 32);
                BeginArmor = 15;
                Damage = 70;
                hpdmg = 0.7f;
                AttackRange = 1;
                WalkRange = 6;
                break;
            
            
            
            }

            type = value;
        Level = 0;
        Hp = 100;
        Pos = new Vector2(x, y);
        nr = Unr;
        fought = false;
        }


        public void NewRound()
        {
            if (Hp <= 80)
                Hp += HpReg;
            else if (Hp > 80 && Hp < 100)
                Hp = 100;
                

            used = false;
            moved = false;
            fight = false;
            fought = false;
            
        }


        public void Update(GameTime GT, Game1 game)
        {
            //kolla vad soldaten står på, t.ex. hus= +20hp per runda
            switch (PlayerColor)
            {
                case "Blue":
                    if (game.Tilemanager.Map[(int)Pos.X + (int)Pos.Y * game.Tilemanager.MapTileHeight].Type == 26)
                    {
                        BonusArmor = 5;
                        HpReg = 20;
                    }
                    else if (game.Tilemanager.Map[(int)Pos.X + (int)Pos.Y * game.Tilemanager.MapTileHeight].Type == 27)
                    {
                        BonusArmor = 5;
                        HpReg = 20;
                    }
                    else
                    {
                        BonusArmor = 0;
                        HpReg = 0;
                    }
                break;

                case "Red":
                if (game.Tilemanager.Map[(int)Pos.X + (int)Pos.Y * game.Tilemanager.MapTileHeight].Type == 32)
                {
                    BonusArmor = 5;
                    HpReg = 20;
                }
                else if (game.Tilemanager.Map[(int)Pos.X + (int)Pos.Y * game.Tilemanager.MapTileHeight].Type == 33)
                {
                    BonusArmor = 5;
                    HpReg = 20;
                }
                else
                {
                    BonusArmor = 0;
                    HpReg = 0;
                }
                    break;

                case "Green":
                    if (game.Tilemanager.Map[(int)Pos.X + (int)Pos.Y * game.Tilemanager.MapTileHeight].Type == 28)
                    {
                        BonusArmor = 5;
                        HpReg = 20;
                    }
                    else if (game.Tilemanager.Map[(int)Pos.X + (int)Pos.Y * game.Tilemanager.MapTileHeight].Type == 29)
                    {
                        BonusArmor = 5;
                        HpReg = 20;
                    }
                    else
                    {
                        BonusArmor = 0;
                        HpReg = 0;
                    }
                    break;

                case "Yellow":
                    if (game.Tilemanager.Map[(int)Pos.X + (int)Pos.Y * game.Tilemanager.MapTileHeight].Type == 30)
                    {
                        BonusArmor = 10;
                        HpReg = 20;
                    }
                    else if (game.Tilemanager.Map[(int)Pos.X + (int)Pos.Y * game.Tilemanager.MapTileHeight].Type == 31)
                    {
                        BonusArmor = 15;
                        HpReg = 20;
                    }
                    else
                    {
                        BonusArmor = 0;
                        HpReg = 0;
                    }
                    break;
            }

            
            Damage = (int)(hpdmg * (float)Hp);            
            Armor = BeginArmor + BonusArmor;
            Rect = new Rectangle((int)(Pos.X * 32), (int)(Pos.Y*32), 32, 32);
            

            //if (moved == true && fight == true)
            //    used = true;
            
        }

        

        





    }

    

}
