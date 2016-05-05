using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Preferans.WebClient.Startup))]
namespace Preferans.WebClient
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
