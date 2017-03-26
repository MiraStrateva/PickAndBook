// Use SingleR
using Microsoft.AspNet.SignalR;

namespace PickAndBook.Hubs
{
    public class LastAddedCompaniesHub : Hub
    {
        public static void UpdateLastAddedCompanies()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<LastAddedCompaniesHub>();
            context.Clients.All.displayCompanies();
        }
    }
}