﻿using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BaselineSolution.WebApp.App_Start;
using BaselineSolution.WebApp.Infrastructure.Constants;
using BaselineSolution.WebApp.Infrastructure.Models.Authentication;

namespace BaselineSolution.WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_PreRequestHandlerExecute()
        {
            // If there is no httpcontext, no session or an authenticated user, there's nothing we need to do
            if (HttpContext.Current == null
                || HttpContext.Current.Request.IsAuthenticated
                || HttpContext.Current.Session == null)
                return;

            // Try to get the current user from session and set it in the httpcontext

            if (HttpContext.Current.Session != null)
            {
                object user = HttpContext.Current.Session[SessionVariables.User];
                if (user != null)
                {
                    HttpContext.Current.User = (AuthenticatedCustomPrincipal)user;
                }
            }
        }
    }
}