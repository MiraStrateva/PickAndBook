using BookingSystem.Data;
using Ninject.Modules;
using PickAndBook.Data;
using PickAndBook.Data.Contracts;
using PickAndBook.Data.Repositories;
using PickAndBook.Data.Repositories.Base;
using PickAndBook.Data.Repositories.Contracts;

namespace PickAndBook.App_Start
{
    public class PickAndBookNinjectModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IPickAndBookDbContext>().To<PickAndBookDbContext>().InSingletonScope();

            // Repositories
            this.Bind(typeof(IEFRepository<>)).To(typeof(EFRepository<>));
            this.Bind<ICategoryRepository>().To<CategoryRepository>();
            this.Bind<ICompanyRepository>().To<CompanyRepository>();
            this.Bind<IBookingRepository>().To<BookingRepository>();
        }
    }
}