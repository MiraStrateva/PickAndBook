using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;

namespace PickAndBook.Helpers.Contracts
{
    public interface IUserRoleManager
    {
        Task<bool> ChangeUserRoleFromClientToCompany(HttpContextBase httpContext, IPrincipal user);
    }
}
