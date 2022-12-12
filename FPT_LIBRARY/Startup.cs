using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FPT_LIBRARY.Startup))]
namespace FPT_LIBRARY
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
