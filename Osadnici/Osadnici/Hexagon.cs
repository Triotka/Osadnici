using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osadnici
{
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

            foreach (var entry in listOfBuildings)
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
                    listRoads.Add(new Tuple<Hexagon, int, int>(this, 11, 3));
                    listRoads.Add(new Tuple<Hexagon, int, int>(this.Neighbours.DownLeft, 7, 0));
                    listRoads.Add(new Tuple<Hexagon, int, int>(this.Neighbours.DownLeft, 9, 1));
                    break;
                case 9:
                    listRoads.Add(new Tuple<Hexagon, int, int>(this, 10, 1));
                    listRoads.Add(new Tuple<Hexagon, int, int>(this, 11, 2));
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
            switch (clickedIndex)
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
        public Tuple<Hexagon, int> GetSharedRoadPlaces(int clickedIndex)
        {
            switch (clickedIndex)
            {
                case 6:
                    return new Tuple<Hexagon, int>(this.Neighbours.Left, 9);
                case 7:
                    return new Tuple<Hexagon, int>(this.Neighbours.UpLeft, 11);
                case 8:
                    return new Tuple<Hexagon, int>(this.Neighbours.DownLeft, 10);
                case 9:
                    return new Tuple<Hexagon, int>(this.Neighbours.Right, 6);
                case 10:
                    return new Tuple<Hexagon, int>(this.Neighbours.UpRight, 8);
                case 11:
                    return new Tuple<Hexagon, int>(this.Neighbours.DownRight, 7);
            }

            return null;

        }
    }

}
