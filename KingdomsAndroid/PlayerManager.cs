using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.IO;

//klass som håller koll på alla spelare
namespace KingdomsAndroid
{
    public class PlayerManager
    {
        Game1 game;
        public Player[] Players { get; set; }
        ContentManager Content;
        int totalplayers;
        public int playing { get; set; }
        public int notplaying { get; set; }
        int MaxU = 25;

        int thiskey;
        int lastkey;

        /// <summary>
        /// skapar x antal spelare beroende på vad man val i menyerna innan
        /// ställer även in vad man valt, enheter
        /// </summary>
        /// <param name="Pgame"></param>
        /// <param name="players"></param>
        /// <param name="MaxU"></param>
        public PlayerManager(Game1 Pgame, int players, int MaxU)
        {
            game = Pgame;
            Players = new Player[players];
            totalplayers = players;
            Content = game.Content;
            playing = 0;
            
            NewGame();     
            
            
        }

        /// <summary>
        /// skapar ett nytt game
        /// </summary>
        public void NewGame()
        {

            for (int Play = 0; Play < totalplayers; Play++)
            {
                Players[Play] = new Player(game);
                switch (Play)
                {
                    case 0:
                        Players[Play].Initialize(MaxU, "Blue", 0,0);
                        Players[Play].Pstate = Player.state.Waiting;
                        break;
                    case 1:
                        Players[Play].Initialize(MaxU, "Red", 1,0);
                        Players[Play].Pstate = Player.state.Waiting;
                        break;
                    //case 2:
                    //    player[Play].Initialize(MaxU, "Green", 2);
                    //    player[Play].Pstate = Player.state.Waiting;
                    //    break;
                    //case 3:
                    //    player[Play].Initialize(MaxU, "Yellow", 3);
                    //    player[Play].Pstate = Player.state.Waiting;
                    //    break;
                }
                Players[Play].Pstate = Player.state.Waiting;
            }


            playing = 0;
            notplaying = 1;
            Players[playing].IsPlaying = true;
            Players[playing].Pstate = Player.state.SelectUnit;
        }

        /// <summary>
        /// ger nästa spelare tillåtelse att spela och sätter de andra som väntande
        /// </summary>
        public void NewRound()
        {
            
            Players[playing].IsPlaying = false;
            Players[playing].Pstate = Player.state.Waiting;
            notplaying = playing;            

            if (playing < 1)
            {
            playing++;
            Players[playing].IsPlaying = true;
            Players[playing].NewRound();
            Players[playing].Pstate = Player.state.SelectUnit;
            }
            else
            {
                playing = 0;
                Players[playing].IsPlaying = true;
                Players[playing].NewRound();
                Players[playing].Pstate = Player.state.SelectUnit;
            }
        }
        

        /// <summary>
        /// uppdaterar spelarna
        /// </summary>
        /// <param name="GT"></param>
        /// <param name="game"></param>
        public void Update(GameTime GT, Game1 game)
        {
            for (int Play = 0; Play < totalplayers; Play++)                
                Players[Play].Update(GT,game);

            KeyboardState key = Keyboard.GetState();
            
            if (key.IsKeyDown(Keys.N) == true)
                thiskey = 1;
            else
                thiskey = 0;

            if (thiskey == 1 && thiskey != lastkey)
            {
                Players[playing].EndRound();
                NewRound();
            }

            lastkey = thiskey;
        }

        /// <summary>
        /// målar upp spelarna, den som spelar överst
        /// </summary>
        /// <param name="SB"></param>
        public void Draw(SpriteBatch SB)
        {
            if (playing == 1)
            {
                for (int Play = 0; Play < 2; Play++)
                    Players[Play].Draw(SB);
            }
            else
            {
                for (int Play = 1; Play >=0; Play--)
                    Players[Play].Draw(SB);
            }
        }


        /// <summary>
        /// Laddar spel, hämtar info om soldater, inställning osv
        /// </summary>
        /// <param name="path"></param>
        public void LoadGame(string path)
        { 
        
        
        }

        /// <summary>
        /// skriver ner info om player - lag,soldater,pengar
        /// </summary>
        /// <param name="path"></param>
        public void SaveGame(string path)
        {
            for (int Play=0; Play <= 3; Play++)
            {
                StreamWriter swFile = new StreamWriter(path + Convert.ToString(Play) + ".txt");
                swFile.WriteLine(Players[Play].name);
                swFile.WriteLine(Players[Play].color);
                swFile.WriteLine(Players[Play].team);

                swFile.Dispose();
                swFile.Close();

            }
        
        }












    }
}
