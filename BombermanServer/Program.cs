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
            BombermanServer server = new BombermanServer(4);
            server.Start();
            // create server with X players
            // server.start() starts accepting connections
            //BombermanServer server = new BombermanServer();
        }
    }
}
