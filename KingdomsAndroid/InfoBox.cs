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
    class InfoBox
    {
        Texture2D box;
        Vector2 boxpos;

        Rectangle textbox;        
        List<string> lines;
        
        int linewidth;
        SpriteFont font;
        
        
        StreamReader reader;

        public InfoBox(Game1 game, Vector2 pos)
        {
            box = game.Content.Load<Texture2D>("InfoBox");
            boxpos = pos;

            font = game.Content.Load<SpriteFont>("UnitFont");

            textbox = new Rectangle((int)boxpos.X + 16, (int)boxpos.Y + 8, box.Width - 32, box.Height - 32);

            lines = new List<string>();

           

        }


        public void LoadInfo(string file)
        {
            lines = new List<string>();
            

            try
            {
                reader = new StreamReader("Units\\" + file);
                

                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                                   
                
                }


            }
            catch
            {            
            
            
            }

            lines[6]=parseText(lines[6]);
        
        }

        private String parseText(String text)
        {
            String line = String.Empty;
            String returnString = String.Empty;
            String[] wordArray = text.Split(' ');

            foreach (String word in wordArray)
            {
                if (font.MeasureString(line + word).Length() > textbox.Width)
                {
                    returnString = returnString + line + '\n';
                    line = String.Empty;
                }

                line = line + word + ' ';
            }

            return returnString + line;
        }


        public void Update()
        { 
        
        
        
        
        
        }

        public void Draw(SpriteBatch SB)
        {
            SB.Draw(box,boxpos,new Color(255,255,255,130));

            int a=0;
            foreach (string lin in lines)
            {
                SB.DrawString(font, lin, new Vector2(textbox.X, textbox.Y+ (a * 20)), Color.White);
                a++;
            
            }
        
        
        
        }


    }
}
