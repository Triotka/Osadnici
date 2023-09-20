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
using System.Diagnostics;

namespace Osadnici
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    /// 


   
    public partial class WindowPickPlayers : Window
    {
        Game game;
        // creates Buttons to pick number of players
        private void CreatePickButtons(int size)
        {
            StackPanel buttonStackPanel = new StackPanel();
            buttonStackPanel.Orientation = Orientation.Horizontal;
            buttonStackPanel.VerticalAlignment = VerticalAlignment.Center;
            buttonStackPanel.HorizontalAlignment = HorizontalAlignment.Center;

            var names = new List<ObjectName>();
            for (int i = game.minimumPlayers; i <= game.maximumPlayers; i++)
            names.Add(new ObjectName(name: $"btnPlayers{i}", label: $"{i} Players"));

            for (int i = 0; i < names.Count; i++)
            {
                System.Windows.Controls.Button newBtn = new Button();
                newBtn.Content = names[i].Label;
                newBtn.Name = names[i].Name;
                newBtn.Width = size;
                newBtn.Height = size;
                newBtn.Background = ColorMaker.CreateButtonPaint();
                newBtn.FontSize = size * 0.18;
                newBtn.Foreground = Brushes.White;
                newBtn.Margin = new Thickness(size / 2, 0, size / 2, 0); // creates spacing
                newBtn.AddHandler(Button.ClickEvent, new RoutedEventHandler(PickButton_Click));
                buttonStackPanel.Children.Add(newBtn);
            }
            outerGrid.Children.Add(buttonStackPanel);
        }
        // creates button when clicked close window
        void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //starts game and sets number of players from button
        void PickButton_Click(object sender, RoutedEventArgs e)
        {
            var btnPressed = (Button)sender;
            var numOfPlayers = Char.GetNumericValue(btnPressed.Name[btnPressed.Name.Length - 1]);
            game.SetPlayers((int)numOfPlayers);
            MainWindow mainWindow = new MainWindow(game, $"Players were set to {game.Players.Count}");
            mainWindow.Show();
            this.Close();
        }

        public WindowPickPlayers()
        {
            game = new Game();
            GenericWindow.SetWindowStyle(window: this);
            InitializeComponent();
            var height = SystemParameters.PrimaryScreenHeight;
            var width = SystemParameters.PrimaryScreenWidth;
          

            GenericWindow.CreateExitButton(handler: new RoutedEventHandler(ExitButton_Click), size: (int)(height / 12), outerGrid: outerGrid);
            CreatePickButtons((int)height/5);
            GenericWindow.CreateAnnoucmentLabel(width: (int) height, height: (int)height/12, outerGrid: outerGrid,
                                   initMessage: "Pick Number of Players");

        }
    }
}
