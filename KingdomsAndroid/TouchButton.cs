using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KingdomsAndroid
{
    class TouchButton
    {
        public Vector2 Position { get; set; }
        private Texture2D texture;
        private Rectangle sourceRectangle;
        private Color overlay;
        private SpriteFont sText;
        public string Text { get; set; }
        public enum ButtonState { Normal, Pressed, Clicked }
        public ButtonState state { get; set; }
        public bool active { get; set; }
        private Game1 game;

        /// <summary>
        /// Creates and initializes a new Touch Button
        /// </summary>
        public TouchButton(Game1 g, Vector2 pos, Texture2D background, Rectangle src, SpriteFont font = null)
        {
            game = g;
            active = true;
            texture = background;
            sourceRectangle = src;
            sText = font;
            Position = pos;
            state = ButtonState.Normal;
        }

        /// <summary>
        /// Updates the button and checks if being touched
        /// </summary>
        public void Update()
        {
            if (active == true)
            {
                Rectangle bounds = new Rectangle((int)this.Position.X,
                                                (int)this.Position.Y,
                                                 this.texture.Width,
                                                 this.texture.Height);
                if (TouchManager.Instance.IsClicked(bounds))
                {
                    state = ButtonState.Clicked;
                }
                else if (TouchManager.Instance.IsPressed(bounds))
                {
                    state = ButtonState.Pressed;
                }
                else
                {
                    state = ButtonState.Normal;
                }
                            
                switch (state)
                {
                    case ButtonState.Pressed:
                        overlay = new Color(255, 255, 255, 160);

                        break;
                    case ButtonState.Normal:
                        overlay = Color.White;

                        break;
                    case ButtonState.Clicked:
                        overlay = new Color(255, 255, 255, 175);

                        break;
                }
            }
            else
                overlay = new Color(255, 255, 255, 100);
        }

        /// <summary>
        /// Draws the button
        /// </summary>
        /// <param name="SB"></param>
        public void Draw(SpriteBatch SB)
        {
            SB.Draw(texture, Position, sourceRectangle, overlay);
            if (sText != null)
                SB.DrawString(sText, Text, new Vector2(Position.X + 8, Position.Y + 4), Color.White);
        }

    }
}
