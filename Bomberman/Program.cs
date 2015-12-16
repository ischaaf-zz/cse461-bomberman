using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bomberman
{
    class Program
    {
        static void Main(string[] args)
        {
            string host = "";
            int port = -1;
            int players = 4;
            bool hosting;
            Process clientProcess = null;
            Process hostProcess = null;
            Console.WriteLine("Bomberman Game");
            try {
                hosting = getHostAndPort(out host, out port, out players);
            } catch (Exception)
            {
                Environment.Exit(-1);
                return;
            }
            bool playAgain = true;
            while (playAgain)
            {
                if (hosting)
                {
                    if (hostProcess != null && !hostProcess.HasExited)
                    {
                        hostProcess.Kill();
                    }
                    hostProcess = Host(port, players, out host);
                }
                clientProcess = Play(host, port);
                Console.WriteLine("-- Begin output from BombermanClient.exe --");
                while (true)
                {
                    Console.WriteLine(clientProcess.StandardOutput.ReadLine());
                    if (clientProcess.HasExited)
                        break;
                }
                Console.WriteLine("Game Over");
                Console.Write("Play again? (y/n): ");
                playAgain = Console.ReadLine().StartsWith("y");
            }
        }

        static bool getHostAndPort(out string host, out int port, out int players)
        {
            host = "";
            port = -1;
            players = 4;
            bool hosting = false;
            Console.Write("Are you hosting? (y/n): ");
            string resp = Console.ReadLine();
            if (resp.StartsWith("y", true, System.Globalization.CultureInfo.InvariantCulture))
            {
                hosting = true;
                Console.Write("Port: ");
                if (!int.TryParse(Console.ReadLine(), out port))
                {
                    Console.WriteLine("invalid port number");
                    throw new Exception();
                }
                Console.Write("Number of Players: ");
                int.TryParse(Console.ReadLine(), out players);
                if (players < 1 || players > 4)
                {
                    Console.WriteLine("player range 1-4");
                    throw new Exception();
                }
                host = "localhost";
            }
            if (host.Length == 0)
            {
                Console.Write("Host: ");
                host = Console.ReadLine();
            }
            if (port == -1)
            {
                Console.Write("Port: ");
                if (!int.TryParse(Console.ReadLine(), out port))
                {
                    Console.WriteLine("invalid port number");
                    throw new Exception();
                }
            }
            return hosting;
        }

        static Process Host(int port, int players, out string host)
        {
            host = "localhost";
            return Process.Start("BombermanServer.exe", $"{port} {players}");
        }

        static Process Play(string host, int port)
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.Arguments = $"{host} {port}";
            p.StartInfo.FileName = "BombermanClient.exe";
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();
            return p;
        }
    }
}
