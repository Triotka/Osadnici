using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osadnici
{
    public class Neighbours
    {
        public Hexagon UpLeft { get; set; }
        public Hexagon UpRight { get; set; }
        public Hexagon DownLeft { get; set; }
        public Hexagon DownRight { get; set; }
        public Hexagon Left { get; set; }
        public Hexagon Right { get; set; }

    }
}
