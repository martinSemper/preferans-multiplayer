﻿using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Preferans.Host.DAL;
using Preferans.Host.Environment;
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
        public void CreateRoom()
        {
            Lobby lobby = new Lobby(Clients);
            lobby.CreateRoom(Context.ConnectionId, Groups);
        }

        [AuthorizeHubMethodAccess]
        public void JoinRoom(string groupId)
        {
            Lobby lobby = new Lobby(Clients);
            lobby.AdRoomMember(groupId, Context.ConnectionId, Groups);            
        }

        public override Task OnConnected()
        {
            AuthorizationProvider authorization = new AuthorizationProvider();

            string username;
            if (authorization.TryAuthorizeUser(Context, out username))
            {
                try
                {
                    Lobby lobby = new Lobby(Clients);
                    lobby.AddUser(username, Context.ConnectionId);
                }
                catch(InvalidOperationException ioe)
                {
                    Clients.Caller.sendErrorMessage(ioe.Message);
                }                
            }
            
            return base.OnConnected();
        }


        public override Task OnDisconnected(bool stopCalled)
        {
            try
            {
                Lobby lobby = new Lobby(Clients);
                lobby.RemoveUser(Context.ConnectionId);
            }
            catch(InvalidOperationException ioe)
            {
                Clients.Caller.sendErrorMessage(ioe.Message);
            }
                        
            return base.OnDisconnected(stopCalled);
        }
    }
}
