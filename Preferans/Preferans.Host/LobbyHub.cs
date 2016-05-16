using Microsoft.AspNet.SignalR;
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
            this.Clients.All.addMessage(name, message);
        }

        public void MakeMove(string name)
        {
            this.Clients.All.makeMove(name);
        }
    }
}
