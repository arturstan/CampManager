using CampManager.Domain.User;
using CampManagerWebUI.Models;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CampManagerWebUI
{
    public class AuthorizeCustomAttribute : AuthorizeAttribute
    {
        private readonly List<Role> _allowedroles;
        public AuthorizeCustomAttribute(params Role[] roles)
        {
            _allowedroles = roles.ToList();
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.User == null)
                return false;

            return UserOrganizationRolesHelper.IsUserRole(httpContext.User.Identity.Name, _allowedroles);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
               new RouteValueDictionary
               {
                    { "controller", "Home" },
                    { "action", "UnAuthorized" }
               });
        }
    }
}