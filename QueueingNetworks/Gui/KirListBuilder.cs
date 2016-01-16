using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace Gui
{
    static class KirListBuilder
    {
        public static List<UIElement> FromMatrix(double[][] kir)
        {
            var list = new List<UIElement>(kir.Length);
            foreach (var kirForOneNode in kir)
            {
                list.Add(getListElement(kirForOneNode));
                list.Add(getSeparator());
            }
            return list;
        }

        private static UIElement getListElement(double[] kirForOneNode)
        {
            var stackPanel = new StackPanel { Orientation = Orientation.Vertical, VerticalAlignment = VerticalAlignment.Center };
            foreach (var kir in kirForOneNode)
            {
                stackPanel.Children.Add(new TextBlock { Text = kir.ToString() + " " });

            }
            var grid = new Grid();
            grid.Children.Add(stackPanel);
            return grid;
        }

        private static UIElement getSeparator()
        {
            return new Separator();
        }
    }
}
