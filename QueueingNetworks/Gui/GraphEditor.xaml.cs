using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Gui
{
    /// <summary>
    /// Interaction logic for GraphEditor.xaml
    /// </summary>
    public partial class GraphEditor : UserControl, INotifyPropertyChanged
    {
        private Node selectedNode;
        public Node SelectedNode
        {
            get { return selectedNode; }
            private set
            {
                if (selectedNode != null) { selectedNode.Selected = false; }
                selectedNode = value;
                if (selectedNode != null) { selectedNode.Selected = true; }
                NotifyPropertyChanged("SelectedNodeType");
            }
        }

        private List<int> classCounts;
        public List<int> ClassCounts
        {
            get { return classCounts; }
            set
            {
                classCounts = value;
                refresh();
            }
        }

        private List<Node> nodes;
        public List<Node> Nodes
        {
            get { return nodes; }
            set
            {
                nodes = value;
                SelectedNode = null;
                refresh();
            }

        }
        public Graph Graph
        {
            get
            {
                return GraphBuilder.FromNodes(Nodes);
            }
        }

        public int SelectedNodeType
        {
            get { return selectedNode != null ? selectedNode.Type == Node.NodeType.Type1 ? 0 : 1 : -1; }
            set
            {
                if (value == 0)
                {
                    selectedNode.Type = Node.NodeType.Type1;
                }
                else if (value == 1)
                {
                    selectedNode.Type = Node.NodeType.Type3;
                }
                NotifyPropertyChanged("SelectedNodeType");
            }

        }

        public GraphEditor()
        {
            nodes = new List<Node>();
            classCounts = new List<int>() { 0 };
            InitializeComponent();
            updateClassCountsList();
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
            var textBlock = (TextBlock)sender;
            var node = (Node)textBlock.Tag;
            onNodeSelected(node);
        }

        private void onAddNewClicked(object sender, RoutedEventArgs e)
        {
            var nodeLabel = (Nodes.Count + 1).ToString();
            Nodes.Add(new Node() { Name = nodeLabel });
            refresh();
        }

        private void onDeleteClicked(object sender, RoutedEventArgs e)
        {
            if (Nodes.Count == 0)
            {
                return;
            }
            int lastId = Nodes.Count - 1;
            ConnectionsDeleter.DeleteForDestination(Nodes, lastId);
            Nodes.RemoveAt(lastId);
            SelectedNode = null;
            refresh();
        }

        private void onNodeSelected(Node node)
        {
            SelectedNode = node;
            updateConnectionsList();
            updateMiList();
            NotifyPropertyChanged("SelectedNodeType");
            NotifyPropertyChanged("SelectedNodeLabel");
            NotifyPropertyChanged("IsNodeSelected");
        }

        private void refresh()
        {
            if (selectedNode != null)
            {
                updateConnectionsList();
                updateMiList();
            }
            updateClassCountsList();
            NotifyPropertyChanged("SelectedNodeType");
            NotifyPropertyChanged("SelectedNodeLabel");
            NotifyPropertyChanged("IsNodeSelected");
            NotifyPropertyChanged("Graph");
        }

        private void updateConnectionsList()
        {
            ConnectionsStackPanel.Children.Clear();
            var nodeFromId = new NodeFromIdProvider(Nodes);
            var list = NodePropertiesBuilder.FromNode(SelectedNode, nodeFromId, refresh);
            foreach (var e in list)
            {
                ConnectionsStackPanel.Children.Add(e);
            }
        }

        private void updateMiList()
        {
            MiStackPanel.Children.Clear();
            var list = MiListBuilder.FromNode(SelectedNode, ClassCounts.Count);
            foreach (var e in list)
            {
                MiStackPanel.Children.Add(e);
            }
        }

        private void updateClassCountsList()
        {
            ClassCountStackPanel.Children.Clear();
            var list = ClassCountListBuilder.GenerateList(ClassCounts, refresh);
            list.ForEach(e => ClassCountStackPanel.Children.Add(e));
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
