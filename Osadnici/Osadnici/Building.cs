using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osadnici
{
    public class Building : SamePawnSet
    {
        // cards gained according to type of building
        public int BuildingToCardsNum()
        {
            if (this.Type == PawnType.Village)
                return 1;
            if (this.Type == PawnType.Town)
                return 2;
            throw new Exception();
        }


        public Building(Color color) : base(color: color)
        {
        }



        // adds cards to player according of building value
        public void AddCardsToPlayer(Hexagon hexagon, List<Player> players)
        {
            var belongPlayer = this.MatchPlayer(players);
            int cardsGained = this.BuildingToCardsNum();
            Material material = hexagon.Material;

            //LINQ
            var cardInHand = from cardSet in belongPlayer.Cards where cardSet.Material == material select cardSet;
            if (cardInHand.Count() == 1)
            {
                foreach (var cardSet in cardInHand)
                    cardSet.CardsCount = cardSet.CardsCount + cardsGained;
            }
            else
            {
                throw new Exception(); // card with material does not exist
            }


        }
    }
}
