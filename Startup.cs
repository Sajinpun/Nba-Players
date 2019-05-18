using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CricketPlayers.Startup))]
namespace CricketPlayers
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
