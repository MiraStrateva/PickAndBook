// Use SingleR
using PickAndBook.Data.Repositories.Contracts;
using PickAndBook.Services.Contracts;
using System.Linq;
using PickAndBook.Data.Models;
using System.Configuration;
using System.Data.SqlClient;
using PickAndBook.Hubs;
using System.Data;
using System.Collections.Generic;
using System;
using PickAndBook.Data.Common;

namespace PickAndBook.Services
{
    public class CompanyService : ICompanyService
    {
        private ICompanyRepository companyRepository;
        readonly string _connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public CompanyService(ICompanyRepository companyRepository)
        {
            this.companyRepository = companyRepository;
        }

        public IQueryable<Company> GetLastAddedCompanies()
        {
            var companies = new List<Company>();
            using (var connection = new SqlConnection(_connString))
            {
      //          new SqlCommand(String.Format(@"SELECT TOP {0} [CompanyId]
      //,[CompanyName]
      //,[CompanyDescription]
      //,[CompanyImage]
      //FROM [dbo].[Companies] ORDER BY CompanyId DESC", DataConstants.LastRegisteredCompaniesCount)
                connection.Open();
                using (var command = new SqlCommand(@"SELECT [CompanyId],[CompanyName],[CompanyDescription],[CompanyImage]
               FROM [dbo].[Companies] ORDER By [CompanyId] DESC", connection))
                {
                    command.Notification = null;

                    var dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);
                    
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        //companies.Add(item: new Company
                        //{
                        //    CompanyImage = reader["CompanyImage"] != DBNull.Value ? (string)reader["CompanyImage"] : "",
                        //    CompanyName = (string)reader["CompanyName"],
                        //    CompanyDescription = reader["CompanyDescription"] != DBNull.Value ? (string)reader["CompanyDescription"] : "",
                        //    CompanyId = reader.GetGuid(reader.GetOrdinal("CompanyId"))
                        //});
                    }
                    return this.companyRepository.GetLastAddedCompanies();
                    //return companies.AsQueryable();
                }
            }
        }

        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            { 
                LastAddedCompaniesHub.UpdateLastAddedCompanies();
            }
        }
    }
}