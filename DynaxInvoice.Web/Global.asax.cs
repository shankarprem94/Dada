using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;

namespace DynaxInvoice.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Sid;
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Name;
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Role;
        }
    }
}
