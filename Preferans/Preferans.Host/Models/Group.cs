using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Preferans.Host.Models
{
    class Group
    {
        const int _capacity = 3;
        string _id;
        HashSet<string> _members = new HashSet<string>();

        public Group(string owner)
        {
            _id = owner;
            _members.Add(owner);           
        }

        public string Id { get { return _id; } }

        public IEnumerable<string> Members { get { return _members; } }

        public bool Add(string member)
        {
            if (_members.Count == _capacity) return false;

            return _members.Add(member);
        }

        public bool Remove(string member)
        {
            if (member == _id) return false;

            return _members.Remove(member);
        }
    }
}
