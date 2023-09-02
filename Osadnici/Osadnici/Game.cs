using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osadnici
{

    public enum Material
    {
        Brick,
        Wood,
        Wheat,
        Lamb,
        Stone,
        None
    }

    public enum Activity // TODO mozna nejake vypustit
    {
        Rolling,
        GettingCards,
        MovingPirate,
        Buying,
        Building,
        Selling,
        None
    }
    public enum Color // TODO mozna nejake vypustit
    {
        Yellow,
        White,
        Red,
        Blue
    }

    public enum PawnType
    {
        None = -1,
        Road = 0,
        Village = 1,
        Town = 2
    }


    public class Board
    {
        public Dictionary<int, List<Hexagon>> MappingNumbers;
        public List<Hexagon> Hexagons;
        private int[] planNumbers = {4, 6, 9, 2, 5, 12, 4, 9, 8, 7, 8, 10, 3, 5, 10, 11, 3, 6, 11}; // start placement of board from rulebook for beginners
        private Material[] planMaterial = { Material.Wheat, Material.Wood, Material.Wheat, Material.Brick, Material.Wood, Material.Lamb, Material.Lamb,
                                            Material.Lamb, Material.Brick, Material.None, Material.Stone, Material.Lamb, Material.Wood, Material.Stone,
                                            Material.Brick, Material.Wood, Material.Wheat, Material.Wheat, Material.Stone  };
        int numberOfHexagons = 19;

        public Board()
        {
            Hexagons = new List<Hexagon>();
            MappingNumbers = new Dictionary<int, List<Hexagon>>();
            CreateHexagons();
        }

        private bool checkStartPlacement()
        {
            return planNumbers.Length == numberOfHexagons && planMaterial.Length == numberOfHexagons;
        }
        private void CreateHexagons()
        {
            if (!checkStartPlacement())
            {
                throw new Exception();
            }
            for (int i = 0; i < numberOfHexagons; i++)
            {
                Hexagon hexagon = new Hexagon {Number =  planNumbers[i], Material = planMaterial[i], HasPirate = false, 
                                               Buildings = new List<Building>(), Roads = new List<Road>()};
                this.Hexagons.Add(hexagon);

                //create mapping numbers on hexagons to hexagons
                if (!MappingNumbers.ContainsKey(planNumbers[i]))
                {
                   var list = new List<Hexagon>();
                   list.Add(hexagon);
                   MappingNumbers.Add(planNumbers[i], list);
                }
                else
                {
                    MappingNumbers[planNumbers[i]].Add(hexagon);
                }
                
            }
        }

    }


    public class Town : Building
    {
        public Town(Color color) : base(color: color)
        {
            this.Type = PawnType.Town;
            this.Number = 4;
        }
    }


    public class Village : Building
    {
        public Village(Color color) : base(color:color)
        {
            this.Type = PawnType.Village;
            this.Number = 5;
        }
    }

    public class Building : Pawn
    {
        // cards gained according to type of building
        public int BuildingToCardsNum()
        {
            throw new NotImplementedException();
            if (this.Type == PawnType.Village)
                return 1;
            if (this.Type == PawnType.Town)
                return 2;
            throw new Exception();
        }
        public Building(Color color) : base(color:color)
        {
        }

        // adds cards to player according of building value
        public void AddCardsToPlayer(Hexagon hexagon, List<Player> players)
        {
            var belongPlayer = this.MatchPlayer(players);
            int cardsGained = this.BuildingToCardsNum();
            Material material = hexagon.Material;

            //LINQ? TODO zkontrolovat jestli funguje
            var cardInHand = from card in belongPlayer.Cards where card.Material == material select card;
            if (cardInHand.Count() == 1)
            {
                foreach (var card in cardInHand) // bude to fungovat nebo to musi byt propojene s player
                    card.Number = card.Number + cardsGained;
            }
            throw new Exception();

        }
    }

    public class Road : Pawn
    {
        public Road(Color color) : base(color: color)
        {
            this.Type = PawnType.Road;
            this.Number = 15; //from rules
        }
    }
   public class Hexagon
   {
       
        public int Number { get; set; }
        public bool HasPirate { get; set; }
        public Material Material { get; set; }
        public List<Building> Buildings; //TODO zmenit typ
        public List<Road> Roads; //TODO zmenit typ
    }

    public class Pawn
    {
        public PawnType Type { get; set; }
        public Color Color { get; set; }
        public int Number { get; set; }


        public Pawn(Color color)
        {
            this.Number = 0;
            this.Color = color;
            this.Type = PawnType.None; //default is None
        }
        public Player MatchPlayer(List<Player> players)
        {
            throw new NotImplementedException();
        }
        
    }
     // TODO použít někde generic method kdekoliv

    // lze zadat string i Card
   public class Recipe<T>
    {
        public List<T> Manual { get; set; }
        public string Name { get; set; }

        public List<Card> getCards()
        {
            if (typeof(T) == typeof(Card))
                return Manual.Cast<Card>().ToList();
            if (typeof(T) == typeof(string))
            {
                var cardList = new List<Card>();
                //TODO doprogramovat string převod
                foreach (var words in Manual)
                {
                    string writtenCard = words as string;
                    var partsOfCard = writtenCard.Split(':');
                    Material material = (Material)Enum.Parse(typeof(Material), partsOfCard[0]);
                    int number = Int32.Parse(partsOfCard[1]);
                    var card = new Card(material, number);
                    cardList.Add(card);
                }
                return cardList;
            }
            throw new Exception(); // invalid type

        }
    }
    public class Player
    {
        public Color Color { get; set; }
        public int Points { get; set; } 
        public Activity Activity { get; set; }
        public List<Pawn> Pawns;
        public List<Card> Cards;

       public Player(Color color)
        {
            this.Points = 0;
            this.Activity = Activity.Rolling;
            this.Color = color;
            this.Pawns = SetPawnsList();
            this.Cards = SetCardsList();
            
        }

        // inits list of pawns for player and returns it
        private List<Card> SetCardsList()
        {
            var cardsList = new List<Card>();
            foreach (Material material in Enum.GetValues(typeof(Material)))
            {
                if (material != Material.None)
                cardsList.Add(new Card(material:material, number: 0));
            }
            return cardsList;
        }
        
        // inits list of pawns for player and returns it
        private List<Pawn> SetPawnsList()
        {
            var pawnsList = new List<Pawn>();
            pawnsList.Add(new Road(color: this.Color));
            pawnsList.Add(new Village(color: this.Color)); 
            pawnsList.Add(new Town(color: this.Color));

            return pawnsList;
        }
        
        // returns ordered pawns by enum PawnType
        public List<Pawn> GetOrderedPawns()
        {
            return Pawns.OrderBy(x => x.Type).ToList();
        }

        // returns ordered cards by enum Material
        public List<Card> GetOrderedCards()
        {
            return this.Cards.OrderBy(x => x.Material).ToList();
        }

        public bool checkPoints()
        {
            return Points == 10;
        }
        public int GetSumOfCards()
        {
            var numbers = Cards.Select(x => x.Number);
            return numbers.Sum(); // lambda expression
        }

        public bool checkIfCardsForRecipe(List<Card> recipe)
        {
            foreach(Card recipeCard in recipe)
            {
                bool recipeCardFound = false;
                foreach(Card playerCard in this.Cards)
                {
                    if (recipeCard.Material == playerCard.Material)
                    {
                        recipeCardFound = true;
                        if (recipeCard.Number > playerCard.Number)
                            return false;
                    }
                }
                if (!recipeCardFound)
                    return false;
            }
            return true;
        }
    }
    public class Card
    {
        public Material Material { get; set; }
        public int Number { get; set; }

        public Card(Material material, int number)
        {
            Number = number;
            Material = material;
        }
    }
     
    public class Game
    {
        public int pirateNumber = 7;
        public List<Player> Players;
        public Dice Dice;
        public Board Board;
        private int currentPlayer;
        public Pirate Pirate;
        public bool IsWinner;
        public int minimumPlayers = 2;
        public  int maximumPlayers = 4;


        public Game() // TODO init
        {
            this.Board = new Board();
            IsWinner = false;
            Dice = new Dice();
            Pirate = new Pirate();
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
           
            GetCurrentPlayer().Activity = Activity.Rolling; 
        }


        public Player GetCurrentPlayer()
        {
            return Players[currentPlayer];
        }
        public Player SwitchPlayers()
        {
            currentPlayer = (currentPlayer + 1) % Players.Count;
            var player = GetCurrentPlayer();
            player.Activity = Activity.Rolling;
            return player;
        }

        // if timing is good roll dice and return true and message, if it is not possible return false and message
        public (bool, string) HandleDiceRequest()
        {
            string message;
            if (this.GetCurrentPlayer().Activity == Activity.Rolling)
            {
                ExecuteDice();
                message = $"You rolled {Dice.Number}";
                return (true, message);
            }
            message = "You cannot roll dice now";
            return (false, message);
        }
        private void ExecuteDice() //TODO
        {
            Dice.Roll();
            //if (Dice.Number == pirateNumber)
            //{
            //    GetCurrentPlayer().Activity = Activity.MovingPirate;
            //    // everybody lose cards if they are above limit
            //    loseCardsAfterPirate(Players);
            //    // TODO move pirate
            //    //TODO kliknu na board a presune se na board s tim cislem a zmeni lokaci pirata Pirate.Location pozor ze musim presunout
            //    GetCurrentPlayer().Activity = Activity.None;
            //}
            //else
            //{
            //    GetCurrentPlayer().Activity = Activity.GettingCards;
            //    giveCardsAfterDice();
            //    GetCurrentPlayer().Activity = Activity.None;
            //}
           
            
        }

        public void Sell() // check Points TODO ?
        {
            throw new NotImplementedException();
        }

        // recipe generic method TODO staci jen cisla nebo dictonary nebo stringy
        public bool Buy<T>(Recipe<T> recipe, Player player) // check Points TODO //called after pressing button with image and with recipe according to image
        {
            player.Activity = Activity.Buying;
            //TODO pokud nemam tuhle Pawn v inventari vratim false
            // check if player has Material
            bool hasMaterial = player.checkIfCardsForRecipe(recipe.getCards());
            if (!hasMaterial)
                return false; // player is not able to buy recipe;

            
            // TODO buy thing according to recipe
            BuyByRecipe();
            player.Activity = Activity.Building;
            // TODO if there is space place it or you can withdraw;

            player.Activity = Activity.None;
            // check points and if enough then winner
            this.IsWinner = checkPointsAllPlayers();
            return true;
        }

        private void BuyByRecipe() //TODO
        {
            throw new NotImplementedException();
        }


        private void giveCardsAfterDice()
        {
            var rolledHexagons = Board.MappingNumbers[Dice.Number];
            if (rolledHexagons.Count == 0)
                throw new Exception(); // pod číslem není hexagon chyba

            foreach (var hexagon in rolledHexagons) 
            {
                if (hexagon != Pirate.Location) // když je na hexagonu pirat tak se nic neděje
                {
                    foreach (var building in hexagon.Buildings)
                    {
                        if (building != null) //TODO mozna zmenit
                            building.AddCardsToPlayer(players: this.Players, hexagon: hexagon);
                    }
                }
            }
        }

        // zkontroluje jestli někdo z hráčů nevyhrál
        private bool checkPointsAllPlayers()
        {
            foreach (var player in Players)
            {
                if (player.checkPoints())
                    return true;
            }
            return false;
        }
        // padl pirát, lidé co mají více jak 7 karet, ztrácí kary
        private void loseCardsAfterPirate(List<Player> players)
        {
            // LINQ TODO zkontrolovat
            var playersOverLimit = from player in players where player.GetSumOfCards() <= pirateNumber select player;
            foreach (Player player in players)
            {
                int numOverLimit = player.GetSumOfCards() - pirateNumber;
                var playersCards = from card in player.Cards where card.Number != 0 select card;
                dropCards(numOverLimit: numOverLimit, playersCards: playersCards);
            }
           
        }

        // drop players cards above pirate limit
        private  void dropCards(int numOverLimit, IEnumerable<Card> playersCards)
        {
            int droppedCards = 0;
            while (droppedCards < numOverLimit)
            {
                foreach (var card in playersCards)
                {
                    if (card.Number > 0)
                    {
                        card.Number--;
                        droppedCards++;
                    }
                }
            }
        }
    }

    public class Pirate
    {
        public Hexagon Location { get; set; }
    }

    public class Dice
    {
        public int Number { get; set; }

        public void Roll()
        {
            Random random = new Random();
            int firstDice = random.Next(1, 6);
            int secondDice = random.Next(1, 6);
            this.Number = firstDice + secondDice;
        }
    }
}
