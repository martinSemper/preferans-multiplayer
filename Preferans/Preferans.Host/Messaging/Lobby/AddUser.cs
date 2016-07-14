using Microsoft.AspNet.SignalR.Hubs;
using Preferans.Host.DAL;
using Preferans.Host.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Preferans.Host.Messaging
{
    class AddUser : ICommand
    {
        IHubCallerConnectionContext<dynamic> _clients;
        IPlayerRepository _players = new PlayerDbRepository();
        UserMapping _users = new UserMapping();

        public AddUser(IHubCallerConnectionContext<dynamic> clients)
        {
            _clients = clients;
        }
             
        public void Execute(params string[] parameters)
        {
            if (parameters.Length == 0) throw new InvalidOperationException("Username value must be placed as a first value within parameters");

            string username = parameters[0];

            Player player = _players.GetPlayer(username);

            if (player == null) throw new InvalidOperationException(String.Format("Player with username {0} is not a member of a repository", username));

            _clients.Others.addPlayer(player);

            var allUsers = _users.GetAllUsers();
            _clients.Caller.addExistingPlayers(_players.GetPlayers(allUsers.Select(u => u.Username)));

            GroupMapping groups = new GroupMapping();
            _clients.Caller.addExistingGroups(groups.GetAllGroups());
        }
    }
}
