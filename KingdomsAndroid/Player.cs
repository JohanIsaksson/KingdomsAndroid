using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

//denna klass styr det mesta som en spelare gör

namespace KingdomsAndroid
{
    public class Player
    {
        Game1 game;
        
        public List<Soldier> Soldiers { get; set; }       
        Texture2D unitset;
        
        public int ID { get; set; }
        public string Name { get; set; }
        public string TeamColor { get; set; }
        public int Team { get; set; }
        public bool IsPlaying { get; set; }

        public int castletype { get; set; }
        public List<Vector2> castlepositions { get; set; }
        public Vector2 castlepos { get; set; }
        int castles;
        public int housetype { get; set; }
        int houses;
        public int money { get; set; }
        int income;
        

        int MaxUnits;
        public int currentUnit { get; set; }
        
        int moveSpeed;
        
        int tid;
        Vector2 newpos;
        public bool end { get; set; }

        Texture2D hit;
        Rectangle hitrect;
        Player victim;
        Soldier victimSoldier;

        public UnitMenu unitmenu { get; set; }
        public CastleMenu castlemenu { get; set; }
        public Shop shop { get; set; }

        public enum state { SelectUnit, UnitMenu, SelectMove, Moving, SelectFight, Fighting, Shop, Waiting,End }
        public state Pstate { get; set; }
        

        Texture2D musB;
        Vector2 tileHover;        
        Color hover;
        Texture2D mus;

        string thiskey;
        string lastkey;

        //int[,] range;
        public List<Vector2> MoveArea { get; set; }
        public List<Vector2> AttackArea { get; set; }

        Texture2D Trange;

        Texture2D mark;

        Marker marker;

        //initialiserar de flesta underklasser och fyller i texturer och annat
        public Player(Game1 g)
        {
            game = g;
            unitset = game.Content.Load<Texture2D>("units2");
            unitmenu = new UnitMenu(game, Vector2.Zero, this);
            castlemenu = new CastleMenu(game);
            hover = new Color(255, 255, 255);
            Trange = game.Content.Load<Texture2D>("RangeMark");
            //range=new int[32,32];
            currentUnit = -1;
            shop = new Shop(this, game);
            hit = game.Content.Load<Texture2D>("Hit");

            marker = new Marker(game, 1000 / 4);
            //marker.Position = new Vector2(64, 64);
        }

        
        /// <summary>
        ///initialiserar spelaren efter inställningar man gjort, t.ex. lag,färg och max antal enheter
        /// </summary>
        /// <param name="maxSoldiers"></param>
        /// <param name="color"></param>
        /// <param name="team"></param>
        /// <param name="startingMoney"></param>
        public void Initialize(int maxSoldiers, string color, int team, int playerID, int startingMoney)
        {
            ID = playerID;
            end = false;
            money = startingMoney;
            MaxUnits = maxSoldiers;
            Soldiers = new List<Soldier>();
            castlepositions = new List<Vector2>();
            shop.Initialize(this);
            moveSpeed = 25;
            

            TeamColor = color;
            switch (TeamColor)
            { 
                case "Blue":
                    housetype = 26;
                    castletype = 27;
                break;
                case "Red":
                    housetype = 32;
                    castletype = 33;
                break;
                case "Green":
                    housetype = 28;
                    castletype = 29;
                break;
                case "Yellow":
                    housetype = 30;
                    castletype = 31;
                break;
            }

            Team = team;
            
        }

        // Returns the soldier at the specified position
        public Soldier SoldierAt(Vector2 pos)
        {
            foreach (Soldier s in Soldiers)
            {
                if (s.Pos == pos)
                {
                    return s;
                }
            }
            return null;
        }
       

        // Creates a new soldier
        public void NewSoldier(int typ)
        {
            if (Soldiers.Count < MaxUnits)
            {
                Soldiers.Add(new Soldier(game,TeamColor));
                Soldiers[Soldiers.Count-1].SetType(typ, (int)castlepos.X, (int)castlepos.Y,Soldiers.Count-1);
                ControlUnit(Soldiers.Count - 1);
            }
        }


