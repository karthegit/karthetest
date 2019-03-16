using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Ceb.MerlinTool.Startup))]
namespace Ceb.MerlinTool
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
