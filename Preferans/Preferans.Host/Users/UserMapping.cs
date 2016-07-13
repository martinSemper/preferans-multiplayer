using Preferans.Host.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Preferans.Host
{
    class UserMapping
    {
        private readonly static Dictionary<string, User> _users = new Dictionary<string, User>();

        public User GetUser(string connectionId)
        {
            User user;

            if (_users.TryGetValue(connectionId, out user)) return user;

            return null;
        }

        public IEnumerable<User> GetAllUsers()
        {            
            return _users.Values;
        }

        public void Add(string connectionId, string username)
        {
            lock (_users)
            {
                if (!CheckUserUnicity(username)) throw new InvalidOperationException(String.Format("User {0} is already connected through another device", username));
                User user = new User() { Username = username, UtcConnected = DateTime.UtcNow };
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

        private bool CheckUserUnicity(string username)
        {
            return !_users.Values.Select(v => v.Username).Contains(username);
        }
    }
}
