using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osadnici
{
    public class Pirate
    {
        public int Location { get; set; }
        public Pirate(int startHexagon)
        {
            Location = startHexagon;
        }
        public bool PlacePirate(int clickedHexagonIndex, Game gameLogic)
        {
            if (Location == clickedHexagonIndex) // cannot place pirate on the same hexagon
            {
                return false;
            }
            else
            {
                Location = clickedHexagonIndex;
                gameLogic.GetCurrentPlayer().Activity = Activity.None;
                return true;

            }
        }
    }
}
