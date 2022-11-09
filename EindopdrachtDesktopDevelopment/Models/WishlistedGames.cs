using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EindopdrachtDesktopDevelopment.Models
{
    public class WishlistedGames
    {
        private int id;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private int gamerId;

        public int GamerID
        {
            get { return gamerId; }
            set { gamerId = value; }
        }

        private int gameId;

        public int GameID
        {
            get { return gameId; }
            set { gameId = value; }
        }

    }
}

