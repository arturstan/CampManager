using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CampManagerWebUI.Startup))]
namespace CampManagerWebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
