using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osadnici
{
    public class SamePawnSet
    {
        public PawnType Type { get; init; }
        public Color Color { get; init; }
        public int PawnsCount { get; set; }


        public SamePawnSet(Color color)
        {
            this.PawnsCount = 0;
            this.Color = color;
            this.Type = PawnType.None; //default is None
        }
        public Player MatchPlayer(List<Player> players)
        {
            foreach (Player player in players)
            {
                if (player.Color == this.Color)
                    return player;
            }
            throw new Exception(); // player of given color does not exist
        }

    }
}
