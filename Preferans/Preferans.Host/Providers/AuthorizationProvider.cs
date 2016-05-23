using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Preferans.Host
{
    class AuthorizationProvider
    {
        internal bool TryAuthorizeUser(HubCallerContext context, out string username)
        {
            username = null;

            string authenticationPathKey = "AuthenticationAddress";

            Uri target = new Uri(ConfigurationManager.AppSettings[authenticationPathKey]);

            RestClient client = new RestClient(target.AbsoluteUri);

            string cookieName = ".AspNet.ApplicationCookie";

            if (context.RequestCookies.Keys.Contains(cookieName))
            {
                var cookie = new System.Net.Cookie(cookieName, context.RequestCookies[cookieName].Value);
                cookie.Domain = target.Host;

                client.Cookie = cookie;
            }

            var token = context.Headers["Authorization"];

            if (token != null)
            {
                client.Headers = new System.Collections.Specialized.NameValueCollection();
                client.Headers.Add("Authorization", token);
            }            

            string user = client.MakeRequest();

            if (String.IsNullOrEmpty(user)) return false;
                        
            UserMapping users = new UserMapping();
            users.Add(context.ConnectionId, user);
            username = user;

            return true;
        }

        internal void RemoveUser(HubCallerContext context)
        {
            UserMapping users = new UserMapping();
            users.Remove(context.ConnectionId);
        }
    }
}
