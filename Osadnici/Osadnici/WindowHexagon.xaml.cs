using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
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

    public partial class WindowHexagon : Window
    {
        Game gameLogic;
        Label annoucmentLabel;
        int clickedHexagonIndex;

        
        void BuildingButton_Click(object sender, EventArgs e)
        {
            // index of button I picked
            int buttonIndex = GenericWindow.FindObjectIndex((Button)sender);
            bool sucessfulBuild = gameLogic.Board.BuildBuilding(clickedHexagon: gameLogic.Board.Hexagons[this.clickedHexagonIndex],
                                  clickedIndex: buttonIndex, game: this.gameLogic);

            if (!sucessfulBuild) // pick other place
            {
                annoucmentLabel.Content = "Invalid build pick better place or press X";
            }
            else
            {
                gameLogic.ChangeBuildActivity(); 
                var winner = gameLogic.CheckWinner();
                if ( winner != null) // check winner
                {
                    var winnerWindow = new WinnerWindow(winner);
                    winnerWindow.Show();
                    this.Close();
                }
                else
                {
                    var mainWindow = new MainWindow(this.gameLogic, "Building was successful");
                    mainWindow.Show();
                    this.Close();
                }
               
            }

            
        }
        void RoadButton_Click(object sender, EventArgs e)
        {
            int buttonIndex = GenericWindow.FindObjectIndex((Button)sender);
            bool sucessfulBuild = gameLogic.Board.BuildRoad(clickedHexagon: gameLogic.Board.Hexagons[this.clickedHexagonIndex],
                                  clickedIndex: buttonIndex, game: this.gameLogic);

            if (!sucessfulBuild) // pick other place
            {
                annoucmentLabel.Content = "Invalid build pick better place or press X";
            }
            else
            {
                gameLogic.ChangeBuildActivity();
                var winner = gameLogic.CheckWinner();
                if (winner != null) // check winner
                {
                    var winnerWindow = new WinnerWindow(winner);
                    winnerWindow.Show();
                    this.Close();
                }
                else
                {
                    var mainWindow = new MainWindow(this.gameLogic, "Building was successful");
                    mainWindow.Show();
                    this.Close();
                };

            }
        }

        void PirateButton_Click(object sender, RoutedEventArgs e)
        {
            string pirateMessage = gameLogic.PirateAction(this.clickedHexagonIndex);
            
            var mainWindow = new MainWindow(this.gameLogic, pirateMessage);
            mainWindow.Show();
            this.Close();
        }
        void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            string exitMessage = "Choose different place for building";
            if (this.gameLogic.Activity == Activity.MovingPirate)
            {
                exitMessage = "Choose different place for a pirate";
            }
            var mainWindow = new MainWindow(this.gameLogic, exitMessage);
            mainWindow.Show();
            this.Close();
        }

        void StopBuildButton_Click(object sender, RoutedEventArgs e)
        {
            var player = gameLogic.GetCurrentPlayer();
            if (gameLogic.Activity == Activity.BuildingTown || gameLogic.Activity == Activity.BuildingVillage || gameLogic.Activity == Activity.BuildingRoad)
            {
                gameLogic.Activity = Activity.None;
                string exitMessage = "Building stopped";
                var mainWindow = new MainWindow(this.gameLogic, exitMessage);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                annoucmentLabel.Content = "Cannot stop building";
            }
            
        }

        private void CreateActionFieldByActivity(Activity activity, int size, Grid outerGrid)
        {
            if (activity == Activity.BuildingTown || activity == Activity.BuildingVillage || activity == Activity.BuildingRoad)
                DrawnActionField.CreateActionButtons(size: size, outerGrid,
                                                                  new List<Handler>() { new Handler(name: "Stop build", function: new RoutedEventHandler(StopBuildButton_Click))});
            if (activity == Activity.MovingPirate)
                DrawnActionField.CreateActionButtons(size: size, outerGrid,
                                                                  new List<Handler>() { new Handler(name: "Place a pirate", function: new RoutedEventHandler(PirateButton_Click))});
        }

        public WindowHexagon(Game game, Polygon clickedHexagon, Button clickedButton, int clickedIndex)
        {
            this.gameLogic = game;
            this.clickedHexagonIndex = clickedIndex;
            GenericWindow.SetWindowStyle(window: this);
            InitializeComponent();
            var height = SystemParameters.PrimaryScreenHeight;
            var width = SystemParameters.PrimaryScreenWidth;

            GenericWindow.CreateExitButton(handler: new RoutedEventHandler(ExitButton_Click), size: (int) height / 12, outerGrid: outerGrid);
            this.annoucmentLabel = GenericWindow.CreateAnnoucmentLabel(width: (int)height, height: (int)height / 12, outerGrid: outerGrid,
                                   initMessage: $"Pick a place to build on");
            var drawnHexagon = new DrawnBigHexagon(game, clickedHexagonIndex);
            drawnHexagon.CreateHexagonWithNum(clickedHexagon:clickedHexagon, clickedButton: clickedButton,
                                margin: new Thickness(0,0, 0, 0), size: (int)height / 8, outerGrid);
            drawnHexagon.CreatePickButtons((int) (height / 2.6667), (int)height/18, new RoutedEventHandler(RoadButton_Click), new RoutedEventHandler(BuildingButton_Click), outerGrid);
            CreateActionFieldByActivity(gameLogic.Activity, (int)height / 12, outerGrid);
            
            


        }
    }
}
