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
    public class DrawnBigHexagon
    {
        Polygon drawnHexagon;
        Game gameLogic;
        int clickedHexagonIndex;

        public DrawnBigHexagon(Game game, int clickedHexagonIndex)
        {
            this.gameLogic = game;
            this.clickedHexagonIndex = clickedHexagonIndex;
        }



        // creates button where you can build

        private void CreateBuildButton(Thickness margin, int size, SolidColorBrush color, int buttonNumber, RoutedEventHandler handler, Grid outerGrid)
        {
            Button newBtn = new Button();
            newBtn.Margin = margin;
            newBtn.Width = size;
            newBtn.Height = size;
            newBtn.Background = color;
            newBtn.Name = $"build{buttonNumber}";
            newBtn.AddHandler(Button.ClickEvent, handler);
            outerGrid.Children.Add(newBtn);

        }

        // creates buttons where you can place a building
        private void CreateBuildingButtons(int hexagonSize, int buttonSize, int buttonNumber, RoutedEventHandler handler, Grid outerGrid)
        {
            var pointsList = GenericWindow.GetHexagonPoints(hexagonSize, drawnHexagon);
            foreach (var point in pointsList)
            {
                if (gameLogic.Board.CheckBuildBuilding(gameLogic.Board.Hexagons[this.clickedHexagonIndex], buttonNumber, gameLogic))
                {
                    CreateBuildButton(margin: new Thickness(point.X, point.Y, 0, 0), size: buttonSize, color: Brushes.LightPink,
                    buttonNumber: buttonNumber, handler: handler, outerGrid: outerGrid);
                }

                buttonNumber++;
            }



        }

        // creates buttons where you can place roads
        private void CreateRoadButtons(int hexagonSize, int buttonSize, int buttonNumber, RoutedEventHandler handler, Grid outerGrid)
        {
            var pointsList = GenericWindow.GetHexagonPoints(hexagonSize, drawnHexagon);

            double hexagonHeight = pointsList[3].Y - pointsList[0].Y + hexagonSize / 3;
            double hexagonWidth = pointsList[2].X - pointsList[4].X;

            for (int i = -1; i <= 1; i += 2)
            {
                if (gameLogic.Board.CheckRoadBuildOK(gameLogic.Board.Hexagons[this.clickedHexagonIndex], gameLogic, buttonNumber))
                {
                    CreateBuildButton(new Thickness((i * hexagonWidth) / 2, 0, 0, 0), buttonSize, color: Brushes.LightPink,
                                                buttonNumber: buttonNumber, handler, outerGrid);
                }
                buttonNumber++;
                for (int j = -1; j <= 1; j += 2)
                {
                    if (gameLogic.Board.CheckRoadBuildOK(gameLogic.Board.Hexagons[this.clickedHexagonIndex], gameLogic, buttonNumber))
                    {
                        CreateBuildButton(new Thickness((i * hexagonWidth) / 4, (j * hexagonHeight) / 4, 0, 0), buttonSize, color: Brushes.LightPink,
                                        buttonNumber: buttonNumber, handler, outerGrid);
                    }
                    buttonNumber++;
                }
            }
        }
        public void CreatePickButtons(int hexagonSize, int buttonSize, RoutedEventHandler roadHandler, RoutedEventHandler buildingHandler, Grid outerGrid)
        {

            if (gameLogic.Activity == Activity.StartFirstVillage || gameLogic.Activity == Activity.StartSecondVillage || gameLogic.Activity == Activity.BuildingVillage || gameLogic.Activity == Activity.BuildingTown)
            {
                CreateBuildingButtons(hexagonSize: hexagonSize, buttonSize: buttonSize, buttonNumber: 0, buildingHandler, outerGrid);
            }
            else if (gameLogic.Activity == Activity.StartFirstRoad || gameLogic.Activity == Activity.StartSecondRoad || gameLogic.Activity == Activity.BuildingRoad)
            {
                CreateRoadButtons(hexagonSize: hexagonSize, buttonSize: buttonSize, buttonNumber: 6, roadHandler, outerGrid);

            }

        }

        private void CreateLabelOnHexagon(Button clickedButton, Thickness margin, int size, Grid outerGrid)
        {
            Label label = new Label();
            label.Foreground = Brushes.Black;
            label.Background = Brushes.Transparent;
            label.Content = clickedButton.Content;
            label.Width = size;
            label.Height = size;
            label.FontSize = size / 2;
            label.Margin = margin;
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.VerticalAlignment = VerticalAlignment.Center;
            label.HorizontalContentAlignment = HorizontalAlignment.Center;
            label.VerticalContentAlignment = VerticalAlignment.Center;
            outerGrid.Children.Add(label);
        }
        public void CreateHexagonWithNum(Polygon clickedHexagon, Button clickedButton, Thickness margin, int size, Grid outerGrid)
        {
            this.drawnHexagon = GenericWindow.CreateHexagon(color: clickedHexagon.Fill, margin: margin, size: size,
                                        outerGrid: outerGrid);
            CreateLabelOnHexagon(clickedButton: clickedButton, margin: margin, size: size, outerGrid);
        }
    }
}
