using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace Osadnici
{
    class DrawnCardsSet
    {
        Game gameLogic;
        Grid outerGrid;
        public StackPanel CardPanel;

        public DrawnCardsSet(Game game, Grid grid, CardType type, int size, RoutedEventHandler handler)
        {
            this.gameLogic = game;
            this.outerGrid = grid;

            switch (type)
            {
                case CardType.Material:
                    this.CardPanel = CreateMaterialCards(size: size, handler: handler);
                    break;
                case CardType.Build:
                    this.CardPanel = CreateBuildCards(size: size, handler: handler);
                    break;
                default:
                    throw new Exception(); //invalid card type
            }

        }
        // updates build cards
        public void UpdateBuildCards()
        {
            StackPanel buttonStackPanel = null;
            foreach (var component in this.CardPanel.Children)
            {
                buttonStackPanel = component as StackPanel;
            }
            if (buttonStackPanel == null)
                throw new Exception(); // no buttons present

            var orderedPawnsList = gameLogic.GetCurrentPlayer().GetOrderedPawns();
            string[] names = Enum.GetNames(typeof(PawnType));
            int buttonsFound = 0;
            foreach (var button in buttonStackPanel.Children)
            {
                if (button is Button)
                {
                    Button foundButton = button as Button;
                    if (foundButton != null)
                    {
                        foundButton.Content = $"Buy\n{names[buttonsFound]}\n{orderedPawnsList[buttonsFound].PawnsCount}";
                        buttonsFound++;
                    }

                }
            }
        }


        // updates material cards labels
        public void UpdateMaterialCards()
        {
            StackPanel buttonStackPanel = null;
            foreach (var component in this.CardPanel.Children)
            {
                buttonStackPanel = component as StackPanel;
            }
            if (buttonStackPanel == null)
                throw new Exception(); // no buttons present

            var orderedCardsList = gameLogic.GetCurrentPlayer().GetOrderedCards();
            string[] names = Enum.GetNames(typeof(Material));
            int buttonsFound = 0;
            foreach (var button in buttonStackPanel.Children)
            {
                if (button is Button)
                {
                    Button foundButton = button as Button;
                    if (foundButton != null)
                    {
                        foundButton.Content = $"Sell\n{names[buttonsFound]}\n{orderedCardsList[buttonsFound].CardsCount}";
                        buttonsFound++;
                    }

                }
            }
        }
        // creates cards with material
        private StackPanel CreateMaterialCards(int size, RoutedEventHandler handler)
        {
            StackPanel materialStackPanel = new StackPanel();
            materialStackPanel.Background = ColorMaker.CreateCardBackground();
            materialStackPanel.Orientation = Orientation.Vertical;
            materialStackPanel.VerticalAlignment = VerticalAlignment.Bottom;
            materialStackPanel.HorizontalAlignment = HorizontalAlignment.Right;
            var buildLabel = createsCardsLabel(size: size, "Material");
            var buttonStackPanel = CreateMaterialButtons(size, handler);
            materialStackPanel.Children.Add(buildLabel);
            materialStackPanel.Children.Add(buttonStackPanel);
            outerGrid.Children.Add(materialStackPanel);
            return materialStackPanel;
        }

        // creates button in shape of card
        private Button CreateButtonCard(int size)
        {
            System.Windows.Controls.Button newBtn = new Button();
            newBtn.Width = size;
            newBtn.Height = size * 1.5;
            newBtn.Background = ColorMaker.CreateCardPaint();
            newBtn.FontSize = size * 0.3;
            newBtn.Foreground = Brushes.White;
            newBtn.Margin = new Thickness(size / 4, 0, size / 4, 0); // creates spacing
            newBtn.VerticalContentAlignment = VerticalAlignment.Center;
            newBtn.HorizontalContentAlignment = HorizontalAlignment.Center;
            return newBtn;
        }


        // creates buttons in shape of card with material
        private StackPanel CreateMaterialButtons(int size, RoutedEventHandler handler)
        {
            StackPanel buttonStackPanel = new StackPanel();
            buttonStackPanel.Orientation = Orientation.Horizontal;
            buttonStackPanel.VerticalAlignment = VerticalAlignment.Bottom;

            var orderedCardsList = gameLogic.GetCurrentPlayer().GetOrderedCards();
            string[] names = Enum.GetNames(typeof(Material));
            for (int i = 0; i < names.Length - 1; i++)
            {
                var newBtn = CreateButtonCard(size);
                newBtn.Content = $"Sell\n{names[i]}\n{orderedCardsList[i].CardsCount}";
                newBtn.Name = "Button" + names[i];
                newBtn.AddHandler(Button.ClickEvent, new RoutedEventHandler(handler));
                buttonStackPanel.Children.Add(newBtn);
            }
            return buttonStackPanel;
        }

        // creates buttons in shape of card with things I can build/buy
        private StackPanel CreateBuildButtons(int size, RoutedEventHandler handler)
        {
            StackPanel buttonStackPanel = new StackPanel();
            buttonStackPanel.Orientation = Orientation.Horizontal;
            buttonStackPanel.VerticalAlignment = VerticalAlignment.Bottom;
            var orderedPawnsList = gameLogic.GetCurrentPlayer().GetOrderedPawns();
            string[] names = Enum.GetNames(typeof(PawnType));
            for (int i = 0; i < names.Length - 1; i++)
            {
                var newBtn = CreateButtonCard(size);
                newBtn.Content = $"Buy\n{names[i]}\n{orderedPawnsList[i].PawnsCount}";
                newBtn.Name = "Button" + names[i];
                newBtn.AddHandler(Button.ClickEvent, new RoutedEventHandler(handler));
                buttonStackPanel.Children.Add(newBtn);
            }
            return buttonStackPanel;
        }

        // creates cards with things I can build/buy
        public StackPanel CreateBuildCards(int size, RoutedEventHandler handler)
        {
            StackPanel outerStackPanel = new StackPanel();
            outerStackPanel.Background = ColorMaker.CreateCardBackground();
            outerStackPanel.Orientation = Orientation.Vertical;
            outerStackPanel.VerticalAlignment = VerticalAlignment.Bottom;
            outerStackPanel.HorizontalAlignment = HorizontalAlignment.Left;
            var buildLabel = createsCardsLabel(size: size, "Buildings");
            var buttonStackPanel = CreateBuildButtons(size, handler);
            outerStackPanel.Children.Add(buildLabel);
            outerStackPanel.Children.Add(buttonStackPanel);
            outerGrid.Children.Add(outerStackPanel);
            return outerStackPanel;
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

    }

}
