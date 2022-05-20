using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FileSystemNode
{
    public class FileSystemNode
    {
        private static TcpListener _tcpListener;
        private string _name;
        private IPAddress _ipAddress;
        private int _port;
        private int _size;
        private string _basePath;

        private List<string> _filePaths;
        private NetworkStream _stream;

        public FileSystemNode(string name, string ipAddress, int port, int size, string basePath)
        {
            _filePaths = new List<string>();
            _name = name;
            _ipAddress = IPAddress.Parse(ipAddress);
            _port = port;
            _size = size;
            _basePath = basePath;

            if (!Directory.Exists(_basePath))
            {
                throw new FileSystemNodeException($"{_name}: Error. There is no directory {_basePath}.");
            }

            _basePath = Path.Combine(_basePath, name);
            Directory.CreateDirectory(_basePath);
        }

        public void Listen()
        {
            try
            {
                _tcpListener = new TcpListener(_ipAddress, _port);
                _tcpListener.Start();
                Console.WriteLine($"{_name}: Node was launched!");

                while (true)
                {
                    TcpClient tcpClient = _tcpListener.AcceptTcpClient();
                    _stream = tcpClient.GetStream();

                    byte[] data = new byte[64];
                    string response = "";
                    int bytes;
                    do
                    {
                        bytes = _stream.Read(data, 0, data.Length);
                        response += Encoding.Unicode.GetString(data, 0, bytes);
                    } while (_stream.DataAvailable);

                    var indexFirst = response.IndexOf(':');
                    string cmd = response.Substring(0, indexFirst);

                    switch (cmd)
                    {
                        case "add":
                            AddFile(response, indexFirst);
                            _stream.WriteByte(1);
                            break;
                        case "remove":
                            RemoveFile(response, indexFirst);
                            _stream.WriteByte(1);
                            break;
                        case "getinfo":
                            GetRelativePaths();
                            _stream.WriteByte(1);
                            break;
                        case "clean-node":
                            CleanNode();
                            _stream.WriteByte(1);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Disconnect();
            }
        }

        protected internal void Disconnect()
        {
            _tcpListener.Stop();

            Environment.Exit(0);
        }

        public void AddFile(string response, int indexFirst)
        {
            try
            {
                var indexSecond = response.IndexOf(':', indexFirst + 1);
                string relativePath = response.Substring(indexFirst + 1, indexSecond - indexFirst - 1);

                string fileText = response.Substring(indexSecond + 1);

                string filePath = Path.Combine(_basePath, relativePath);

                File.WriteAllText(filePath, fileText);

                _filePaths.Add(relativePath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void RemoveFile(string response, int indexFirst)
        {
            try
            {
                string relativePath = response.Substring(indexFirst + 1);

                File.Delete(Path.Combine(_basePath, relativePath));

                _filePaths.Remove(relativePath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void RemoveFile(string relativePath)
        {
            try
            {
                File.Delete(Path.Combine(_basePath, relativePath));

                _filePaths.Remove(relativePath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void GetRelativePaths()
        {
            string relativePaths = "";

            _filePaths.ForEach(f => relativePaths += f + ":");
            byte[] data = Encoding.Unicode.GetBytes(relativePaths);
            _stream.Write(data, 0, data.Length);
        }

        public void CleanNode()
        {
            byte[] data;
            int count = 0;
            _filePaths.ForEach(f =>
            {
                string fileText = File.ReadAllText(Path.Combine(_basePath, f));
                if (count > 0)
                    fileText = "\0" + f + ":" + fileText.Length + ":" + fileText;
                else
                    fileText = f + ":" + fileText.Length + ":" + fileText;
                data = Encoding.Unicode.GetBytes(fileText);
                _stream.Write(data, 0, data.Length);
                ++count;
            });

            for (int i = _filePaths.Count - 1; i >= 0; --i)
            {
                RemoveFile(_filePaths[i]);
            }
        }
    }
}