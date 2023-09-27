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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Printing;
using System.Windows.Media.Media3D;
using System.Collections;

namespace Osadnici
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 


    enum CardType
    {
        Material,
        Build
    }
    public partial class MainWindow : Window
    {
        List<Polygon> hexagons;
        Game gameLogic;
        Label playerLabel;
        Label annoucmentLabel;
        DrawnCardsSet materialCards;
        DrawnCardsSet buildCards;

        // updates player's label info
        private void UpdatePlayerLabel()
        {
            var player = gameLogic.GetCurrentPlayer();
            this.playerLabel.Content = $"Player: {player.Color}\nPoints: {player.Points}\nActivity: {gameLogic.ActivityToString()}";
        }
        // creates label with players color, points and activity
        private void CreatePlayerLabel(int size, int margin)
        {

            Label label = new Label();
            label.Background = Brushes.Transparent;
            label.Foreground = Brushes.White;
            label.Width = size;
            label.Height = margin * 2;
            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.VerticalAlignment = VerticalAlignment.Center;
            label.FontSize = margin / 3.25;     
            outerGrid.Children.Add(label);
            this.playerLabel = label;
            UpdatePlayerLabel();
        }

        // creates label with prices of pawns
        private void CreatePricesLabel(int size, int margin)
        {
            string[] prices = {"Road Price: Brick 1x, Wood 1x\n",
                                "Town Price: Stone 3x, Wheat 2x\n",
                                "Village Price: Brick 1x, Wood 1x, Lamb 1x, Wheat 1x"};
            Label label = new Label();
            label.Background = Brushes.Transparent;
            label.Foreground = Brushes.White;
            label.Width = size * 1.25;
            label.Height = margin * 2;
            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.VerticalAlignment = VerticalAlignment.Center;
            label.Margin = new Thickness(0, playerLabel.Height * 2 + margin, 0, 0);

            string priceString = "";
            foreach (var price in prices)
            {
                priceString += price;
            }
            label.Content = priceString;

            label.FontSize = margin / 3.25;
            outerGrid.Children.Add(label);
        }

        void BoardButton_Click(object sender, RoutedEventArgs e) 
        {
            Button clickedButton = (Button)sender;
            int clickedIndex = GenericWindow.FindObjectIndex(clickedButton);
            var clickedHexagon = this.hexagons[clickedIndex];

            if (gameLogic.IsBoardInteraction())
            { 
                var hexagonWindow = new WindowHexagon(game: this.gameLogic, clickedHexagon: clickedHexagon,
                                                clickedButton: clickedButton, clickedIndex: clickedIndex);
                hexagonWindow.Show();
                this.Close();
            }
        }
        
        void BuildButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            this.annoucmentLabel.Content = gameLogic.BuildAction(clickedButton.Name);
            UpdatePlayerLabel();

        }

        void MaterialButton_Click(object sender, RoutedEventArgs e)
        {

            Button clickedButton = (Button)sender;

            (string message, SameCardsSet clickedCardsSet) = gameLogic.SellAction(clickedButton.Name);
            if (clickedCardsSet == null)
                annoucmentLabel.Content = message;
            else
            {
                var sellWinddow = new SellWindow(game: gameLogic, clickedCardsSet);
                sellWinddow.Show();
                this.Close();
            }
        }
        void SwitchButton_Click(object sender, RoutedEventArgs e) 
        {
            var message = gameLogic.SwitchPlayers();
            UpdatePlayerLabel();
            materialCards.UpdateMaterialCards();
            buildCards.UpdateBuildCards();
            annoucmentLabel.Content = message;  

        }
        void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public MainWindow(Game game, string initMessage)
        {

            this.gameLogic = game;
            GenericWindow.SetWindowStyle(window: this);
            InitializeComponent();
            var height = SystemParameters.PrimaryScreenHeight;
            var width = SystemParameters.PrimaryScreenWidth;

            GenericWindow.CreateExitButton(handler: new RoutedEventHandler(ExitButton_Click), size: (int)height / 12, outerGrid: outerGrid);
            this.hexagons = new DrawnBoard(game: game, boardGrid: boardGrid, handler: new RoutedEventHandler(BoardButton_Click)).CreateBoard(size: (int)height / 18, start: new Point(0, (int)(height / 7.2)));
            this.buildCards = new DrawnCardsSet(game: game, grid: outerGrid, size: (int)height / 8, handler: new RoutedEventHandler(BuildButton_Click), type: CardType.Build);
            this.materialCards = new DrawnCardsSet(game: game, grid: outerGrid, size: (int)height / 8, handler: new RoutedEventHandler(MaterialButton_Click), type: CardType.Material);
            DrawnActionField.CreateActionButtons(size: (int)height/12, outerGrid: outerGrid, new List<Handler>{new Handler(function: new RoutedEventHandler(SwitchButton_Click), "Next Player")});
            this.annoucmentLabel = GenericWindow.CreateAnnoucmentLabel(width: (int)height, height: (int)height / 12, outerGrid: outerGrid,
                                   initMessage: initMessage);
            CreatePlayerLabel(size: (int)height / 2, margin: (int)height / 12);
            CreatePricesLabel(size: (int)height / 2, margin: (int)height / 12);
            new DrawnPawnSet(game, (int)height / 18, outerGrid, hexagons).CreatePawns((int)height / 36);
           
        }
    }
}
    