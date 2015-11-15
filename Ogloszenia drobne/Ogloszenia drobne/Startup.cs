using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Ogloszenia_drobne.Startup))]
namespace Ogloszenia_drobne
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
