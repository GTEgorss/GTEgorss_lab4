using System;

namespace FileSystemNode
{
    public class FileSystemNodeException : Exception
    {
        public FileSystemNodeException()
            : base()
        {
        }

        public FileSystemNodeException(string message)
            : base(message)
        {
        }

        public FileSystemNodeException(string message, Exception baseException)
            : base(message, baseException)
        {
        }
    }
}