using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KingdomsAndroid
{

    class Marker
    {

        Texture2D background;
        Rectangle source;
        Rectangle dest;

        int state;
        int elapsedTime;
        int animationSpeed;

        public Vector2 Position { get; set; }

        Game1 game;

        public Marker(Game1 g, int speed)
        {
            game = g;
            animationSpeed = speed;
            background = game.Content.Load<Texture2D>("TileMarker");

        }

        public void Update(GameTime gt)
        {

            // Update animation
            int[] Xs =
            {
                0,
                0,
                0,
                0,
                1,
                1,
                1,
                1,
                2,
                2,
                2,
                2,
                3,
                3,
                3
            };
            int[] Ys =
            {
                0,
                1,
                2,
                3,
                0,
                1,
                2,
                3,
                0,
                1,
                2,
                3,
                0,
                1,
                2
            };

            elapsedTime += gt.ElapsedGameTime.Milliseconds;

            if (elapsedTime > animationSpeed)
            {
                state = (state + 1) % 15;
                elapsedTime -= animationSpeed;
            }

            // Set source rectangle
            source = new Rectangle(Xs[state] * 32, Ys[state] * 32, 32, 32);

            // Set destination rectangle
            dest = new Rectangle((int)Position.X, (int)Position.Y, 32, 32);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(background, dest, source, new Color(255,255,255,150));
        }

    }
}