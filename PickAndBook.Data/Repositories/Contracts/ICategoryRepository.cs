using PickAndBook.Data.Contracts;
using PickAndBook.Data.Models;
using System;
using System.Linq;

namespace PickAndBook.Data.Repositories.Contracts
{
    public interface ICategoryRepository : IEFRepository<Category>
    {
        IQueryable<Category> GetAll(int page = 0, int pageSize = 0);

        IQueryable<Category> GetAllCategoriesWithIncludedCompanies();

        string GetCategoryNameById(Guid? categoryId);
    }
}
