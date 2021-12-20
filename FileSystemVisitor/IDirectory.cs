namespace FileSystemVisitor
{
    public interface IDirectory
    {
        string[] GetFiles(string root);
        string[] GetDirectories(string root);
    }
}
