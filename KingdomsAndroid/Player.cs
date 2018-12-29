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
        Game1 pgame;
        
        public List<Soldat> soldat { get; set; }       
        Texture2D unitset;
        
        public string name { get; set; }
        public string color { get; set; }
        public int team { get; set; }
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
        int victim;
        Vector2 victimpos;

        public UnitMenu unitmenu { get; set; }
        public CastleMenu castlemenu { get; set; }
        public Shop shop { get; set; }

        public enum state { SelectUnit, UnitMenu, SelectMove, Moving, SelectFight, Fighting, Shop, Waiting,End }
        public state Pstate { get; set; }

        public HUD hud { get; set; }

        Texture2D musB;
        Vector2 musPos;
        Vector2 tileHover;        
        Color hover;
        Texture2D mus;

        string thiskey;
        string lastkey;

        int[,] range;
        Texture2D Trange;

        Texture2D mark;

        //initialiserar de flesta underklasser och fyller i texturer och annat
        public Player(Game1 game)
        {
            pgame = game;
            unitset = game.Content.Load<Texture2D>("units2");
            musB = game.Content.Load<Texture2D>("mus2");
            mus = game.Content.Load<Texture2D>("musInGame1");
            mark = pgame.Content.Load<Texture2D>("musInGame1");
            hud = new HUD(game,this);
            unitmenu = new UnitMenu(game);
            castlemenu = new CastleMenu(game);
            hover = new Color(255, 255, 255);
            Trange = game.Content.Load<Texture2D>("RangeMark");
            range=new int[32,32];
            currentUnit = -1;
            shop = new Shop(this, pgame);
            hit = game.Content.Load<Texture2D>("Hit");
        }

        
        /// <summary>
        ///initialiserar spelaren efter inställningar man gjort, t.ex. lag,färg och max antal enheter
        /// </summary>
        /// <param name="MaxU"></param>
        /// <param name="colour"></param>
        /// <param name="lag"></param>
        /// <param name="cash"></param>
        public void Initialize(int MaxU,string colour,int lag,int cash)
        {
            end = false;
            money = cash;
            MaxUnits = MaxU;
            soldat = new List<Soldat>();
            castlepositions = new List<Vector2>();
            shop.Initialize(this);
            moveSpeed = 25;
            

            color = colour;
            switch (color)
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
            hud.Initialize(this);
            hud.SetTopBar(this);

            team = lag;
            
        } 
       

        //skapar ny enhet
        public void NewUnit(int typ)
        {
            if (soldat.Count < MaxUnits)
            {
                soldat.Add(new Soldat(pgame,color));
                soldat[soldat.Count-1].SetType(typ, (int)castlepos.X, (int)castlepos.Y,soldat.Count-1);
                ControlUnit(soldat.Count - 1);
            }
        }


        //körs varenda gång det är en ny runda, återställer enheter hpregar om möjligt
        public void NewRound()
        {
            foreach (Soldat unit in soldat)
            {
                unit.NewRound(); 
                
            }
            Income();
            hud.topBool = true;

            if (castlepositions.Count>0)
            castlepos = castlepositions[0];

            hud.newBool = true;
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

            foreach (Tile item in pgame.Tilemanager.Map)
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
            hud.SetTopBar(this);
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
            unitmenu.Initialize(pPos,this);
            unitmenu.visible = true;
        }
        public void HideUnitMenu()
        {
            unitmenu.visible = false;
            hud.infoBool = false;
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




        
        /// <summary>
        /// räknar ut hur långt soldaten kan gå, fyller i en tvådimensionell men koordinater som soldaten kan gå till
        /// räknar även ut om det är soldater i vägen
        /// </summary>
        /// <param name="selected"></param>
        /// <param name="pgame"></param>
        public void setRange(int selected, Game1 pgame)
        {
            int Xwidth=0;

            for (int X=0; X < 26; X++)
            {
                for (int Y=0; Y < 16; Y++)
                {
                    range[X,Y] = 0;
                }
            }

            for (int y = ((int)soldat[selected].Pos.Y - soldat[selected].WalkRange); y <= soldat[selected].Pos.Y; y++)
            {
                for (int x = ((int)soldat[selected].Pos.X - Xwidth); x <= soldat[selected].Pos.X + Xwidth; x++)
                {
                    if (y >= 0 && y < 16 && x >= 0 && x < 26)
                    {
                        bool move = true;

                        foreach (Soldat item in soldat)
                        { 
                        if (item.Pos==new Vector2(x,y))                        
                            move=false;                        
                        }

                        foreach (Soldat item in pgame.Playermanager.player[pgame.Playermanager.notplaying].soldat)
                        {
                        if (item.Pos == new Vector2(x, y))
                                move = false; 
                        }

                        if (move == true)
                            range[x, y] = 1;
                        else
                            range[x, y] = 0;
                    }
                }
                Xwidth++;
            }

            Xwidth = 0;
            for (int y = (((int)soldat[selected].Pos.Y) + soldat[selected].WalkRange); y >= soldat[selected].Pos.Y; y--)
            {
                for (int x = (((int)soldat[selected].Pos.X) - Xwidth); x <= soldat[selected].Pos.X + Xwidth; x++)
                {
                    if (y >= 0 && y < 16 && x >= 0 && x < 26)
                    {
                        bool move = true;

                        foreach (Soldat item in soldat)
                        {
                            if (item.Pos == new Vector2(x, y))
                                move = false;
                        }

                        foreach (Soldat item in pgame.Playermanager.player[pgame.Playermanager.notplaying].soldat)
                        {
                            if (item.Pos == new Vector2(x, y))
                                move = false;
                        }

                        if (move == true)
                            range[x, y] = 1;
                        else
                            range[x, y] = 0;
                    }
                }
                Xwidth++;
            }

            

        }
        //flyttar den markerade soldaten
        public void MoveUnit(GameTime GT)
        {
            tid += GT.ElapsedGameTime.Milliseconds;

            if (tid > moveSpeed)
            {
                tid = 0;
                if (soldat[currentUnit].Pos.X != newpos.X)
                {
                    if (soldat[currentUnit].Pos.X < newpos.X)
                        soldat[currentUnit].Pos += new Vector2(0.125f, 0);
                    else if (soldat[currentUnit].Pos.X > newpos.X)
                        soldat[currentUnit].Pos -= new Vector2(0.125f, 0);
                }
                else if (soldat[currentUnit].Pos.Y != newpos.Y && soldat[currentUnit].Pos.X == newpos.X)
                {
                    if (soldat[currentUnit].Pos.Y < newpos.Y)
                        soldat[currentUnit].Pos += new Vector2(0, 0.125f);
                    else if (soldat[currentUnit].Pos.Y > newpos.Y)
                        soldat[currentUnit].Pos -= new Vector2(0, 0.125f);
                }
                else
                {
                    soldat[currentUnit].moved = true;
                    ShowUnitMenu(soldat[currentUnit].Pos);
                    Pstate = state.SelectUnit;
                }
            }
        }





        
        /// <summary>
        ///räknar ut hur långt soldaten kan slå. möjliga attacker läggs i en tvådimensionell array 
        /// </summary>
        /// <param name="selected"></param>
        public void setattack(int selected)
        {
            soldat[currentUnit].fight = false;
            int Xwidth = 0;

            for (int X = 0; X < 26; X++)
            {
                for (int Y = 0; Y < 16; Y++)
                {
                    range[X, Y] = 0;
                }
            }

            for (int y = ((int)soldat[selected].Pos.Y - soldat[selected].AttackRange); y <= soldat[selected].Pos.Y; y++)
            {
                for (int x = ((int)soldat[selected].Pos.X - Xwidth); x <= soldat[selected].Pos.X + Xwidth; x++)
                {
                    if (y >= 0 && y < 16 && x >= 0 && x < 26)
                    {
                        bool attack = true;

                        foreach (Soldat item in soldat)
                        {
                            if (item.Pos == new Vector2(x, y))
                                attack = false;                            
                        }

                        foreach (Soldat item in pgame.Playermanager.player[pgame.Playermanager.notplaying].soldat)
                        {
                            if (item.Pos == new Vector2(x, y))
                                soldat[currentUnit].fight = true;
                        }

                        if (attack == true)
                            range[x, y] = 2;
                        else
                            range[x, y] = 0;
                    }
                }
                Xwidth++;
            }

            Xwidth = 0;
            for (int y = (((int)soldat[selected].Pos.Y) + soldat[selected].AttackRange); y >= soldat[selected].Pos.Y; y--)
            {
                for (int x = (((int)soldat[selected].Pos.X) - Xwidth); x <= soldat[selected].Pos.X + Xwidth; x++)
                {
                    if (y >= 0 && y < 16 && x >= 0 && x < 26)
                    {
                        bool attack = true;

                        foreach (Soldat item in soldat)
                        {
                            if (item.Pos == new Vector2(x, y))
                                attack = false;                            
                        }

                        
                        foreach (Soldat item in pgame.Playermanager.player[pgame.Playermanager.notplaying].soldat)
                        { 
                        if (item.Pos == new Vector2(x,y))
                            soldat[currentUnit].fight = true;                        
                        }
                        
                        if (attack == true)
                            range[x, y] = 2;
                        else
                            range[x, y] = 0;
                    }
                }
                Xwidth++;
            }

            range[(int)soldat[selected].Pos.X, (int)soldat[selected].Pos.Y] = 0;

            if (soldat[selected].AttackRange == 6)
            {
                range[(int)soldat[selected].Pos.X-1, (int)soldat[selected].Pos.Y] = 0;
                range[(int)soldat[selected].Pos.X+1, (int)soldat[selected].Pos.Y] = 0;
                range[(int)soldat[selected].Pos.X, (int)soldat[selected].Pos.Y+1] = 0;
                range[(int)soldat[selected].Pos.X, (int)soldat[selected].Pos.Y-1] = 0;
            }
        }

        /// <summary>
        /// räknar ut hur mycket de skadar varandra
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="victim"></param>
        /// <param name="range"></param>
        /// <param name="game"></param>
        public void Fight(int attacker, int victim, int range,Game1 game)
        {
            int dmg=0;
            

            if (range==1)
            {
                // anfallarnes attack
                dmg=soldat[attacker].Damage;
                game.Playermanager.player[game.Playermanager.notplaying].soldat[victim].Hp -= (int)(dmg - (dmg * (float)(game.Playermanager.player[game.Playermanager.notplaying].soldat[victim].Armor) * 0.01f));
                soldat[attacker].fought = true;
                soldat[attacker].fight = false;

                CheckWin();

                //motattack
                if (game.Playermanager.player[game.Playermanager.notplaying].soldat[victim].Hp > 0)
                {
                    dmg = (int)((float)game.Playermanager.player[game.Playermanager.notplaying].soldat[victim].Hp * game.Playermanager.player[game.Playermanager.notplaying].soldat[victim].hpdmg);
                    soldat[attacker].Hp -= dmg - (dmg * (int)((float)soldat[attacker].Armor * 0.01f));
                    hud.SetInfoBar(soldat[attacker]);
                }
                else
                    game.Playermanager.player[game.Playermanager.notplaying].Death(victim);


                if (soldat[attacker].Hp <= 0)
                    Death(attacker);
                CheckWin();

            }
            else if (range > 1)
            {
                //din attack  
                dmg = soldat[attacker].Damage;
                game.Playermanager.player[game.Playermanager.notplaying].soldat[victim].Hp -= (int)(dmg - (dmg * (float)(game.Playermanager.player[game.Playermanager.notplaying].soldat[victim].Armor) * 0.01f));
                soldat[attacker].fought = true;
                soldat[attacker].fight = false;

                if (game.Playermanager.player[game.Playermanager.notplaying].soldat[victim].Hp <= 0)
                    game.Playermanager.player[game.Playermanager.notplaying].Death(victim);

                CheckWin();
            }

            
        }

        /// <summary>
        /// removes a unit if dead
        /// </summary>
        /// <param name="nr"></param>
        public void Death(int nr)
        {
            soldat.RemoveAt(nr);

            int a = 0;
            foreach (Soldat unit in soldat)
            {
                unit.nr = a;
                a++;
            }

        }

        /// <summary>
        /// funktion som räknar ut om det är närstrid eller distansattack
        /// med hjälp av soldatpositionen och fiendens position
        /// </summary>
        /// <param name="pos1"></param>
        /// <param name="pos2"></param>
        /// <returns></returns>
        int attackrange(Vector2 pos1,Vector2 pos2)
        {
            int range = 0;
            int Xrange = 0;
            int Yrange = 0;

            if (pos1.X > pos2.X)
                Xrange = (int)(pos1.X - pos2.X);
            else
                Xrange = (int)(pos2.X - pos1.X);

            if (pos1.Y > pos2.Y)
                Yrange = (int)(pos1.Y - pos2.Y);
            else
                Yrange = (int)(pos2.Y - pos1.Y);

            if (Xrange >= 2 || Yrange >= 2)
                range = 2;
            else if (Xrange == 1 && Yrange == 1)
                range = 2;
            else
                range = 1;


            return range;
        
        }

        public void CheckWin()
        {

            // kolla fiendelaget--------------------
            int enemycastles=0;
            int enemyunits = pgame.Playermanager.player[pgame.Playermanager.notplaying].soldat.Count;
                        
            foreach (Tile ruta in pgame.Tilemanager.Map)
            {
                if (ruta.Type == pgame.Playermanager.player[pgame.Playermanager.notplaying].castletype)
                {
                    enemycastles++;
                }
            }

            //kolla egna laget--------------------------------
            castles=0;

            foreach (Tile ruta in pgame.Tilemanager.Map)
            {
                if (ruta.Type == castletype)
                {
                    castles++;
                }
            }

            
            if (enemycastles==0 && enemyunits==0)
            {
                hud.Newtext = "Blue Player won";
                hud.newBool = true;
                end = true;
                
            }
            else if (castles == 0 && soldat.Count == 0)
            {
                hud.Newtext = "Red Player won";                
                hud.newBool = true;
                end = true;
                
            }
        
        
        }






        /// <summary>
        /// kontrollerar soldat och visar sedan menyer och egenskaper hos soldaten
        /// </summary>
        /// <param name="value"></param>
        public void ControlUnit(int value)
        {
            currentUnit = value;
            hud.SetInfoBar(soldat[currentUnit]);
            hud.infoBool=true;
            HideCastleMenu();
            ShowUnitMenu(soldat[currentUnit].Pos);  
        }
        /// <summary>
        /// snabbyte mellan enheter
        /// </summary>
        /// <param name="updown"></param>
        public void QuickSwitch(bool updown)
        {
            if (soldat.Count != 0)
            {
                HideUnitMenu();

                if (updown == true)
                {
                    if (currentUnit != soldat.Count - 1)
                        ControlUnit(currentUnit + 1);
                    else
                        ControlUnit(0);
                }
                else
                {
                    if (currentUnit > 0)
                        ControlUnit(currentUnit - 1);
                    else
                        ControlUnit(soldat.Count - 1);
                }
            }
        }


        

        /// <summary>
        /// kollar om ´man håller musen över en fiende eller en allierad
        /// </summary>
        /// <param name="mus"></param>
        public void CheckHover(Vector2 mus)
        {
            hover = new Color(0, 0, 0);

            foreach (Soldat item in soldat)
            {
                if (tileHover == item.Pos)
                    hover = new Color(0, 230, 0);            
            }

            foreach (Soldat item in pgame.Playermanager.player[pgame.Playermanager.notplaying].soldat)
            {
                if (tileHover == item.Pos)
                    hover = new Color(260,0, 0);
            }
        }


        /// <summary>
        /// uppdater spelaren, soldater,hud,mus,tangentbord
        /// </summary>
        /// <param name="GT"></param>
        /// <param name="game"></param>
        public void Update(GameTime GT, Game1 game)
        {
            pgame = game;

            if (IsPlaying == true)
            {
                KeyboardState key = Keyboard.GetState();

                if (key.IsKeyDown(Keys.F2) == true)
                    thiskey = "F2";
                else if (key.IsKeyDown(Keys.F4) == true)
                    thiskey = "F4";
                else if (key.IsKeyDown(Keys.F5))
                    thiskey = "F5";
                else if (key.IsKeyDown(Keys.F6) == true)
                    thiskey = "F6";
                else if (key.IsKeyDown(Keys.F7) == true)
                    thiskey = "F7";
                else if (key.IsKeyDown(Keys.F8) == true)
                    thiskey = "F8";
                else if (key.IsKeyDown(Keys.Left) == true)
                    thiskey = "Left";
                else if (key.IsKeyDown(Keys.Right) == true)
                    thiskey = "Right";

                else
                    thiskey = "";


                if (thiskey == "F4" && thiskey != lastkey)
                    hud.topBool = !hud.topBool;
                else if (thiskey == "F5" && thiskey != lastkey)
                    hud.infoBool = !hud.infoBool;
                else if (thiskey == "F6" && thiskey != lastkey)
                    hud.newBool = !hud.newBool;
                else if (thiskey == "F8" && thiskey != lastkey)
                    NewUnit(1);
                else if (thiskey == "F7" && thiskey != lastkey)
                    NewUnit(3);
                else if (thiskey == "F2" && thiskey != lastkey)
                    NewUnit(2);
                else if (thiskey == "Left" && thiskey != lastkey)
                    QuickSwitch(false);
                else if (thiskey == "Right" && thiskey != lastkey)
                    QuickSwitch(true);



                lastkey = thiskey;
            }
                MouseState mus = Mouse.GetState();
                musPos = new Vector2(mus.X, mus.Y);
                tileHover = new Vector2((float)Math.Floor((double)mus.X / 32), (float)Math.Floor((double)mus.Y / 32));
                
                hover = new Color(0, 0, 0);

            
            switch (Pstate)
            {
                case state.SelectUnit:

                    if (currentUnit == -1)
                    {                        
                        foreach (Soldat unit in soldat)
                        {
                            
                            if (tileHover == unit.Pos)
                            {
                                if (mus.LeftButton == ButtonState.Pressed)
                                {
                                    ControlUnit(unit.nr);
                                    break;
                                    
                                }
                            }
                            
                        }
                        if (castlepositions.Count > 0)
                        {
                            foreach (Vector2 item in castlepositions)
                            {
                                if (tileHover == item)
                                {
                                    if (mus.LeftButton == ButtonState.Pressed)
                                    {
                                        castlepos = item;
                                        ShowCastleMenu();
                                        break;
                                    }
                                }
                            }
                        }
                        CheckHover(tileHover);
                        //if (tileHover.X >= 0 && tileHover.X <= 25 && tileHover.Y >= 0 && tileHover.Y <= 15)
                        //{
                        //    if (game.Tilemanager.Map[(int)tileHover.X, (int)tileHover.Y].typ == castletype && mus.LeftButton == ButtonState.Pressed)
                        //        Pstate = state.Shop;
                        //}
                    }
                    else if (currentUnit >=0)
                        setattack(currentUnit);

                    
                        
                    break;



                case state.SelectMove:
                    if (mus.RightButton == ButtonState.Pressed && range[(int)tileHover.X, (int)tileHover.Y] == 1)
                    {
                        newpos = tileHover;                        
                        Pstate = state.Moving;
                    }
                    break;



                case state.Moving:
                   MoveUnit(GT);
                    break;



                case state.SelectFight:
                    if (tileHover == soldat[currentUnit].Pos && mus.RightButton == ButtonState.Pressed)
                        Pstate = state.SelectUnit;

                    foreach (Soldat item in game.Playermanager.player[game.Playermanager.notplaying].soldat)
                    {
                        if (mus.RightButton == ButtonState.Pressed && item.Pos == tileHover && range[(int)tileHover.X,(int)tileHover.Y]==2)
                        {
                            victim = item.nr;
                            victimpos = item.Pos;
                            Pstate = state.Fighting;
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
                        Fight(currentUnit, victim,attackrange(soldat[currentUnit].Pos,victimpos), game);
                        Pstate = state.SelectUnit;
                    }

                    hud.ShowInfoBar();
                    break;



                case state.Waiting:
                    hud.newBool = true;
                    break;

                case state.End:

                    if (end == true && hud.newBool == false)
                    {
                        pgame.state = Game1.GameState.MainMenu;

                    }
                    break;



                case state.Shop:
                    shop.Update(GT, this);
                    break;

                    


            }

            if (unitmenu.visible == true)
                unitmenu.Update(GT, game);

            if (castlemenu.visible == true)
                castlemenu.Update(game);

                hud.Update(GT);            
            
        

        foreach (Soldat item in soldat)
                {
                    item.Update(GT,game);
                }

        if (currentUnit >= 0)
            hud.SetInfoBar(soldat[currentUnit]);
        
        }

     

        

        /// <summary>
        /// målar upp soldater, hud och menyer
        /// </summary>
        /// <param name="SB"></param>
        public void Draw(SpriteBatch SB)
        {

            foreach (Soldat item in soldat)
            {
                if (item.used == false)
                    SB.Draw(unitset, item.Rect, item.Bild, Color.White);
                else
                    SB.Draw(unitset, item.Rect, item.Bild, Color.Gray);
            }
                            
            switch (Pstate)
            {            

            case state.SelectMove:
                for (int x = 0; x < 32; x++)
                {
                    for (int y = 0; y < 32; y++)
                    { 
                    if (range[x,y]==1)
                        SB.Draw(Trange, new Vector2(x * 32, y * 32), new Color(0, 100, 150, 150));                        
                    }
                }
                    SB.Draw(mus, new Rectangle((int)tileHover.X * 32, (int)tileHover.Y * 32, 32, 32), new Rectangle(0, 0, 32, 32), hover);

                    SB.Draw(musB, musPos, Color.White);
                    hud.Draw(SB);
            break;

                case state.SelectFight:
                    for (int x = 0; x < 26; x++)
                {
                    for (int y = 0; y < 16; y++)
                    {                    
                        if (range[x,y]==2)
                        SB.Draw(Trange,new Vector2(x*32,y*32), new Color(200,20,20,150));
                    }
                }
                    SB.Draw(mus, new Rectangle((int)tileHover.X * 32, (int)tileHover.Y * 32, 32, 32), new Rectangle(0, 0, 32, 32), hover);


                    SB.Draw(musB, musPos, Color.White);
                hud.Draw(SB);
            break;

                case state.Fighting:
                    SB.Draw(hit,new Rectangle((int)victimpos.X*32,(int)victimpos.Y*32,32,32),hitrect,Color.White);
            break;


            case state.SelectUnit:
            if (unitmenu.visible == false && castlemenu.visible==false)
                SB.Draw(mus, new Rectangle((int)tileHover.X * 32, (int)tileHover.Y * 32, 32, 32), new Rectangle(0, 0, 32, 32), hover);

            if (currentUnit != -1)
                SB.Draw(mark, new Vector2(soldat[currentUnit].Rect.Left,soldat[currentUnit].Rect.Top), new Color(0, 230, 0));
                    
            hud.Draw(SB);
            unitmenu.Draw(SB);
            castlemenu.Draw(SB);

            SB.Draw(musB, musPos, Color.White);
            break;

            case state.Shop:
            shop.Draw(SB);
            break;
            
            case state.Waiting:


            break;
            }

                








                

                


                



                    
        }






    }
}
