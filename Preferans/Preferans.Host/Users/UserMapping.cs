using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Preferans.Host
{
    class UserMapping
    {
        private readonly static Dictionary<string, string> _users = new Dictionary<string, string>();

        public string GetUser(string connectionId)
        {
            string user;

            if (_users.TryGetValue(connectionId, out user)) return user;

            return null;
        }

        public IEnumerable<string> GetAllUsers()
        {
            return _users.Keys.Select(key => _users[key]);
        }

        public void Add(string connectionId, string user)
        {
            lock (_users)
            {
                _users.Add(connectionId, user);
            }
        }

        public void Remove(string connectionId)
        {
            lock (_users)
            {
                _users.Remove(connectionId);
            }
        }
    }
}
