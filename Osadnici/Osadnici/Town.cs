using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osadnici
{
    public class Town : Building
    {
        public Town(Color color) : base(color: color)
        {
            this.Type = PawnType.Town;
            this.PawnsCount = 4;
        }
    }
}
