using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Preferans.Host.DAL;
using Preferans.Host.Messaging.Lobby;
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
        public void CreateGame()
        {
            UserMapping users = new UserMapping();
            User user = users.GetUser(Context.ConnectionId);

            GroupMapping groups = new GroupMapping();

            try
            {
                Group group = groups.Create(user.Username);
                Groups.Add(Context.ConnectionId, user.Username);
                Clients.Caller.enterRoom(group);
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
                LobbyMessenger messenger = new LobbyMessenger(Clients);

                try
                {
                    users.Add(Context.ConnectionId, username);
                }
                catch(InvalidOperationException e)
                {
                    
                    Clients.Caller.displayErrorMessage(e.Message);
                    return base.OnConnected();
                }

                IPlayerRepository players = new PlayerDbRepository();
                Player player = players.TryRegisterPlayer(username);

                messenger.AddUser(username);

                Console.WriteLine("Player {0} joined the lobby", player.Username);                     
            }
            
            return base.OnConnected();
        }


        public override Task OnDisconnected(bool stopCalled)
        {
            UserMapping users = new UserMapping();
            User user = users.GetUser(Context.ConnectionId);

            //GroupMapping groups = new GroupMapping();
            //groups.RemoveMember(user.Username);
            
            users.Remove(Context.ConnectionId);
                       

            LobbyMessenger messenger = new LobbyMessenger(Clients);
            messenger.RemoveUser(user.Username);
                        
            return base.OnDisconnected(stopCalled);
        }
    }
}
