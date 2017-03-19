using BookingSystem.Data;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Migrations;
using System.Linq;

namespace PickAndBook.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<PickAndBookDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(PickAndBookDbContext context)
        {
            if (context.Roles.Any())
            {
                return;
            }

            this.SeedRoles(context);
        }

        protected void SeedRoles(PickAndBookDbContext context)
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
