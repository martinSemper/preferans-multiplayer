using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Preferans.Host.Messaging.Lobby
{
    class LobbyMessenger
    {
        ICommand _addUser;
        ICommand _removeUser;
        ICommand _sendErrorMessage;

        public LobbyMessenger(IHubCallerConnectionContext<dynamic> clients)
        {
            _addUser = new AddUser(clients);
            _sendErrorMessage = new SendErrorMessage(clients);
            _removeUser = new RemoveUser(clients);
        }

        public void AddUser(string username)
        {
            _addUser.Execute(username);
        }

        public void RemoveUser(string username)
        {
            _removeUser.Execute(username);
        }

        public void SendErrorMessage(string message)
        {            
            _sendErrorMessage.Execute(message);
        }
    }
}
