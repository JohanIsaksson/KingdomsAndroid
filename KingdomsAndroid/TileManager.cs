using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework.Input.Touch;

namespace KingdomsAndroid
{
    public class TileManager
    {

        Texture2D TileSet;   
        
        public List<Tile> Map { get; set; } 
        public int MapTileHeight { get; set; }
        public int MapTileWidth { get; set; }

        public int TileSize { get; set; }

        public Rectangle MapBounds { get; set; }

        private Game1 game;       
             

        public TileManager(Game1 g)
        {
            game = g;
            TileSet = game.Content.Load<Texture2D>("Graphics4");

            Map = new List<Tile>();
            TileSize = 32;
            LoadPresetMap();
        }

        private void LoadPresetMap()
        {
            int[] preset1 =
            {
                11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,
                11,18,13,13,13,13,13,13,17,11,11,11,11,18,13,13,13,13,17,11,
                11,16,00,02,03,09,05,05,24,17,11,11,18,23,00,00,00,00,15,11,
                11,16,00,04,25,06,00,00,00,15,11,11,16,00,00,00,00,00,15,11,
                11,16,00,00,40,36,00,00,00,15,11,18,23,00,00,00,00,00,15,11,
                11,16,01,25,02,06,00,00,22,20,11,16,00,00,00,00,00,00,15,11,
                11,16,01,40,05,08,00,00,15,11,18,23,00,00,00,00,00,00,15,11,
                11,19,21,00,03,00,00,00,15,11,16,00,00,00,00,00,00,00,15,11,
                11,11,19,14,14,14,14,14,20,11,19,14,14,14,14,14,14,14,20,11,
                11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11
            };

            int[] preset2 =
            {
                11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,
                11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,
                11,11,11,11,11,18,13,13,17,11,11,11,11,11,11,11,11,11,11,11,11,11,11,
                11,11,11,11,18,23,00,02,15,11,11,11,11,11,11,11,11,11,11,11,11,11,11,
                11,11,11,11,16,00,02,00,15,11,11,11,11,11,11,11,11,11,11,11,11,11,11,
                11,11,11,11,16,00,06,00,15,11,11,11,11,11,11,11,11,11,11,11,11,11,11,
                11,11,11,18,23,00,06,00,24,17,11,11,11,11,11,11,11,11,11,11,11,11,11,
                11,11,18,23,04,00,06,01,00,15,11,11,11,11,11,11,11,11,18,13,17,11,11,
                11,11,16,02,02,26,06,00,04,24,17,11,11,11,18,13,13,13,23,25,15,11,11,
                11,11,16,02,00,00,06,02,04,01,24,13,13,13,23,04,04,00,00,02,15,11,11,
                11,11,16,00,27,00,06,02,04,01,00,02,00,00,09,05,05,05,10,00,15,11,11,
                11,11,16,02,09,05,08,04,00,00,02,25,09,05,08,25,01,01,06,02,15,11,11,
                11,11,16,26,06,04,00,00,02,00,09,05,08,00,02,00,00,04,06,32,15,11,11,
                11,11,16,02,06,01,01,25,09,05,08,25,02,00,00,04,09,05,08,02,15,11,11,
                11,11,16,00,07,05,05,05,08,00,00,02,00,01,04,02,06,00,33,00,15,11,11,
                11,11,16,02,00,00,04,04,22,14,14,14,21,01,04,02,06,00,00,02,15,11,11,
                11,11,16,25,22,14,14,14,20,11,11,11,19,21,04,00,06,32,02,02,15,11,11,
                11,11,19,14,20,11,11,11,11,11,11,11,11,16,00,01,06,00,04,22,20,11,11,
                11,11,11,11,11,11,11,11,11,11,11,11,11,19,21,00,06,00,22,20,11,11,11,
                11,11,11,11,11,11,11,11,11,11,11,11,11,11,16,00,06,00,15,11,11,11,11,
                11,11,11,11,11,11,11,11,11,11,11,11,11,11,16,00,02,00,15,11,11,11,11,
                11,11,11,11,11,11,11,11,11,11,11,11,11,11,16,02,00,22,20,11,11,11,11,
                11,11,11,11,11,11,11,11,11,11,11,11,11,11,19,14,14,20,11,11,11,11,11,
                11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,
                11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11
            };

            MapTileHeight = 25;
            MapTileWidth = 23;
            for (int y = 0; y < MapTileHeight; y++)
            {
                for (int x = 0; x < MapTileWidth; x++)
                {                
                    Map.Add(new Tile(preset2[y*MapTileWidth + x], x, y));
                }
            }

            MapBounds = new Rectangle(0, 0, MapTileWidth * TileSize, MapTileHeight * TileSize);
        }
        

        public void LoadMap(string mapname)
        {
            StreamReader srFile = new StreamReader(game.Content.RootDirectory + "/Maps/" + mapname);
            string strLine = "";            
            int x = 0;
            int y = 0;

            try
            {
                do
                {
                    strLine = srFile.ReadLine();
                    string[] splitThis = Regex.Split(strLine.Substring(0, strLine.Length - 1), ",");
                    MapTileWidth = splitThis.Length;
                    for (x = 0; x < MapTileWidth; x++)
                    {
                        if (splitThis[x] == "")
                            Map[y*MapTileHeight + x].SetTile(0, x, y);
                        else if (x <= 25)
                            Map[y*MapTileHeight + x].SetTile(Convert.ToInt32(splitThis[x]), x, y);
                    }

                    x = 0;
                    y += 1;

                } while (!srFile.EndOfStream);
                MapTileHeight = y;
            }
            catch
            { 
            
            
            }

            srFile.Close();
            srFile.Dispose();

        }


        public void SaveMap(string mapname)
        {
        
        }

        public Tile tileAt(Vector2 pos)
        {
            foreach (Tile t in Map)
            {
                if (t.Position == pos)
                {
                    return t;
                }
            }
            return null;
        }
       
        public void Update()
        {
            //MoveCam();

            // Update only the visible tiles
            for (int y = 0; y < MapTileHeight; y++)
            {
                for (int x = 0; x < MapTileWidth; x++)
                {
                    Map[y * MapTileWidth + x].Update();
                }
            }
        }


        public void Draw(SpriteBatch SB)
        {
            int size = 32;
            for (int x = 0; x < MapTileWidth; x++)
            {
                for (int y = 0; y < MapTileHeight; y++)
                {
                    SB.Draw(TileSet, new Rectangle(x * size, y * size, size, size), Map[y*MapTileWidth + x].BackGround, Color.White);                    
                }
            }
        }








    }
}