        //körs varenda gång det är en ny runda, återställer enheter hpregar om möjligt
        public void NewRound()
        {
            foreach (Soldier unit in Soldiers)
            {
                unit.NewRound(); 
                
            }
            Income();

            if (castlepositions.Count>0)
            castlepos = castlepositions[0];
            
            Pstate = state.SelectUnit;
        }

        //inkomster som räknas ut genom hur många hus och slott man har
        /// <summary>
        /// Kalkylerar inkomster
        /// </summary>
        public void Income()
        {
            income = 0;
            houses = 0;
            castles = 0;
            castlepositions = new List<Vector2>();
            castlepos = new Vector2(-1, -1);

            foreach (Tile item in game.Tilemanager.Map)
            {
                if (item.Type == housetype)
                    houses++;
                if (item.Type == castletype)
                {
                    castlepositions.Add(item.Position);
                    castles++;                    
                }
            }

            income = (30 * houses) + (50 * castles);
            money += income;
        }
       
        
        /// <summary>
        /// avslutar rundan och stänger ner allt!
        /// </summary>
        public void EndRound()
        {
            currentUnit = -1;
            HideUnitMenu();
            HideCastleMenu();      
        
        }




        //visar och gömmer soldatmenyn
        public void ShowUnitMenu(Vector2 pPos)
        {
            unitmenu = new UnitMenu(game, pPos, this);
            //unitmenu.Initialize(pPos,this);
            unitmenu.visible = true;
        }
        public void HideUnitMenu()
        {
            unitmenu.visible = false;
        }
        //visar och gömmer slottsmenyn
        public void ShowCastleMenu()
        {
            castlemenu.Initialize(castlepos,this);
            castlemenu.visible = true;
            shop.Gold(this);
        }
        public void HideCastleMenu()
        {
            castlemenu.visible = false;            
        }

        
        //flyttar den markerade soldaten
        public void MoveUnit(GameTime GT)
        {
            tid += GT.ElapsedGameTime.Milliseconds;

            if (tid > moveSpeed)
            {
                tid = 0;
                if (Soldiers[currentUnit].Pos.X != newpos.X)
                {
                    if (Soldiers[currentUnit].Pos.X < newpos.X)
                        Soldiers[currentUnit].Pos += new Vector2(0.125f, 0);
                    else if (Soldiers[currentUnit].Pos.X > newpos.X)
                        Soldiers[currentUnit].Pos -= new Vector2(0.125f, 0);
                }
                else if (Soldiers[currentUnit].Pos.Y != newpos.Y && Soldiers[currentUnit].Pos.X == newpos.X)
                {
                    if (Soldiers[currentUnit].Pos.Y < newpos.Y)
                        Soldiers[currentUnit].Pos += new Vector2(0, 0.125f);
                    else if (Soldiers[currentUnit].Pos.Y > newpos.Y)
                        Soldiers[currentUnit].Pos -= new Vector2(0, 0.125f);
                }
                else
                {
                    Soldiers[currentUnit].moved = true;
                    ShowUnitMenu(Soldiers[currentUnit].Pos);
                    Pstate = state.SelectUnit;
                }
            }
        }


        /// <summary>
        /// räknar ut hur mycket de skadar varandra
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="victimSoldier"></param>
        /// <param name="range"></param>
        /// <param name="game"></param>
        public void Fight()
        {
            int dmg = 0;

            Soldier attacker = Soldiers[currentUnit];

            // Calculate distance between soldiers
            int range = (int)Math.Abs(attacker.Pos.X - victimSoldier.Pos.X) +
                        (int)Math.Abs(attacker.Pos.Y - victimSoldier.Pos.Y);

            if (range == 1)
            {
                // anfallarnes attack
                dmg = attacker.Damage;
                victimSoldier.Hp -= (int)(dmg - (dmg * (float)(victimSoldier.Armor) * 0.01f));
                attacker.fought = true;
                attacker.fight = false;

                CheckWin();

                //motattack
                if (victimSoldier.Hp > 0)
                {
                    dmg = (int)((float)victimSoldier.Hp * victimSoldier.hpdmg);
                    attacker.Hp -= dmg - (dmg * (int)((float)attacker.Armor * 0.01f));
                }
                else
                    victim.Death(victimSoldier.ID);


                if (attacker.Hp <= 0)
                    Death(attacker.ID);
                CheckWin();

            }
            else if (range > 1)
            {
                //din attack  
                dmg = attacker.Damage;
                victimSoldier.Hp -= (int)(dmg - (dmg * (float)(victimSoldier.Armor) * 0.01f));
                attacker.fought = true;
                attacker.fight = false;

                if (victimSoldier.Hp <= 0)
                    victim.Death(victimSoldier.ID);

                CheckWin();
            }

            
        }

