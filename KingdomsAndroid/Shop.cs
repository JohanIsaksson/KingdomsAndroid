using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace KingdomsAndroid
{
    /// <summary>
    /// 
    /// </summary>
    public class Shop
    {

        Texture2D background;
        Vector2 backpos;

        List<PictureBox> boxes;
        Texture2D unitset;
        int color;

        MenuButton purchase;
        MenuButton cancel;

        Vector2 musPos;
        Texture2D mouse;

        InfoBox infobox;






        Game1 game;
        Player player;
        int gold=0;
        int cost=99999;
        int units;
        bool king;


        public Shop(Player Pplayer,Game1 game1)        
        {
            player = Pplayer;
            boxes = new List<PictureBox>();
            game = game1;

            background = game.Content.Load<Texture2D>("NewGame");
            backpos = new Vector2((game.graphics.PreferredBackBufferWidth / 2) - (background.Width / 2), (game.graphics.PreferredBackBufferHeight / 2) - (background.Height / 2));

            unitset = game.Content.Load<Texture2D>("units2");

            mouse = game.Content.Load<Texture2D>("mus2");

            infobox = new InfoBox(game, new Vector2(backpos.X+32,backpos.Y+180));
        }

        public void Initialize(Player pplayer)
        {
            player=pplayer;
            

            if (player.color=="Blue")
                color=0;
            else if (player.color=="Red")
                color =1;

            purchase = new MenuButton(game);
            purchase.Initialize(new Vector2(backpos.X + (background.Width - 288), (background.Height - 64 - 8)));
            purchase.Text = " Purchase";

            cancel = new MenuButton(game);
            cancel.Initialize(new Vector2((backpos.X + 32), (background.Height - 64 - 8)));
            cancel.Text = "   Cancel";



            for (int box = 0; box <= 5; box++)
            {
                boxes.Add(new PictureBox());
                boxes[box].marked = false;
                boxes[box].background = game.Content.Load<Texture2D>("PicBox");
                boxes[box].Initialize(backpos + new Vector2(64, 64) + new Vector2(box * 64, 0) + new Vector2(box*24, 0));
                boxes[box].image = unitset;
                boxes[box].sour_img=new Rectangle(box*32,color*32,32,32);                
            }

            purchase.active = false;
            //boxes[0].marked = true;
            //boxes[0].background = game.Content.Load<Texture2D>("PicBox3");
                        
        }

        public void Gold(Player play)
        {
            gold = play.money;        
        
        }


        public void Update(GameTime GT,Player pplayer)
        {
            KeyboardState key = Keyboard.GetState();
            if (key.IsKeyDown(Keys.Escape) == true)
                player.Pstate = Player.state.SelectUnit;

            MouseState mus = Mouse.GetState();
            musPos = new Vector2(mus.X, mus.Y);


            int typ = 1;
            foreach (PictureBox box in boxes)
            {                
                if (box.state == PictureBox.BState.hover && mus.LeftButton == ButtonState.Pressed)
                {
                    foreach (PictureBox boxe in boxes)
                    {
                        boxe.marked = false;
                        boxe.background = game.Content.Load<Texture2D>("PicBox");                    
                    }
                    box.marked = true;
                    box.background = game.Content.Load<Texture2D>("PicBox3");
                    break;                    
                }
                
            typ++;

            box.Update();
            }

            switch (typ)
            { 
                case 1:
                    cost = 400;
                    infobox.LoadInfo("King.txt");
                    break;

                case 2:
                    cost = 200;
                    infobox.LoadInfo("Swordman.txt");
                    break;

                case 3:
                    cost = 250;
                    infobox.LoadInfo("Archer.txt");
                    break;

                case 4:
                    cost =600;
                    infobox.LoadInfo("Shieldman.txt");
                    break;

                case 5:
                    cost = 700;
                    infobox.LoadInfo("Catapult.txt");
                    break;

                case 6:
                    cost = 1000;
                    infobox.LoadInfo("Cavalry.txt");
                    break;

                    default:
                    cost = 99999;
                    break;
            
            }

            if (cost > gold)
                purchase.active = false;
            else
                purchase.active = true;
                

            purchase.Update();
            cancel.Update();

            if (cancel.state == MenuButton.ButtonState.Pressed)
            {
                player.Pstate = Player.state.SelectUnit;
                player.HideCastleMenu();           
            }
            else if (purchase.state == MenuButton.ButtonState.Pressed)
            { 
                int a=0;
                foreach (PictureBox box in boxes)
                {
                    if (box.marked==true)
                        break;
                    a++;               
                
                }
                gold -= cost;
                player.money = gold;
                player.hud.SetTopBar(player);
                player.NewUnit(a+1);
                player.Pstate = Player.state.SelectUnit;
                
            }

        }



        public void Draw(SpriteBatch SB)
        {
            SB.Draw(background, backpos, new Color(255, 255, 255, 185));



            foreach (PictureBox box in boxes)
                box.Draw(SB);

            purchase.Draw(SB);
            cancel.Draw(SB);

            infobox.Draw(SB);


            SB.Draw(mouse, musPos, Color.White);
        }

        
    }
}
