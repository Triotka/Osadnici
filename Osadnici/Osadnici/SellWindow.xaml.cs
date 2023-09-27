using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
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
    /// Interaction logic for SellWindow.xaml
    /// </summary>
    public partial class SellWindow : Window
    {
        StackPanel cardsPanel;
        Game gameLogic;
        double windowWidth;
        double windowHeight;
        Player pickedPlayer;
        SameCardsSet offeredCardSet;
     


        void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow(gameLogic, "Left selling");
            mainWindow.Show();
            this.Close();
        }

        void PlayerButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveStackPanel(this.cardsPanel, outerGrid);
            Button clickedButton = (Button)sender;
            int clickedBtnIndex  = GenericWindow.FindObjectIndex(clickedButton);
            if (clickedBtnIndex == gameLogic.Players.Count) // bank is picked
            {
                this.pickedPlayer = gameLogic.SetBankCards();
                
                this.cardsPanel = CreateCardButtons(pickedPlayer, (int)this.windowHeight / 5, outerGrid);
            }
            else if (clickedBtnIndex < gameLogic.Players.Count)
            {
                this.pickedPlayer = gameLogic.Players[clickedBtnIndex];
                this.cardsPanel = CreateCardButtons(pickedPlayer, (int) this.windowHeight / 5, outerGrid);
            }
            else
            {
                throw new Exception(); // invalid index
            }
        }
        void CardButton_Click(object sender, RoutedEventArgs e)
        {
            Button pickedButton = (Button)sender;
            int pickedBtnIndex = GenericWindow.FindObjectIndex(pickedButton);


            gameLogic.ExchangeCards(offeredCardSet, pickedBtnIndex, pickedPlayer);
            var mainWindow = new MainWindow(gameLogic, "Selling successful");
            mainWindow.Show();
            this.Close();
        }


        private void RemoveStackPanel(StackPanel panel, Grid outerGrid)
        {
            outerGrid.Children.Remove(panel);
        }
        private Button CreatePlayerButton(int index, int size, string playerName)
        {
            Button newBtn = new Button();
            newBtn.Content = playerName;
            newBtn.Name = $"playerBtn{index}";
            newBtn.Width = size * 1.25;
            newBtn.Height = size;
            newBtn.Background = ColorMaker.CreateButtonPaint();
            newBtn.FontSize = size * 0.18;
            newBtn.Foreground = Brushes.White;
            newBtn.Margin = new Thickness(size / 2, 0, size / 2, 0); // creates spacing
            newBtn.AddHandler(Button.ClickEvent, new RoutedEventHandler(PlayerButton_Click));
            return newBtn;

        }


        private Button CreateCardButton(int index, int size, string label)
        {
            Button newBtn = new Button();
            newBtn.Content = label;
            newBtn.Name = $"cardBtn{index}";
            newBtn.Width = size;
            newBtn.Height = size;
            newBtn.Background = ColorMaker.CreateButtonPaint();
            newBtn.FontSize = size * 0.18;
            newBtn.Foreground = Brushes.White;
            newBtn.Margin = new Thickness(size / 2, 0, size / 2, 0); // creates spacing
            newBtn.AddHandler(Button.ClickEvent, new RoutedEventHandler(CardButton_Click));
            return newBtn;

        }
        private StackPanel CreatePlayerButtons(int size, List<Player> players, int bankConstant, Player currentPlayer, Grid outerGrid)
        {

            StackPanel buttonStackPanel = new StackPanel();
            buttonStackPanel.Orientation = Orientation.Horizontal;
            buttonStackPanel.VerticalAlignment = VerticalAlignment.Center;
            buttonStackPanel.HorizontalAlignment = HorizontalAlignment.Center;

            for (int i = 0; i < players.Count; i++)
            {
                if (currentPlayer != players[i])
                {
                    string playerName = "Player " + players[i].Color.ToString();
                    buttonStackPanel.Children.Add(CreatePlayerButton(i, size, playerName));
                }
                
            }
            if (offeredCardSet.CardsCount >= bankConstant) // creates button to sell to bank
            {
                string playerName = "Bank";
                buttonStackPanel.Children.Add(CreatePlayerButton(players.Count, size, playerName));
            }
            outerGrid.Children.Add(buttonStackPanel);
            return buttonStackPanel;
        }
        public StackPanel CreateCardButtons(Player player, int size, Grid outerGrid)
        {
            StackPanel buttonStackPanel = new StackPanel();
            buttonStackPanel.Orientation = Orientation.Horizontal;
            buttonStackPanel.VerticalAlignment = VerticalAlignment.Bottom;
            buttonStackPanel.HorizontalAlignment = HorizontalAlignment.Center;

            List<SameCardsSet> offeredCards = player.GetOfferedCards().ToList();
            for(int i = 0; i < offeredCards.Count; i++)
            {
                string label = $"{offeredCards[i].Material.ToString()}\n{offeredCards[i].CardsCount}";
                buttonStackPanel.Children.Add(CreateCardButton(i, size, label));
            }

            outerGrid.Children.Add(buttonStackPanel);
            return buttonStackPanel;
        }
        public SellWindow(Game game, SameCardsSet offeredCardSet)
        {
            this.gameLogic = game;
            this.offeredCardSet = offeredCardSet;
            GenericWindow.SetWindowStyle(window: this);
            InitializeComponent();
            this.windowHeight = SystemParameters.PrimaryScreenHeight;
            this.windowWidth = SystemParameters.PrimaryScreenWidth;

            GenericWindow.CreateAnnoucmentLabel((int)windowHeight, (int)windowHeight / 12, "Exchange cards", outerGrid);
            GenericWindow.CreateExitButton(handler: new RoutedEventHandler(ExitButton_Click), size: (int)windowHeight / 12, outerGrid: outerGrid);
            StackPanel playerButtons = CreatePlayerButtons((int)windowHeight / 5, game.Players, game.SellConstant, game.GetCurrentPlayer(), outerGrid);
        }
    }
}
