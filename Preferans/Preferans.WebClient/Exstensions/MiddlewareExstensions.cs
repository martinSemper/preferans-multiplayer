using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Preferans.WebClient
{
    public static class MiddlewareExstensions
    {
        public static bool IsApiAuthentication(this Microsoft.Owin.Security.Cookies.CookieApplyRedirectContext context)
        {
            return (context.Request.Uri.Segments.Contains("api/"));
        }
    }
}