using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Preferans.Host.DAL;
using Preferans.Host.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Preferans.Host
{    
    public class LobbyHub : Hub
    {
        [AuthorizeHubMethodAccess]
        public void Send(string message)
        {
            UserMapping users = new UserMapping();
            User user = users.GetUser(Context.ConnectionId);

            this.Clients.All.addMessage(user.Username, message);
        }

        [AuthorizeHubMethodAccess]
        public void MakeMove()
        {
            UserMapping users = new UserMapping();
            User user = users.GetUser(Context.ConnectionId);

            this.Clients.All.makeMove(user.Username);
        }    
    
        [AuthorizeHubMethodAccess]
        public void CreateRoom()
        {
            UserMapping users = new UserMapping();
            User user = users.GetUser(Context.ConnectionId);

            GroupMapping groups = new GroupMapping();

            try
            {
                Group group = groups.Create(user.Username);
                Clients.All.addRoom(group);
            }
            catch(InvalidOperationException e)
            {
                Clients.Caller.displayErrorMessage(e.Message);
            }            
        }

        [AuthorizeHubMethodAccess]
        public void JoinRoom(string groupId)
        {
            UserMapping users = new UserMapping();
            User user = users.GetUser(Context.ConnectionId);

            GroupMapping groups = new GroupMapping();

            try
            {
                Group group = groups.AddMember(user.Username, groupId);                
                Clients.All.addRoomMember(group);
            }
            catch(InvalidOperationException e)
            {
                Clients.Caller.displayErrorMessage(e.Message);
            }
        }

        public override Task OnConnected()
        {
            AuthorizationProvider authorization = new AuthorizationProvider();

            string username;
            if (authorization.TryAuthorizeUser(Context, out username))
            {
                UserMapping users = new UserMapping();

                users.Add(Context.ConnectionId, username);

                IPlayerRepository players = new PlayerDbRepository();

                Player player = null;

                player = players.GetPlayer(username);

                if (player == null) player = players.RegisterPlayer(username);

                                
                Console.WriteLine("Player {0} joined the lobby", player.Username);

                Clients.AllExcept(Context.ConnectionId).addPlayer(player);

                var allUsers = users.GetAllUsers().OrderBy(u => u.UtcConnected);
                Clients.Caller.addExistingPlayers(players.GetPlayers(allUsers.Select(u => u.Username)));
            }
            
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            UserMapping users = new UserMapping();
            User user = users.GetUser(Context.ConnectionId);

            users.Remove(Context.ConnectionId);
            Clients.All.removePlayer(user.Username);
                        
            return base.OnDisconnected(stopCalled);
        }
    }
}
