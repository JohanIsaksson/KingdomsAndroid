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
    class UnitMenuButton : TouchButton
    {

        public UnitMenuButton(Game1 g, Vector2 pos) :
            base(g, pos, g.Content.Load<Texture2D>("InGameButton"), new Rectangle(0,0,96,32), g.Content.Load<SpriteFont>("UnitFont"))
        {
        }
        public UnitMenuButton(Game1 g) :
            base(g, Vector2.Zero, g.Content.Load<Texture2D>("InGameButton"), new Rectangle(0, 0, 96, 32), g.Content.Load<SpriteFont>("UnitFont"))
        {
        }
    }
}

