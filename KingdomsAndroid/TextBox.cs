using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KingdomsAndroid
{
    class TextBox
    {
        Texture2D Back;
        Rectangle Rect;
        public Vector2 Pos { get; set; }
        SpriteFont font;
        Color color;

        KeyboardState oldkey;
        public string Text { get; set; }       

        public bool Writing { get; set; }

        


        public TextBox(Game1 game,Vector2 Bpos,Rectangle Brect)
        {
            Pos = Bpos;
            Back = game.Content.Load<Texture2D>("TextBox");
            Rect = new Rectangle((int)Bpos.X, (int)Bpos.Y, Brect.Width, Brect.Height);
            font = game.Content.Load<SpriteFont>("UnitFont");

            color = new Color(100,100,100);
            Text = "";
        
        }

        public void Update(GameTime GT)
        {
            Rect = new Rectangle((int)Pos.X, (int)Pos.Y, Rect.Width, Rect.Height);
            
            MouseState mus = Mouse.GetState();

            if (mus.X >= Rect.Left && mus.X <= Rect.Right && mus.Y >= Rect.Top && mus.Y <= Rect.Bottom)
            {
                if (mus.LeftButton == ButtonState.Pressed)
                {
                    Writing = true;
                    color = new Color(0,0,0);
                }
            }
            else
            {
                if (mus.LeftButton == ButtonState.Pressed)
                {
                    Writing = false;
                    color = new Color(100,100,100);
                }
            }
            


            if (Writing == true)
            {
                Input();
            }
            else
            {
            }
            
        }

        public void Input()
        {
            
            KeyboardState keystate = Keyboard.GetState();

            foreach (Keys key in keystate.GetPressedKeys())
            { 
                if (oldkey.IsKeyUp(key))
                {
                    if (key == Keys.Back)
                    {
                        if (Text.Length >= 1)
                            Text = Text.Remove(Text.Length - 1, 1);
                    }
                    else if (key == Keys.Space)
                        Text += " ";
                    else
                    {
                        if (key.ToString().Length == 1)
                        {
                            if (Text.Length == 0)
                                Text += key.ToString();
                            else
                                Text += key.ToString().ToLower();
                        }
                        
                    }
                }
            
            
            
            
            }

            oldkey = keystate;        
        }


        public void Draw(SpriteBatch SB)
        {
            SB.Draw(Back, Rect, Color.White);
            SB.DrawString(font, Text, new Vector2(Pos.X + 4, Pos.Y + 4), color);
        
        }
    }

    
}
