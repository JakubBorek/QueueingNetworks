using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace Gui
{
    static class NodePropertiesBuilder
    {
        public static List<UIElement> FromNode(Node node, NodeFromIdProvider nodeFromId, Action refreshAction)
        {
            var list = new List<UIElement>(node.Connections.Count + 1);
            for (int i = 0; i < node.Connections.Count; i++)
            {
                list.Add(getConnectionElement(node, i, nodeFromId, refreshAction));
            }
            list.Add(getAddNewButton(node, refreshAction));
            return list;
        }

        private static UIElement getConnectionElement(Node n, int connectionIndex, NodeFromIdProvider nodeFromId, Action refreshAction)
        {
            var stackPanel = new StackPanel { Orientation = Orientation.Horizontal };
            var connection = n.Connections[connectionIndex];
            stackPanel.Children.Add(new Label { Content = "To:" });
            stackPanel.Children.Add(getDestinationUpDown(connection, nodeFromId, refreshAction));
            stackPanel.Children.Add(new Label { Content = "W:" });
            stackPanel.Children.Add(getWeightUpDown(connection));
            stackPanel.Children.Add(new Label { Content = "Cl:" });
            stackPanel.Children.Add(getClassUpDown(connection));
            stackPanel.Children.Add(getRemoveConnectionButton(n, connectionIndex, refreshAction));
            return stackPanel;
        }

        private static UIElement getDestinationUpDown(Connection connection, NodeFromIdProvider nodeFromId, Action refreshAction)
        {
            var upDown = new IntegerUpDown() { Minimum = 1, Maximum = nodeFromId.getNodesCount() };
            upDown.Value = int.Parse(connection.To.Name);
            upDown.ValueChanged += (_1, _2) =>
            {
                if (upDown.Value != null)
                {
                    connection.To = nodeFromId.FromId((int)upDown.Value);
                    refreshAction();
                }
            };
            return upDown;
        }

        private static UIElement getWeightUpDown(Connection connection)
        {
            var upDown = new DoubleUpDown() { Minimum = 0, Maximum = 1 };
            upDown.Value = connection.Weight;
            upDown.ValueChanged += (_1, _2) =>
            {
                connection.Weight = upDown.Value ?? 0;
            };
            return upDown;
        }

        private static UIElement getClassUpDown(Connection connection)
        {
            var upDown = new IntegerUpDown() { Minimum = 1 };
            upDown.Value = connection.Class;
            upDown.ValueChanged += (_1, _2) =>
            {
                connection.Class = upDown.Value ?? 0;
            };
            return upDown;
        }

        private static UIElement getRemoveConnectionButton(Node n, int connectionIndex, Action refreshAction)
        {
            var btn = new Button { Content = "X" };
            btn.Click += (_1, _2) =>
            {
                n.Connections.RemoveAt(connectionIndex);
                refreshAction();
            };
            return btn;
        }

        private static UIElement getAddNewButton(Node node, Action refreshAction)
        {
            var btn = new Button { Content = "Add" };
            btn.Click += (_1, _2) =>
            {
                node.Connections.Add(new Connection { Class = 1, To = node, Weight = 0 });
                refreshAction();
            };
            return btn;
        }

    }
}
