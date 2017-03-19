using System.Data.Entity;
using PickAndBook.Data.Models;
using PickAndBook.Data;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BookingSystem.Data
{
    public class PickAndBookDbContext : IdentityDbContext, IPickAndBookDbContext
    {
        public PickAndBookDbContext()
            :base("DefaultConnection")
        {
        }

        public PickAndBookDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        public virtual IDbSet<Booking> Bookings { get; set; }

        public virtual IDbSet<Category> Categories { get; set; }

        public virtual IDbSet<Company> Companies { get; set; }

        public virtual IDbSet<Workingtime> Workingtimes { get; set; }

        public DbContext DbContext => this;

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public void ClearDatabase()
        {
            /*
            // Possible solution to foreign key deletes: http://www.ridgway.co.za/articles/174.aspx
            // The above solution does not work with cyclic relations.
            */

            this.SaveChanges();
            var tableNames =
                new List<string>
                    {
                        "AspNetUserRoles",
                        "AspNetRoles",
                        "AspNetUserLogins",
                        "AspNetUserClaims",
                        "Bookings",
                        "Workingtimes",
                        "Companies",
                        "Categories",
                        "AspNetUsers",
                    };

            foreach (var tableName in tableNames)
            {
                this.Database.ExecuteSqlCommand(string.Format("DELETE FROM {0}", tableName));
            }

            this.SaveChanges();
        }

        public new IDbSet<T> Set<T>()
            where T : class
        {
            return base.Set<T>();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}