using PickAndBook.Helpers.Contracts;
using System.IO;

namespace PickAndBook.Tests.Helpers
{
    public class TestPathProvider : IPathProvider
    {
        public string MapPath(string path)
        {
            return Path.Combine(@"C:\temp\", path);
        }
    }
}
