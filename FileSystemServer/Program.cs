using System;


namespace FileSystemServer
{
    internal static class Program
    {
        public static void AddNode(FileSystemServer server, string[] cmdSplit)
        {
            server.AddNode(cmdSplit[1], cmdSplit[2], Convert.ToInt64(cmdSplit[3]));
        }

        public static void AddFile(FileSystemServer server, string[] cmdSplit)
        {
            server.AddFile(cmdSplit[1], cmdSplit[2]);
        }

        public static void RemoveFile(FileSystemServer server, string[] cmdSplit)
        {
            server.RemoveFile(cmdSplit[1]);
        }

        public static void ExecFile(FileSystemServer server, string[] cmdSplit)
        {
            var lines = System.IO.File.ReadLines(cmdSplit[1]);

            foreach (var line in lines)
            {
                DoCommand(server, line);
            }
        }

        public static void CleanNode(FileSystemServer server, string[] cmdSplit)
        {
            server.CleanNode(cmdSplit[1]);
        }

        public static void BalanceNode(FileSystemServer server)
        {
            server.BalanceNode();
        }

        public static void DoCommand(FileSystemServer server, string? cmd)
        {
            if (cmd != null)
            {
                string[] cmdSplit = cmd.Split(" ");

                switch (cmdSplit[0])
                {
                    case "/add-node":
                        AddNode(server, cmdSplit);
                        break;
                    case "/add-file":
                        AddFile(server, cmdSplit);
                        break;
                    case "/remove-file":
                        RemoveFile(server, cmdSplit);
                        break;
                    case "/exec":
                        ExecFile(server, cmdSplit);
                        break;
                    case "/clean-node":
                        CleanNode(server, cmdSplit);
                        break;
                    case "/balanced-node":
                        BalanceNode(server);
                        break;
                    case "/exit":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine($"No such command {cmdSplit[0]}. Try again!");
                        break;
                }
            }
        }
        
        public static void Main(string[] args)
        {
            FileSystemServer server = new FileSystemServer();

            while (true)
            {
                var cmd = Console.ReadLine();
                try
                {
                    DoCommand(server, cmd);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }   
}