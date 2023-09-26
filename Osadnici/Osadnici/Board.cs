using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Osadnici
{
    public class Board
    {
        public Dictionary<int, List<Hexagon>> MappingNumbers;
        public List<Hexagon> Hexagons;
        private int[] planNumbers = { 4, 6, 9, 2, 5, 12, 4, 9, 8, 7, 8, 10, 3, 5, 10, 11, 3, 6, 11 }; // start placement of board from rulebook for beginners
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


        public bool CheckRoadBuildOK(Hexagon clickedHexagon, Game game, int clickedIndex)
        {
            var buildPlace = clickedHexagon.Roads[clickedIndex - 6];
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

            return true;

        }
        public bool BuildRoad(Hexagon clickedHexagon, int clickedIndex, Game game)
        {
            if (!CheckRoadBuildOK(clickedHexagon: clickedHexagon, clickedIndex: clickedIndex, game: game))
                return false;

            var player = game.GetCurrentPlayer();
            var road = new Road(color: player.Color);
            clickedHexagon.Roads[clickedIndex - 6] = road;
            var roadSharedPlace = clickedHexagon.GetSharedRoadPlaces(clickedIndex);
            if (roadSharedPlace != null && roadSharedPlace.Item1 != null)
                roadSharedPlace.Item1.Roads[roadSharedPlace.Item2 - 6] = road;

            player.RemovePawn(PawnType.Road);
            return true;
        }


        public bool CheckBuildBuilding(Hexagon clickedHexagon, int clickedIndex, Game game)
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
            if ((player.Activity == Activity.StartFirstVillage || player.Activity == Activity.StartSecondVillage) && !clickedHexagon.CheckBuildingToRoadConnectivity(clickedIndex, game))
                return false;

            return true;
        }
        public bool BuildBuilding(Hexagon clickedHexagon, int clickedIndex, Game game)
        {
            if (!CheckBuildBuilding(clickedHexagon, clickedIndex, game)) 
                return false;

            var player = game.GetCurrentPlayer();
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
                    Hexagons[i - 1].Neighbours.Right = Hexagons[i];

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
                Hexagon hexagon = new Hexagon(number: planNumbers[i], material: planMaterial[i]);
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
}
