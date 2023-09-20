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


    public class Board
    {
        public Dictionary<int, List<Hexagon>> MappingNumbers;
        public List<Hexagon> Hexagons;
        private int[] planNumbers = {4, 6, 9, 2, 5, 12, 4, 9, 8, 7, 8, 10, 3, 5, 10, 11, 3, 6, 11}; // start placement of board from rulebook for beginners
        private Material[] planMaterial = { Material.Wheat, Material.Wood, Material.Wheat, Material.Brick, Material.Wood, Material.Lamb, Material.Lamb,
                                            Material.Lamb, Material.Brick, Material.None, Material.Stone, Material.Lamb, Material.Wood, Material.Stone,
                                            Material.Brick, Material.Wood, Material.Wheat, Material.Wheat, Material.Stone  };
        int numberOfHexagons = 19;
        int[] hexagonsStartRow = { 0, 3, 7, 12, 16 };

        public Board()
        {
            Hexagons = new List<Hexagon>();
            MappingNumbers = new Dictionary<int, List<Hexagon>>();
            CreateHexagons();
            AssignNeighbours();
        }

        public bool BuildRoad(Hexagon clickedHexagon, int clickedIndex, Game game)
        {
            var buildPlace = clickedHexagon.Roads[clickedIndex-6];
            var player = game.GetCurrentPlayer();

            if (player.Activity != Activity.BuildingRoad && player.Activity != Activity.StartFirstRoad
                && player.Activity != Activity.StartSecondRoad) // you are not building a road
                return false;
            if (buildPlace.Color != Color.None) // there is akready a road
                return false;
            if (!(clickedHexagon.CheckRoadConnedtivity(clickedIndex, game)))
                return false;
            if (!player.IsEnoughPawns(PawnType.Road))
            {
                return false;
            }

            
            var road = new Road(color: player.Color);
            clickedHexagon.Roads[clickedIndex - 6] = road;
            var roadSharedPlaces = clickedHexagon.GetSharedRoadPlaces(clickedIndex);
            foreach (var neighbour in roadSharedPlaces)
            {
                if (neighbour != null && neighbour.Item1 != null)
                    neighbour.Item1.Roads[neighbour.Item2 - 6] = road;
            }

            player.RemovePawn(PawnType.Road);
            return true;
        }


        public bool BuildBuilding(Hexagon clickedHexagon, int clickedIndex, Game game) 
        {
            var buildPlace = clickedHexagon.Buildings[clickedIndex];
            var player = game.GetCurrentPlayer();
            if (player.Activity != Activity.BuildingVillage && player.Activity != Activity.BuildingTown &&
                player.Activity != Activity.StartFirstVillage && player.Activity != Activity.StartSecondVillage) // not building right type
                return false;
            if (buildPlace.Color != Color.None && buildPlace.Color != player.Color) // there is foreign building
                return false;

            if (buildPlace.Color != Color.None && buildPlace.Type == PawnType.Village && player.Activity != Activity.BuildingTown) // you can build on village only if you upgrade to town
                return false;

            if (buildPlace.Color != Color.None && buildPlace.Type == PawnType.Town) // there is already your town you cannot uprgrade
                return false;
            if (player.Activity == Activity.BuildingVillage && !(clickedHexagon.CheckBuildingToRoadConnectivity(clickedIndex, game)))
                return false;
            if (player.Activity == Activity.BuildingTown && !player.IsEnoughPawns(PawnType.Town))
                return false;
            if (player.Activity == Activity.BuildingVillage && !player.IsEnoughPawns(PawnType.Village))
                return false;
            if (player.Activity == Activity.BuildingTown && buildPlace.Color != player.Color)
               return false;
            var buildingSharedPlaces = clickedHexagon.GetSharedBuildingPlaces(clickedIndex); // list of pairs neighbour and index

            if (player.Activity == Activity.BuildingVillage || player.Activity == Activity.StartFirstVillage || player.Activity == Activity.StartSecondVillage) // build village
            {
                var village = new Village(color: player.Color);
                clickedHexagon.Buildings[clickedIndex] = village;
                foreach (var neighbour in buildingSharedPlaces)
                {
                    if (neighbour != null && neighbour.Item1 != null)
                    neighbour.Item1.Buildings[neighbour.Item2] = village;
                }

                player.RemovePawn(PawnType.Village);
            }
            else // build town
            {
                var town = new Town(color: player.Color);
                clickedHexagon.Buildings[clickedIndex] = town;
                foreach (var neighbour in buildingSharedPlaces)
                {
                    if (neighbour != null && neighbour.Item1 != null)
                    neighbour.Item1.Buildings[neighbour.Item2] = town;
                }
                player.RemovePawn(PawnType.Town);
                player.AddPawn(PawnType.Village); // you got village back after building a town
            }

            player.Points++;
            return true;
        }
        

        private bool checkStartPlacement()
        {
            return planNumbers.Length == numberOfHexagons && planMaterial.Length == numberOfHexagons;
        }

        private void AssignNeighbours()
        {
            AssignSideNeighbours();
            AssignTopBottomNeighbours();
        }
        private void AssignSideNeighbours()
        {
            if (Hexagons.Count != numberOfHexagons)
                throw new Exception();
            
            for (int i = 1; i < Hexagons.Count; i++)
            {
                if (!this.hexagonsStartRow.Contains(i))
                {
                    Hexagons[i].Neighbours.Left = Hexagons[i - 1];
                    Hexagons[i-1].Neighbours.Right = Hexagons[i];

                }

            }
        }

        private void AssignTopBottomNeighbours()
        {
            AssignTopHalfNeighbours();
            AssignBottomHalfNeighbours(7, 12);
        }
        private void AssignTopHalfNeighbours(int startDownLeftIndex = 3, int endHalfindex = 7)
        {
            int downLeftIndex = startDownLeftIndex;
            for (int i = 0; i < endHalfindex; i++) // two top rows
            {
                Hexagons[i].Neighbours.DownLeft = Hexagons[downLeftIndex];
                Hexagons[downLeftIndex].Neighbours.UpRight = Hexagons[i];

                Hexagons[i].Neighbours.DownRight = Hexagons[downLeftIndex + 1];
                Hexagons[downLeftIndex + 1].Neighbours.UpLeft = Hexagons[i];
                downLeftIndex++;
                if (downLeftIndex == endHalfindex - 1)
                    downLeftIndex++;
            }

        }
    private void AssignBottomHalfNeighbours(int startNeighbourIndex = 7, int startOfHalf = 12)
    {
        int upLeftIndex = startNeighbourIndex;
        for (int i = startOfHalf; i < numberOfHexagons; i++) // two bottom rows
        {
            Hexagons[i].Neighbours.UpLeft = Hexagons[upLeftIndex];
            Hexagons[upLeftIndex].Neighbours.DownRight = Hexagons[i];

            Hexagons[i].Neighbours.UpRight = Hexagons[upLeftIndex + 1];
            Hexagons[upLeftIndex + 1].Neighbours.DownLeft = Hexagons[i];
            upLeftIndex++;
            if (upLeftIndex == startOfHalf - 1)
                upLeftIndex++;
        }
    }


    private void CreateHexagons()
    {
            if (!checkStartPlacement())
            {
                throw new Exception();
            }
            for (int i = 0; i < numberOfHexagons; i++)
            {
                Hexagon hexagon = new Hexagon (number:planNumbers[i], material: planMaterial[i]);
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
            this.Number = 5; // from rules
        }
    }

    public class Building : Pawn
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

        
        public Building(Color color) : base(color:color)
        {
        }

       

        // adds cards to player according of building value
        public void AddCardsToPlayer(Hexagon hexagon, List<Player> players)
        {
            var belongPlayer = this.MatchPlayer(players);
            int cardsGained = this.BuildingToCardsNum();
            Material material = hexagon.Material;

            //LINQ
            var cardInHand = from card in belongPlayer.Cards where card.Material == material select card;
            if (cardInHand.Count() == 1)
            {
                foreach (var card in cardInHand)
                    card.Number = card.Number + cardsGained;
            }
            else
            {
                throw new Exception(); // card with material does not exist
            }
            

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

   public class Neighbours
    {
        public Hexagon UpLeft { get; set; }
        public Hexagon UpRight { get; set; }
        public Hexagon DownLeft { get; set; }
        public Hexagon DownRight { get; set; }
        public Hexagon Left { get; set; }
        public Hexagon Right { get; set; }

    }
   public class Hexagon
   {
       
        public int Number { get; set; }
        public Material Material { get; set; }
        public List<Building> Buildings; 
        public List<Road> Roads; 
        public Neighbours Neighbours;

        public Hexagon(int number, Material material)
        {
            this.Number = number;
            this.Material = material;
            this.Neighbours = new Neighbours();
            this.Buildings = new List<Building>();
            this.Roads = new List<Road>();

            for (int i = 0; i < 6; i++)   //six verteces its hexagon
            {
                this.Buildings.Add(new Building(color: Color.None));
                this.Roads.Add(new Road(color: Color.None));
            }
        }

        // check if building is connected to road
        private bool CheckRoadConnectedToBuilding(int roadIndex)
        {
            var list = GetBuildingsConnectedToRoad(roadIndex);
            foreach (var entry in list)
            {
                if (entry != null && entry.Item1 != null)
                    if (entry.Item1.Buildings[entry.Item2].Color != Color.None)
                        return true;
            }
            return false;

        }

        // returns true if it has road ant this road is not connected to other building
        public bool CheckBuildingToRoadConnectivity(int clickedIndex, Game game)
        {
            var listOfRoads = GetRoadsConnectedToBuildingSite(clickedIndex);
            if (listOfRoads.Count() == 0) // no roads to connect
                return false;
            foreach (var entry in listOfRoads)
            {
                if (entry.Item1 == null) // hexagon does not exist
                    continue;
                Road road = entry.Item1.Roads[entry.Item2 - 6];
                int roadIndex = entry.Item2 - 6;
                
                    if (road.Color != Color.None && CheckRoadConnectedToBuilding(roadIndex)) // there is a road and is connected to town or village even foreign, road cannot connect two towns
                        return false;
                if (road.Color == game.GetCurrentPlayer().Color)
                    return true;
            }

            // there are only foreign roads or no roads
            if (game.GetCurrentPlayer().Activity == Activity.StartFirstVillage || 
                game.GetCurrentPlayer().Activity == Activity.StartSecondVillage)
            {
                return true;
            }
            return false;

        }

        public bool CheckRoadConnedtivity(int clickedIndex, Game game)
        {
            var listOfBuildings = GetBuildingsConnectedToRoad(clickedIndex);
            var listOfRoads = GetRoadsConnectedToRoad(clickedIndex);
            if (listOfBuildings.Count() != 2) // road does not connect two towns
                throw new Exception();

            foreach(var entry in listOfBuildings) 
            {
                if (entry.Item1 != null && entry.Item1 != null)
                {
                    var building = entry.Item1.Buildings[entry.Item2];
                    if (building.Color == game.GetCurrentPlayer().Color)
                        return true;
                }
            }
            foreach (var entry in listOfRoads)
            {
                if (entry.Item1 != null && entry.Item1 != null)
                {
                    var road = entry.Item1.Roads[entry.Item2 - 6];      
                    Building connectingBuilding = entry.Item1.Buildings[entry.Item3];

                    if (connectingBuilding.Color == Color.None && road.Color == game.GetCurrentPlayer().Color) // roads is interupted by elses building
                        return true;
                }
            }
            return false;

        }

        // creates list of connected buildings 
        private List<Tuple<Hexagon, int>> GetBuildingsConnectedToRoad(int clickedIndex)
        {
            var connectedBuildings = new List<Tuple<Hexagon, int>>();
            switch (clickedIndex)
            {
                case 6:
                    connectedBuildings.Add(new Tuple<Hexagon, int>(this, 5));
                    connectedBuildings.Add(new Tuple<Hexagon, int>(this, 4));
                    break;
                case 7:
                    connectedBuildings.Add(new Tuple<Hexagon, int>(this, 0));
                    connectedBuildings.Add(new Tuple<Hexagon, int>(this, 5));
                    break;
                case 8:
                    connectedBuildings.Add(new Tuple<Hexagon, int>(this, 4));
                    connectedBuildings.Add(new Tuple<Hexagon, int>(this, 3));
                    break;
                case 9:
                    connectedBuildings.Add(new Tuple<Hexagon, int>(this, 1));
                    connectedBuildings.Add(new Tuple<Hexagon, int>(this, 2));
                    break;
                case 10:
                    connectedBuildings.Add(new Tuple<Hexagon, int>(this, 0));
                    connectedBuildings.Add(new Tuple<Hexagon, int>(this, 1));
                    break;
                case 11:
                    connectedBuildings.Add(new Tuple<Hexagon, int>(this, 2));
                    connectedBuildings.Add(new Tuple<Hexagon, int>(this, 3));
                    break;
            }
            return connectedBuildings;

        }

        // creates list of connected roads starts and stores the building with connects them
        private List<Tuple<Hexagon, int, int>> GetRoadsConnectedToRoad(int clickedIndex)
        {
            var listRoads = new List<Tuple<Hexagon, int, int>>();
            switch (clickedIndex)
            {
                case 6:
                    listRoads.Add(new Tuple<Hexagon, int, int>(this, 7, 5));
                    listRoads.Add(new Tuple<Hexagon, int, int>(this, 8, 4));
                    listRoads.Add(new Tuple<Hexagon, int, int>(this.Neighbours.Left, 10, 1));
                    listRoads.Add(new Tuple<Hexagon, int, int>(this.Neighbours.Left, 11, 2));
                    break;
                case 7:
                    listRoads.Add(new Tuple<Hexagon, int, int>(this, 10, 0));
                    listRoads.Add(new Tuple<Hexagon, int, int>(this, 6, 5));
                    listRoads.Add(new Tuple<Hexagon, int, int>(this.Neighbours.UpLeft, 8, 3));
                    listRoads.Add(new Tuple<Hexagon, int, int>(this.Neighbours.UpLeft, 9, 2));

                    break;
                case 8:
                    listRoads.Add(new Tuple<Hexagon, int, int>(this, 6, 4));
                    listRoads.Add(new Tuple<Hexagon, int, int>(this, 11,3));
                    listRoads.Add(new Tuple<Hexagon, int, int>(this.Neighbours.DownLeft, 7, 0));
                    listRoads.Add(new Tuple<Hexagon, int, int>(this.Neighbours.DownLeft, 9, 1));
                    break;
                case 9:
                    listRoads.Add(new Tuple<Hexagon, int, int>(this, 10, 1));
                    listRoads.Add(new Tuple<Hexagon, int,int>(this, 11, 2));
                    listRoads.Add(new Tuple<Hexagon, int, int>(this.Neighbours.Right, 7, 5));
                    listRoads.Add(new Tuple<Hexagon, int, int>(this.Neighbours.Right, 8, 4));
                    break;
                case 10:
                    listRoads.Add(new Tuple<Hexagon, int, int>(this, 7, 0));
                    listRoads.Add(new Tuple<Hexagon, int, int>(this, 9, 1));
                    listRoads.Add(new Tuple<Hexagon, int, int>(this.Neighbours.UpRight, 11, 3));
                    listRoads.Add(new Tuple<Hexagon, int, int>(this.Neighbours.UpRight, 6, 4));
                    break;
                case 11:
                    listRoads.Add(new Tuple<Hexagon, int, int>(this, 9, 2));
                    listRoads.Add(new Tuple<Hexagon, int, int>(this, 8, 3));
                    listRoads.Add(new Tuple<Hexagon, int, int>(this.Neighbours.DownRight, 10, 0));
                    listRoads.Add(new Tuple<Hexagon, int, int>(this.Neighbours.DownRight, 6, 5));
                    break;
            }
            return listRoads;
        }
            // returs list of roads connected to clicked building site 
            private List<Tuple<Hexagon, int>> GetRoadsConnectedToBuildingSite(int clickedIndex)
        {
            var connectedRoads = new List<Tuple<Hexagon, int>>();
            switch (clickedIndex)
            {
                case 0:
                    connectedRoads.Add(new Tuple<Hexagon, int>(this, 7));
                    connectedRoads.Add(new Tuple<Hexagon, int>(this, 10));
                    connectedRoads.Add(new Tuple<Hexagon, int>(this.Neighbours.UpLeft, 9));
                    break;
                case 1:
                    connectedRoads.Add(new Tuple<Hexagon, int>(this, 10));
                    connectedRoads.Add(new Tuple<Hexagon, int>(this, 9));
                    connectedRoads.Add(new Tuple<Hexagon, int>(this.Neighbours.Right, 7));
                    break;
                case 2:
                    connectedRoads.Add(new Tuple<Hexagon, int>(this, 11));
                    connectedRoads.Add(new Tuple<Hexagon, int>(this, 9));
                    connectedRoads.Add(new Tuple<Hexagon, int>(this.Neighbours.Right, 8));
                    break;
                case 3:
                    connectedRoads.Add(new Tuple<Hexagon, int>(this, 11));
                    connectedRoads.Add(new Tuple<Hexagon, int>(this, 8));
                    connectedRoads.Add(new Tuple<Hexagon, int>(this.Neighbours.DownLeft, 9));

                    break;
                case 4:
                    connectedRoads.Add(new Tuple<Hexagon, int>(this, 6));
                    connectedRoads.Add(new Tuple<Hexagon, int>(this, 8));
                    connectedRoads.Add(new Tuple<Hexagon, int>(this.Neighbours.DownLeft, 7));
                    break;
                case 5:
                    connectedRoads.Add(new Tuple<Hexagon, int>(this, 6));
                    connectedRoads.Add(new Tuple<Hexagon, int>(this, 7));
                    connectedRoads.Add(new Tuple<Hexagon, int>(this.Neighbours.Left, 10));
                    break;
            }
            return connectedRoads;
        }
        public List<Tuple<Hexagon, int>> GetSharedBuildingPlaces(int clickedIndex)
        {
            var sharedBuildingPlaces = new List<Tuple<Hexagon, int>>();
            switch(clickedIndex)
            {
                case 0:
                    sharedBuildingPlaces.Add(new Tuple<Hexagon, int>(this.Neighbours.UpLeft, 2));
                    sharedBuildingPlaces.Add(new Tuple<Hexagon, int>(this.Neighbours.UpRight, 4));
                    break;
                case 1:
                    sharedBuildingPlaces.Add(new Tuple<Hexagon, int>(this.Neighbours.UpRight, 3));
                    sharedBuildingPlaces.Add(new Tuple<Hexagon, int>(this.Neighbours.Right, 5));
                    break;
                case 2:
                    sharedBuildingPlaces.Add(new Tuple<Hexagon, int>(this.Neighbours.Right, 4));
                    sharedBuildingPlaces.Add(new Tuple<Hexagon, int>(this.Neighbours.DownRight, 0));
                    break;
                case 3:
                    sharedBuildingPlaces.Add(new Tuple<Hexagon, int>(this.Neighbours.DownRight, 5));
                    sharedBuildingPlaces.Add(new Tuple<Hexagon, int>(this.Neighbours.DownLeft, 1));
                    break;
                case 4:
                    sharedBuildingPlaces.Add(new Tuple<Hexagon, int>(this.Neighbours.Left, 2));
                    sharedBuildingPlaces.Add(new Tuple<Hexagon, int>(this.Neighbours.DownLeft, 0));
                    break;
                case 5:
                    sharedBuildingPlaces.Add(new Tuple<Hexagon, int>(this.Neighbours.Left, 1));
                    sharedBuildingPlaces.Add(new Tuple<Hexagon, int>(this.Neighbours.UpLeft, 3));
                    break;
            }
            return sharedBuildingPlaces;

        }
        public List<Tuple<Hexagon, int>> GetSharedRoadPlaces(int clickedIndex)
        {
            var sharedRoadPlaces = new List<Tuple<Hexagon, int>>();
            switch (clickedIndex)
            {
                case 6:
                    sharedRoadPlaces.Add(new Tuple<Hexagon, int>(this.Neighbours.Left, 9));
                    break;
                case 7:
                    sharedRoadPlaces.Add(new Tuple<Hexagon, int>(this.Neighbours.UpLeft, 11));
                    break;
                case 8:
                    sharedRoadPlaces.Add(new Tuple<Hexagon, int>(this.Neighbours.DownLeft, 10));
                    break;
                case 9:
                    sharedRoadPlaces.Add(new Tuple<Hexagon, int>(this.Neighbours.Right, 6));
                    break;
                case 10:
                    sharedRoadPlaces.Add(new Tuple<Hexagon, int>(this.Neighbours.UpRight, 8));
                    break;
                case 11:
                    sharedRoadPlaces.Add(new Tuple<Hexagon, int>(this.Neighbours.DownRight, 7));
                    break;
            }
            return sharedRoadPlaces;

        }
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
            foreach (Player player in players)
            {
                if (player.Color == this.Color)
                    return player;
            }
            throw new Exception(); // player of given color does not exist
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
            this.Activity = Activity.StartFirstVillage;
            this.Color = color;
            this.Pawns = SetPawnsList();
            this.Cards = SetCardsList();
        }

        public bool IsEnoughPawns(PawnType type)
        {
            foreach(var pawn in Pawns)
            {
                if (pawn.Type == type)
                {
                    if (pawn.Number > 0)
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
            foreach (var pawn in Pawns)
            {
                if (pawn.Type == pawnType)
                {
                    pawn.Number++;
                    return;
                }
            }
            throw new Exception(); // pawn does not exist
        }

        // removes one pawn from list
        public void RemovePawn(PawnType pawnType)
        {
            foreach (var pawn in Pawns)
            {
                if (pawn.Type == pawnType)
                {
                    if (pawn.Number <= 0)
                        throw new Exception(); // should have been positive
                    else
                    {
                        pawn.Number--;
                        return;
                    }
                }
            }
            throw new Exception(); // pawn does not exist
        }

        // checks if searched pawn is in players inventory
        public bool CheckPawnInInventory(PawnType searchedPawn)
        {
            foreach (var inventoryPawn in this.Pawns)
            {
                if (inventoryPawn.Type == searchedPawn)
                {
                    if (inventoryPawn.Number > 0)
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
                        playerCard.Number = playerCard.Number - recipeCard.Number;
                    }
                }
            }
        }
                
        // inits list of pawns for player and returns it
        private List<Card> SetCardsList()
        {
            var cardsList = new List<Card>();
            foreach (Material material in Enum.GetValues(typeof(Material)))
            {
                if (material != Material.None)
                cardsList.Add(new Card(material:material, number: 1)); // begin with one card from each material
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

        // check if it is possible to sell constant amount of given material
        public bool Sell(Material material, Game gameLogic)
        {
            
            foreach (Card card in this.Cards)
            {
                if (card.Material == material)
                {
                    if (card.Number >= gameLogic.SellConstant)
                    {
                        card.Number = card.Number - gameLogic.SellConstant;

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
            var numbers = Cards.Select(x => x.Number);
            return numbers.Sum();
        }

        public bool CheckCardsForRecipe(List<Card> recipe)
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

    public class Recipe
    {
        public List<Card> Manual { get; set; }
        public PawnType PawnName { get; set; }
        public Recipe(List<Card> recipe, PawnType pawnName)
        {
            this.Manual = recipe;
            this.PawnName = pawnName;
        }
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
            var roadList = new List<Card>();
            roadList.Add(new Card(Material.Wood, 1));
            roadList.Add(new Card(Material.Brick, 1));
            var villageList = new List<Card>();
            villageList.Add(new Card(Material.Wood, 1));
            villageList.Add(new Card(Material.Brick, 1));
            villageList.Add(new Card(Material.Lamb, 1));
            villageList.Add(new Card(Material.Wheat, 1));
            var townList = new List<Card>();
            townList.Add(new Card(Material.Stone, 3));
            townList.Add(new Card(Material.Wheat, 2));

            recipes.Add(PawnType.Road, new Recipe(pawnName: PawnType.Road, recipe: roadList));
            recipes.Add(PawnType.Village, new Recipe(pawnName: PawnType.Village, recipe: villageList));
            recipes.Add(PawnType.Town, new Recipe(pawnName: PawnType.Town, recipe: townList));
            return recipes;
        }
        public void ChangeBuildActivity()
        {
            var currentPlayer = this.GetCurrentPlayer();
            if (currentPlayer.Activity == Activity.StartFirstVillage)
            {
                currentPlayer.Activity = Activity.StartFirstRoad;
                return;
            }
            if (currentPlayer.Activity == Activity.StartFirstRoad)
            {
                currentPlayer.Activity = Activity.StartSecondVillage;
                return;
            }     
            if (currentPlayer.Activity == Activity.StartSecondVillage)
            {
                currentPlayer.Activity = Activity.StartSecondRoad;
                return;
            }   
            if (currentPlayer.Activity == Activity.StartSecondRoad)
            {
                currentPlayer.Activity = Activity.NoPossibilities;
                return;
            } 
            if (currentPlayer.Activity == Activity.BuildingRoad)
            {
               currentPlayer.Activity = Activity.None;
               return;
            }
            if (currentPlayer.Activity == Activity.BuildingVillage)
            {
               currentPlayer.Activity = Activity.None;
                return;
            } 
            if (currentPlayer.Activity == Activity.BuildingTown)
            {
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

        // if timing is good roll dice and return true and message, if it is not possible return false and message
        public (bool, string) HandleDiceRequest()
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
                return (true, message);
            }
            message = "You cannot roll dice now";
            return (false, message);
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
                var playersCards = from card in player.Cards where card.Number != 0 select card;
                DropCards(numOverLimit: losingNumber, playersCards: playersCards);
            }
           
        }

        // drop players cards above pirate limit equally according to material
        private  void DropCards(int numOverLimit, IEnumerable<Card> playersCards)
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
        public int Location { get; set; }
        public Pirate(int startHexagon)
        {
            Location = startHexagon;
        }
        public bool PlacePirate(int clickedHexagonIndex, Game gameLogic)
        {
            if (Location == clickedHexagonIndex) // cannot place pirate on the same hexagon
            {
                return false;
            }
            else
            {
                Location = clickedHexagonIndex;
                gameLogic.GetCurrentPlayer().Activity = Activity.None;
                return true;

            }
        }
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
