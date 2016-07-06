using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
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
            string appCookieName = ".AspNet.ApplicationCookie";
            string cookieContainerName = "AuthenticationCookie";

            Uri target = new Uri(ConfigurationManager.AppSettings[authenticationPathKey]);

            RestClient client = new RestClient(target.AbsoluteUri);                       

            var token = context.Headers["Authorization"];
            if (token != null)
            {
                client.Headers = new System.Collections.Specialized.NameValueCollection();
                client.Headers.Add("Authorization", token);
            }


            var cookieValue = context.QueryString[cookieContainerName];
            if (cookieValue != null)
            {
                var cookie = new System.Net.Cookie(appCookieName, cookieValue);
                cookie.Domain = target.Host;
                client.Cookie = cookie;
            }

            string user = null;
            try
            {
                user = client.MakeRequest();
            }
            catch(WebException we)
            {
                Console.WriteLine("Authorization error: " + we.Message);
                throw we;
            }
            

            if (String.IsNullOrEmpty(user)) return false;

            username = JsonConvert.DeserializeObject<UserData>(user).Username;

            return true;
        }
    }

    class UserData
    {
        public string Username { get; set; }
    }
}
