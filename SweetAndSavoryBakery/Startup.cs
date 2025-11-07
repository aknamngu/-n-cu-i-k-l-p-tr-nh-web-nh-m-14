using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SweetAndSavoryBakery.Startup))]
namespace SweetAndSavoryBakery
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
