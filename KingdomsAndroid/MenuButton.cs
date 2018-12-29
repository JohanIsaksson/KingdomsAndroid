using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input.Touch;

namespace KingdomsAndroid
{
    
    class MenuButton
    {

        public Vector2 Position { get; set; }
        Texture2D background;
        Color Hover;
        SpriteFont sText;
        public string Text { get; set; }
        public enum ButtonState { Clicked, Pressed, Normal }
        public ButtonState state { get; set; }
        public bool active { get; set; }
        Game1 game;

        /// <summary>
        /// skapar en ny knapp med textur och text
        /// </summary>
        public MenuButton(Game1 Pgame)
        {
            game = Pgame;
            active = true;
            background = game.Content.Load<Texture2D>("MenuButton");
            sText = game.Content.Load<SpriteFont>("MenuFont");
        }

        /// <summary>
        /// säger till var knappen ska vara och i vilket state
        /// </summary>
        /// <param name="Bpos"></param>
        public void Initialize(Vector2 Bpos)
        {            
            Position = Bpos;            
            state = ButtonState.Normal;             
        }

        bool IsInBounds(Vector2 pos)
        {
            Camera cam = this.game.Camera;
            var transformedPos = (pos + cam.Position)/cam.Zoom;
            //var inverseTransform = Matrix.Invert(cam.Transform);
            //var transformedPos = Vector2.Transform(pos, cam.Transform);
            if ((transformedPos.X >= this.Position.X) && 
                (transformedPos.Y >= this.Position.Y) && 
                (transformedPos.X < (this.Position.X + this.background.Width)) && 
                (transformedPos.Y < (this.Position.Y + this.background.Height)))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// uppdaterar var man håller musen och kollar om man hovrar knappen
        /// kollar sedan om man klickar på en knappen
        /// </summary>
        public void Update()
        {
            //MouseState mus = Mouse.GetState();
            TouchCollection touchCollection = TouchPanel.GetState();

            if (touchCollection.Count == 0)
            {
                if (state == ButtonState.Pressed)
                {
                    state = ButtonState.Clicked;
                }
                else
                {
                    state = ButtonState.Normal;
                }
            }
            else
            {
                bool touchFound = false;
                foreach (TouchLocation touch in touchCollection)
                {
                    if (!IsInBounds(touch.Position))
                        continue;

                    touchFound = true;
                    if ((touch.State == TouchLocationState.Pressed) || (touch.State == TouchLocationState.Moved))
                    {
                        state = ButtonState.Pressed;
                    }
                    else if (touch.State == TouchLocationState.Released)
                    {
                        TouchLocation prevLocation;

                        // Sometimes TryGetPreviousLocation can fail. Bail out early if this happened
                        // or if the last state didn't move
                        if (touch.TryGetPreviousLocation(out prevLocation))
                        {
                            // get your delta
                            var delta = touch.Position - prevLocation.Position;

                            // Allow some errors
                            if (delta.LengthSquared() > 2)
                                continue;
                        }

                        // Everything is fine
                        state = ButtonState.Clicked;
                    }
                }
                if (!touchFound)
                    state = ButtonState.Normal;
            }

            if (active == true)
            {
                switch (state)
                {
                    case ButtonState.Pressed:
                        Hover = new Color(240, 240, 240, 160);

                        break;
                    case ButtonState.Normal:
                        Hover = new Color(155, 155, 155, 185);

                        break;
                    case ButtonState.Clicked:
                        Hover = new Color(200, 200, 200, 175);

                        break;
                }
            }
            else
                Hover = new Color(100, 100, 100, 100);
        }

        /// <summary>
        /// målar upp knappen och texten
        /// </summary>
        /// <param name="SB"></param>
        public void Draw(SpriteBatch SB)
        {
            SB.Draw(background, Position, Hover);
            SB.DrawString(sText, Text, new Vector2(Position.X + 32, Position.Y + 16), Color.White);
        }

        


    }
}
