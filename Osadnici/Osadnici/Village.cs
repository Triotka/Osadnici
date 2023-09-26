using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osadnici
{
    public class Village : Building
    {
        public Village(Color color) : base(color: color)
        {
            this.Type = PawnType.Village;
            this.PawnsCount = 5; // from rules
        }
    }
}
