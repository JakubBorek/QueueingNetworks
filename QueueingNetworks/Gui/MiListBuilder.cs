using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace Gui
{
    static class MiListBuilder
    {
        public static List<UIElement> FromNode(Node node, int classCount)
        {
            node.UpdateMiLength(classCount);
            var list = new List<UIElement>(classCount);
            for (int i = 0; i < classCount; i++)
            {
                list.Add(getListElement(node, i));
            }
            return list;
        }

        private static UIElement getListElement(Node node, int classId)
        {
            var stackPanel = new StackPanel { Orientation = Orientation.Horizontal };
            stackPanel.Children.Add(new Label { Content = "Class " + (classId + 1).ToString() });
            stackPanel.Children.Add(getMiUpDown(node, classId));
            return stackPanel;
        }

        private static UIElement getMiUpDown(Node node, int classId)
        {
            var upDown = new DoubleUpDown() { Minimum = 0 };
            upDown.Value = node.Mi[classId];
            upDown.ValueChanged += (_1, _2) =>
            {
                node.Mi[classId] = upDown.Value ?? 0;
            };
            return upDown;
        }
    }
}
