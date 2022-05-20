using System;

namespace FileSystemServer
{
    public class FileSystemException : Exception
    {
        public FileSystemException()
        {
        }

        public FileSystemException(string message) 
            : base(message)
        {
        }

        public FileSystemException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}