using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Osadnici
{
    /// <summary>
    /// Interaction logic for WindowHexagon.xaml
    /// </summary>
    public partial class WindowHexagon : Window
    {
        Polygon drawnHexagon;
        Game GameLogic;
        Label annoucmentLabel;
        int clickedHexagonIndex;

        // creates button where you can build
        private void CreateBuildButton(Thickness margin, int size, SolidColorBrush color, int buttonNumber, RoutedEventHandler handler) 
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
        private int CreateBuildingButtons(int hexagonSize, int buttonSize, int buttonNumber)
        {
            var pointsList = GenericWindow.GetHexagonPoints(hexagonSize, drawnHexagon);
            foreach (var point in pointsList)
            {
                CreateBuildButton(margin: new Thickness(point.X, point.Y, 0, 0), size: buttonSize, color:Brushes.LightPink,
                    buttonNumber: buttonNumber, handler: new RoutedEventHandler(BuildingButton_Click));
                buttonNumber++;
            }
            return buttonNumber;
           
            
        }

        // creates buttons where you can place roads
        private void CreateRoadButtons(int hexagonSize, int buttonSize, int buttonNumber)
        {
            var pointsList = GenericWindow.GetHexagonPoints(hexagonSize, drawnHexagon);

            double hexagonHeight = pointsList[3].Y - pointsList[0].Y + hexagonSize/3;
            double hexagonWidth = pointsList[2].X - pointsList[4].X;
           
            for (int i = -1; i <= 1; i+=2)
            {
                CreateBuildButton(new Thickness((i*hexagonWidth) / 2, 0, 0, 0), buttonSize, color: Brushes.LightPink,
                                                buttonNumber: buttonNumber, new RoutedEventHandler(RoadButton_Click));
                buttonNumber++;
                for (int j = -1; j<= 1; j+=2)
                {
                    CreateBuildButton(new Thickness((i * hexagonWidth)/ 4, (j * hexagonHeight) / 4, 0, 0), buttonSize, color: Brushes.LightPink,
                                        buttonNumber: buttonNumber, new RoutedEventHandler(RoadButton_Click));
                    buttonNumber++;
                }
            }
        }
        private void CreatePickButtons(int hexagonSize, int buttonSize)
        {
           int buttonNumber = CreateBuildingButtons(hexagonSize: hexagonSize, buttonSize: buttonSize, buttonNumber: 0); //TODO zmenit parametr na prebirani
            CreateRoadButtons(hexagonSize: hexagonSize, buttonSize: buttonSize, buttonNumber);
        }

        private void CreateLabelOnHexagon(Button clickedButton, Thickness margin, int size)
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
        private void CreateHexagonWithNum(Polygon clickedHexagon, Button clickedButton, Thickness margin, int size)
        {
            this.drawnHexagon = GenericWindow.CreateHexagon(color: clickedHexagon.Fill, margin: margin, size: size,
                                        outerGrid: outerGrid);
            CreateLabelOnHexagon(clickedButton: clickedButton, margin: margin, size: size);
        }

        
        void BuildingButton_Click(object sender, EventArgs e)
        {
            // index of button I picked
            int buttonIndex = GenericWindow.FindObjectIndex((Button)sender);
            bool sucessfulBuild = GameLogic.Board.BuildBuilding(clickedHexagon: GameLogic.Board.Hexagons[this.clickedHexagonIndex],
                                  clickedIndex: buttonIndex, game: this.GameLogic);

            if (!sucessfulBuild) // pick other place
            {
                annoucmentLabel.Content = "Invalid build pick better place or press X on the right to get board view";
            }
            else
            {
               GameLogic.ChangeBuildActivity(); //TODO odkomentovat
                var mainWindow = new MainWindow(this.GameLogic, "Building was successful");
                mainWindow.Show();
                this.Close();
            }

            // zobrazeni buildings na main je TODO
        }
        void RoadButton_Click(object sender, EventArgs e)
        {
            int buttonIndex = GenericWindow.FindObjectIndex((Button)sender);
            bool sucessfulBuild = GameLogic.Board.BuildRoad(clickedHexagon: GameLogic.Board.Hexagons[this.clickedHexagonIndex],
                                  clickedIndex: buttonIndex, game: this.GameLogic);

            if (!sucessfulBuild) // pick other place
            {
                annoucmentLabel.Content = "Invalid build pick better place or press X on the right to get board view";
            }
            else
            {
                GameLogic.ChangeBuildActivity(); //TODO odkomentovat
                var mainWindow = new MainWindow(this.GameLogic, "Building was successful");
                mainWindow.Show();
                this.Close();
            }
        }
        void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow(this.GameLogic, "Choose different place for building");
            mainWindow.Show();
            this.Close();
        }

        public WindowHexagon(Game game, Polygon clickedHexagon, Button clickedButton, int clickedIndex) // TODO upravit velikost
        {
            this.GameLogic = game;
            this.clickedHexagonIndex = clickedIndex;
            GenericWindow.SetWindowStyle(window: this);
            InitializeComponent();
            GenericWindow.CreateExitButton(handler: new RoutedEventHandler(ExitButton_Click), size: 60, outerGrid: outerGrid);
            this.annoucmentLabel = GenericWindow.CreateAnnoucmentLabel(width: 500, height: 60, outerGrid: outerGrid,
                                   initMessage: $"Pick a place to build on");
            CreateHexagonWithNum(clickedHexagon:clickedHexagon, clickedButton: clickedButton,
                                margin: new Thickness(0,0, 0, 0), size: 90);
           CreatePickButtons(270, 40);


        }
    }
}
