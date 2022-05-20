using System.Collections.Generic;
using System.Linq;

namespace FileSystemServer
{
    public class NodeInfo
    {
        private List<MyFileInfo> _files;

        public NodeInfo(string name, string ipAndPort, long size)
        {
            Name = name;

            string[] ipAndPortSplit = ipAndPort.Split(":");
            Ip = ipAndPortSplit[0];
            Port = int.Parse(ipAndPortSplit[1]);

            Size = size;

            _files = new List<MyFileInfo>();
        }

        public string Name { get; }
        public string Ip { get; }
        public int Port { get; }
        public long Size { get; }

        public void AddFile(string relativePath, long size)
        {
            _files.Add(new MyFileInfo(relativePath, size));
        }

        public void RemoveFile(string relativePath)
        {
            var fileToRemove = _files.FirstOrDefault(f => f.RelativePath == relativePath);

            if (fileToRemove == null)
            {
                throw new FileSystemException($"There is not file with relative path {relativePath} in node {Name}.");
            }

            _files.Remove(fileToRemove);
        }

        public long GetFreeSpace()
        {
            long sum = 0;
            _files.ForEach(f => sum += f.Size);
            return Size - sum;
        }

        public bool ContainsFile(string path)
        {
            foreach (var file in _files)
            {
                if (file.RelativePath == path)
                {
                    return true;
                }
            }

            return false;
        }

        public void ClearFiles()
        {
            _files.Clear();
        }
    }
}