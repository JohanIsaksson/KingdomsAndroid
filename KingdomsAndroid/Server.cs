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


using System.Threading;
using System.Net;
using System.Net.Sockets;

using Microsoft.Xna.Framework;
using System.ComponentModel;

namespace KingdomsAndroid
{
    class Server
    {
        // Game related
        private struct Client
        {
            public TcpClient tcpClient;
            public Color color;
            public int order;
            public List<Soldier> soldiers;
            public int money;
            // TODO: Statistics (kill, losses, money spent, etc.)
        }
        private int currentPlayer;
        List<Tile> map;

        public enum ServerState
        {
            Initializing,
            Lobby,
            Pregame,
            Running,
            PostGame
        }


        // Server related
        public string address { get; }
        public int port { get; }
        private List<Client> clients;

        private bool waitForPlayers;
        private BackgroundWorker worker;


        public Server(int _port, string mapName)
        {
            // Set port
            port = _port;

            // Get local ip
            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress ip in localIPs)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    address = ip.ToString();
                    break;
                }
            }

            
        }

        public void Run()
        {
            

            // Wait 1 min for clients
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            Timer timer = new Timer(this.WaitForPlayers, autoResetEvent, 60000, 1000);
            ConnectToPlayers();
            timer.Dispose();
            
        }

        void ConnectToPlayers()
        {
            // Start listening for players
            TcpListener listener = new TcpListener(IPAddress.Any, int.Parse(address));
            listener.Start();
            int order = 0;
            while (waitForPlayers)
            {
                TcpClient tcpClient = listener.AcceptTcpClient();

                Client client;
                client.tcpClient = tcpClient;
                client.order = order;
                switch (order)
                {
                    case 0:
                        client.color = Color.Blue;
                        break;
                    case 1:
                        client.color = Color.Red;
                        break;
                    case 2:
                        client.color = Color.Green;
                        break;
                    case 3:
                        client.color = Color.Yellow;
                        break;
                }
            }
        }

        public void WaitForPlayers(Object stateInfo)
        {
            waitForPlayers = false;

        }
    }
}