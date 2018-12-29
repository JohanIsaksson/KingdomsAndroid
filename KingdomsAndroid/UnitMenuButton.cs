using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace KingdomsAndroid
{
    class UnitMenuButton
    {

        Vector2 Pos;
        Texture2D Bild;
        Color Hover;
        SpriteFont sText;
        public string Text { get; set; }
        public enum BState { hover, pressed, normal, disabled }
        public BState state { get; set; }
        public bool active { get; set; }


        public UnitMenuButton(ContentManager Content)
        {
            Bild = Content.Load<Texture2D>("InGameButton");
            sText = Content.Load<SpriteFont>("UnitFont");
        }

        public void Initialize(Vector2 Bpos)
        {
            Pos = Bpos;
            state = BState.normal;
            active = true;
        }

        public void Update()
        {
            MouseState mus = Mouse.GetState();

            if (active == true)
            {

                if (mus.X >= Pos.X && mus.X <= (Pos.X + Bild.Width) && mus.Y >= Pos.Y && mus.Y <= (Pos.Y + Bild.Height))
                {
                    state = BState.hover;
                    if (mus.LeftButton == ButtonState.Pressed)
                        state = BState.pressed;
                }
                else
                    state = BState.normal;


                switch (state)
                {
                    case BState.hover:
                        Hover = new Color(240, 240, 240, 160);

                        break;
                    case BState.normal:
                        Hover = new Color(155, 155, 155, 185);

                        break;
                    case BState.pressed:
                        Hover = new Color(200, 200, 200, 150);

                        break;
                }
            }
            else
                Hover = new Color(100, 100, 100, 100);
        }

        public void Draw(SpriteBatch SB)
        {
            if (active == true)
            {
                SB.Draw(Bild, Pos, Hover);
                SB.DrawString(sText, Text, new Vector2(Pos.X + 5, Pos.Y + 5), Color.White);
            }
            else
            {
                SB.Draw(Bild, Pos, new Color(150, 150, 150, 150));
                SB.DrawString(sText, Text, new Vector2(Pos.X + 5, Pos.Y + 5), Color.DarkGray);

            }

        }




    }
}

