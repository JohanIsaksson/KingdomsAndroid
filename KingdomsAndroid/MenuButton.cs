using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KingdomsAndroid
{
    class MenuButton : TouchButton
    {
        public MenuButton(Game1 g, Vector2 pos) :
            base(g, pos, g.Content.Load<Texture2D>("MenuButton"), new Rectangle(0, 0, 256, 64), g.Content.Load<SpriteFont>("MenuFont"))
        {            
        }
        public MenuButton(Game1 g) :
            base(g, Vector2.Zero, g.Content.Load<Texture2D>("MenuButton"), new Rectangle(0, 0, 256, 64), g.Content.Load<SpriteFont>("MenuFont"))
        {
        }
    }
}
