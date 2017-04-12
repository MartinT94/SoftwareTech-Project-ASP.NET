using ASP.NET_Blog.Migrations;
using System.Data.Entity;
using ASP.NET_Blog.Models;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ASP.NET_Blog.Startup))]
namespace ASP.NET_Blog
{
    public partial class Startup
    {
        
        public void Configuration(IAppBuilder app)
        {
            Database.SetInitializer(
            new MigrateDatabaseToLatestVersion<BlogDbContext, Configuration>());
            ConfigureAuth(app);
        }
    }
}
