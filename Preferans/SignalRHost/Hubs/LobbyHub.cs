using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRHost.Hubs
{
    class LobbyHub : Hub
    {
        public void Send(string name, string message)
        {
            this.Clients.All.addMessage(name, message);
        }   
    }
}
