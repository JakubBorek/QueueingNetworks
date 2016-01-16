using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace Gui
{
    static class ClassCountListBuilder
    {
        public static List<UIElement> GenerateList(List<int> counts, Action refreshAction)
        {
            var list = new List<UIElement>(counts.Count + 1);
            for (int i = 0; i < counts.Count; i++)
            {
                list.Add(getClassCountElement(counts, i));
            }
            list.Add(getAddNewButton(counts, refreshAction));
            list.Add(getRemoveButton(counts, refreshAction));
            return list;
        }

        private static UIElement getClassCountElement(List<int> counts, int id)
        {
            var stackPanel = new StackPanel { Orientation = Orientation.Horizontal };
            stackPanel.Children.Add(new Label { Content = (id + 1).ToString() });
            stackPanel.Children.Add(getCountUpDown(counts, id));
            return stackPanel;
        }

        private static UIElement getCountUpDown(List<int> counts, int id)
        {
            var upDown = new IntegerUpDown() { Minimum = 1 };
            upDown.Value = counts[id];
            upDown.ValueChanged += (_1, _2) =>
            {
                counts[id] = upDown.Value ?? 0;
            };
            return upDown;
        }

        private static UIElement getAddNewButton(List<int> counts, Action refreshAction)
        {
            var btn = new Button { Content = "Add" };
            btn.Click += (_1, _2) =>
            {
                counts.Add(1);
                refreshAction();
            };
            return btn;
        }

        private static UIElement getRemoveButton(List<int> counts, Action refreshAction)
        {
            var btn = new Button { Content = "Remove" };
            if (counts.Count <= 1)
            {
                btn.IsEnabled = false;
            }
            else
            {
                btn.Click += (_1, _2) =>
                {
                    counts.RemoveAt(counts.Count - 1);
                    refreshAction();
                };
            }
            return btn;
        }

    }
}
