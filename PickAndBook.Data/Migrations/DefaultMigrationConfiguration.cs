using BookingSystem.Data;
using Microsoft.AspNet.Identity.EntityFramework;
using PickAndBook.Data.Models;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace PickAndBook.Data.Migrations
{
    public class DefaultMigrationConfiguration : DbMigrationsConfiguration<PickAndBookDbContext>
    {
        public DefaultMigrationConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(PickAndBookDbContext context)
        {
            if (!context.Roles.Any())
            {
                this.SeedRoles(context);
            }


            if (!context.Categories.Any())
            {
                this.SeedCategories(context);
            }
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

        protected void SeedCategories(PickAndBookDbContext context)
        { 
            IList<Category> categories = new List<Category>()
            {
                new Category() { CategoryName = "Beauty Salons",
                                CategoryDescription = "Book your next beauty procedure",
                                CategoryImage = "/Images/Categories/beautysalon.jpg"},
                new Category() { CategoryName = "Consultants",
                                CategoryDescription = "Book your next consultant appointment",
                                CategoryImage = "/Images/Categories/consultant.jpg"},
                new Category() { CategoryName = "Education",
                                CategoryDescription = "Book your next private lesson",
                                CategoryImage = "/Images/Categories/tutor.jpg"},
                new Category() { CategoryName = "Health Clinics",
                                CategoryDescription = "Book your next medical examination",
                                CategoryImage = "/Images/Categories/medical.jpg"},
                new Category() { CategoryName = "Sport & Fitness",
                                CategoryDescription = "Book your next individual training hour",
                                CategoryImage = "/Images/Categories/sport.jpg"},
                new Category() { CategoryName = "Wellness",
                                CategoryDescription = "Book your next SPA procedure",
                                CategoryImage = "/Images/Categories/wellness.jpg"},
                new Category() { CategoryName = "Dental Surgeries",
                                CategoryDescription = "Book your next dental procedure",
                                CategoryImage="/Images/Categories/dentist.jpg"},
                new Category() { CategoryName = "Make Up",
                                CategoryDescription = "Book your make up hour. Enjoy!",
                                CategoryImage = "/Images/Categories/make-up.jpg"},
                new Category() { CategoryName = "Other",
                                CategoryDescription = "Book your hour in some other hourly services",
                                CategoryImage = "/Images/Categories/other.jpg"}
            };

            context.Categories.AddOrUpdate(categories.ToArray());
        }
    }
}
