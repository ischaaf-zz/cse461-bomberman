using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // command line: number of players?
            if (args.Length < 1)
            {
                Usage();
                return;
            }
            int port;
            int players = 4;
            bool parsePort = int.TryParse(args[0], out port);
            bool playerParse = true;
            if (args.Length == 2)
            {
                playerParse = int.TryParse(args[1], out players);
            }
            if (!parsePort || !playerParse)
            {
                Usage();
                return;
            }
            BombermanServer server = new BombermanServer(players, port);
            server.Start();
            // create server with X players
            // server.start() starts accepting connections
            //BombermanServer server = new BombermanServer();
        }

        static void Usage()
        {
            Console.WriteLine("BombermanServer.exe port [players, default: 4]");
            Console.WriteLine("Press any key to quit...");
            Console.Read();
        }
    }
}
