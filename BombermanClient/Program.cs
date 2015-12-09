using System;

namespace BombermanClient
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length != 2)
            {
                Usage();
                return -1;
            }
            int port;
            bool parsed = int.TryParse(args[1], out port);
            if (!parsed)
            {
                Usage();
                return -1;
            }
            string host = args[0];
            Console.WriteLine($"Connecting game client to: {host}:{port}");
            BombermanGame game = new BombermanGame(host, port);
            game.Run();
            return 0;
        }

        static void Usage()
        {
            Console.WriteLine("BombermanClient.exe [host] [port]");
            Console.WriteLine("Press any key to quit...");
            Console.Read();
        }
    }
}
