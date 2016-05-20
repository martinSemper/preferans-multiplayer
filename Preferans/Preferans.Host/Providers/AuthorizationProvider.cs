using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
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
        internal void AuthorizeUser(HubCallerContext context)
        {
            Uri target = new Uri("http://localhost:2197/api/Account/verifyuserauthenticity");

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

            if (String.IsNullOrEmpty(user)) return;
                        
            UserMapping users = new UserMapping();
            users.Add(context.ConnectionId, user);                      
        }
    }
}
