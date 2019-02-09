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
    class PictureButton// : TouchButton
    {
        
        public Texture2D image {get;set;}
        public Texture2D background {get;set;}

        public Rectangle sour_img{get;set;}
        public Rectangle dest_img{get;set;}

        public Vector2 backpos{get;set;}
        public Vector2 imgpos{get;set;}

        Color transparency;
        public enum State { marked, normal }
        public State state { get; set; }

        /*public PictureButton (Game1 g, Vector2 pos, Texture2D bg, SpriteFont sf) :
            base(g, pos, bg, sf)
        {

        }*/
        Game1 game;
        public PictureButton(Game1 g)
        {
            game = g;
        }

        public void Initialize(Vector2 pos)
        {
            backpos = pos;
            imgpos = backpos + new Vector2(16, 16);
            dest_img = new Rectangle((int)imgpos.X, (int)imgpos.Y, 32, 32);
        }


        public void Update()
        {
            Rectangle bounds = new Rectangle((int)backpos.X, (int)backpos.Y, background.Width, background.Height);

            TouchManager.Instance.IsClicked(bounds);
            if (TouchManager.Instance.IsClicked(bounds))
            {
                game.Playermanager.player[game.Playermanager.playing].shop.UnMarkAll();
                state = State.marked;
            }
        
            if (state == State.marked)
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
