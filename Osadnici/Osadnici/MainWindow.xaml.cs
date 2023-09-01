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
        public int Right { get; set; }
        public int Up { get; set; }
        public int Down { get; set; }

        public Start(int left, int up )
        {
            this.Left = left;
            this.Up = up;
        }
    }
    public partial class MainWindow : Window
    {
        Game gameLogic = new Game();
        Label label;
        private void CreateBoard(int size, Start start)
        {
            CreateHexagons(size: size, start: start);
            CreateBoardButtons(size: size, start: start);
        }
        private void CreateBoardButtons(int size, Start start)
        {
            CreateBoardButton(margin: new Thickness(0, 0, start.Left, start.Up), size: size);
            for (int i = 1; i <= 2; i++) // rada delky 5
            {
                CreateBoardButton(margin: new Thickness(0, 0, 6 * i * size + start.Left, start.Up), size: size);
                CreateBoardButton(margin: new Thickness(0, 0, -6 * i * size + start.Left, start.Up), size: size);
            }

            for (int i = -2; i <= 1; i++) // rady delky 4
            {
                CreateBoardButton(margin: new Thickness(0, 0, 6 * i * size + 3 * size + start.Left, 4 * size + start.Up), size: size);
                CreateBoardButton(margin: new Thickness(0, 0, 6 * i * size + 3 * size + start.Left, -4 * size + start.Up), size: size);
            }
            for (int i = -1; i <= 1; i++) // rady delky 4
            {
                CreateBoardButton(margin: new Thickness(0, 0, 6 * i * size + start.Left, 8 * size + start.Up), size: size);
                CreateBoardButton(margin: new Thickness(0, 0, 6 * i * size + start.Left, -8 * size + start.Up), size: size);
            }
        }
        private void CreateHexagons(int size, Start start)
        {
            CreateHexagon(Brushes.Yellow, 1, new Thickness(0, 0, start.Left, start.Up), size: size);
            for (int i = 1; i <= 2; i++) // rada delky 5
            {
                CreateHexagon(Brushes.Blue, 1, new Thickness(0, 0, 6*i*size + start.Left, start.Up), size: size);
                CreateHexagon(Brushes.Blue, 1, new Thickness(0, 0, -6 * i * size + start.Left, start.Up), size: size);
            }

            for (int i = -2; i <= 1; i++) // rady delky 4
            {
                CreateHexagon(Brushes.Blue, 1, new Thickness(0, 0, 6 * i * size + 3 * size + start.Left, 4 * size + start.Up), size: size);
                CreateHexagon(Brushes.Blue, 1, new Thickness(0, 0, 6 * i * size + 3 * size + start.Left, -4 * size + start.Up), size: size);
            }
            for (int i = -1; i <= 1; i++) // rady delky 3
            {
                CreateHexagon(Brushes.Blue, 1, new Thickness(0, 0, 6 * i * size + start.Left, 8 * size + start.Up), size: size);
                CreateHexagon(Brushes.Blue, 1, new Thickness(0, 0, 6 * i * size + start.Left, -8 * size + start.Up), size: size);
            }
        }
        private void CreateHexagon(SolidColorBrush color, int number, Thickness margin, int size)
        {
            Polygon polygon = new Polygon();
            polygon.Points = new PointCollection() { new Point(size * 1.5, 0), new Point(size*3, size), new Point(size*3, size*2), new Point(size * 1.5, size * 3), new Point(0, size *2), new Point(0, size)};
            polygon.Stroke = Brushes.Black;
            polygon.StrokeThickness = 3;
            polygon.Fill = color;
            polygon.HorizontalAlignment = HorizontalAlignment.Center;
            polygon.VerticalAlignment = VerticalAlignment.Center;
            polygon.Margin = margin;
            boardGrid.Children.Add(polygon);
        }

        private void CreateBoardButton(int size, Thickness margin)
        {
            Button boardButton = new Button();
            boardButton.HorizontalAlignment = HorizontalAlignment.Center;
            boardButton.VerticalAlignment = VerticalAlignment.Center;
            boardButton.Width = size * 3 + 10;
            boardButton.Height = size * 3 + 10;
            boardButton.Margin = margin;
            boardButton.Background = Brushes.Transparent;
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

        private void CreateCardButtons(int size) // TODO vybrat barvu a přidat Label
        {
            StackPanel buttonStackPanel = new StackPanel();
            buttonStackPanel.Orientation = Orientation.Horizontal;
            buttonStackPanel.VerticalAlignment = VerticalAlignment.Bottom;
            buttonStackPanel.HorizontalAlignment = HorizontalAlignment.Right;
            string[] names = { "Brick", "Wood", "Lamb", "Stone", "Wheat" };
            for (int i = 0; i < names.Length; i++)
            {
                System.Windows.Controls.Button newBtn = new Button();
                newBtn.Content = "Sell " + names[i];
                newBtn.Name = "Button" + names[i];
                newBtn.Width = size;
                newBtn.Height = size;
                newBtn.Background = Brushes.Orange;
                newBtn.FontSize = size * 0.18;
                newBtn.Foreground = Brushes.White;
                newBtn.Margin = new Thickness(0, 0, size / 4, 0); // creates spacing
                newBtn.AddHandler(Button.ClickEvent, new RoutedEventHandler(CardButton_Click));
                buttonStackPanel.Children.Add(newBtn);
            }
            outerGrid.Children.Add(buttonStackPanel);
            
        }

        // creates button to switch to next player in right order
        private void CreateSwitchButton(int size)
        {
            Button switchButton = new Button();
            switchButton.HorizontalAlignment = HorizontalAlignment.Left;
            switchButton.VerticalAlignment = VerticalAlignment.Top;
            switchButton.Width = size * 2;
            switchButton.Height = size;
            switchButton.Background = Brushes.Chocolate;
            switchButton.Content = "Next player";
            switchButton.FontSize = size * 0.35;
            switchButton.Foreground = Brushes.White;
            switchButton.AddHandler(Button.ClickEvent, new RoutedEventHandler(SwitchButton_Click));
            outerGrid.Children.Add(switchButton);
        }
        private void CreateBuildButtons(int size) //TODO
        {
            StackPanel buttonStackPanel = new StackPanel();
            buttonStackPanel.Orientation = Orientation.Horizontal;
            buttonStackPanel.VerticalAlignment = VerticalAlignment.Bottom;
            string[] names = { "Road", "Village", "Town", "Special" };
            for (int i = 0; i < names.Length; i++)
            {
                System.Windows.Controls.Button newBtn = new Button();
                newBtn.Content = "Buy " + names[i];
                newBtn.Name = "Button" + names[i];
                newBtn.Width = size;
                newBtn.Height = size;
                newBtn.Background = Brushes.Orange;
                newBtn.FontSize = size * 0.18;
                newBtn.Foreground = Brushes.White;
                newBtn.Margin = new Thickness(0, 0, size/4, 0); // creates spacing
                newBtn.AddHandler(Button.ClickEvent, new RoutedEventHandler(BuildButton_Click));
                buttonStackPanel.Children.Add(newBtn);
            }
            outerGrid.Children.Add(buttonStackPanel);
        }

        private void CreateTextBox(int width, int height)
        {
            TextBox textBox = new TextBox();

            textBox.Height = height;
            textBox.Width = width;
            textBox.Text = "Text Box content"; //TODO
            textBox.FontSize = height / 2;
            textBox.TextAlignment = TextAlignment.Center;
            textBox.Background = new SolidColorBrush(Colors.White);
            textBox.Foreground = new SolidColorBrush(Colors.Black);
            textBox.HorizontalAlignment = HorizontalAlignment.Center;
            textBox.VerticalAlignment = VerticalAlignment.Top;
            outerGrid.Children.Add(textBox);
        }

        private Label CreateLabel(int size, int margin)
        {
            Label label = new Label();
            label.Background = Brushes.Transparent;
            label.Foreground = Brushes.Red;
            label.Width = size;
            label.Height = margin * 2;
            label.Margin = new Thickness(0, margin, 0, 0);
            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.VerticalAlignment = VerticalAlignment.Top;
            label.FontSize = margin / 3;
            label.Content = "Current Player: 1\nPoints: 0"; //TODO co dát default
            outerGrid.Children.Add(label);
            return label;
        }

        void BoardButton_Click(object sender, RoutedEventArgs e) // TODO
        {
            throw new NotImplementedException();
        }
        void BuildButton_Click(object sender, RoutedEventArgs e) // TODO
        {
            throw new NotImplementedException();
        }

        void CardButton_Click(object sender, RoutedEventArgs e) // TODO
        {
            throw new NotImplementedException();
        }
        void SwitchButton_Click(object sender, RoutedEventArgs e) // TODO
        {
            Player player = gameLogic.SwitchPlayers();
            label.Content = $"Player: {player.Color}\nPoints: {player.Points}";
            

        }
        void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public MainWindow() // TODO upravit velikosti objektů v zavislosti na poli
        {

            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;
            InitializeComponent();
            CreateExitButton(size: 60);
            CreateBoard(size: 30, start:new Start(0, 0));
            CreateBuildButtons(size: 100);
            CreateCardButtons(size: 100);
            CreateSwitchButton(size: 60);
            CreateTextBox(width:300, height:60);
            this.label = CreateLabel(size: 300, margin: 60);
            Game game = new Game();
        }
    }
}
    