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
    class DrawnBoard
    {
        Game gameLogic;
        Grid boardGrid;
        List<Polygon> hexagons;
        RoutedEventHandler handler;

        public DrawnBoard(Game game, Grid boardGrid, RoutedEventHandler handler)
        {
            this.gameLogic = game;
            this.boardGrid = boardGrid;
            this.handler = handler;
            this.hexagons = new List<Polygon>();
        }

        // creates board
        public List<Polygon> CreateBoard(int size, Point start)
        {
            List<Polygon> hexagons = CreateHexagons(size: size, start: start);
            CreateBoardButtons(size: size, start: start);

            return hexagons;
        }
        private void CreateBoardButtons(int size, Point start)
        {
            int hexagonIndex = 18;
            for (int i = -1; i <= 1; i++) // rows of length 3 bottom
            {
                CreateBoardButton(margin: new Thickness(0, 0, 6 * i * size + start.X, -8 * size + start.Y), size: size, numInRow: hexagonIndex);
                hexagonIndex--;
            }
            for (int i = -2; i <= 1; i++) // rows of length 4 bottom
            {
                CreateBoardButton(margin: new Thickness(0, 0, 6 * i * size + 3 * size + start.X, -4 * size + start.Y), size: size, numInRow: hexagonIndex);
                hexagonIndex--;
            }

            for (int i = -2; i <= 2; i++) // rows of length 5
            {
                CreateBoardButton(margin: new Thickness(0, 0, 6 * i * size + start.X, start.Y), size: size, numInRow: hexagonIndex);
                hexagonIndex--;
            }

            for (int i = -2; i <= 1; i++) // rows of length 4 top
            {
                CreateBoardButton(margin: new Thickness(0, 0, 6 * i * size + 3 * size + start.X, 4 * size + start.Y), size: size, numInRow: hexagonIndex);
                hexagonIndex--;
            }

            for (int i = -1; i <= 1; i++) // rows of length 3 top
            {
                CreateBoardButton(margin: new Thickness(0, 0, 6 * i * size + start.X, 8 * size + start.Y),
                                  size: size, numInRow: hexagonIndex);
                hexagonIndex--;
            }

        }
        // creates buttons on board
        private void CreateBoardButton(int size, Thickness margin, int numInRow)
        {
            int number = MatchIndexToNumber(numInRow);
            Button boardButton = new Button();
            boardButton.HorizontalAlignment = HorizontalAlignment.Center;
            boardButton.VerticalAlignment = VerticalAlignment.Center;
            boardButton.Width = size * 3 + 10;
            boardButton.Height = size * 3 + 10;
            boardButton.Margin = margin;
            boardButton.Background = Brushes.Transparent;
            boardButton.Name = $"boardButton{numInRow}";
            boardButton.Foreground = Brushes.Black;
            boardButton.FontSize = size;

            if (number != gameLogic.PirateNumber)
            {
                boardButton.Content = $"{number}";
            }
            boardButton.AddHandler(Button.ClickEvent, handler);
            boardGrid.Children.Add(boardButton);
        }

        private List<Polygon> CreateHexagons(int size, Point start)
        {
            int hexagonIndex = 18;
            for (int i = -1; i <= 1; i++) // row of length 3 bottom
            {
                var hexagon = GenericWindow.CreateHexagon(MatchIndexToColor(hexagonIndex), new Thickness(0, 0, 6 * i * size + start.X, -8 * size + start.Y),
                                            size: size, boardGrid);
                hexagonIndex--;
                this.hexagons.Add(hexagon);
            }

            for (int i = -2; i <= 1; i++) // rows of length 4 bottom
            {
                var hexagon = GenericWindow.CreateHexagon(MatchIndexToColor(hexagonIndex), new Thickness(0, 0, 6 * i * size + 3 * size + start.X, -4 * size + start.Y),
                                            size: size, boardGrid);
                hexagonIndex--;
                this.hexagons.Add(hexagon);

            }
            for (int i = -2; i <= 2; i++) // rows of length 5
            {
                var hexagon = GenericWindow.CreateHexagon(MatchIndexToColor(hexagonIndex), new Thickness(0, 0, 6 * i * size + start.X, start.Y),
                                            size: size, boardGrid);
                hexagonIndex--;
                this.hexagons.Add(hexagon);
            }

            for (int i = -2; i <= 1; i++) // rows of length 4 top
            {
                var hexagon = GenericWindow.CreateHexagon(MatchIndexToColor(hexagonIndex), new Thickness(0, 0, 6 * i * size + 3 * size + start.X, 4 * size + start.Y),
                                            size: size, boardGrid);
                hexagonIndex--;
                this.hexagons.Add(hexagon);
            }
            for (int i = -1; i <= 1; i++) // row of length 3 top
            {
                var hexagon = GenericWindow.CreateHexagon(MatchIndexToColor(hexagonIndex), new Thickness(0, 0, 6 * i * size + start.X, 8 * size + start.Y),
                                            size: size, boardGrid);
                hexagonIndex--;
                this.hexagons.Add(hexagon);
            }
            hexagons.Reverse();
            return hexagons;
        }

        private int MatchIndexToNumber(int index)
        {
            return gameLogic.Board.Hexagons[index].Number;
        }
        private LinearGradientBrush MatchIndexToColor(int index)
        {
            var material = gameLogic.Board.Hexagons[index].Material;
            switch(material)
            {
                case Material.Brick:
                    return ColorMaker.BoardBrick();
                case Material.Wood:
                    return ColorMaker.BoardWood();
                case Material.Lamb:
                    return ColorMaker.BoardLamb();
                case Material.Wheat:
                    return ColorMaker.BoardWheat();
                case Material.Stone:
                    return ColorMaker.BoardStone();
            }
            return ColorMaker.BoardDesert();
        }
    }
}
