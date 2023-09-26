using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osadnici
{
    public class Player
    {
        public Color Color { get; set; }
        public int Points { get; set; }
        public Activity Activity { get; set; }
        public List<SamePawnSet> PawnSets;
        public List<SameCardsSet> Cards;

        public Player(Color color)
        {
            this.Points = 0;
            this.Activity = Activity.StartFirstVillage;
            this.Color = color;
            this.PawnSets = SetPawnsList();
            this.Cards = SetCardsList();
        }

        public bool IsEnoughPawns(PawnType type)
        {
            foreach (var pawnSet in PawnSets)
            {
                if (pawnSet.Type == type)
                {
                    if (pawnSet.PawnsCount > 0)
                        return true;
                    else
                    {
                        return false;
                    }
                }
            }
            throw new Exception(); // pawn does not exist
        }

        // buys a pawn according to recipe using player's cards if successful returns true
        public bool Buy(Recipe recipe)
        {
            if (!this.CheckPawnInInventory(recipe.PawnName)) // if pawn is not in inventory I cannot buy it
                return false;

            if (!this.CheckCardsForRecipe(recipe.Manual)) // player does not have material for recipe;
                return false;

            this.BuyByRecipe(recipe); // remove things from inventory according to recipe
            return true;

        }

        // adds one pawn to the list of given type
        public void AddPawn(PawnType pawnType)
        {
            foreach (var pawnSet in PawnSets)
            {
                if (pawnSet.Type == pawnType)
                {
                    pawnSet.PawnsCount++;
                    return;
                }
            }
            throw new Exception(); // pawn does not exist
        }

        // removes one pawn from list
        public void RemovePawn(PawnType pawnType)
        {
            foreach (var pawnSet in PawnSets)
            {
                if (pawnSet.Type == pawnType)
                {
                    if (pawnSet.PawnsCount <= 0)
                        throw new Exception(); // should have been positive
                    else
                    {
                        pawnSet.PawnsCount--;
                        return;
                    }
                }
            }
            throw new Exception(); // pawn does not exist
        }

        // checks if searched pawn is in players inventory
        public bool CheckPawnInInventory(PawnType searchedPawn)
        {
            foreach (var inventoryPawn in this.PawnSets)
            {
                if (inventoryPawn.Type == searchedPawn)
                {
                    if (inventoryPawn.PawnsCount > 0)
                        return true;
                    else
                        return false;
                }
            }
            throw new Exception(); //pawn is not declared as possibility in players inventory
        }

        // removes cards from players inventory according to recipe
        public void BuyByRecipe(Recipe recipe)
        {
            foreach (var recipeCard in recipe.Manual)
            {
                foreach (var playerCard in this.Cards)
                {
                    if (recipeCard.Material == playerCard.Material)
                    {
                        playerCard.CardsCount = playerCard.CardsCount - recipeCard.CardsCount;
                    }
                }
            }
        }

        // inits list of pawns for player and returns it
        private List<SameCardsSet> SetCardsList()
        {
            var cardsList = new List<SameCardsSet>();
            foreach (Material material in Enum.GetValues(typeof(Material)))
            {
                if (material != Material.None)
                    cardsList.Add(new SameCardsSet(material: material, cardsCount: 1)); // begin with one card from each material
            }
            return cardsList;
        }

        // inits list of pawns for player and returns it
        private List<SamePawnSet> SetPawnsList()
        {
            var pawnsList = new List<SamePawnSet>();
            pawnsList.Add(new Road(color: this.Color));
            pawnsList.Add(new Village(color: this.Color));
            pawnsList.Add(new Town(color: this.Color));

            return pawnsList;
        }

        // returns ordered pawns by enum PawnType
        public List<SamePawnSet> GetOrderedPawns()
        {
            return PawnSets.OrderBy(x => x.Type).ToList();
        }

        // returns ordered cards by enum Material
        public List<SameCardsSet> GetOrderedCards()
        {
            return this.Cards.OrderBy(x => x.Material).ToList();
        }

        public bool checkPoints()
        {
            return Points == 10;
        }

        // check if it is possible to sell constant amount of given material
        public bool Sell(Material material, Game gameLogic)
        {

            foreach (SameCardsSet cardSet in this.Cards)
            {
                if (cardSet.Material == material)
                {
                    if (cardSet.CardsCount >= gameLogic.SellConstant)
                    {
                        cardSet.CardsCount = cardSet.CardsCount - gameLogic.SellConstant;

                        this.Points++;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
            throw new Exception(); // card does not exist
        }
        public int GetSumOfCards()
        {
            return Cards.Sum(c => c.CardsCount);
        }

        public bool CheckCardsForRecipe(List<SameCardsSet> recipe)
        {
            foreach (SameCardsSet recipeCard in recipe)
            {
                bool recipeCardFound = false;
                foreach (SameCardsSet playerCard in this.Cards)
                {
                    if (recipeCard.Material == playerCard.Material)
                    {
                        recipeCardFound = true;
                        if (recipeCard.CardsCount > playerCard.CardsCount)
                            return false;
                    }
                }
                if (!recipeCardFound)
                    return false;
            }
            return true;
        }
    }
}
