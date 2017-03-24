using Microsoft.AspNet.Identity.EntityFramework;
using PickAndBook.Data.Common;
using System.Data.Entity.Migrations;

namespace PickAndBook.Auth.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            foreach (var entity in context.Roles)
            {
                context.Roles.Remove(entity);
            }

            context.Roles.AddOrUpdate(new IdentityRole(DataConstants.AdministratorRoleName));
            context.Roles.AddOrUpdate(new IdentityRole(DataConstants.CompanyRoleName));
            context.Roles.AddOrUpdate(new IdentityRole(DataConstants.ClientRoleName));
        }
    }
}
