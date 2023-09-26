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


    class Start
    {
        public int Left { get; set; }
        public int Up { get; set; }

        public Start(int left, int up)
        {
            this.Left = left;
            this.Up = up;
        }
    }

    class ActionField
    {
        // creates action buttons switch player button and roll dice button
        public static void CreateActionButtons(int size, Grid outerGrid, RoutedEventHandler switchButtonHandler, RoutedEventHandler diceButtonHandler)
        {
            StackPanel actionStackPanel = new StackPanel();
            var switchButton = GenericWindow.CreateActionButton(size: size, content: "Next player", stackPanel: actionStackPanel, handler: switchButtonHandler);
            var diceButton = GenericWindow.CreateActionButton(size: size, content: "Roll dice", stackPanel: actionStackPanel, handler: new RoutedEventHandler(diceButtonHandler));
            outerGrid.Children.Add(actionStackPanel);
        }

       
       
    }

    enum CardType
    {
        Material,
        Build
    }
    class CardsSet
    {
        Game gameLogic;
        Grid outerGrid;
        public StackPanel CardPanel;

        public CardsSet(Game game, Grid grid, CardType type, int size, RoutedEventHandler handler)
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
                        foundButton.Content = $"\n{names[buttonsFound]}\n{orderedPawnsList[buttonsFound].PawnsCount}";
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

    class Pawns
    {
        Game gameLogic;
        int sizeOfHexagon;
        List<Hexagon> hexagons;
        Grid outerGrid;
        List<Polygon> drawnHexagons;

        public Pawns(Game game, int sizeOfHexagon, Grid grid, List<Polygon> drawnHexagons)
        {
            gameLogic = game;
            this.sizeOfHexagon = sizeOfHexagon;
            this.hexagons = gameLogic.Board.Hexagons;
            outerGrid = grid;
            this.drawnHexagons = drawnHexagons;
        }
        public void CreatePawns(int pawnSize)
        {
            CreatePirate(pawnSize: pawnSize, drawnHexagons[gameLogic.Pirate.Location]);
            for (int i = 0; i < hexagons.Count; i++)
            {
                CreateBuildings(pawnSize: pawnSize, hexagonIndex: i, drawnHexagons[i]);
                CreateRoads(pawnSize: pawnSize, hexagonIndex: i, drawnHexagons[i]);
            }

        }
        private void CreatePirate(int pawnSize, Polygon drawnHexagon)
        {
            
            var point = GetPiratePoint(drawnHexagon);
            var margin = new Thickness(point.X + drawnHexagon.Margin.Left, point.Y + drawnHexagon.Margin.Top, drawnHexagon.Margin.Right + 2 * sizeOfHexagon, drawnHexagon.Margin.Bottom + 2 * sizeOfHexagon);
            GenericWindow.CreateSquare(Brushes.Black, margin, pawnSize, this.outerGrid);

        }

        // get positin on drawn hexagon to place pirate, in the middle
        private Point GetPiratePoint(Polygon drawnHexagon)
        {
            Point piratePoint = new Point();
            var hexagonPoints = GenericWindow.GetHexagonPoints(sizeOfHexagon, drawnHexagon);
            piratePoint.X = hexagonPoints[5].X  + Math.Abs(hexagonPoints[5].X - hexagonPoints[1].X) / 2;
            piratePoint.Y = hexagonPoints[0].Y + Math.Abs(hexagonPoints[3].Y- hexagonPoints[0].Y) /2;

            return piratePoint;
        }
        // creates buildings for one hexagon
        private void CreateBuildings(int pawnSize, int hexagonIndex, Polygon drawnHexagon)
        {
            var currentHexagon = hexagons[hexagonIndex];
            var hexagonPoints = GenericWindow.GetHexagonPoints(sizeOfHexagon, drawnHexagon);
            

            for (int i = 0; i < currentHexagon.Buildings.Count(); i++)
            {
                var point = hexagonPoints[i];
                var building = currentHexagon.Buildings[i];
                CreateBuilding(pawnSize: pawnSize, building: building, point: point, drawnHexagon: drawnHexagon);
            }
        }

        
        private void CreateBuilding(int pawnSize, Building building, Polygon drawnHexagon, Point point)
        {
            var margin = new Thickness(point.X + drawnHexagon.Margin.Left, point.Y + drawnHexagon.Margin.Top, drawnHexagon.Margin.Right + 2 * sizeOfHexagon, drawnHexagon.Margin.Bottom + 2 * sizeOfHexagon);

            if (building.Type == PawnType.Village)
            {     
                GenericWindow.CreateSquare(MatchBuildingColor(building.Color), margin, pawnSize, this.outerGrid); 
            }
            if (building.Type == PawnType.Town)
            {
                GenericWindow.CreateTriangle(MatchBuildingColor(building.Color), margin, pawnSize, this.outerGrid);
            }
        }

        // get list of points where roads can be placed on one hexagon
        private List<Point> GetRoadPoints(Polygon drawnHexagon)
        {
            var roadPoints = new List<Point>();
            var hexagonPoints =  GenericWindow.GetHexagonPoints(sizeOfHexagon, drawnHexagon);
            for(int i = 0; i < hexagonPoints.Count; i++)
            {
                var firstPoint = hexagonPoints[i];
                var secondPoint = hexagonPoints[(i + 1) % hexagonPoints.Count];
                var roadPoint = new Point();
                var differenceX = Math.Abs(firstPoint.X - secondPoint.X) / 2;
                var differenceY = Math.Abs(firstPoint.Y - secondPoint.Y) / 2;

                if (firstPoint.X < secondPoint.X)
                {
                    roadPoint.X = firstPoint.X + differenceX;
                }
                else
                {
                    roadPoint.X = secondPoint.X + differenceX;
                }

                if (firstPoint.Y < secondPoint.Y)
                {
                    roadPoint.Y = firstPoint.Y + differenceY;
                }
                else
                {
                    roadPoint.Y = secondPoint.Y + differenceY;
                }
                roadPoints.Add(roadPoint);
            }
            return roadPoints;
            
        }
        // creates buildings for one hexagon
        private void CreateRoads(int pawnSize, int hexagonIndex, Polygon drawnHexagon)
        {
            var currentHexagon = hexagons[hexagonIndex];
            var hexagonPoints = GetRoadPoints(drawnHexagon);


            for (int i = 0; i < currentHexagon.Roads.Count(); i++)
            {
                var positionOnMap = MapRoadPoints(6 + i);
                var point = hexagonPoints[positionOnMap];
                var road = currentHexagon.Roads[i];
                CreateRoad(pawnSize: pawnSize, road: road, point: point, drawnHexagon: drawnHexagon);
            }
        }
        private int MapRoadPoints(int roadIndex)
        {
            
            switch(roadIndex)
            {
                case 6:
                    return 4;
                case 7:
                    return 5;
                case 8:
                    return 3;
                case 9:
                    return 1;
                case 10:
                    return 0;
                case 11:
                    return 2;
            }
            throw new Exception();
        }

        private void CreateRoad(int pawnSize, Road road, Polygon drawnHexagon, Point point)
        {
            var margin = new Thickness(point.X + drawnHexagon.Margin.Left, point.Y + drawnHexagon.Margin.Top, drawnHexagon.Margin.Right + 2 * sizeOfHexagon, drawnHexagon.Margin.Bottom + 2 * sizeOfHexagon);
            if (road.Color != Color.None)
            GenericWindow.CreateCircle(MatchBuildingColor(road.Color), margin, pawnSize, this.outerGrid);
        }

        private SolidColorBrush MatchBuildingColor(Color color)
        {
            if (color == Color.Blue)
                return Brushes.Blue;
            if (color == Color.Yellow)
                return Brushes.Yellow;
            if (color == Color.White)
                return Brushes.White;
            if (color == Color.Red)
                return Brushes.Red;

            return Brushes.Transparent;

        }

    }
    class DrawnBoard
    {
        Game gameLogic;
        Grid boardGrid;
        List<Polygon> hexagons;
        RoutedEventHandler handler;

        public DrawnBoard(Game game, Grid boardGrid, RoutedEventHandler handler)
        {
            this.gameLogic = game;
            this.boardGrid = boardGrid;
            this.handler = handler;
            this.hexagons = new List<Polygon>();
        }

        // creates board
        public List<Polygon> CreateBoard(int size, Start start)
        {
            List<Polygon> hexagons =  CreateHexagons(size: size, start: start);
            CreateBoardButtons(size: size, start: start);
            return hexagons;
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
                CreateBoardButton(margin: new Thickness(0, 0, 6 * i * size + 3 * size + start.Left, 4 * size + start.Up), size: size, numInRow: hexagonIndex);
                hexagonIndex--;
            }

            for (int i = -1; i <= 1; i++) // rows of length 3 top
            {
                CreateBoardButton(margin: new Thickness(0, 0, 6 * i * size + start.Left, 8 * size + start.Up),
                                  size: size, numInRow: hexagonIndex);
                hexagonIndex--;
            }

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
            boardButton.Name = $"boardButton{numInRow}";
            boardButton.Foreground = Brushes.Black;
            boardButton.FontSize = size;

            if (number != gameLogic.PirateNumber)
            {
                boardButton.Content = $"{number}";
            }
            boardButton.AddHandler(Button.ClickEvent, handler);
            boardGrid.Children.Add(boardButton);
        }

        private List<Polygon> CreateHexagons(int size, Start start)
        {
            int hexagonIndex = 18;
            for (int i = -1; i <= 1; i++) // row of length 3 bottom
            {
                var hexagon = GenericWindow.CreateHexagon(MatchIndexToColor(hexagonIndex), new Thickness(0, 0, 6 * i * size + start.Left, -8 * size + start.Up),
                                            size: size, boardGrid);
                hexagonIndex--;
                this.hexagons.Add(hexagon);
            }

            for (int i = -2; i <= 1; i++) // rows of length 4 bottom
            {
                var hexagon = GenericWindow.CreateHexagon(MatchIndexToColor(hexagonIndex), new Thickness(0, 0, 6 * i * size + 3 * size + start.Left, -4 * size + start.Up),
                                            size: size, boardGrid);
                hexagonIndex--;
                this.hexagons.Add(hexagon);

            }
            for (int i = -2; i <= 2; i++) // rows of length 5
            {
                var hexagon = GenericWindow.CreateHexagon(MatchIndexToColor(hexagonIndex), new Thickness(0, 0, 6 * i * size + start.Left, start.Up),
                                            size: size, boardGrid);
                hexagonIndex--;
                this.hexagons.Add(hexagon);
            }

            for (int i = -2; i <= 1; i++) // rows of length 4 top
            {
                var hexagon = GenericWindow.CreateHexagon(MatchIndexToColor(hexagonIndex), new Thickness(0, 0, 6 * i * size + 3 * size + start.Left, 4 * size + start.Up),
                                            size: size, boardGrid);
                hexagonIndex--;
                this.hexagons.Add(hexagon);
            }
            for (int i = -1; i <= 1; i++) // row of length 3 top
            {
                var hexagon = GenericWindow.CreateHexagon(MatchIndexToColor(hexagonIndex), new Thickness(0, 0, 6 * i * size + start.Left, 8 * size + start.Up),
                                            size: size, boardGrid);
                hexagonIndex--;
                this.hexagons.Add(hexagon);
            }
            hexagons.Reverse();
            return hexagons;
        }

        private int MatchIndexToNumber(int index)
        {
            return gameLogic.Board.Hexagons[index].Number;
        }
        private LinearGradientBrush MatchIndexToColor(int index)
        {
            var material = gameLogic.Board.Hexagons[index].Material;

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
    }
    public partial class MainWindow : Window
    {
        List<Polygon> hexagons;
        Game gameLogic;
        Label playerLabel;
        Label annoucmentLabel;
        CardsSet materialCards;
        CardsSet buildCards;

        // maps activity to displayed string
        private string DisplayActivity(Activity activity)
        {
            switch (activity)
            {
                case Activity.StartFirstVillage:
                    return "Building first village";
                case Activity.StartSecondVillage:
                    return "Building second village";
                case Activity.StartFirstRoad:
                    return "Building first road";
                case Activity.StartSecondRoad:
                    return "Building second road";
                case Activity.Rolling:
                    return "Must roll a dice";
                case Activity.BuildingVillage:
                    return "Building a village";
                case Activity.BuildingRoad:
                    return "Building a road";
                case Activity.BuildingTown:
                    return "Building a town";
                case Activity.MovingPirate:
                    return "Must move a pirate";
                case Activity.NoPossibilities:
                    return "No more action";
                case Activity.None:
                    return "Waiting for action";
            }
            throw new Exception(); //unknown action
        }

        // updates player's label info
        private void UpdatePlayerLabel()
        {
            var player = gameLogic.GetCurrentPlayer();
            this.playerLabel.Content = $"Player: {player.Color}\nPoints: {player.Points}\nActivity: {DisplayActivity(player.Activity)}";
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
            var hexagonWindow = new WindowHexagon(game: this.gameLogic, clickedHexagon:clickedHexagon,
                                                clickedButton: clickedButton, clickedIndex: clickedIndex);

            hexagonWindow.Show();
            this.Close();
        }
        void DiceButton_Click(object sender, RoutedEventArgs e)
        {
            var message = gameLogic.HandleDiceRequest();
            UpdatePlayerLabel();
            annoucmentLabel.Content = message;
            materialCards.UpdateMaterialCards();
            

        }
        void BuildButton_Click(object sender, RoutedEventArgs e)
        {
            string unsuccessfulMessage = "Unable to buy";
            string successfulMessage = "Bought";
            if (gameLogic.GetCurrentPlayer().Activity != Activity.None)
            {
                annoucmentLabel.Content = unsuccessfulMessage;
                return;
            }
            Button clickedButton = (Button)sender;
          
            string[] names = Enum.GetNames(typeof(PawnType));
            
            
            for (int i = 0; i < names.Length; i++)
            {
                if (clickedButton.Name.EndsWith(names[i]))
                {
                    
                    bool successfulBuy = false;
                    if (names[i] == PawnType.Village.ToString())
                    {
                        successfulBuy = gameLogic.GetCurrentPlayer().Buy(gameLogic.RecipeRules[PawnType.Village]);
                        if (!successfulBuy)
                        {
                            annoucmentLabel.Content = unsuccessfulMessage + " " + nameof(PawnType.Village);
                            return;
                        }
                        else
                        {
                            gameLogic.GetCurrentPlayer().Activity = Activity.BuildingVillage;
                            annoucmentLabel.Content = successfulMessage + " " + nameof(PawnType.Village);
                            UpdatePlayerLabel();
                            return;

                        }
                    }
                    if (names[i] == nameof(PawnType.Town))
                    {
                        successfulBuy = gameLogic.GetCurrentPlayer().Buy( gameLogic.RecipeRules[PawnType.Town]);
                        if (!successfulBuy)
                        {
                            annoucmentLabel.Content = unsuccessfulMessage + " " + nameof(PawnType.Town);
                            return;
                        }
                        else
                        {
                            gameLogic.GetCurrentPlayer().Activity = Activity.BuildingTown;
                            annoucmentLabel.Content = successfulMessage + " " + nameof(PawnType.Town);
                            UpdatePlayerLabel();
                            return;
                        }
                    }
                    if (names[i] == nameof(PawnType.Road))
                    {
                        successfulBuy = gameLogic.GetCurrentPlayer().Buy(gameLogic.RecipeRules[PawnType.Road]);

                        if (!successfulBuy)
                        {
                            annoucmentLabel.Content = unsuccessfulMessage + " " + nameof(PawnType.Road);
                            return;
                        }
                        else
                        {
                            gameLogic.GetCurrentPlayer().Activity = Activity.BuildingRoad;
                            annoucmentLabel.Content = successfulMessage + " " + nameof(PawnType.Road);
                            UpdatePlayerLabel();
                            return;
                        }
                            
                    }

                }
            }
           
        }

        void MaterialButton_Click(object sender, RoutedEventArgs e)
        {
            if (gameLogic.GetCurrentPlayer().Activity != Activity.None)
            {
                annoucmentLabel.Content = "Cannot sell";
                return;
            }

            Button clickedButton = (Button)sender;
            foreach (var material in Enum.GetValues(typeof(Material)).Cast<Material>())
            {
                if (clickedButton.Name.EndsWith(material.ToString()))
                {
                    Trace.WriteLine(material);
                    bool succesfulSell = gameLogic.GetCurrentPlayer().Sell(material, gameLogic);
                    if (succesfulSell)
                    {
                        var winner = gameLogic.CheckWinner();
                        if (winner != null)
                        {
                            var winnerWindow = new WinnerWindow(winner);
                            winnerWindow.Show();
                            this.Close();
                        }
                        else
                        {
                            var mainWindow = new MainWindow(game: this.gameLogic, "Succesful sell");
                            mainWindow.Show(); // TODO stejne se smaze
                            this.Close();
                        }
                       
                    }
                    else
                    {
                        annoucmentLabel.Content = "You cannot sell this";
                    }
                    break;
                }

               
            }




        }
        void SwitchButton_Click(object sender, RoutedEventArgs e) 
        {

            var currentPlayer = gameLogic.GetCurrentPlayer();
            if (currentPlayer.Activity == Activity.None || currentPlayer.Activity == Activity.NoPossibilities)
            {
                Player player = gameLogic.SwitchPlayers();
                UpdatePlayerLabel();
                materialCards.UpdateMaterialCards();
                buildCards.UpdateBuildCards();
                this.annoucmentLabel.Content = "Switched players";
            }
            else
            {
                annoucmentLabel.Content = "You cannot switch players";
            }

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
            this.hexagons = new DrawnBoard(game: game, boardGrid: boardGrid, handler: new RoutedEventHandler(BoardButton_Click)).CreateBoard(size: (int)height / 18, start: new Start(0, (int)(height / 7.2)));
            this.buildCards = new CardsSet(game: game, grid: outerGrid, size: (int)height / 8, handler: new RoutedEventHandler(BuildButton_Click), type: CardType.Build);
            this.materialCards = new CardsSet(game: game, grid: outerGrid, size: (int)height / 8, handler: new RoutedEventHandler(MaterialButton_Click), type: CardType.Material);
            ActionField.CreateActionButtons(size: (int)height/12, outerGrid: outerGrid,
                                            switchButtonHandler: new RoutedEventHandler(SwitchButton_Click), diceButtonHandler: DiceButton_Click);
            this.annoucmentLabel = GenericWindow.CreateAnnoucmentLabel(width: (int)height, height: (int)height / 12, outerGrid: outerGrid,
                                   initMessage: initMessage);
            CreatePlayerLabel(size: (int)height / 2, margin: (int)height / 12);
            CreatePricesLabel(size: (int)height / 2, margin: (int)height / 12);
            new Pawns(game, (int)height / 18, outerGrid, hexagons).CreatePawns((int)height / 36);
           
        }
    }
}
    