using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace KingdomsAndroid
{
    public class HUD
    {
        Texture2D topTexture;        
        Vector2 topPos;
        public bool topBool { get; set; }

        Texture2D infoTexture;
        Rectangle infoBar;
        Vector2 infoPos;
        public bool infoBool { get; set; }

        SpriteFont itemfont;
        Texture2D items;

        Rectangle attack;
        string ad;
        Rectangle armor;
        string arm;
        Rectangle hpr;
        string hp;
        Rectangle money;
        string cash;
        
        Texture2D newTexture;        
        Vector2 newPos;
        Color newColor;
        Color Fontcolor;
        SpriteFont NewFont;
        public bool newBool { get; set; }
        public string Newtext { get; set; }

        Player player;
        Color color;



        public HUD(Game1 game,Player play)
        {
            player = play;
            topTexture = game.Content.Load<Texture2D>("TopBar");
            topPos = new Vector2(0,-32);
            topBool = false;
            
            infoTexture = game.Content.Load<Texture2D>("InfoBar");
            infoBar = new Rectangle(0,Game1.screenH+1,2000,infoTexture.Height);
            infoPos = new Vector2(0,Game1.screenH);
            items = game.Content.Load<Texture2D>("attack");
            attack = new Rectangle(0, 0, 32, 32);
            hpr = new Rectangle(32, 0, 32, 32);
            armor = new Rectangle(64, 0, 32, 32);
            itemfont = game.Content.Load<SpriteFont>("UnitFont");
            SetInfoBar(new Soldier(game,"Blue"));
            infoBool = false;

            newTexture = game.Content.Load<Texture2D>("NewRound");            
            NewFont = game.Content.Load<SpriteFont>("MenuFont");
            newColor = new Color(255,255,255, 0);

            
                color = new Color(255, 255, 255, 175);
            
        }
        public void Initialize(Player play)
        {
            player = play;

            if (player.color == "Blue")
            {
                Newtext = "Blue player's turn";
                Fontcolor = new Color(10, 100, 255);
            }
            else if (player.color == "Red")
            {
                Newtext = "Red player's turn";
                Fontcolor = new Color(255, 0, 0);
            }
        
        
        }


        public void ShowTopBar()
        {
            if (topPos.Y<0)
                topPos.Y+=2;        
        }
        public void SetTopBar(Player player)
        {
            cash = Convert.ToString(player.money);
        }
        public void HideTopBar()
        {
            if (topPos.Y > -32)
                topPos.Y-=2;        
        }

        
        public void ShowInfoBar()
        {
            if (infoPos.Y > (Game1.screenH-((infoTexture.Height/4)+3)))
                infoPos.Y-=8;
            if (infoPos.Y < (Game1.screenH - infoTexture.Height))
                infoPos.Y += 8;
            infoBar = new Rectangle(0, (int)infoPos.Y, 2000, infoTexture.Height);
        }
        public void SetInfoBar(Soldier unit)
        {
            ad = Convert.ToString(unit.Damage);
            hp = Convert.ToString(unit.Hp);
            arm = Convert.ToString(unit.Armor);       
        
        }
        public void HideInfoBar()
        {
            if (infoPos.Y < Game1.screenH)
                infoPos.Y+=8;
            infoBar = new Rectangle(0, (int)infoPos.Y, 2000, infoTexture.Height);
        }


        public void ShowNewturn()
        {
            if (newColor.A < 240)
            {
                newColor.A += 5;
            }
            if (newColor.A >= 240)
                newBool = false;

            newPos.X = Game1.screenW / 2 - newTexture.Width/2;
            newPos.Y = Game1.screenH / 2 - newTexture.Height/2;

        }
        
        public void HideNewTurn()
        {
            if (newColor.A > 0)
            {
                newColor.A -= 2;
            }

            newPos.X = Game1.screenW / 2 - newTexture.Width/2;
            newPos.Y = Game1.screenH / 2 - newTexture.Height/2;
        }

        
        














        public void Update(GameTime GT)
        {
            if (topBool == true)
                ShowTopBar();
            else
                HideTopBar();

            if (infoBool == true)
                ShowInfoBar();
            else
                HideInfoBar();

            if (newBool==true)
                ShowNewturn();
            else
                HideNewTurn();
                
        
        }


        public void Draw(SpriteBatch SB)
        {
            SB.Draw(topTexture, topPos, color);
            SB.Draw(infoTexture, infoBar, color);
            if (newColor.A > 0)
            {
                SB.Draw(newTexture, newPos, color);
                SB.DrawString(NewFont,Newtext,newPos + new Vector2(100,42),Fontcolor);

            }

            if (infoBool == true)
            {
                SB.Draw(items, new Rectangle((int)infoPos.X + 5, (int)infoPos.Y + 5, 32, 32), hpr, Color.White);
                SB.DrawString(itemfont, hp, new Vector2(infoPos.X + 5 + 35 + 3, infoPos.Y + 10), Color.Green);

                SB.Draw(items, new Rectangle((int)infoPos.X + 105, (int)infoPos.Y + 5, 32, 32),attack, Color.White);
                SB.DrawString(itemfont, ad, new Vector2(infoPos.X + 105 + 35 + 3, infoPos.Y + 10), Color.IndianRed);

                SB.Draw(items, new Rectangle((int)infoPos.X + 205, (int)infoPos.Y + 5, 32, 32), armor, Color.White);
                SB.DrawString(itemfont, arm, new Vector2(infoPos.X + 205 + 35 + 3, infoPos.Y + 10), Color.White);
            }

            if (topBool==true)
            {
                SB.DrawString(itemfont,cash,new Vector2(topPos.X+5,topPos.Y+5),Color.Gold);
            
            }

            

        }




    }
}
