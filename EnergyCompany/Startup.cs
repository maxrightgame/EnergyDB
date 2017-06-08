using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EnergyCompany.Startup))]
namespace EnergyCompany
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
