using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PickAndBook.Startup))]
namespace PickAndBook
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
