using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Osadnici
{

    public enum Material // card material
    {
        Brick,
        Wood,
        Wheat,
        Lamb,
        Stone,
        None
    }

    public enum Activity // some important actions during the game
    {
        StartFirstVillage,
        StartSecondVillage,
        StartFirstRoad,
        StartSecondRoad,
        Rolling,
        MovingPirate,
        BuildingVillage,
        BuildingTown,
        BuildingRoad,
        NoPossibilities,
        None
    }
    public enum Color // colors of the players
    {
        Yellow,
        White,
        Red,
        Blue,
        None
    }

    public enum PawnType
    {
        None = -1,
        Road = 0,
        Village = 1,
        Town = 2
    }
    
    public class Game
    {
        public int SellConstant = 10;
        public int PirateNumber = 7;
        public List<Player> Players;
        public Dice Dice;
        public Board Board;
        private int currentPlayer;
        public Pirate Pirate;
        public int minimumPlayers = 2;
        public  int maximumPlayers = 4;
        public Dictionary <PawnType, Recipe> RecipeRules;


        public Game()
        {

            this.Board = new Board();
            Dice = new Dice();
            Pirate = new Pirate(9); // place pirate to the middle
            RecipeRules = SetRecipes();
        }

        // creates recipes for game from rules
        private Dictionary<PawnType, Recipe> SetRecipes()
        {
            var recipes = new Dictionary<PawnType, Recipe>();
            var roadList = new List<SameCardsSet>();
            roadList.Add(new SameCardsSet(Material.Wood, 1));
            roadList.Add(new SameCardsSet(Material.Brick, 1));
            var villageList = new List<SameCardsSet>();
            villageList.Add(new SameCardsSet(Material.Wood, 1));
            villageList.Add(new SameCardsSet(Material.Brick, 1));
            villageList.Add(new SameCardsSet(Material.Lamb, 1));
            villageList.Add(new SameCardsSet(Material.Wheat, 1));
            var townList = new List<SameCardsSet>();
            townList.Add(new SameCardsSet(Material.Stone, 3));
            townList.Add(new SameCardsSet(Material.Wheat, 2));

            recipes.Add(PawnType.Road, new Recipe(pawnName: PawnType.Road, recipe: roadList));
            recipes.Add(PawnType.Village, new Recipe(pawnName: PawnType.Village, recipe: villageList));
            recipes.Add(PawnType.Town, new Recipe(pawnName: PawnType.Town, recipe: townList));
            return recipes;
        }
        public void ChangeBuildActivity()
        {
            var currentPlayer = this.GetCurrentPlayer();

            switch(currentPlayer.Activity)
            {
                case Activity.StartFirstVillage:
                    currentPlayer.Activity = Activity.StartFirstRoad;
                    return;
                case Activity.StartFirstRoad:
                    currentPlayer.Activity = Activity.StartSecondVillage;
                    return;
                case Activity.StartSecondVillage:
                    currentPlayer.Activity = Activity.StartSecondRoad;
                    return;
                case Activity.StartSecondRoad:
                    currentPlayer.Activity = Activity.NoPossibilities;
                    return;
                case Activity.BuildingRoad:
                    currentPlayer.Activity = Activity.None;
                    return;
                case Activity.BuildingVillage:
                    currentPlayer.Activity = Activity.None;
                    return;
                case Activity.BuildingTown:
                    currentPlayer.Activity = Activity.None;
                    return;
            }
            

        }

        public void SetPlayers(int numberOfPlayers)
        {
            Players = new List<Player>();
            currentPlayer = 0;

            var colors = Enum.GetValues(typeof(Color));
            int playersAssigned = 0;
            foreach (Color color in colors)
            {
                Players.Add(new Player(color));
                playersAssigned++;
                if (playersAssigned == numberOfPlayers)
                    break;
            }
        }


        public Player GetCurrentPlayer()
        {
            return Players[currentPlayer];
        }
        public Player SwitchPlayers()
        {
            var oldPlayer = GetCurrentPlayer();
            oldPlayer.Activity = Activity.Rolling;
            currentPlayer = (currentPlayer + 1) % Players.Count;
            var newPlayer = GetCurrentPlayer();
            return newPlayer;
        }

        // if timing is good roll dice and return right message if invalid return invalid message
        public string HandleDiceRequest()
        {
            string message;
            if (this.GetCurrentPlayer().Activity == Activity.Rolling)
            {
                ExecuteDice();
                if (Dice.Number == PirateNumber)
                {
                    message = $"You rolled {Dice.Number}, click hexagon to move pirate";
                }
                else
                {
                    message = $"You rolled {Dice.Number}, cards were given";
                }
                return message;
            }
            message = "You cannot roll dice now";
            return message;
        }

        // fufill actions related to rolling a dice
        private void ExecuteDice()
        {
            Dice.Roll();
            if (Dice.Number == PirateNumber)
            {
                LoseCardsAfterPirate(Players); // everybody lose cards if they are above limit
                GetCurrentPlayer().Activity = Activity.MovingPirate; // current player has to move a pirate
            }
            else
            {
                giveCardsAfterDice();
                GetCurrentPlayer().Activity = Activity.None;
            }
           
            
        }

       

       
       
        private void giveCardsAfterDice()
        {
            var rolledHexagons = Board.MappingNumbers[Dice.Number];
            if (rolledHexagons.Count == 0)
                throw new Exception(); // error there is no hexagon with given number

            foreach (var hexagon in rolledHexagons) 
            {
                if (hexagon != Board.Hexagons[Pirate.Location]) // if there is pirate on hexagon nothing happens
                {
                    foreach (var building in hexagon.Buildings)
                    {
                       if (building != null && building.Color != Color.None)
                            building.AddCardsToPlayer(players: this.Players, hexagon: hexagon);
                    }
               }
            }
        }

        // checks if someone has won
        public Player CheckWinner()
        {
            foreach (var player in Players)
            {
                if (player.checkPoints())
                    return player;
            }
            return null;
        }
        // players who have more cards than pirateNumber lose half of them. 
        private void LoseCardsAfterPirate(List<Player> players)
        {
            // LINQ
            var playersOverLimit = from player in players where player.GetSumOfCards() <= PirateNumber select player;
            foreach (Player player in players)
            {
                int losingNumber = (int)((player.GetSumOfCards() / 2) + 0.5);
                var playersCards = from cardSet in player.Cards where cardSet.CardsCount != 0 select cardSet;
                DropCards(numOverLimit: losingNumber, playersCards: playersCards);
            }
           
        }

        // drop players cards above pirate limit equally according to material
        private  void DropCards(int numOverLimit, IEnumerable<SameCardsSet> playersCards)
        {
            int droppedCards = 0;
            while (droppedCards < numOverLimit)
            {
                foreach (var cardSet in playersCards)
                {
                    if (cardSet.CardsCount > 0)
                    {
                        cardSet.CardsCount--;
                        droppedCards++;
                    }
                }
            }
        }
    }    
}
