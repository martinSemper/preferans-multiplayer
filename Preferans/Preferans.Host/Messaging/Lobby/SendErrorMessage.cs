using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Preferans.Host.Messaging.Lobby
{
    class SendErrorMessage : ICommand
    {
        IHubCallerConnectionContext<dynamic> _clients;

        public SendErrorMessage(IHubCallerConnectionContext<dynamic> clients)
        {
            _clients = clients;
        }
        public void Execute(params string[] parameters)
        {
            
        }
    }
}
