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
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace KingdomsAndroid
{
    public class MapList
    {
        Texture2D box;
        Vector2 boxpos;

        List<ListItem> listitems;

        Game1 game;

        string text;
        public string map { get; set; }


        public MapList(Game1 game1,Vector2 pos)
        {
            game = game1;
            box = game.Content.Load<Texture2D>("InfoBox");
            boxpos = pos;
            listitems = new List<ListItem>();
        
        }


        public void LoadInfo(string path)
        {
            listitems= new List<ListItem>();

            try
            {
                int a=0;
                foreach (string d in Directory.GetFiles(path))
                {
                    text=d.Substring(5,6);
                    listitems.Add(new ListItem(game,new Vector2(boxpos.X,boxpos.Y+5+(a*32)),new Rectangle(0,0,box.Width,32),text));
                    
                    a++;
                }
            }
            catch (System.Exception excpt)
            {
                
            }
        }


        public void Update()
        {

            MouseState mus = Mouse.GetState();

            

            
            foreach (ListItem sak in listitems)
            {
                if (sak.state == ListItem.BState.hover && mus.LeftButton == ButtonState.Pressed)
                {
                    foreach (ListItem saken in listitems)
                        saken.marked = false;


                    sak.marked = true;
                    map = sak.text;
                }






                sak.ChangePos(mus.ScrollWheelValue);
                sak.Update();                
            }

            
            

        }


        public void Draw(SpriteBatch SB)
        {

            SB.Draw(box,boxpos,new Color(255,255,255,130));
            
            foreach (ListItem sak in listitems)
            { 
            sak.Draw(SB);
            
            }

            
            
        }    
        

    }
}
