using Preferans.Host.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Preferans.Host.DAL
{
    class PlayerDbRepository : IPlayerRepository
    {
        private readonly PlayerContext _context;

        public PlayerDbRepository()
        {
            _context = new PlayerContext();
        }

        public Player GetPlayer(string username)
        {
            return _context.Players.SingleOrDefault(p => p.Username == username);
        }

        public Player RegisterPlayer(string username)
        {
            Player player = GetPlayer(username);

            if (player != null) throw new InvalidOperationException(String.Format("Player with username {0} already exists in the database", username));

            player = new Player()
            {
                Username = username,
                GamesPlayed = 0,
                Score = 0
            };

            _context.Players.Add(player);
            _context.SaveChanges();

            return player;
        }


        public IEnumerable<Player> GetPlayers(IEnumerable<string> users)
        {
            IEnumerable<Player> players = _context.Players.Where(p => users.Contains(p.Username));

            return players;
        }


        public Player TryRegisterPlayer(string username)
        {
            Player player = null;

            player = GetPlayer(username);

            if (player == null) player = RegisterPlayer(username);

            return player;
        }
    }
}
