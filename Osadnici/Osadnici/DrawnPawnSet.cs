using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace Osadnici
{
    class DrawnPawnSet
    {
        Game gameLogic;
        int sizeOfHexagon;
        List<Hexagon> hexagons;
        Grid outerGrid;
        List<Polygon> drawnHexagons;

        public DrawnPawnSet(Game game, int sizeOfHexagon, Grid grid, List<Polygon> drawnHexagons)
        {
            gameLogic = game;
            this.sizeOfHexagon = sizeOfHexagon;
            this.hexagons = gameLogic.Board.Hexagons;
            outerGrid = grid;
            this.drawnHexagons = drawnHexagons;
        }
        public void CreatePawns(int pawnSize)
        {
            CreatePirate(pawnSize: pawnSize, drawnHexagons[gameLogic.Pirate.Location]);
            for (int i = 0; i < hexagons.Count; i++)
            {
                CreateBuildings(pawnSize: pawnSize, hexagonIndex: i, drawnHexagons[i]);
                CreateRoads(pawnSize: pawnSize, hexagonIndex: i, drawnHexagons[i]);
            }

        }
        private void CreatePirate(int pawnSize, Polygon drawnHexagon)
        {

            var point = GetPiratePoint(drawnHexagon);
            var margin = new Thickness(point.X + drawnHexagon.Margin.Left, point.Y + drawnHexagon.Margin.Top, drawnHexagon.Margin.Right + 2 * sizeOfHexagon, drawnHexagon.Margin.Bottom + 2 * sizeOfHexagon);
            GenericWindow.CreateSquare(Brushes.Black, margin, pawnSize, this.outerGrid);

        }

        // get positin on drawn hexagon to place pirate, in the middle
        private Point GetPiratePoint(Polygon drawnHexagon)
        {
            Point piratePoint = new Point();
            var hexagonPoints = GenericWindow.GetHexagonPoints(sizeOfHexagon, drawnHexagon);
            piratePoint.X = hexagonPoints[5].X + Math.Abs(hexagonPoints[5].X - hexagonPoints[1].X) / 2;
            piratePoint.Y = hexagonPoints[0].Y + Math.Abs(hexagonPoints[3].Y - hexagonPoints[0].Y) / 2;

            return piratePoint;
        }
        // creates buildings for one hexagon
        private void CreateBuildings(int pawnSize, int hexagonIndex, Polygon drawnHexagon)
        {
            var currentHexagon = hexagons[hexagonIndex];
            var hexagonPoints = GenericWindow.GetHexagonPoints(sizeOfHexagon, drawnHexagon);


            for (int i = 0; i < currentHexagon.Buildings.Count(); i++)
            {
                var point = hexagonPoints[i];
                var building = currentHexagon.Buildings[i];
                CreateBuilding(pawnSize: pawnSize, building: building, point: point, drawnHexagon: drawnHexagon);
            }
        }


        private void CreateBuilding(int pawnSize, Building building, Polygon drawnHexagon, Point point)
        {
            var margin = new Thickness(point.X + drawnHexagon.Margin.Left, point.Y + drawnHexagon.Margin.Top, drawnHexagon.Margin.Right + 2 * sizeOfHexagon, drawnHexagon.Margin.Bottom + 2 * sizeOfHexagon);

            if (building.Type == PawnType.Village)
            {
                GenericWindow.CreateSquare(MatchBuildingColor(building.Color), margin, pawnSize, this.outerGrid);
            }
            if (building.Type == PawnType.Town)
            {
                GenericWindow.CreateTriangle(MatchBuildingColor(building.Color), margin, pawnSize, this.outerGrid);
            }
        }

        // get list of points where roads can be placed on one hexagon
        private List<Point> GetRoadPoints(Polygon drawnHexagon)
        {
            var roadPoints = new List<Point>();
            var hexagonPoints = GenericWindow.GetHexagonPoints(sizeOfHexagon, drawnHexagon);
            for (int i = 0; i < hexagonPoints.Count; i++)
            {
                var firstPoint = hexagonPoints[i];
                var secondPoint = hexagonPoints[(i + 1) % hexagonPoints.Count];
                var roadPoint = new Point();
                var differenceX = Math.Abs(firstPoint.X - secondPoint.X) / 2;
                var differenceY = Math.Abs(firstPoint.Y - secondPoint.Y) / 2;

                if (firstPoint.X < secondPoint.X)
                {
                    roadPoint.X = firstPoint.X + differenceX;
                }
                else
                {
                    roadPoint.X = secondPoint.X + differenceX;
                }

                if (firstPoint.Y < secondPoint.Y)
                {
                    roadPoint.Y = firstPoint.Y + differenceY;
                }
                else
                {
                    roadPoint.Y = secondPoint.Y + differenceY;
                }
                roadPoints.Add(roadPoint);
            }
            return roadPoints;

        }
        // creates buildings for one hexagon
        private void CreateRoads(int pawnSize, int hexagonIndex, Polygon drawnHexagon)
        {
            var currentHexagon = hexagons[hexagonIndex];
            var hexagonPoints = GetRoadPoints(drawnHexagon);


            for (int i = 0; i < currentHexagon.Roads.Count(); i++)
            {
                var positionOnMap = MapRoadPoints(6 + i);
                var point = hexagonPoints[positionOnMap];
                var road = currentHexagon.Roads[i];
                CreateRoad(pawnSize: pawnSize, road: road, point: point, drawnHexagon: drawnHexagon);
            }
        }
        private int MapRoadPoints(int roadIndex)
        {

            switch (roadIndex)
            {
                case 6:
                    return 4;
                case 7:
                    return 5;
                case 8:
                    return 3;
                case 9:
                    return 1;
                case 10:
                    return 0;
                case 11:
                    return 2;
            }
            throw new Exception();
        }

        private void CreateRoad(int pawnSize, Road road, Polygon drawnHexagon, Point point)
        {
            var margin = new Thickness(point.X + drawnHexagon.Margin.Left, point.Y + drawnHexagon.Margin.Top, drawnHexagon.Margin.Right + 2 * sizeOfHexagon, drawnHexagon.Margin.Bottom + 2 * sizeOfHexagon);
            if (road.Color != Color.None)
                GenericWindow.CreateCircle(MatchBuildingColor(road.Color), margin, pawnSize, this.outerGrid);
        }

        private SolidColorBrush MatchBuildingColor(Color color)
        {

            switch(color)
            {
                case Color.Blue:
                    return Brushes.Blue;
                case Color.Yellow:
                    return Brushes.Yellow;
                case Color.White:
                    return Brushes.White;
                case Color.Red:
                    return Brushes.Red;
        }
            return Brushes.Transparent;

        }

    }
}
