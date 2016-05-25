using Preferans.Host.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Preferans.Host.DAL
{
    interface IPlayerRepository
    {
        Player GetPlayer(string username);
        Player RegisterPlayer(string username);
        IEnumerable<Player> GetPlayers(IEnumerable<string> users);        
    }
}
