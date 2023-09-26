using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osadnici
{
    public class SameCardsSet
    {
        public Material Material { get; set; }
        public int CardsCount { get; set; }

        public SameCardsSet(Material material, int cardsCount)
        {
            CardsCount = cardsCount;
            Material = material;
        }
    }
}
