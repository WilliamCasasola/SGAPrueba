using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SGA.Startup))]
namespace SGA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
