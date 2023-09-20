using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Osadnici
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 

    public class ObjectName
    {
        public string Name;
        public string Label;

        public ObjectName(string name, string label)
        {
            this.Name = name;
            this.Label = label;
        }
    }
    // colors used in game
    public class ColorMaker
    {
        public static LinearGradientBrush CreateButtonPaint()
        {
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.Gray, 0.0));
            brush.GradientStops.Add(new GradientStop(Colors.Purple, 1.0));
            return brush;
        }

        public static LinearGradientBrush CreateCardPaint()
        {
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.Gray, 0.0));
            brush.GradientStops.Add(new GradientStop(Colors.Purple, 1.0));
            return brush;
        }
        public static LinearGradientBrush CreateCardBackground()
        {
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.Black, 0.0));
            brush.GradientStops.Add(new GradientStop(Colors.DarkBlue, 1.0));
            return brush;
        }

        public static LinearGradientBrush BoardLamb()
        {
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.LightGreen, 0.0));
            brush.GradientStops.Add(new GradientStop(Colors.White, 1.0));
            return brush;
        }
        public static LinearGradientBrush BoardWheat()
        {
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.Brown, 0.0));
            brush.GradientStops.Add(new GradientStop(Colors.Yellow, 1.0));
            return brush;
        }
        public static LinearGradientBrush BoardStone()
        {
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.White, 0.0));
            brush.GradientStops.Add(new GradientStop(Colors.DarkGray, 1.0));
            return brush;
        }
        public static LinearGradientBrush BoardBrick()
        {
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.DarkOrange, 0.0));
            brush.GradientStops.Add(new GradientStop(Colors.DarkRed, 1.0));
            return brush;
        }
        public static LinearGradientBrush BoardWood()
        {
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.LawnGreen, 0.0));
            brush.GradientStops.Add(new GradientStop(Colors.DarkGreen, 1.0));
            return brush;
        }
        public static LinearGradientBrush BoardDesert()
        {
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.DarkBlue, 0.0));
            brush.GradientStops.Add(new GradientStop(Colors.Black, 1.0));
            return brush;
        }
    }
    class GenericWindow : Window
    {
        
        // creates exit button 
        static public void CreateExitButton(RoutedEventHandler handler, int size, Grid outerGrid)
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
            exitButton.AddHandler(Button.ClickEvent, new RoutedEventHandler(handler));
            outerGrid.Children.Add(exitButton);
        }


        public static List<List<Point>> GetListOfPoints(PointCollection points)
        {
           return points.Select((p, i) => new { p, i }).GroupBy(x => x.i / 2).Where(g => g.Skip(1).Any()).Select(g => g.Select(x => x.p).ToList()).ToList();
        }
        //creates hexagon
        public static Polygon CreateHexagon(Brush color, Thickness margin, int size, Grid outerGrid)
        {
            
            Polygon polygon = new Polygon();
            polygon.Points = new PointCollection() { new Point(size * 1.5, 0), new Point(size * 3, size), new Point(size * 3, size * 2),
                            new Point(size * 1.5, size * 3), new Point(0, size * 2), new Point(0, size) };
            polygon.Stroke = Brushes.Black;
            polygon.StrokeThickness = 3;
            polygon.Fill = color;
            polygon.HorizontalAlignment = HorizontalAlignment.Center;
            polygon.VerticalAlignment = VerticalAlignment.Center;
            polygon.Margin = margin;
            outerGrid.Children.Add(polygon);
            return polygon;
        }

        public static Polygon CreateTriangle(Brush color, Thickness margin, double size, Grid outerGrid)
        {
            size = (double )size * 0.8;
            Polygon polygon = new Polygon();
            polygon.Points = new PointCollection() { new Point(-size * 0.5, size/2), new Point(0.5 * size, -size), new Point(size * 1.5, size/2)}; 
            polygon.Stroke = Brushes.Black;
            polygon.StrokeThickness = 3;
            polygon.Fill = color;
            polygon.HorizontalAlignment = HorizontalAlignment.Center;
            polygon.VerticalAlignment = VerticalAlignment.Center;
            polygon.Margin = margin;
            outerGrid.Children.Add(polygon);
            return polygon;
        }

        public static Polygon CreateSquare(Brush color, Thickness margin, int size, Grid outerGrid)
        {

            Polygon polygon = new Polygon();
            polygon.Points = new PointCollection() { new Point(0, 0), new Point(size, 0), new Point(size, size), new Point(0, size)};
            polygon.Stroke = Brushes.Black;
            polygon.StrokeThickness = 3;
            polygon.Fill = color;
            polygon.HorizontalAlignment = HorizontalAlignment.Center;
            polygon.VerticalAlignment = VerticalAlignment.Center;
            polygon.Margin = margin;
            outerGrid.Children.Add(polygon);
            return polygon;
        }

       // creates action button to place into a stack panel
        public static Button CreateActionButton(int size, string content, StackPanel stackPanel, RoutedEventHandler handler)
        {
            Button actionButton = new Button();
            actionButton.HorizontalAlignment = HorizontalAlignment.Left;
            actionButton.VerticalAlignment = VerticalAlignment.Top;
            actionButton.Width = size * 2.5;
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
        public static Ellipse CreateCircle(Brush color, Thickness margin, int size, Grid outerGrid)
        {

            Ellipse polygon = new Ellipse();
            polygon.Width = size;
            polygon.Height = size;
            polygon.Stroke = Brushes.Black;
            polygon.StrokeThickness = 3;
            polygon.Fill = color;
            polygon.HorizontalAlignment = HorizontalAlignment.Center;
            polygon.VerticalAlignment = VerticalAlignment.Center;
            polygon.Margin = margin;
            outerGrid.Children.Add(polygon);
            return polygon;
        }


        // creates label for displaying messages during the game
        public static Label CreateAnnoucmentLabel(int width, int height, string initMessage, Grid outerGrid)
        {
            Label annoucementLabel = new Label();
            annoucementLabel.Height = height;
            annoucementLabel.Width = width;
            //annoucementLabel.Content = $"Players were set to {GameLogic.Players.Count}";
            annoucementLabel.Content = initMessage;
            annoucementLabel.FontSize = height / 2;
            annoucementLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
            annoucementLabel.Background = Brushes.Transparent;
            annoucementLabel.Foreground = new SolidColorBrush(Colors.White);
            annoucementLabel.HorizontalAlignment = HorizontalAlignment.Center;
            annoucementLabel.VerticalAlignment = VerticalAlignment.Top;
            outerGrid.Children.Add(annoucementLabel);
            return annoucementLabel;
        }
        public static void SetWindowStyle(Window window)
        {
            window.WindowState = WindowState.Maximized;
            window.WindowStyle = WindowStyle.None;
            window.Background = Brushes.Black;
        }

        // return index in object name, if index is not present returns -1
        public static int FindObjectIndex<T>(T giveObject)
        {
            string name = FindObjectName<T>(giveObject);
            if (name != null)
            {
                string number = "";
                foreach (char character in name)
                {
                    if (char.IsDigit(character))
                    {
                        number += character;
                    }
                }
                if (number == "")
                {
                    return -1;
                }
                else
                {
                    return Int32.Parse(number);
                }
            }
            return -1;
        }

        // returns objects property name if it has one, if not returns null
        public static string FindObjectName<T>(T givenObject)
        {
            string nameValue = null;
            if (givenObject.GetType().GetProperty("Name") != null)
            {
                System.Reflection.PropertyInfo property = givenObject.GetType().GetProperty("Name");
                Type propertyType = property.PropertyType;

                if (propertyType == typeof(string))
                {

                    nameValue = (string)property.GetValue(givenObject, null);
                }
            }
            return nameValue;
        }

        public static List<Point> GetHexagonPoints(int size, Polygon drawnHexagon)
        {
            List<Point> points = new List<Point>();
            var listPoints = GenericWindow.GetListOfPoints(drawnHexagon.Points);
            for (int i = 0; i < listPoints.Count; i++)
            {
                foreach (var point in listPoints[i])
                {
                    points.Add(new Point(2 * point.X - size, 2 * point.Y - size));
                }
            }
            return points;
        }
    }
    public partial class App : Application
    {
    }
}
