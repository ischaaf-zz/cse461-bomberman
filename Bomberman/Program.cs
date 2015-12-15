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
            Console.WriteLine("Bomberman Game");
            Console.Write("Are you hosting? (y/n): ");
            string resp = Console.ReadLine();
            if (resp.StartsWith("y", true, System.Globalization.CultureInfo.InvariantCulture))
            {
                Console.Write("Port: ");
                if (!int.TryParse(Console.ReadLine(), out port))
                {
                    Console.WriteLine("invalid port number");
                    return;
                }
                Console.Write("Number of Players: ");
                int.TryParse(Console.ReadLine(), out players);
                Host(port, players, out host);
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
                    return;
                }
            }
            var p = Play(host, port);
            Console.WriteLine("-- Begin output from BombermanClient.exe --");
            while (true)
            {
                Console.WriteLine(p.StandardOutput.ReadLine());
            }
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
