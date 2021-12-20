using System.IO;

namespace FileSystemVisitor
{
    class MyDirectoryInfo : IDirectory
    {
        public string[] GetDirectories(string path)
        {
            return Directory.GetDirectories(path);
        }

        public string[] GetFiles(string path)
        {
            return Directory.GetFiles(path);
        }
    }
}
