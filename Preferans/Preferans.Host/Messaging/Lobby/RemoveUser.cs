using Microsoft.AspNet.SignalR.Hubs;
using Preferans.Host.DAL;
using Preferans.Host.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Preferans.Host.Messaging.Lobby
{
    class RemoveUser : ICommand
    {
        IHubCallerConnectionContext<dynamic> _clients;
        IPlayerRepository _players = new PlayerDbRepository();

        public RemoveUser(IHubCallerConnectionContext<dynamic> clients)
        {
            _clients = clients;
        }

        public void Execute(params string[] parameters)
        {
            if (parameters.Length == 0) throw new InvalidOperationException("Username value must be placed as a first value within parameters");

            string username = parameters[0];

            Player player = _players.GetPlayer(username);

            if (player == null) throw new InvalidOperationException(String.Format("Player with username {0} is not a member of a repository", username));

            _clients.Others.removePlayer(username);

            GroupMapping groups = new GroupMapping();
            Group group = groups.Get(username);

            if (group != null)
            {
                groups.RemoveMember(username);
                _clients.Others.removePlayer(username);

                if (group == null)
                    _clients.Others.removeGroup(username);
            }               

        }
    }
}
