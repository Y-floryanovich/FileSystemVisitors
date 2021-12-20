using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemVisitor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var user = new User();
            var varVisitor = new Visitor(new MyDirectoryInfo());
            varVisitor.FilteredFilesFinded += user.OnFilteredFinded;
            varVisitor.Finish += user.OnFinish;
            varVisitor.Start += user.OnStart;
            varVisitor.FilesFinded += user.OnFilesFinded;

            foreach (var i in varVisitor.GetDirs(@"C:\.NET Basic", x => x.Length <= 35))
            {
                Console.WriteLine(i);
            }
            Console.ReadKey();
        }
    }
}
