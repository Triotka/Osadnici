using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osadnici
{
    public class Recipe
    {
        public List<SameCardsSet> Manual { get; set; }
        public PawnType PawnName { get; set; }
        public Recipe(List<SameCardsSet> recipe, PawnType pawnName)
        {
            this.Manual = recipe;
            this.PawnName = pawnName;
        }
    }
}