        /// <summary>
        /// removes a unit if dead
        /// </summary>
        /// <param name="nr"></param>
        public void Death(int nr)
        {
            Soldiers.RemoveAt(nr);

            int a = 0;
            foreach (Soldier unit in Soldiers)
            {
                unit.ID = a;
                a++;
            }

        }

        public void CheckWin()
        {

            // Check teams
            var allPlayers = game.Playermanager.Players;
            List<int> playerCastles = new List<int>(allPlayers.Count);
            List<int> playerSoldiers = new List<int>(allPlayers.Count);
            
            for (int i = 0; i < game.Playermanager.Players.Count; i++)
            {
                playerSoldiers[i] = allPlayers[i].Soldiers.Count;

                foreach (Tile tile in game.Tilemanager.Map)
                {
                    if (tile.Type == allPlayers[i].castletype)
                    {
                        playerCastles[i]++;
                    }
                }
            }

            // Check whether there is a sole survivor
            int numAliveWithSoldiers = 0;
            int numAliveWithCastles = 0;
            foreach (int castles in playerCastles)
            {
                if (castles > 0)
                {
                    numAliveWithCastles++;
                }
            }
            foreach (int soldiers in playerSoldiers)
            {
                if (soldiers > 0)
                {
                    numAliveWithSoldiers++;
                }
            }

            if (numAliveWithCastles == 1 && numAliveWithSoldiers == 1)
            {
                for (int i = 0; i < allPlayers.Count; i++)
                {
                    if (playerCastles[i] == 1 && playerSoldiers[i] == 1)
                    {
                        end = true;
                        break;
                    }
                }                
            }
        }


        /// <summary>
        /// kontrollerar soldat och visar sedan menyer och egenskaper hos soldaten
        /// </summary>
        /// <param name="value"></param>
        public void ControlUnit(int value)
        {
            currentUnit = value;
            HideCastleMenu();
            ShowUnitMenu(Soldiers[currentUnit].Pos);  
        }


