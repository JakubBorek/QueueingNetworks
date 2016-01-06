using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Gui
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            OptimizationControl.NetworkGetter = ()=>{ return NetworkConverter.NodesToNetwork(GraphEditor.Nodes); };
        }


        private void onLoadClicked(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Queueing Network Files | *.txt";
            Nullable<bool> result = dlg.ShowDialog();
            if(result == true)
            {
                string filename = dlg.FileName;
                var reader = new System.IO.StreamReader(filename);
                var network = QueueingNetworks.Network.Read(reader);
                reader.Close();
                GraphEditor.Nodes = NetworkConverter.NetworkToNodes(network);
            }
        }

        private void onSaveClicked(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Queueing Network Files | *.txt";
            Nullable<bool> result = dialog.ShowDialog();
            if (result == true)
            {
                string filename = dialog.FileName;
                var writer = new System.IO.StreamWriter(filename);
                var network = NetworkConverter.NodesToNetwork(GraphEditor.Nodes);
                network.Write(writer);
                writer.Close();
            }

        }

        private void onNewClicked(object sender, RoutedEventArgs e)
        {
            GraphEditor.Nodes = new List<Node>();
        }

    }
}
