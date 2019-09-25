using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AssignmentFIT5032.Startup))]
namespace AssignmentFIT5032
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
