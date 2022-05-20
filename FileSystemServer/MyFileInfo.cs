namespace FileSystemServer
{
    public class MyFileInfo
    {
        public MyFileInfo(string relativePath, long size)
        {
            RelativePath = relativePath;
            Size = size;
        }
        
        public string RelativePath { get; }
        public long Size { get; }
    }
}