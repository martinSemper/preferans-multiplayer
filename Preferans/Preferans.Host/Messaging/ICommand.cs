using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Preferans.Host.Messaging
{
    interface ICommand
    {
        void Execute(params string[] parameters);
    }
}
