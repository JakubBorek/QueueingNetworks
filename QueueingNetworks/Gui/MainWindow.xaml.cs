using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Gui
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Node selectedNode;
        public Node SelectedNode
        {
            get { return selectedNode; }
            private set
            {
                if (selectedNode != null) { selectedNode.Selected = false; }
                selectedNode = value;
                value.Selected = true;
            }
        }

        public Graph Graph
        {
            get
            {
                return GraphBuilder.GetSample();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            ConnectionsStackPanel.Children.Add(new Button { Content = "Btn" });
        }

        public string SelectedNodeLabel
        {
            get { return selectedNode != null ? "Node " + selectedNode.Name : "No node selected"; }
        }

        public bool IsNodeSelected
        {
            get { return selectedNode != null; }
        }

        private void onNodeMouseDown(object sender, RoutedEventArgs e)
        {
            var label = (Label)sender;
            var node = (Node)label.Tag;
            onNodeSelected(node);
        }


        private void onNodeSelected(Node node)
        {
            SelectedNode = node;
            updateConnectionsList();
            NotifyPropertyChanged("SelectedNodeLabel");
            NotifyPropertyChanged("IsNodeSelected");
        }

        private void refresh()
        {
            updateConnectionsList();
            NotifyPropertyChanged("SelectedNodeLabel");
            NotifyPropertyChanged("IsNodeSelected");
            NotifyPropertyChanged("Graph");
        }

        private void updateConnectionsList()
        {
            ConnectionsStackPanel.Children.Clear();
            var list = ConnectionListBuilder.FromNode(SelectedNode, refresh);
            foreach (var e in list)
            {
                ConnectionsStackPanel.Children.Add(e);
            }
        }


        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion
    }
}
