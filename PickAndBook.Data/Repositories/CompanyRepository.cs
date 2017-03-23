using PickAndBook.Data.Models;
using PickAndBook.Data.Repositories.Base;
using PickAndBook.Data.Repositories.Contracts;
using System;
using System.Linq;

namespace PickAndBook.Data.Repositories
{
    public class CompanyRepository : EFRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(IPickAndBookDbContext context)
            : base(context)
        {
        }

        public IQueryable<Company> GetCompaniesByCategoryId(Guid? categoryId)
        {
            return categoryId.HasValue ?
                this.All().Where(c => c.CategoryId == categoryId) :
                null;
        }

        public IQueryable<Company> GetCompaniesByCategoryIdNameOrDescription(Guid? categoryId, string searchText)
        {
            return GetCompaniesByCategoryId(categoryId)
                .Where(c => (string.IsNullOrEmpty(c.CompanyName) ? false : c.CompanyName.ToLower().Contains(searchText)) ||
                (string.IsNullOrEmpty(c.CompanyDescription) ? false : c.CompanyDescription.ToLower().Contains(searchText)));

        }

        public IQueryable<Company> GetCompaniesByNameOrDescription(string searchText)
        {
            return this.All()
                .Where(c => (string.IsNullOrEmpty(c.CompanyName) ? false : c.CompanyName.ToLower().Contains(searchText)) ||
                (string.IsNullOrEmpty(c.CompanyDescription) ? false : c.CompanyDescription.ToLower().Contains(searchText)));
        }

        public Company GetCompanyByUserId(string userId)
        {
            return this.All()
                .FirstOrDefault(c => c.UserId == userId);
        }
    }
}
