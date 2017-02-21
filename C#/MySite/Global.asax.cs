using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MySite.Models;

namespace MySite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //初始化OpenApiClient
            OpenApi.Utility.OpenApiClient.Init(AppProperty.AppID, OpenApi.Utility.PKCSType.PKCS8, AppProperty.AppPublicKey, AppProperty.AppPrivateKey);
        }
    }
}
