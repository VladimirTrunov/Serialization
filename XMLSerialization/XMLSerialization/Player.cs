using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLSerialization
{
    public class Player : User
    {
        public int PlayRank
        {
            get; set;
        }
        public Player(string n, int a, int playRank) : base(n, a)
        {
            PlayRank = playRank;
        }
        public Player() { }

    }
}
