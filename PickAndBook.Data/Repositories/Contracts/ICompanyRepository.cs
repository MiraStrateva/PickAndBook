using System;
using System.Linq;
using PickAndBook.Data.Contracts;
using PickAndBook.Data.Models;

namespace PickAndBook.Data.Repositories.Contracts
{
    public interface ICompanyRepository : IEFRepository<Company>
    {
        IQueryable<Company> GetCompaniesByCategoryId(Guid? categoryId);

        IQueryable<Company> GetCompaniesByNameOrDescription(string searchText);

        IQueryable<Company> GetCompaniesByCategoryIdNameOrDescription(Guid? categoryId, string searchText);

        IQueryable<Company> GetLastAddedCompanies();

        Company GetCompanyByUserId(string userId);
    }
}
