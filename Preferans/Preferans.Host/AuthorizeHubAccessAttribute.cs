﻿using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Preferans.Host
{    
    class AuthorizeHubAccessAttribute : AuthorizeAttribute
    {
        public override bool AuthorizeHubMethodInvocation(Microsoft.AspNet.SignalR.Hubs.IHubIncomingInvokerContext hubIncomingInvokerContext, bool appliesToMethod)
        {
            Uri target = new Uri("http://localhost:2197/api/Account/verifyuserauthenticity");

            RestClient client = new RestClient(target.AbsoluteUri);

            string cookieName = ".AspNet.ApplicationCookie";

            if (!hubIncomingInvokerContext.Hub.Context.RequestCookies.Keys.Contains(cookieName)) return false;

            var cookie = new System.Net.Cookie(cookieName, hubIncomingInvokerContext.Hub.Context.RequestCookies[cookieName].Value);
            cookie.Domain = target.Host;

            client.Cookie = cookie;

            string response = client.MakeRequest();

            return response == "true";           
        }
    }
}
