using Preferans.Host.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Preferans.Host
{
    class GroupMapping
    {
        private readonly static List<Group> _groups = new List<Group>();

        public Group Get(string username)
        {
            return _groups.SingleOrDefault(g => g.Members.Any(m => m == username));
        }

        public IEnumerable<Group> GetAllGroups()
        {
            return _groups;
        }
        public Group Create(string username)
        {
            if (!CheckUserUnicity(username)) throw new InvalidOperationException(String.Format("User {0} is already a member of a group", username));

            Group group = null;

            lock (_groups)
            {
                group = new Group(username);
                _groups.Add(group);
            }

            return group;
        }
        
        public Group AddMember(string username, string groupId)
        {
            Group group = _groups.SingleOrDefault(g => g.Id == groupId);

            if (group == null) throw new InvalidOperationException(String.Format("Group with Id {0} does not exist", groupId));

            if (!CheckUserUnicity(username)) throw new InvalidOperationException(String.Format("User {0} is already a member of some group", username));

            lock (group)
            {
                if (!group.Add(username)) throw new InvalidOperationException(String.Format("Member {0} is already a member of this group", username)); 
            }           

            return group;
        }

        private bool CheckUserUnicity(string username)
        {
            var allMembers = _groups.SelectMany(g => g.Members);

            return !allMembers.Contains(username);
        }



        internal void RemoveMember(string username)
        {
            string member = _groups.SelectMany(g => g.Members).SingleOrDefault(m => m == username);
            
            if (member != null)
            {
                lock(_groups)
                {
                    var group = _groups.Single(g => g.Members.Any(m => m == username));

                    if (group.Id == username)
                    {
                        _groups.Remove(group);
                        return;
                    }

                    group.Remove(username);
                }
            }
        }
    }
}
