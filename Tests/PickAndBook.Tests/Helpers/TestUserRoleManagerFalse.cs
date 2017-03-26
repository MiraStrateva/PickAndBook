using PickAndBook.Helpers.Contracts;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;

namespace PickAndBook.Tests.Helpers
{
    public class TestUserRoleManagerFalse : IUserRoleManager
    {
        public async Task<bool> ChangeUserRoleFromClientToCompany(HttpContextBase httpContext, IPrincipal user)
        {
            return false;
        }
    }
}
