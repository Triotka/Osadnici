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
    /// 

    public class HexagonActionField
    {
        public static void CreateActionButtons(int size, RoutedEventHandler pirateHandler, RoutedEventHandler stopBuildHandler, Grid outerGrid)
        {
            StackPanel actionStackPanel = new StackPanel();
            var switchButton = GenericWindow.CreateActionButton(size: size, content: "Stop build", stackPanel: actionStackPanel, handler: stopBuildHandler);
            var pirateButton = GenericWindow.CreateActionButton(size: size, content: "Place a pirate", stackPanel: actionStackPanel, handler: pirateHandler);
            outerGrid.Children.Add(actionStackPanel);
        }
    }

    public class DrawnBigHexagon
    {
        Polygon drawnHexagon;
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
        private int CreateBuildingButtons(int hexagonSize, int buttonSize, int buttonNumber, RoutedEventHandler handler, Grid outerGrid)
        {
            var pointsList = GenericWindow.GetHexagonPoints(hexagonSize, drawnHexagon);
            foreach (var point in pointsList)
            {
                CreateBuildButton(margin: new Thickness(point.X, point.Y, 0, 0), size: buttonSize, color: Brushes.LightPink,
                    buttonNumber: buttonNumber, handler: handler, outerGrid: outerGrid);
                buttonNumber++;
            }
            return buttonNumber;


        }

        // creates buttons where you can place roads
        private void CreateRoadButtons(int hexagonSize, int buttonSize, int buttonNumber, RoutedEventHandler handler, Grid outerGrid)
        {
            var pointsList = GenericWindow.GetHexagonPoints(hexagonSize, drawnHexagon);

            double hexagonHeight = pointsList[3].Y - pointsList[0].Y + hexagonSize / 3;
            double hexagonWidth = pointsList[2].X - pointsList[4].X;

            for (int i = -1; i <= 1; i += 2)
            {
                CreateBuildButton(new Thickness((i * hexagonWidth) / 2, 0, 0, 0), buttonSize, color: Brushes.LightPink,
                                                buttonNumber: buttonNumber, handler, outerGrid);
                buttonNumber++;
                for (int j = -1; j <= 1; j += 2)
                {
                    CreateBuildButton(new Thickness((i * hexagonWidth) / 4, (j * hexagonHeight) / 4, 0, 0), buttonSize, color: Brushes.LightPink,
                                        buttonNumber: buttonNumber, handler, outerGrid);
                    buttonNumber++;
                }
            }
        }
        public void CreatePickButtons(int hexagonSize, int buttonSize, RoutedEventHandler roadHandler, RoutedEventHandler buildingHandler, Grid outerGrid)
        {
            int buttonNumber = CreateBuildingButtons(hexagonSize: hexagonSize, buttonSize: buttonSize, buttonNumber: 0, buildingHandler, outerGrid);
            CreateRoadButtons(hexagonSize: hexagonSize, buttonSize: buttonSize, buttonNumber, roadHandler, outerGrid);
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
    public partial class WindowHexagon : Window
    {
        Game GameLogic;
        Label annoucmentLabel;
        int clickedHexagonIndex;


        

        
       



        

        
        void BuildingButton_Click(object sender, EventArgs e)
        {
            // index of button I picked
            int buttonIndex = GenericWindow.FindObjectIndex((Button)sender);
            bool sucessfulBuild = GameLogic.Board.BuildBuilding(clickedHexagon: GameLogic.Board.Hexagons[this.clickedHexagonIndex],
                                  clickedIndex: buttonIndex, game: this.GameLogic);

            if (!sucessfulBuild) // pick other place
            {
                annoucmentLabel.Content = "Invalid build pick better place or press X";
            }
            else
            {
                GameLogic.ChangeBuildActivity(); 
                var winner = GameLogic.CheckWinner();
                if ( winner != null) // check winner
                {
                    var winnerWindow = new WinnerWindow(winner);
                    winnerWindow.Show();
                    this.Close();
                }
                else
                {
                    var mainWindow = new MainWindow(this.GameLogic, "Building was successful");
                    mainWindow.Show();
                    this.Close();
                }
               
            }

            
        }
        void RoadButton_Click(object sender, EventArgs e)
        {
            int buttonIndex = GenericWindow.FindObjectIndex((Button)sender);
            bool sucessfulBuild = GameLogic.Board.BuildRoad(clickedHexagon: GameLogic.Board.Hexagons[this.clickedHexagonIndex],
                                  clickedIndex: buttonIndex, game: this.GameLogic);

            if (!sucessfulBuild) // pick other place
            {
                annoucmentLabel.Content = "Invalid build pick better place or press X";
            }
            else
            {
                GameLogic.ChangeBuildActivity();
                var winner = GameLogic.CheckWinner();
                if (winner != null) // check winner
                {
                    var winnerWindow = new WinnerWindow(winner);
                    winnerWindow.Show();
                    this.Close();
                }
                else
                {
                    var mainWindow = new MainWindow(this.GameLogic, "Building was successful");
                    mainWindow.Show();
                    this.Close();
                };

            }
        }

        void PirateButton_Click(object sender, RoutedEventArgs e)
        {
            string pirateMessage;
            if (GameLogic.GetCurrentPlayer().Activity == Activity.MovingPirate)
            {
                bool sucessfulPirate = GameLogic.Pirate.PlacePirate(clickedHexagonIndex: this.clickedHexagonIndex, gameLogic: this.GameLogic);
                if (!sucessfulPirate) // pick another place
                {
                    pirateMessage = "Pirate is already here, pick different one";
                }
                else
                {
                    pirateMessage = "Pirate was successful";
                }
            }
            else
            {
                pirateMessage = "You cannot move a pirate right now";
            }
           

            var mainWindow = new MainWindow(this.GameLogic, pirateMessage);
            mainWindow.Show();
            this.Close();
        }
        void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            string exitMessage = "Choose different place for building";
            if (this.GameLogic.GetCurrentPlayer().Activity == Activity.MovingPirate)
            {
                exitMessage = "Choose different place for a pirate";
            }
            var mainWindow = new MainWindow(this.GameLogic, exitMessage);
            mainWindow.Show();
            this.Close();
        }

        void StopBuildButton_Click(object sender, RoutedEventArgs e)
        {
            var player = GameLogic.GetCurrentPlayer();
            if (player.Activity == Activity.BuildingTown || player.Activity == Activity.BuildingVillage || player.Activity == Activity.BuildingRoad)
            {
                player.Activity = Activity.None;
                string exitMessage = "Building stopped";
                var mainWindow = new MainWindow(this.GameLogic, exitMessage);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                annoucmentLabel.Content = "Cannot stop building";
            }
            
        }

        public WindowHexagon(Game game, Polygon clickedHexagon, Button clickedButton, int clickedIndex)
        {
            this.GameLogic = game;
            this.clickedHexagonIndex = clickedIndex;
            GenericWindow.SetWindowStyle(window: this);
            InitializeComponent();
            var height = SystemParameters.PrimaryScreenHeight;
            var width = SystemParameters.PrimaryScreenWidth;

            GenericWindow.CreateExitButton(handler: new RoutedEventHandler(ExitButton_Click), size: (int) height / 12, outerGrid: outerGrid);
            this.annoucmentLabel = GenericWindow.CreateAnnoucmentLabel(width: (int)height, height: (int)height / 12, outerGrid: outerGrid,
                                   initMessage: $"Pick a place to build on");
            var drawnHexagon = new DrawnBigHexagon();
            drawnHexagon.CreateHexagonWithNum(clickedHexagon:clickedHexagon, clickedButton: clickedButton,
                                margin: new Thickness(0,0, 0, 0), size: (int)height / 8, outerGrid);
            drawnHexagon.CreatePickButtons((int) (height / 2.6667), (int)height/18, new RoutedEventHandler(RoadButton_Click), new RoutedEventHandler(BuildingButton_Click), outerGrid);
            HexagonActionField.CreateActionButtons((int)height / 12, new RoutedEventHandler(PirateButton_Click), new RoutedEventHandler(StopBuildButton_Click), outerGrid);
            


        }
    }
}
