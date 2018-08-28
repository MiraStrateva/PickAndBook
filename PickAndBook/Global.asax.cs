using PickAndBook.Data;
using PickAndBook.Data.Migrations;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PickAndBook
{
    public class MvcApplication : HttpApplication
    {
        // Use SingleR
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Application_Start()
        {
            // Database.SetInitializer<PickAndBookDbContext>(new MigrateDatabaseToLatestVersion<PickAndBookDbContext, DefaultMigrationConfiguration>());
            // var db = new PickAndBookDbContext();
            // db.Database.Initialize(true);
            
            ViewEnginesConfig.RegisterViewEngines();
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Start SqlDependency with application initialization
            // Use SingleR
            SqlDependency.Start(connectionString);
        }

        protected void Application_End()
        {
            // Stop SQL dependency
            // Use SingleR
            SqlDependency.Stop(connectionString);
        }

        public override string GetVaryByCustomString(HttpContext context, string arg)
        {
            if (arg.ToLower() == "customsqldependency")
            {
                StringBuilder categories = new StringBuilder();
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(@"SELECT [CategoryId], [CategoryName], [CategoryDescription], [CategoryImage], [OrderBy] FROM Categories", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                categories.AppendLine(string.Format("{0};{1};{2};{3};{4}", reader.GetGuid(0), reader.GetString(1), reader.GetString(2), reader.IsDBNull(3) == true ? "null" : reader.GetString(3), reader.GetInt32(4)));
                            }
                            return categories.ToString();
                        }
                    }
                }
            }
            return base.GetVaryByCustomString(context, arg);
        }
    }
}
