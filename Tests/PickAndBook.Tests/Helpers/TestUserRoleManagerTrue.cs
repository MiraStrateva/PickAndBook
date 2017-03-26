using PickAndBook.Helpers.Contracts;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Web;

namespace PickAndBook.Tests.Helpers
{
    public class TestUserRoleManagerTrue : IUserRoleManager
    {
        public async Task<bool> ChangeUserRoleFromClientToCompany(HttpContextBase httpContext, IPrincipal user)
        {
            return true;
        }
    }
}
