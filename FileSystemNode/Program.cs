using System;

namespace FileSystemNode
{
    internal static class Program
    {
        private static FileSystemNode? _server;

        public static void Main(string[] args)
        {
            try
            {
                _server = new FileSystemNode(args[0], "127.0.0.1", Convert.ToInt32(args[1]), 250,
                    "/Users/egorsergeev/RiderProjects/GTEgorss_PerfTips/FileSystemNode/NodeFolders");
                _server.Listen();
            }
            catch (Exception ex)
            {
                _server?.Disconnect();
                Console.WriteLine(ex.Message);
            }
        }
    }
}