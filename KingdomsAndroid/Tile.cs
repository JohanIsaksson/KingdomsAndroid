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
    public class Tile
    {
        public Rectangle BackGround { get; set; }
        Rectangle Animation;

        public int Type { get; set; }
        public int Armor { get; set; }
        public int WalkPenalty { get; set; }
        


        public int TileSize
        {
            get { return 32; }
        }
        public Vector2 Position { get; set; }

        public Tile (int type, int x, int y)
        {
            SetTile(type, x, y);
        }
        public void SetTile(int type, int X, int Y)
        {
            Vector2[] TileNumbers =
            {
                new Vector2(0,0), // 00 Grass 1
                new Vector2(0,1), // 01 Grass 2
                new Vector2(3,1), // 02 Tree 1
                new Vector2(6,3), // 03 Tree 2
                new Vector2(4,3), // 04 Rock
                new Vector2(2,0), // 05 Road horizontal
                new Vector2(3,0), // 06 Road vertical
                new Vector2(6,0), // 07 Road ↓→
                new Vector2(7,0), // 08 Road →↑
                new Vector2(4,0), // 09 Road ↑→
                new Vector2(5,0), // 10 Road →↓
                new Vector2(1,1), // 11 Water
                new Vector2(2,1), // 12 Castle neutral
                new Vector2(3,2), // 13 Beach ↓
                new Vector2(2,2), // 14 Beach ↑
                new Vector2(0,2), // 15 Beach ←
                new Vector2(1,2), // 16 Beach →
                new Vector2(1,3), // 17 Beach ↓←
                new Vector2(0,3), // 18 Beach →↓
                new Vector2(2,3), // 19 Beach →↑
                new Vector2(3,3), // 20 Beach ↑←
                new Vector2(0,4), // 21 Beach ↑→
                new Vector2(0,5), // 22 Beach ←↑
                new Vector2(1,4), // 23 Beach ↓→
                new Vector2(1,5), // 24 Beach ←↓
                new Vector2(1,0), // 25 House neutral 
                new Vector2(4,1), // 26 House blue 
                new Vector2(4,2), // 27 Castle blue
                new Vector2(5,1), // 28 House green 
                new Vector2(5,2), // 29 Castle green
                new Vector2(6,1), // 30 House yellow
                new Vector2(6,2), // 31 Castle yellow
                new Vector2(7,1), // 32 House red
                new Vector2(7,2), // 33 Castle red
                new Vector2(5,3), // 34 Road ←↑↓→
                new Vector2(2,4), // 35 Road ←↓→
                new Vector2(3,4), // 36 Road ←↑↓
                new Vector2(4,4), // 37 Road ←↑→
                new Vector2(5,4), // 38 Road ↑↓→
                new Vector2(2,5), // 39 Road end →
                new Vector2(3,5), // 40 Road end ←
                new Vector2(4,5), // 41 Road end ↓
                new Vector2(5,5) // 42 Road end ↑
            };

            Vector2 tilePos = TileNumbers[type];
            BackGround = new Rectangle((int)tilePos.X * TileSize, (int)tilePos.Y * TileSize, TileSize, TileSize);

            /*switch (type)
            {
                case 0: // gräs1
                    BackGround = new Rectangle(0, 0, TileSize, TileSize);
                    break;

                case 1: // gräs2
                    BackGround = new Rectangle(0, 32, TileSize, TileSize);
                    break;

                case 2: // skog1
                    BackGround = new Rectangle(96, 32, TileSize, TileSize);
                    break;

                case 3: // skog2
                    BackGround = new Rectangle(192, 96, TileSize, TileSize);
                    break;

                case 4: // sten
                    BackGround = new Rectangle(128, 96, TileSize, TileSize);
                    break;

                case 5: // väg hori
                    BackGround = new Rectangle(64, 0, TileSize, TileSize);
                    break;

                case 6: // väg vert
                    BackGround = new Rectangle(96, 0, TileSize, TileSize);
                    break;

                case 7: // väg ↓→
                    BackGround = new Rectangle(192, 0, TileSize, TileSize);
                    break;

                case 8: // väg →↑
                    BackGround = new Rectangle(224, 0, TileSize, TileSize);
                    break;

                case 9: // väg ↑→
                    BackGround = new Rectangle(128, 0, TileSize, TileSize);
                    break;

                case 10: // väg →↓
                    BackGround = new Rectangle(160, 0, TileSize, TileSize);
                    break;

                case 11: // vatten1
                    BackGround = new Rectangle(32, 32, TileSize, TileSize);
                    break;

                case 12: //Neutralt slott
                    BackGround = new Rectangle(64, 32, TileSize, TileSize);
                    break;

                case 13: // strand top
                    BackGround = new Rectangle(96, 64, TileSize, TileSize);
                    break;

                case 14: // strand bot
                    BackGround = new Rectangle(64, 64, TileSize, TileSize);
                    break;

                case 15: // strand h
                    BackGround = new Rectangle(0, 64, TileSize, TileSize);
                    break;

                case 16: // strand v
                    BackGround = new Rectangle(32, 64, TileSize, TileSize);
                    break;

                case 17: // strand v bot ut
                    BackGround = new Rectangle(32, 96, TileSize, TileSize);
                    break;

                case 18: // strand h bot ut
                    BackGround = new Rectangle(0, 96, TileSize, TileSize);
                    break;

                case 19: // strand h top ut
                    BackGround = new Rectangle(64, 96, TileSize, TileSize);
                    break;

                case 20: // strand v top ut
                    BackGround = new Rectangle(96, 96, TileSize, TileSize);
                    break;

                case 21: // stran h top in
                    BackGround = new Rectangle(0, 128, TileSize, TileSize);
                    break;

                case 22: // strand v top in
                    BackGround = new Rectangle(0, 160, TileSize, TileSize);
                    break;

                case 23: // strand h bot in
                    BackGround = new Rectangle(32, 128, TileSize, TileSize);
                    break;

                case 24: // strand v bot in
                    BackGround = new Rectangle(32, 160, TileSize, TileSize);
                    break;

                case 25: // n hus
                    BackGround = new Rectangle(32, 0, TileSize, TileSize);
                    break;

                case 26: // blå hus
                    BackGround = new Rectangle(128, 32, TileSize, TileSize);
                    break;

                case 27: // blå slott
                    BackGround = new Rectangle(128, 64, TileSize, TileSize);
                    break;

                case 28: // grön hus
                    BackGround = new Rectangle(160, 32, TileSize, TileSize);
                    break;

                case 29: // grön slott
                    BackGround = new Rectangle(160, 64, TileSize, TileSize);
                    break;

                case 30: // gul hus
                    BackGround = new Rectangle(192, 32, TileSize, TileSize);
                    break;

                case 31: // gul slott
                    BackGround = new Rectangle(192, 64, TileSize, TileSize);
                    break;

                case 32: // röd hus
                    BackGround = new Rectangle(224, 32, TileSize, TileSize);
                    break;
                case 33: // röd slott
                    BackGround = new Rectangle(224, 64, TileSize, TileSize);
                    break;
                case 34: // 4-vägs kors
                    BackGround = new Rectangle(160, 96, TileSize, TileSize);
                    break;
                case 35: // 3-vägs kors ↓
                    BackGround = new Rectangle(64, 128, TileSize, TileSize);
                    break;
                case 36: // 3-vägs kors <-
                    BackGround = new Rectangle(96, 128, TileSize, TileSize);
                    break;
                case 37: // 3-vägs kors ↑
                    BackGround = new Rectangle(128, 128, TileSize, TileSize);
                    break;
                case 38: // 3-väs kors →
                    BackGround = new Rectangle(160, 128, TileSize, TileSize);
                    break;
                case 39: //väg slut →
                    BackGround = new Rectangle(64, 160, TileSize, TileSize);
                    break;
                case 40: //väg slut <-
                    BackGround = new Rectangle(96, 160, TileSize, TileSize);
                    break;
                case 41: //väg slut ↓
                    BackGround = new Rectangle(128, 160, TileSize, TileSize);
                    break;
                case 42: //väg slut ↑
                    BackGround = new Rectangle(160, 160, TileSize, TileSize);
                    break;
                case 43: //n slott
                    BackGround = new Rectangle(64, 32, TileSize, TileSize);
                    break;



                //mer vägar och sånt kommer...
                //osv ↑→↓
            }*/

            Position = new Vector2(X, Y);
            Type = type;
        }

        public void Update()
        {
            // Update animations



        }

    }
}


