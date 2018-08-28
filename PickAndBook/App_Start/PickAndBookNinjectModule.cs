using Ninject.Modules;
using Ninject.Web.Common;
using PickAndBook.Data;
using PickAndBook.Data.Contracts;
using PickAndBook.Data.Repositories;
using PickAndBook.Data.Repositories.Base;
using PickAndBook.Data.Repositories.Contracts;
using PickAndBook.Helpers;
using PickAndBook.Helpers.Contracts;
using PickAndBook.Services;
using PickAndBook.Services.Contracts;

namespace PickAndBook.App_Start
{
    public class PickAndBookNinjectModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IPickAndBookDbContext>().To<PickAndBookDbContext>().InRequestScope();
            this.Bind<IPickAndBookData>().To<PickAndBookData>().InRequestScope();

            // Repositories
            this.Bind(typeof(IEFRepository<>)).To(typeof(EFRepository<>));
            this.Bind<ICategoryRepository>().To<CategoryRepository>();
            this.Bind<ICompanyRepository>().To<CompanyRepository>();
            this.Bind<IBookingRepository>().To<BookingRepository>();

            // Other
            this.Bind<IPathProvider>().To<PathProvider>().InSingletonScope();
            this.Bind<IFileUploader>().To<FileUploader>().InSingletonScope();
            this.Bind<IUserRoleManager>().To<UserRoleManager>().InSingletonScope();

            // Use SingleR
            this.Bind<ICompanyService>().To<CompanyService>();
        }
    }
}