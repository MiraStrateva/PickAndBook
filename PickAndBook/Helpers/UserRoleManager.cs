using PickAndBook.Helpers.Contracts;
using System.Web;
using System.Threading.Tasks;
using PickAndBook.Auth;
using Microsoft.AspNet.Identity.Owin;
using PickAndBook.Data.Common;
using System.Security.Principal;
using Microsoft.AspNet.Identity;

namespace PickAndBook.Helpers
{
    public class UserRoleManager : IUserRoleManager
    {
        public async Task<bool> ChangeUserRoleFromClientToCompany(HttpContextBase httpContext, IPrincipal user)
        {
            // Assign Company Role to current user
            var userManager = httpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var authenticationManager = httpContext.GetOwinContext().Authentication;
            var roleResult = await userManager.AddToRoleAsync(user.Identity.GetUserId(), DataConstants.CompanyRoleName);
            if (roleResult.Succeeded)
            {
                roleResult = await userManager.RemoveFromRoleAsync(user.Identity.GetUserId(), DataConstants.ClientRoleName);
            }
            if (roleResult.Succeeded)
            {
                var updatedUser = await userManager.FindByNameAsync(user.Identity.Name);
                var newIdentity = await updatedUser.GenerateUserIdentityAsync(userManager);
                authenticationManager.SignOut();
                authenticationManager.SignIn(newIdentity);
            }

            return roleResult.Succeeded;
        }
    }
}