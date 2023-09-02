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

namespace Osadnici
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

   
    class Start
    {
        public int Left { get; set; }
        public int Up { get; set; }

        public Start(int left, int up )
        {
            this.Left = left;
            this.Up = up;
        }
    }
    public partial class MainWindow : Window
    {
        public Game GameLogic;
        Label playerLabel;
        Label annoucmentLabel;
        private void CreateBoard(int size, Start start)
        {
            CreateHexagons(size: size, start: start);
            CreateBoardButtons(size: size, start: start);
        }
        private void CreateBoardButtons(int size, Start start)
        {
            int hexagonIndex = 18;
            for (int i = -1; i <= 1; i++) // rows of length 3 bottom
            {
                CreateBoardButton(margin: new Thickness(0, 0, 6 * i * size + start.Left, -8 * size + start.Up), size: size, numInRow: hexagonIndex);
                hexagonIndex--;
            }
            for (int i = -2; i <= 1; i++) // rows of length 4 bottom
            {
                CreateBoardButton(margin: new Thickness(0, 0, 6 * i * size + 3 * size + start.Left, -4 * size + start.Up), size: size, numInRow: hexagonIndex);
                hexagonIndex--;
            }

            for (int i = -2; i <= 2; i++) // rows of length 5
            {
                CreateBoardButton(margin: new Thickness(0, 0, 6 * i * size + start.Left, start.Up), size: size, numInRow: hexagonIndex);
                hexagonIndex--;
            }

            for (int i = -2; i <= 1; i++) // rows of length 4 top
            {
                CreateBoardButton(margin: new Thickness(0, 0, 6 * i * size + 3 * size + start.Left, 4 * size + start.Up), size: size, numInRow:hexagonIndex);
                hexagonIndex--;
            }

            for (int i = -1; i <= 1; i++) // rows of length 3 top
            {
                CreateBoardButton(margin: new Thickness(0, 0, 6 * i * size + start.Left, 8 * size + start.Up), size: size, numInRow:hexagonIndex);
                hexagonIndex--;
            }

        }
        private LinearGradientBrush MatchIndexToColor(int index)
        {
            var material = GameLogic.Board.Hexagons[index].Material;

            if (material == Material.Brick)
            {
                return ColorMaker.BoardBrick();
            }
            if (material == Material.Wood)
            {
                return ColorMaker.BoardWood();
            }
            if (material == Material.Lamb)
            {
                return ColorMaker.BoardLamb();
            }
            if (material == Material.Wheat)
            {
                return ColorMaker.BoardWheat();
            }
            if (material == Material.Stone)
            {
                return ColorMaker.BoardStone();
            }
            return ColorMaker.BoardDesert();
        }
        private int MatchIndexToNumber(int index)
        {
            return GameLogic.Board.Hexagons[index].Number;
        }
        private void CreateHexagons(int size, Start start)
        {
            int hexagonIndex = 18;
            for (int i = -1; i <= 1; i++) // row of length 3 bottom
            {
                CreateHexagon(MatchIndexToColor(hexagonIndex), new Thickness(0, 0, 6 * i * size + start.Left, -8 * size + start.Up), size: size);
                hexagonIndex--;
            }

            for (int i = -2; i <= 1; i++) // rows of length 4 bottom
            {
                CreateHexagon(MatchIndexToColor(hexagonIndex), new Thickness(0, 0, 6 * i * size + 3 * size + start.Left, -4 * size + start.Up), size: size);
                hexagonIndex --;
            }
            for (int i = -2; i <= 2; i++) // rows of length 5
            {
                CreateHexagon(MatchIndexToColor(hexagonIndex), new Thickness(0, 0, 6 * i * size + start.Left, start.Up), size: size);
                hexagonIndex--;
            }

            for (int i = -2; i <= 1; i++) // rows of length 4 top
            {
                CreateHexagon(MatchIndexToColor(hexagonIndex), new Thickness(0, 0, 6 * i * size + 3 * size + start.Left, 4 * size + start.Up), size: size);
                hexagonIndex--;
            }
            for (int i = -1; i <= 1; i++) // row of length 3 top
            {
                CreateHexagon(MatchIndexToColor(hexagonIndex), new Thickness(0, 0, 6 * i * size + start.Left, 8 * size + start.Up), size: size);
                hexagonIndex--;
            }
        }

        // creates hexagons
        private void CreateHexagon(Brush color, Thickness margin, int size)
        {
            Polygon polygon = new Polygon();
            polygon.Points = new PointCollection() { new Point(size * 1.5, 0), new Point(size * 3, size), new Point(size * 3, size * 2), new Point(size * 1.5, size * 3), new Point(0, size * 2), new Point(0, size) };
            polygon.Stroke = Brushes.Black;
            polygon.StrokeThickness = 3;
            polygon.Fill = color;
            polygon.HorizontalAlignment = HorizontalAlignment.Center;
            polygon.VerticalAlignment = VerticalAlignment.Center;
            polygon.Margin = margin;
            boardGrid.Children.Add(polygon);
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
            boardButton.Name = $"BoardButton{numInRow}";
            boardButton.Foreground = Brushes.Black;
            boardButton.FontSize = size;

            if (number != GameLogic.pirateNumber)
            {
                boardButton.Content = $"{number}";
            }
            boardButton.AddHandler(Button.ClickEvent, new RoutedEventHandler(BoardButton_Click));
            boardGrid.Children.Add(boardButton);
        }
        // creates button to exit the game
        private void CreateExitButton(int size)
        {
            Button exitButton = new Button();
            exitButton.HorizontalAlignment = HorizontalAlignment.Right;
            exitButton.VerticalAlignment = VerticalAlignment.Top;
            exitButton.Width = size;
            exitButton.Height = size;
            exitButton.Background = Brushes.Red;
            exitButton.Content = "X";
            exitButton.FontSize = size * 0.75;
            exitButton.Foreground = Brushes.White;
            exitButton.AddHandler(Button.ClickEvent, new RoutedEventHandler(ExitButton_Click));
            outerGrid.Children.Add(exitButton);
        }
        private void CreateActionButtons(int size)
        {
            StackPanel actionStackPanel = new StackPanel();
            var switchButton = CreateActionButton(size: size, content: "Next player", stackPanel: actionStackPanel, handler: new RoutedEventHandler(SwitchButton_Click));
            var diceButton = CreateActionButton(size:size, content:"Roll dice", stackPanel: actionStackPanel, handler: new RoutedEventHandler(DiceButton_Click));
            outerGrid.Children.Add(actionStackPanel);
           
        }

        private Button CreateActionButton(int size, string content, StackPanel stackPanel, RoutedEventHandler handler)
        {
            Button actionButton = new Button();
            actionButton.HorizontalAlignment = HorizontalAlignment.Left;
            actionButton.VerticalAlignment = VerticalAlignment.Top;
            actionButton.Width = size * 2;
            actionButton.Height = size;
            actionButton.Background = ColorMaker.CreateButtonPaint();
            actionButton.Foreground = Brushes.White;
            actionButton.FontSize = size * 0.35;
            actionButton.Margin = new Thickness(size / 4, 0, size / 4, 0);
            actionButton.Content = content;
            actionButton.AddHandler(Button.ClickEvent, handler);
            stackPanel.Children.Add(actionButton);

            return actionButton;
        }

        // creates cards with material
        private void CreateMaterialCards(int size) // TODO přidat Label
        {
            StackPanel outerStackPanel = new StackPanel();
            outerStackPanel.Background = ColorMaker.CreateCardBackground();
            outerStackPanel.Orientation = Orientation.Vertical;
            outerStackPanel.VerticalAlignment = VerticalAlignment.Bottom;
            outerStackPanel.HorizontalAlignment = HorizontalAlignment.Right;
            var buildLabel = createsCardsLabel(size: size, "Material");
            var buttonStackPanel = CreateMaterialButtons(size);
            outerStackPanel.Children.Add(buildLabel);
            outerStackPanel.Children.Add(buttonStackPanel);
            outerGrid.Children.Add(outerStackPanel);
        }

        // creates button in shape of card
        private Button CreateButtonCard(int size)
        {
            System.Windows.Controls.Button newBtn = new Button();
            newBtn.Width = size;
            newBtn.Height = size * 1.5;
            newBtn.Background =  ColorMaker.CreateCardPaint();
            newBtn.FontSize = size * 0.3;
            newBtn.Foreground = Brushes.White;
            newBtn.Margin = new Thickness(size / 4, 0, size / 4, 0); // creates spacing
            newBtn.VerticalContentAlignment = VerticalAlignment.Center;
            newBtn.HorizontalContentAlignment = HorizontalAlignment.Center;
            return newBtn;
        }


        // creates buttons in shape of card with material
        private StackPanel CreateMaterialButtons(int size) 
        {
            StackPanel buttonStackPanel = new StackPanel();
            buttonStackPanel.Orientation = Orientation.Horizontal;
            buttonStackPanel.VerticalAlignment = VerticalAlignment.Bottom;

            var orderedCardsList = GameLogic.GetCurrentPlayer().GetOrderedCards();
            string[] names = Enum.GetNames(typeof(Material));
            for (int i = 0; i < names.Length -1; i++)
            {
                var newBtn = CreateButtonCard(size);
                newBtn.Content = $"Sell\n{names[i]}\n{orderedCardsList[i].Number}";
                newBtn.Name = "Button" + names[i];
                newBtn.AddHandler(Button.ClickEvent, new RoutedEventHandler(MaterialButton_Click));
                buttonStackPanel.Children.Add(newBtn);
            }
            return buttonStackPanel;
        }

        // creates buttons in shape of card with things I can build/buy
        private StackPanel CreateBuildButtons(int size) //TODO barva
        {
            StackPanel buttonStackPanel = new StackPanel();
            buttonStackPanel.Orientation = Orientation.Horizontal;
            buttonStackPanel.VerticalAlignment = VerticalAlignment.Bottom;
            var orderedPawnsList = GameLogic.GetCurrentPlayer().GetOrderedPawns();
            string[] names = Enum.GetNames(typeof(PawnType));
            for (int i = 0; i < names.Length-1; i++)
            {
                var newBtn = CreateButtonCard(size);
                newBtn.Content = $"Buy\n{names[i]}\n{orderedPawnsList[i].Number}";
                newBtn.Name = "Button" + names[i];
                newBtn.AddHandler(Button.ClickEvent, new RoutedEventHandler(BuildButton_Click));
                buttonStackPanel.Children.Add(newBtn);
            }
            return buttonStackPanel;
        }

        // creates cards with things I can build/buy
        private void CreateBuildCards(int size)
        {
            StackPanel outerStackPanel = new StackPanel();
            outerStackPanel.Background = ColorMaker.CreateCardBackground();
            outerStackPanel.Orientation = Orientation.Vertical;
            outerStackPanel.VerticalAlignment = VerticalAlignment.Bottom;
            outerStackPanel.HorizontalAlignment = HorizontalAlignment.Left;
            var buildLabel = createsCardsLabel(size: size, "Buildings");
            var buttonStackPanel = CreateBuildButtons(size);
            outerStackPanel.Children.Add(buildLabel);
            outerStackPanel.Children.Add(buttonStackPanel);
            outerGrid.Children.Add(outerStackPanel);
        }

        // creates label to describe types of cards
        private Label createsCardsLabel(int size, string text)
        {
            Label label = new Label();
            label.Content = text;
            label.Foreground = Brushes.White;
            label.Background = Brushes.Transparent;
            label.Width = size * 2;
            label.Height = size / 2;
            label.FontSize = size * 0.3;
            label.VerticalAlignment = VerticalAlignment.Center;
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.VerticalContentAlignment = VerticalAlignment.Center;
            label.HorizontalContentAlignment = HorizontalAlignment.Center;
            return label;
        }
        // creates label for displaying messages during the game
        private Label CreateAnnoucmentLabel(int width, int height)
        {
            Label annoucementLabel = new Label();
            annoucementLabel.Height = height;
            annoucementLabel.Width = width;
            annoucementLabel.Content = $"Players were set to {GameLogic.Players.Count}";
            annoucementLabel.FontSize = height / 2;
            annoucementLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
            annoucementLabel.Background = Brushes.Transparent;
            annoucementLabel.Foreground = new SolidColorBrush(Colors.White);
            annoucementLabel.HorizontalAlignment = HorizontalAlignment.Center;
            annoucementLabel.VerticalAlignment = VerticalAlignment.Top;
            outerGrid.Children.Add(annoucementLabel);
            return annoucementLabel;
        }

        // create label with players color and points
        private Label CreatePlayerLabel(int size, int margin)
        {
            Label label = new Label();
            label.Background = Brushes.Transparent;
            label.Foreground = Brushes.White;
            label.Width = size;
            label.Height = margin * 2;
            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.VerticalAlignment = VerticalAlignment.Center;
            label.FontSize = margin / 3;
            Player player = GameLogic.GetCurrentPlayer();
            label.Content = $"Player: {player.Color.ToString()}\nPoints: {player.Points}";
            outerGrid.Children.Add(label);
            return label;
        }

        private void DisplayDiceMessage(bool isRolled, string message)
        {
            if (isRolled)
            {
                this.annoucmentLabel.Content = message;
                
            }
            else
            {
                this.annoucmentLabel.Content = message;
            }
        }
        void BoardButton_Click(object sender, RoutedEventArgs e) // TODO
        {
            throw new NotImplementedException();
        }
        void DiceButton_Click(object sender, RoutedEventArgs e) // TODO
        {
            (var isRolled, var message) = GameLogic.HandleDiceRequest();
            DisplayDiceMessage(isRolled, message);


        }
        void BuildButton_Click(object sender, RoutedEventArgs e) // TODO
        {
            throw new NotImplementedException();
        }

        void MaterialButton_Click(object sender, RoutedEventArgs e) // TODO
        {
            throw new NotImplementedException();
        }
        void SwitchButton_Click(object sender, RoutedEventArgs e) // TODO
        {
            Player player = GameLogic.SwitchPlayers();
            playerLabel.Content = $"Player: {player.Color}\nPoints: {player.Points}";
        }
        void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public MainWindow(Game game) // TODO upravit velikosti objektů v zavislosti na poli
        {
            this.GameLogic = game; 
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;
            InitializeComponent();
            CreateExitButton(size: 60);
            CreateBoard(size: 40, start:new Start(0, 100));
            CreateBuildCards(size: 90);
            CreateMaterialCards(size: 90);
            CreateActionButtons(size: 60);
            this.annoucmentLabel = CreateAnnoucmentLabel(width:300, height:60);
            this.playerLabel = CreatePlayerLabel(size: 300, margin: 60);
        }
    }
}
    