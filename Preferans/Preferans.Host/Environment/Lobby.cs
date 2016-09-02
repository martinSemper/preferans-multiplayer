using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Preferans.Host.DAL;
using Preferans.Host.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Preferans.Host.Environment
{
    class Lobby
    {
        IHubCallerConnectionContext<dynamic> _clients;
        IPlayerRepository _players = new PlayerDbRepository();
        UserMapping _users = new UserMapping();
        GroupMapping _groups = new GroupMapping();

        public Lobby(IHubCallerConnectionContext<dynamic> clients)
        {
            _clients = clients;
        }

        internal void AddUser(string username, string connectionId)
        {
            
            _users.Add(connectionId, username);
            
            Player player = _players.TryRegisterPlayer(username);

            if (player == null)
                throw new InvalidOperationException(String.Format("Failed to register user {0}", username));

            Console.WriteLine("Player {0} joined the lobby", username);                     

            _clients.Others.addPlayer(player);

            var allUsers = _users.GetAllUsers();
            _clients.Caller.addExistingPlayers(_players.GetPlayers(allUsers.Select(u => u.Username)));

            _clients.Caller.addExistingRooms(_groups.GetAllGroups());
        }

        internal void RemoveUser(string connectionId)
        {
            User user = _users.GetUser(connectionId);

            if (user == null) throw new InvalidOperationException(String.Format("User with given connection Id does not exist"));

            _users.Remove(connectionId);

            Console.WriteLine("Player {0} exited the lobby", user.Username);                     

            _clients.Others.removePlayer(user.Username);

            Group group = _groups.Get(user.Username);

            if (group != null)
            {
                string id = group.Id;
                _groups.RemoveMember(user.Username);

                if (_groups.Get(id) != null)
                    _clients.Others.removeRoomMember(group);
                else
                    _clients.Others.removeRoom(id);
            }
        }

        internal async Task AdRoomMember(string groupId, string connectionId, IGroupManager groups)
        {
            UserMapping users = new UserMapping();
            User user = users.GetUser(connectionId);

            GroupMapping rooms = new GroupMapping();

            try
            {
                Group group = rooms.AddMember(user.Username, groupId);
                await groups.Add(connectionId, groupId);
                _clients.All.addRoomMember(group);                
            }
            catch (InvalidOperationException e)
            {
                _clients.Caller.displayErrorMessage(e.Message);
            }
        }

        
        internal async Task CreateRoom(string connectionId, IGroupManager groups)
        {
            UserMapping users = new UserMapping();
            User user = users.GetUser(connectionId);

            GroupMapping rooms = new GroupMapping();

            try
            {
                Group room = rooms.Create(user.Username);

                await groups.Add(connectionId, user.Username);
                _clients.Caller.enterRoom(room);
                _clients.Others.addRoom(room);
            }
            catch(InvalidOperationException ioe)
            {
                _clients.Caller.displayErrorMessage(ioe.Message);
            }
        }
    }
}
