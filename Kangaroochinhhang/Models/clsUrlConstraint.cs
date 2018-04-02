using Kangaroochinhhang.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
 namespace Kangaroochinhhang.Models
{
    public class clsUrlConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName,  RouteValueDictionary values, RouteDirection routeDirection)
        {
             KangaroochinhhangContext db = new KangaroochinhhangContext();
             if (values[parameterName] != null)
            {
                var tag = values[parameterName].ToString();
                 return db.SelectListItem.Any(p => p.Tag == tag);
            }
            return false;
        }
    }
}