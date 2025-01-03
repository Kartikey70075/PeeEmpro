using Golden_Terry_Towels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Golden_Terry_Towels
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        //protected void Application_BeginRequest()
        //{
        //    Response.Cache.SetCacheability(HttpCacheability.Public); // Set cacheability to Public if appropriate
        //    Response.Cache.SetExpires(DateTime.UtcNow.AddHours(8)); // Set expiration time to 8 hours from now
        //    Response.Cache.SetMaxAge(TimeSpan.FromHours(8)); // Set Max-Age to 8 hours to control caching duration
        //    Response.Cache.SetSlidingExpiration(true); // Enable sliding expiration if needed
        //}
        public override void Init()
        {
            this.PostAuthenticateRequest += MvcApplication_PostAuthenticateRequest;
            base.Init();
        }
        void MvcApplication_PostAuthenticateRequest(object sender,EventArgs e)
        {
            System.Web.HttpContext.Current.SetSessionStateBehavior( System.Web.SessionState.SessionStateBehavior.Required
                );
        }
    }
}
