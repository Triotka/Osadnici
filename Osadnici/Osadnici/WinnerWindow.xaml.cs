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
    /// Interaction logic for WinnerWindow.xaml
    /// </summary>
    public partial class WinnerWindow : Window
    {
        // creates button to restart game
        private void CreateRestartButton(int size)
        {
            Button newBtn = new Button();
            newBtn.Content = "Restart game";
            newBtn.Width = size * 2;
            newBtn.Height = size;
            newBtn.Background = ColorMaker.CreateButtonPaint();
            newBtn.FontSize = size * 0.18;
            newBtn.Foreground = Brushes.White;
            newBtn.HorizontalAlignment = HorizontalAlignment.Center;
            newBtn.VerticalAlignment = VerticalAlignment.Center;
            newBtn.AddHandler(Button.ClickEvent, new RoutedEventHandler(RestartButton_Click));
            outerGrid.Children.Add(newBtn);
        }
        // creates button when clicked close window
        void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // restarts the game
        void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            WindowPickPlayers startWindow = new WindowPickPlayers();
            startWindow.Show();
            this.Close();
        }
        public WinnerWindow(Player winner)
        {
            GenericWindow.SetWindowStyle(window: this);
            InitializeComponent();
            var height = SystemParameters.PrimaryScreenHeight;
            var width = SystemParameters.PrimaryScreenWidth;
            GenericWindow.CreateExitButton(handler: new RoutedEventHandler(ExitButton_Click), size: (int)height / 12, outerGrid: outerGrid);
            GenericWindow.CreateAnnoucmentLabel(width: (int)height, height: (int)height/12, outerGrid: outerGrid,
                                   initMessage: $"PLAYER {winner.Color} HAS WON");
            CreateRestartButton((int)height / 12);

        }
    }
}
