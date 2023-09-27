using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace Osadnici
{
    class Handler
    {
        public RoutedEventHandler Function { get; init; }
        public string Name { get; init; }

        public Handler(RoutedEventHandler function, string name)
        {
            Function = function;
            Name = name;
        }
    }
    class DrawnActionField
    {
        // creates action buttons by given list
        public static void CreateActionButtons(int size, Grid outerGrid, List <Handler> handlers)
        {
            StackPanel actionStackPanel = new StackPanel();
            foreach (Handler handler in handlers)
            {
                GenericWindow.CreateActionButton(size: size, content: handler.Name, stackPanel: actionStackPanel, handler: handler.Function);
            }
            
            outerGrid.Children.Add(actionStackPanel);
        }
    }

}
