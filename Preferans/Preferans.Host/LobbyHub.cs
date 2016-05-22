using Microsoft.AspNet.SignalR;
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

        public override Task OnConnected()
        {
            AuthorizationProvider authorization = new AuthorizationProvider();
            authorization.AuthorizeUser(Context);

            UserMapping users = new UserMapping();

            Console.WriteLine("users:");
            foreach(var user in users.GetAllUsers())
            {
                Console.WriteLine(user);
            }
            
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            AuthorizationProvider authorization = new AuthorizationProvider();
            authorization.RemoveUser(Context);

            UserMapping users = new UserMapping();

            Console.WriteLine("users:");
            foreach (var user in users.GetAllUsers())
            {
                Console.WriteLine(user);
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}
