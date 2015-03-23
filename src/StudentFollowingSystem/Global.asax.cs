using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using StudentFollowingSystem.AutoMapper;
using StudentFollowingSystem.Data.Dommel;

namespace StudentFollowingSystem
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AutoMapperConfig.Init();
            FluentMapper.Initialize(c =>
                                    {
                                        c.AddMap(new ClassesMap());
                                        c.ForDommel();
                                    });
        }
    }
}