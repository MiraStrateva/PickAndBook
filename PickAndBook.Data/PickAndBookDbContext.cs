using System.Data.Entity;
using PickAndBook.Data.Models;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PickAndBook.Data
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

        public new IDbSet<T> Set<T>()
            where T : class
        {
            return base.Set<T>();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public void RefreshAll()
        {
            foreach (var entity in this.ChangeTracker.Entries())
            {
                entity.Reload();
            }
        }
    }
}