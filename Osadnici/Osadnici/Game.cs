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
        public int SellConstant = 4;
        public int PirateNumber = 7;
        public List<Player> Players;
        public Dice Dice;
        public Board Board;
        private int currentPlayer;
        public Pirate Pirate;
        public int minimumPlayers = 2;
        public  int maximumPlayers = 4;
        public Dictionary <PawnType, Recipe> RecipeRules;
        public Activity Activity { get; set; }


        public Game()
        {

            this.Board = new Board();
            Dice = new Dice();
            Pirate = new Pirate(9); // place pirate to the middle
            RecipeRules = SetRecipes();
            this.Activity = Activity.StartFirstVillage;
        }

        // creates recipes for game from rules
        private Dictionary<PawnType, Recipe> SetRecipes()
        {
            var recipes = new Dictionary<PawnType, Recipe>();
            var roadList = new List<SameCardsSet>
            {
                new SameCardsSet(Material.Wood, 1),
                new SameCardsSet(Material.Brick, 1)
            };
            var villageList = new List<SameCardsSet>
            {
                new SameCardsSet(Material.Wood, 1),
                new SameCardsSet(Material.Brick, 1),
                new SameCardsSet(Material.Lamb, 1),
                new SameCardsSet(Material.Wheat, 1)
            };
            var townList = new List<SameCardsSet>
            {
                new SameCardsSet(Material.Stone, 3),
                new SameCardsSet(Material.Wheat, 2)
            };

            recipes.Add(PawnType.Road, new Recipe(pawnName: PawnType.Road, recipe: roadList));
            recipes.Add(PawnType.Village, new Recipe(pawnName: PawnType.Village, recipe: villageList));
            recipes.Add(PawnType.Town, new Recipe(pawnName: PawnType.Town, recipe: townList));
            return recipes;
        }


        // checks if building is possible if so does action and returns messages
        public string BuildAction(string pickedName)
        {
            string unsuccessfulMessage = "Unable to buy";
            string successfulMessage = "Bought";
            if (this.Activity != Activity.None)
            {
                 return unsuccessfulMessage;
            }

            string[] names = Enum.GetNames(typeof(PawnType));


            for (int i = 0; i < names.Length; i++)
            {
                if (pickedName.EndsWith(names[i]))
                {

                    bool successfulBuy = false;
                    if (names[i] == PawnType.Village.ToString())
                    {
                        successfulBuy = GetCurrentPlayer().Buy(RecipeRules[PawnType.Village]);
                        if (!successfulBuy)
                        {
                            return unsuccessfulMessage + " " + nameof(PawnType.Village);
                        }
                        else
                        {
                            Activity = Activity.BuildingVillage;
                            return successfulMessage + " " + nameof(PawnType.Village);
                        }
                    }
                    if (names[i] == nameof(PawnType.Town))
                    {
                        successfulBuy = GetCurrentPlayer().Buy(RecipeRules[PawnType.Town]);
                        if (!successfulBuy)
                        {
                            return unsuccessfulMessage + " " + nameof(PawnType.Town);
                        }
                        else
                        {
                            Activity = Activity.BuildingTown;
                            return successfulMessage + " " + nameof(PawnType.Town);
                        }
                    }
                    if (names[i] == nameof(PawnType.Road))
                    {
                        successfulBuy = GetCurrentPlayer().Buy(RecipeRules[PawnType.Road]);

                        if (!successfulBuy)
                        {
                            return unsuccessfulMessage + " " + nameof(PawnType.Road);
                        }
                        else
                        {
                            Activity = Activity.BuildingRoad;
                            return successfulMessage + " " + nameof(PawnType.Road);
                        }

                    }

                }
            }
            return unsuccessfulMessage;
        }

        // checks if selling action is ok and if yes returs picked cards, can return a message
        public (string, SameCardsSet) SellAction(string clickedName)
        {
            if (Activity != Activity.None)
            {
                return ("Cannot sell", null);
            }

            foreach (var material in Enum.GetValues(typeof(Material)).Cast<Material>())
            {
                if (clickedName.EndsWith(material.ToString()))
                {
                    return ("Successful sell", GetCurrentPlayer().CardSetByMaterial(material));
                    
                }
            }
            return ("Cannot sell", null);
        }

        // returns true if it ok to interact with board according to activity
        public bool IsBoardInteraction() // you can zoom on board piece
        {
            if (this.Activity == Activity.StartFirstVillage || this.Activity == Activity.StartSecondVillage || this.Activity == Activity.BuildingVillage
                || this.Activity == Activity.BuildingTown || this.Activity == Activity.BuildingRoad || this.Activity == Activity.StartFirstRoad || this.Activity == Activity.StartSecondRoad
                || this.Activity == Activity.MovingPirate)
                return true;

            return false;
        }

        // returns message according to result of pirate action
        public string PirateAction(int clickedHexagonIndex)
        {
            if (Activity == Activity.MovingPirate)
            {
                bool sucessfulPirate = Pirate.PlacePirate(clickedHexagonIndex: clickedHexagonIndex, gameLogic: this);
                if (!sucessfulPirate) // pick another place
                {
                    return "Pirate is already here, pick different one";
                }
                else
                {
                    return "Pirate was successful";
                }
            }
            else
            {
                return "You cannot move a pirate right now";
            }
        }
        public string ActivityToString()
        {
            switch (this.Activity)
            {
                case Activity.StartFirstVillage:
                    return "Building first village";
                case Activity.StartSecondVillage:
                    return "Building second village";
                case Activity.StartFirstRoad:
                    return "Building first road";
                case Activity.StartSecondRoad:
                    return "Building second road";
                case Activity.Rolling:
                    return "Must roll a dice";
                case Activity.BuildingVillage:
                    return "Building a village";
                case Activity.BuildingRoad:
                    return "Building a road";
                case Activity.BuildingTown:
                    return "Building a town";
                case Activity.MovingPirate:
                    return "Must move a pirate";
                case Activity.NoPossibilities:
                    return "No more action";
                case Activity.None:
                    return "Waiting for action";
            }
            throw new Exception(); //unknown action
        }
        public void ChangeBuildActivity()
        {
            switch(Activity)
            {
                case Activity.StartFirstVillage:
                    Activity = Activity.StartFirstRoad;
                    return;
                case Activity.StartFirstRoad:
                    Activity = Activity.StartSecondVillage;
                    return;
                case Activity.StartSecondVillage:
                    Activity = Activity.StartSecondRoad;
                    return;
                case Activity.StartSecondRoad:
                    Activity = Activity.NoPossibilities;
                    return;
                case Activity.BuildingRoad:
                    Activity = Activity.None;
                    return;
                case Activity.BuildingVillage:
                    Activity = Activity.None;
                    return;
                case Activity.BuildingTown:
                    Activity = Activity.None;
                    return;
            }
            

        }


        // exchanges cards betwen players, one if player sell constant if it is bank
        public void ExchangeCards(SameCardsSet offeredCardSet, int pickedBtnIndex, Player pickedPlayer)
        {
            var gainedCardSet = pickedPlayer.GetOfferedCards().ToList()[pickedBtnIndex];
            if (pickedPlayer.Color == Color.None) // bot
            {
                offeredCardSet.CardsCount -= this.SellConstant;
                pickedPlayer.CardSetByMaterial(offeredCardSet.Material).CardsCount += this.SellConstant;
            }
            else
            {
                offeredCardSet.CardsCount -= 1;
                pickedPlayer.CardSetByMaterial(offeredCardSet.Material).CardsCount += 1;
            }

            gainedCardSet.CardsCount -= 1;
            GetCurrentPlayer().CardSetByMaterial(gainedCardSet.Material).CardsCount += 1;

            
            
        }
        // returns a player  Bank with cards that can be offered by bank during an exchange
        public Player SetBankCards()
        {
            return new Player(Color.None);
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

        private void ChangeSwitchActivity()
        {
            if (this.Activity == Activity.NoPossibilities)
            {
                if (currentPlayer == (Players.Count - 1)) // start finished
                {
                    this.Activity = Activity.Rolling;
                    return;
                }
                else
                {
                    this.Activity = Activity.StartFirstVillage;
                    return;
                }
            }
            else if (this.Activity == Activity.None) 
            {
                this.Activity = Activity.Rolling;
                return;
            }
        }
        // switches players and do action and returs message according to activity
        public string SwitchPlayers()
        {
            if (Activity == Activity.None || Activity == Activity.NoPossibilities)
            {
                ChangeSwitchActivity();
                currentPlayer = (currentPlayer + 1) % Players.Count;

                if (this.Activity == Activity.Rolling)
                {
                    return HandleDiceRequest();

                }
                return "Players switched";
            }
            return "You cannot switch players";
        }

        // if timing is good roll dice and return right message if invalid return invalid message
        public string HandleDiceRequest()
        {
            string message;
            if (Activity == Activity.Rolling)
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
                Activity = Activity.MovingPirate; // current player has to move a pirate
            }
            else
            {
                giveCardsAfterDice();
                Activity = Activity.None;
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
            var playersOverLimit = from player in players where player.GetSumOfCards() >= PirateNumber select player;
            foreach (Player player in playersOverLimit)
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
