﻿using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Preferans.Host
{
    
    public class LobbyHub : Hub
    {
        [AuthorizeHubAccess]
        public void Send(string name, string message)
        {
            Uri target = new Uri("http://localhost:2197/verification");

            RestClient client = new RestClient(target.AbsoluteUri);

            string cookieName = ".AspNet.ApplicationCookie";

            var cookie = new System.Net.Cookie(cookieName, this.Context.RequestCookies[cookieName].Value);
            cookie.Domain = target.Host;

            client.Cookie = cookie;

            string response = client.MakeRequest();


            this.Clients.All.addMessage(name, message);
        }

        public void MakeMove(string name)
        {
            this.Clients.All.makeMove(name);
        }
    }
}
