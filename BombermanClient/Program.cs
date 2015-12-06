using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanClient
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length != 2)
            {
                Usage();
                //return -1;
            }
            BombermanGame game = new BombermanGame("127.0.0.1", 5555);
            //BombermanGame game = new BombermanGame(args[0], Convert.ToInt32(args[1]));
            game.Run();
            return 0;
        }

        static void Usage()
        {
            Console.WriteLine("IP address and port");

        }
    }
}
