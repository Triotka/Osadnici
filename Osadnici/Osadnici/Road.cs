using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osadnici
{
    public class Road : SamePawnSet
    {
        public Road(Color color) : base(color: color)
        {
            this.Type = PawnType.Road;
            this.PawnsCount = 15; //from rules
        }
    }
}
