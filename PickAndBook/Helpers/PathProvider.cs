using PickAndBook.Helpers.Contracts;
using System.Web;

namespace PickAndBook.Helpers
{
    public class PathProvider : IPathProvider
    {
        public string MapPath(string path)
        {
            return HttpContext.Current.Server.MapPath(path);
        }
    }
}