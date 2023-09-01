using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osadnici
{

    class Board
    { // TODO
       public Dictionary<int, List<Hexagon>> MappingNumbers;
    }
    enum Material
    {
        Brick,
        Wood,
        Wheat,
        Lamb,
        Stone
    }

    enum Activity // TODO mozna nejake vypustit
    {
        None,
        GettingCards,
        MovingPirate,
        Buying,
        Building,
        Selling
    }
    enum Color // TODO mozna nejake vypustit
    {
        None,
        Yellow,
        White,
        Red,
        Blue
    }

    enum PawnType
    {
        Road = 0,
        Village = 1,
        Town = 2
    }

    class Building : Pawn
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

        // přidá karty hrači, kterému patři budova, podle hodnoty budovy
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

    class Road : Pawn
    {

    }
    class Hexagon
    {
       
        public string Number { get; set; }
        public bool HasPirate { get; set; }
        public Material Material { get; set; }
        public List<Building> Buildings; //TODO zmenit typ
        public List<Road> Roads; //TODO zmenit typ
    }

    class Pawn
    {
        public PawnType Type { get; set; }
        public Color Color { get; set; }

        public Player MatchPlayer(List<Player> players)
        {
            throw new NotImplementedException();
        }
        
    }
     // TODO použít někde generic method kdekoliv

    // lze zadat string i Card
    class Recipe<T>
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
    class Player
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public int Points { get; set; }  //TODO nastavit na 0
        public Activity Activity { get; set; }
        public List<Pawn> Pawns;
        public List<Card> Cards;
        
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
    class Card
    {
        public Material Material { get; set; }
        public int Number { get; set; }

        public Card(Material material, int number)
        {
            Number = number;
            Material = material;
        }
    }
     
    class Game
    {
        const int pirateNumber = 7;
        public List<Player> Players;
        public Dice Dice;
        public Board Board;
        private int numOfPlayers = 4; //TODO
        private int currentPlayer;
        public Pirate Pirate;
        public bool IsWinner;

        public void PickNumPlayers()
        {
            throw new NotImplementedException();
        }
        public Player SwitchPlayers()
        {
            currentPlayer = (currentPlayer + 1) % numOfPlayers;
            return Players[currentPlayer];
        }

        public void DealWithDice() //TODO
        {
            Dice.Roll();
            if (Dice.Number == pirateNumber)
            {
                Players[currentPlayer].Activity = Activity.MovingPirate;
                // everybody lose cards if they are above limit
                loseCardsAfterPirate(Players);
                // TODO move pirate
                //TODO kliknu na board a presune se na board s tim cislem a zmeni lokaci pirata Pirate.Location pozor ze musim presunout
                Players[currentPlayer].Activity = Activity.None;
            }
            else
            {
                Players[currentPlayer].Activity = Activity.GettingCards;
                giveCardsAfterDice();
                Players[currentPlayer].Activity = Activity.None;
            }
           
            
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

    class Pirate
    {
        public Hexagon Location { get; set; }
    }

    class Dice
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
