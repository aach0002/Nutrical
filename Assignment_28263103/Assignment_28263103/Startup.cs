using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Assignment_28263103.Startup))]
namespace Assignment_28263103
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