        /// <summary>
        /// uppdater spelaren, soldater,hud,mus,tangentbord
        /// </summary>
        /// <param name="gt"></param>
        /// <param name="game"></param>
        public void Update(GameTime gt)
        {

            marker.Update(gt);

            if (IsPlaying == true)
            {

                // Set marker position
                if (TouchManager.Instance.ClickPoints.Count == 1)
                {
                    Vector2 pos = (TouchManager.Instance.ClickPoints[0] / game.Camera.Zoom) 
                                    + game.Camera.Position;
                    marker.Position = new Vector2((float)Math.Floor(pos.X / 32.0) * 32, 
                                                  (float)Math.Floor(pos.Y / 32.0) * 32);
                }

                
            }

            hover = new Color(0, 0, 0);

            
            switch (Pstate)
            {
                case state.SelectUnit:

                    if (currentUnit == -1)
                    {
                        foreach (Soldier unit in Soldiers)
                        {

                            if (TouchManager.Instance.IsClicked(new Rectangle((int)unit.Pos.X * 32, (int)unit.Pos.Y * 32, 32, 32)))
                            {
                                ControlUnit(unit.ID);
                                break;
                            }

                        }
                        if (castlepositions.Count > 0)
                        {
                            foreach (Vector2 item in castlepositions)
                            {
                                if (TouchManager.Instance.IsClicked(new Rectangle((int)item.X * 32, (int)item.Y * 32, 32, 32)))
                                {
                                    castlepos = item;
                                    ShowCastleMenu();
                                    break;
                                }
                            }
                        }
                    }
                    else if (currentUnit >= 0)
                    {
                        //setattack(currentUnit);
                    }

                    
                        
                    break;



                case state.SelectMove:
                    foreach (Vector2 pos in MoveArea)
                    {
                        if (TouchManager.Instance.IsClicked(new Rectangle((int)pos.X * 32,
                                                                          (int)pos.Y * 32,
                                                                          32,
                                                                          32)))
                        {
                            newpos = pos;
                            Pstate = state.Moving;
                            break;
                        }
                    }
                    break;



                case state.Moving:
                   MoveUnit(gt);
                    break;



                case state.SelectFight:
                    if (TouchManager.Instance.IsClicked(new Rectangle((int)Soldiers[currentUnit].Pos.X * 32,
                                                                      (int)Soldiers[currentUnit].Pos.Y * 32,
                                                                      32, 32)))
                    {
                        Pstate = state.SelectUnit;
                    }
                    else
                    {
                        // Check if any tile in the highligted area is
                        // clicked and if there is an enemy unit there.
                        foreach (Vector2 pos in AttackArea)
                        {
                            if (TouchManager.Instance.IsClicked(new Rectangle((int)pos.X * 32,
                                                                (int)pos.Y * 32,
                                                                32, 32)))
                            {
                                foreach (Player p in game.Playermanager.Players)
                                {
                                    if (p.ID != this.ID)
                                    {
                                        Soldier v = p.SoldierAt(pos);
                                        if (v != null)
                                        {
                                            victim = p;
                                            victimSoldier = v;
                                            Pstate = state.Fighting;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;



                case state.Fighting:
                    

                    if (tid < 4)
                    {
                        tid++;
                        hitrect = new Rectangle(tid*32, 0, 32, 32);
                    }
                    else
                    {
                        tid=0;
                        
                        Fight();
                        Pstate = state.SelectUnit;
                    }
                    
                    break;



                case state.Waiting:
                    break;

                case state.End:

                    if (end == true)
                    {
                        this.game.state = Game1.GameState.MainMenu;

                    }
                    break;



                case state.Shop:
                    shop.Update(gt, this);
                    break;

                    


            }

            if (unitmenu.visible == true)
                unitmenu.Update(gt, game);

            if (castlemenu.visible == true)
                castlemenu.Update(game);
    
            
        

        foreach (Soldier item in Soldiers)
                {
                    item.Update(gt,game);
                }

        }

     

        

        /// <summary>
        /// målar upp soldater, hud och menyer
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {

            foreach (Soldier item in Soldiers)
            {
                if (item.used == false)
                    sb.Draw(unitset, item.Rect, item.Bild, Color.White);
                else
                    sb.Draw(unitset, item.Rect, item.Bild, Color.Gray);
            }
                            
            switch (Pstate)
            {            

                case state.SelectMove:
                    foreach (Vector2 pos in MoveArea)
                        sb.Draw(Trange, pos*32, new Color(0, 100, 150, 150));
                    break;

                case state.SelectFight:
                    foreach (Vector2 pos in AttackArea)
                    {
                        sb.Draw(Trange, pos*32, new Color(200, 20, 20, 100));
                    }

                    break;

                case state.Fighting:
                    sb.Draw(hit,new Rectangle((int)victimSoldier.Pos.X*32,(int)victimSoldier.Pos.Y*32,32,32),hitrect,Color.White);
                    break;


                case state.SelectUnit:

                    if (currentUnit != -1)
                        sb.Draw(mark, new Vector2(Soldiers[currentUnit].Rect.Left,Soldiers[currentUnit].Rect.Top), new Color(0, 230, 0));
                    

                    unitmenu.Draw(sb);
                    castlemenu.Draw(sb);

                    //SB.Draw(musB, musPos, Color.White);
                    break;

                case state.Shop:
                    shop.Draw(sb);
                    break;
            
                case state.Waiting:


                    break;
            }

            marker.Draw(sb);
                    
        }

    }
}
