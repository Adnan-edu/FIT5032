using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(W7Authentication.Startup))]
namespace W7Authentication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
