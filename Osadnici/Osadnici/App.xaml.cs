using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

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
    public partial class App : Application
    {
    }
}
