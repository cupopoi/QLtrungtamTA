using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TrungTamTA.Startup))]
namespace TrungTamTA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
