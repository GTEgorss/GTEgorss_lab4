using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace FileSystemServer
{
    public class FileSystemServer
    {
        private readonly List<NodeInfo> _nodes;
        private static TcpClient _tcpClient;
        private static NetworkStream _stream;

        public FileSystemServer()
        {
            _nodes = new List<NodeInfo>();
            _tcpClient = new TcpClient();
        }

        public void ConnectNodeInfo(NodeInfo nodeInfo)
        {
            _tcpClient = new TcpClient();
            _tcpClient.Connect(nodeInfo.Ip, nodeInfo.Port);
            _stream = _tcpClient.GetStream();
        }

        public void AddNode(string name, string ip, long size)
        {
            try
            {
                _nodes.Add(new NodeInfo(name, ip, size));
                Console.WriteLine("Node was successfully added.");

                /*
                ConnectNodeInfo(_nodes.Last());
                string infoAndFileText = "getinfo:";
                byte[] data = Encoding.Unicode.GetBytes(infoAndFileText);
                _stream.Write(data, 0, data.Length);
                */
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void AddFile(string filePath, string relativePath)
        {
            if (_nodes.Count == 0)
            {
                throw new FileSystemException("There are no nodes available.");
            }

            int count = 0;
            var node = _nodes[count];

            while (node.GetFreeSpace() < new System.IO.FileInfo(filePath).Length || node.ContainsFile(relativePath))
            {
                ++count;

                if (count == _nodes.Count)
                {
                    throw new FileSystemException("There are no nodes with enough free space.");
                }

                node = _nodes[count];
            }

            try
            {
                ConnectNodeInfo(node);

                string fileText = File.ReadAllText(filePath);
                string infoAndFileText = "add:" + relativePath + ":" + fileText;
                byte[] data = Encoding.Unicode.GetBytes(infoAndFileText);
                _stream.Write(data, 0, data.Length);

                int result = _stream.ReadByte();

                if (result == 1)
                {
                    node.AddFile(relativePath, new FileInfo(filePath).Length);
                    Console.WriteLine("File was added successfully.");

                    _tcpClient.Close();
                    _stream.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
        }

        public void AddFile(string relativePath, int size, string fileText, string restrictedNodeName)
        {
            if (_nodes.Count == 0)
            {
                throw new FileSystemException("There are no nodes available.");
            }

            int count = 0;
            var node = _nodes[count];

            while (node.GetFreeSpace() < size || node.ContainsFile(relativePath) || node.Name == restrictedNodeName)
            {
                ++count;

                if (count == _nodes.Count)
                {
                    throw new FileSystemException("There are no nodes with enough free space.");
                }

                node = _nodes[count];
            }

            try
            {
                ConnectNodeInfo(node);
                
                string infoAndFileText = "add:" + relativePath + ":" + fileText;
                byte[] data = Encoding.Unicode.GetBytes(infoAndFileText);
                _stream.Write(data, 0, data.Length);

                int result = _stream.ReadByte();

                if (result == 1)
                {
                    node.AddFile(relativePath, size);

                    _tcpClient.Close();
                    _stream.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
        }

        public void RemoveFile(string relativePath)
        {
            foreach (var node in _nodes)
            {
                if (node.ContainsFile(relativePath))
                {
                    try
                    {
                        ConnectNodeInfo(node);

                        string infoAndFileText = "remove:" + relativePath;
                        byte[] data = Encoding.Unicode.GetBytes(infoAndFileText);
                        _stream.Write(data, 0, data.Length);

                        int result = _stream.ReadByte();

                        if (result == 1)
                        {
                            node.RemoveFile(relativePath);
                            Console.WriteLine("File was removed successfully.");
                            _tcpClient.Close();
                            _stream.Close();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Environment.Exit(2);
                    }
                }
            }
        }

        public void CleanNode(string name)
        {
            var node = _nodes.FirstOrDefault(n => n.Name == name);

            if (node == null)
            {
                throw new FileSystemException($"Error. There is no node name: {name}.");
            }

            try
            {
                ConnectNodeInfo(node);
                
                string info = "clean-node:";
                byte[] data = Encoding.Unicode.GetBytes(info);
                _stream.Write(data, 0, data.Length);

                string cleanedFiles = "";

                data = new byte[64];
                int bytes;
                do
                {
                    bytes = _stream.Read(data, 0, data.Length);
                    cleanedFiles += Encoding.Unicode.GetString(data, 0, bytes);
                } while (_stream.DataAvailable);

                string[] stringSplit = cleanedFiles.Split('\0');

                foreach (var str in stringSplit)
                {
                    string[] strSplit = str.Split(':');
                    AddFile(strSplit[0], Convert.ToInt32(strSplit[1]), strSplit[2], name);
                }

                Console.WriteLine($"Node {name} was cleaned successfully.");
                node.ClearFiles();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void BalanceNode()
        {
            //TODO algorithm
        }
    }
}