using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SwissMoteWebsite.Startup))]
namespace SwissMoteWebsite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
