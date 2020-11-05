using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;

namespace Parakeet
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var Settings = new FriendlyUrlSettings();
            Settings.AutoRedirectMode = RedirectMode.Off;
            routes.EnableFriendlyUrls(Settings);
        }
    }
}
