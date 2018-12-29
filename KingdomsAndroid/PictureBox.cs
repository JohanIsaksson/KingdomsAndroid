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

namespace KingdomsAndroid
{
    public class PictureBox
    {
        
        public Texture2D image {get;set;}
        public Texture2D background {get;set;}

        public Rectangle sour_img{get;set;}
        public Rectangle dest_img{get;set;}

        public Vector2 backpos{get;set;}
        public Vector2 imgpos{get;set;}

        Color transparency;
        public bool marked { get; set; }
        public enum BState { hover, normal }
        public BState state { get; set; }

        

        public void Initialize(Vector2 pos)
        {
            backpos = pos;
            imgpos = backpos + new Vector2(16, 16);
            dest_img = new Rectangle((int)imgpos.X, (int)imgpos.Y, 32, 32);
            marked = false;
        }


        public void Update()
        {
            MouseState mus = Mouse.GetState();

            if (mus.X >= backpos.X && mus.X <= (backpos.X + background.Width) && mus.Y >= backpos.Y && mus.Y <= (backpos.Y + background.Height))
                state = BState.hover;  
            else
                state = BState.normal;

        
            if (state==BState.hover)
                transparency = new Color(240, 240, 240, 160);
            else
                transparency = new Color(155, 155, 155, 185);   
        
        
        }


        public void Draw(SpriteBatch SB)
        {
            SB.Draw(background, backpos, transparency);
            SB.Draw(image, dest_img, sour_img, Color.White);     
        
        
        }
    }
}
