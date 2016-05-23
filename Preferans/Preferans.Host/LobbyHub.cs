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
        public void Send(string name, string message)
        {
            this.Clients.All.addMessage(name, message);
        }

        [AuthorizeHubMethodAccess]
        public void MakeMove(string name)
        {          
            this.Clients.All.makeMove(name);
        }    
    
        [AuthorizeHubMethodAccess]
        public void CreateRoom()
        {
            
        }

        public override Task OnConnected()
        {
            AuthorizationProvider authorization = new AuthorizationProvider();

            string username;
            if (authorization.TryAuthorizeUser(Context, out username))
            {
                IPlayerRepository players = new PlayerDbRepository();

                Player player = null;

                player = players.GetPlayer(username);

                if (player == null) player = players.RegisterPlayer(username);

                string json = JsonConvert.SerializeObject(player);

                Console.WriteLine("Player {0} joined the lobby", player.Username);
                Clients.All.addPlayer(json);
            }
            
            

            //UserMapping users = new UserMapping();
            
            //Console.WriteLine("users:");
            //foreach(var user in users.GetAllUsers())
            //{
            //    Console.WriteLine(user);
            //}
            
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            AuthorizationProvider authorization = new AuthorizationProvider();
            authorization.RemoveUser(Context);

            //UserMapping users = new UserMapping();

            //Console.WriteLine("users:");
            //foreach (var user in users.GetAllUsers())
            //{
            //    Console.WriteLine(user);
            //}

            return base.OnDisconnected(stopCalled);
        }
    }
}
