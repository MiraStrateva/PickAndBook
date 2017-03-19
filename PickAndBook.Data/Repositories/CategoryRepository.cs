using System;
using System.Linq;
using PickAndBook.Data.Models;
using PickAndBook.Data.Repositories.Contracts;
using PickAndBook.Data.Repositories.Base;
using System.Data.Entity;

namespace PickAndBook.Data.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(IPickAndBookDbContext context)
            : base(context)
        {
        }

        public IQueryable<Category> GetAllCategoriesWithIncludedCompanies()
        {
            return this.Context.Categories.Include(c => c.Companies);
        }

        public string GetCategoryNameById(Guid? categoryId)
        {
            if (categoryId.HasValue)
            {
                Category category = this.GetById(categoryId);
                if (category == null)
                {
                    return string.Empty;
                }
                return category.CategoryName;
            }
            return string.Empty;
        }
    }
}
