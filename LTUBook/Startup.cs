using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LTUBook.Startup))]
namespace LTUBook
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
