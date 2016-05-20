using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Preferans.Host
{    
    class AuthorizeHubMethodAccessAttribute : AuthorizeAttribute
    {
        public override bool AuthorizeHubMethodInvocation(Microsoft.AspNet.SignalR.Hubs.IHubIncomingInvokerContext hubIncomingInvokerContext, bool appliesToMethod)
        {
            
            UserMapping users = new UserMapping();

            string user = users.GetUser(hubIncomingInvokerContext.Hub.Context.ConnectionId);

            return user != null;                     
        }        
    }
}
