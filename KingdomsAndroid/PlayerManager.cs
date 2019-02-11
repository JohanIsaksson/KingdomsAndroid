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
        public List<Player> Players { get; set; }
        public Player CurrentPlayer { get; set; }
        ContentManager Content;
        int totalPlayers;
        public int CurrentPlayerID { get; set; }
        //public int notplaying { get; set; }
        int MaxU = 25;

        int thiskey;
        int lastkey;

        /// <summary>
        /// skapar x antal spelare beroende på vad man val i menyerna innan
        /// ställer även in vad man valt, enheter
        /// </summary>
        /// <param name="g"></param>
        /// <param name="numPlayers"></param>
        /// <param name="numSoldiers"></param>
        public PlayerManager(Game1 g, int numPlayers, int numSoldiers)
        {
            game = g;
            Players = new List<Player>();
            totalPlayers = numPlayers;
            Content = game.Content;
            CurrentPlayerID = 0;

            for (int i = 0; i < totalPlayers; i++)
            {
                Player p = new Player(game);
                switch (i)
                {
                    case 0:
                        p.Initialize(numSoldiers, "Blue", 0, 0, 0);
                        p.Pstate = Player.state.Waiting;
                        break;
                    case 1:
                        p.Initialize(numSoldiers, "Red", 1, 1, 0);
                        p.Pstate = Player.state.Waiting;
                        break;
                }
                p.Pstate = Player.state.Waiting;
                Players.Add(p);
            }

            CurrentPlayerID = 0;
            CurrentPlayer = Players[CurrentPlayerID];
            CurrentPlayer.IsPlaying = true;
            Players[CurrentPlayerID].NewRound();
            CurrentPlayer.Pstate = Player.state.SelectUnit;
          
            
        }

        /// <summary>
        /// ger nästa spelare tillåtelse att spela och sätter de andra som väntande
        /// </summary>
        public void NewRound()
        {
            // Inactivate current player
            CurrentPlayer.IsPlaying = false;
            CurrentPlayer.Pstate = Player.state.Waiting;

            // Next player
            CurrentPlayerID = (CurrentPlayerID + 1) % totalPlayers;

            // Activate new player
            Players[CurrentPlayerID].IsPlaying = true;
            Players[CurrentPlayerID].NewRound();
            Players[CurrentPlayerID].Pstate = Player.state.SelectUnit;
            CurrentPlayer = Players[CurrentPlayerID];

        }
        

        /// <summary>
        /// uppdaterar spelarna
        /// </summary>
        /// <param name="gt"></param>
        /// <param name="game"></param>
        public void Update(GameTime gt, Game1 game)
        {
            foreach (Player p in Players)                
                p.Update(gt);

        }

        /// <summary>
        /// målar upp spelarna, den som spelar överst
        /// </summary>
        /// <param name="SB"></param>
        public void Draw(SpriteBatch SB)
        {
            if (CurrentPlayerID == 1)
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
                swFile.WriteLine(Players[Play].Name);
                swFile.WriteLine(Players[Play].TeamColor);
                swFile.WriteLine(Players[Play].Team);

                swFile.Dispose();
                swFile.Close();

            }
        
        }












    }
}
