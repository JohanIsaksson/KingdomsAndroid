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
        public Viewport Viewport { get; set; }
        

        Vector2 CamPos;
        string thiskey;
        string lastkey;

        
             

        Random rand; //tillfällig



        public TileManager(ContentManager Content)
        {
            TileSet = Content.Load<Texture2D>("Graphics4");
            MapTileHeight = 64;
            MapTileWidth = 64;
            TileSize = 32;
            Viewport = new Viewport(0, 0, MapTileWidth * TileSize, MapTileHeight * TileSize, 0, 1);
        }

        public void initialize()
        {           

            CamPos = new Vector2(0, 0);
            rand = new Random();
            Map = new List<Tile>();

            for (int x = 0; x < MapTileWidth; x++)
            {
                for (int y = 0; y < MapTileHeight; y++)
                {
                    Map.Add(new Tile(rand.Next(0, 42), x, y));
                }
            }
            //Map[10*MapTileHeight + 10].SetTile(27, 10, 10);
         
        }
        

        public void LoadMap(string mapname)
        {
            return;
            StreamReader srFile = new StreamReader("Maps/" + mapname);
            string strLine = "";            
            int x = 0;
            int y = 0;

            try
            {
                do
                {
                    strLine = srFile.ReadLine();
                    string[] splitThis = Regex.Split(strLine.Substring(0, strLine.Length - 1), ",");

                    for (x = 0; x < MapTileWidth; x++)
                    {
                        if (splitThis[x] == "")
                            Map[y*MapTileHeight + x].SetTile(0, x, y);
                        else if (x <= 25)
                            Map[y*MapTileHeight + x].SetTile(Convert.ToInt32(splitThis[x]), x, y);
                    }

                    x = 0;
                    y += 1;

                } while (!srFile.EndOfStream && y < MapTileHeight);
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
        

        public void MoveCam()
        {
            Vector2 cameraMovement = Vector2.Zero;

            TouchCollection touchCollection = TouchPanel.GetState();

            foreach (TouchLocation touch in touchCollection)
            {

                if ((touch.State == TouchLocationState.Moved)/* || (touch.State == TouchLocationState.Pressed)*/)
                {
                    TouchLocation prevLocation;

                    // Sometimes TryGetPreviousLocation can fail. Bail out early if this happened
                    // or if the last state didn't move
                    if (touch.TryGetPreviousLocation(out prevLocation))
                    {
                        // get your delta
                        var delta = touch.Position - prevLocation.Position;

                        //CamPos = delta;
                    }
                    break;
                    // Everything is fine
                    //state = ButtonState.Clicked;
                }
            }


            /*KeyboardState key = Keyboard.GetState();


            if (key.IsKeyDown(Keys.Up) == true && CamPos.Y > 0)
                thiskey = "Up";
            else if (key.IsKeyDown(Keys.Down) == true && (CamPos.Y + viewH) < 31)
                thiskey = "Down";
            else if (key.IsKeyDown(Keys.Left) == true && CamPos.X > 0)
                thiskey = "Left";
            else if (key.IsKeyDown(Keys.Right) == true && (CamPos.X + viewW) < 31)
                thiskey = "Right";
            else if (key.IsKeyDown(Keys.C) == true)
                thiskey = "Castle";
            else
                thiskey = "";


            if (thiskey == "Up")
                CamPos.Y -= 1;
            if (thiskey == "Down")
                CamPos.Y += 1;
            if (thiskey == "Left")
                CamPos.X -= 1;
            if (thiskey=="Right")
                CamPos.X += 1;
            if (thiskey == "Castle" && thiskey != lastkey)            
                Map[10, 10].SetTile(27, 10, 10);

            
            lastkey = thiskey;*/

        }


        public void Update()
        {
            MoveCam();

            // Update only the visible tiles
            for (int x = 0; x < MapTileWidth; x++)
            {
                for (int y = 0; y < MapTileHeight; y++)
                {
                    Map[y * MapTileHeight + x].Update();
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
                    SB.Draw(TileSet, new Rectangle(x * size, y * size, size, size), Map[y*MapTileHeight + x].BackGround, Color.White);                    
                }
            }
        }








    }
}
