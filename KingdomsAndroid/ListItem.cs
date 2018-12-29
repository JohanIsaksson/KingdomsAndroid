using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KingdomsAndroid
{
    public class ListItem
    {
        Texture2D back;
        Rectangle rect;
        public string text { get; set; }
        SpriteFont font;

        Color transparency;
        public bool marked { get; set; }
        public enum BState { hover, normal }
        public BState state { get; set; }

        int yplus;

        public ListItem(Game1 game,Vector2 pos,Rectangle size,string t)
        {
            back = game.Content.Load<Texture2D>("maprect");
            font = game.Content.Load<SpriteFont>("UnitFont");
            rect = new Rectangle((int)pos.X, (int)pos.Y, size.Width, size.Height);
            text = t;
            marked = false;

        
        }

        public void ChangePos(int value)
        {                       
            {
                yplus = value/100;
                
            }        
        }

        

        public void Update()
        {
            MouseState mus = Mouse.GetState();

            if (mus.X >= rect.X && mus.X <= (rect.X + rect.Width) && mus.Y >= rect.Y && mus.Y <= (rect.Y + rect.Height))
                state = BState.hover;
            else
                state = BState.normal;

            

            if (marked == false)
            {
                if (state == BState.hover)
                    transparency = new Color(210, 210, 210, 160);
                else
                    transparency = new Color(155, 155, 155, 185);
            }
            else
                transparency = new Color(255, 255, 255, 185);
        }

        public void Draw(SpriteBatch SB)
        {
            SB.Draw(back, new Rectangle(rect.X,rect.Y+yplus,rect.Width,rect.Height), new Rectangle(0, 0, 5, 5), transparency);
            SB.DrawString(font, text, new Vector2(rect.X + 32, rect.Y + 8+yplus), Color.White);
        
        }

    }
}
