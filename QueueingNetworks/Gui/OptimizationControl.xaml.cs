using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gui
{
    /// <summary>
    /// Interaction logic for Optimization.xaml
    /// </summary>
    public partial class OptimizationControl : UserControl, Optimization.IProgressReporter
    {
        private int nextPointIndex;

        public IList<OxyPlot.DataPoint> PlotPoints { get; private set; }
        public OptimizationControl()
        {
            PlotPoints = new List<OxyPlot.DataPoint>();
            PlotPoints.Add(new OxyPlot.DataPoint(1, 1));
            PlotPoints.Add(new OxyPlot.DataPoint(2, 2));
            InitializeComponent();
            LineSeries.ItemsSource = PlotPoints;
            this.Dispatcher.ShutdownStarted += Dispatcher_ShutdownStarted;
            ScoutCount.Value = 20;
            ELiteCount.Value = 2;
            BestCount.Value = 4;
            ProcessorMaxCount.Value = 5;
        }

        public Func<QueueingNetworks.Network> NetworkGetter { get; set; }
        private void Dispatcher_ShutdownStarted(object sender, EventArgs e)
        {
            Optimization.Optimization.Stop();
        }

        public void ReportProgress(double fitness, int[] solution)
        {
            PlotPoints.Add(new OxyPlot.DataPoint(nextPointIndex, fitness));
            this.Dispatcher.Invoke((Action)(() =>
            {
                Plot.InvalidatePlot();
            }));
            nextPointIndex++;
        }

        private void resetPlot()
        {
            PlotPoints.Clear();
            nextPointIndex = 0;
        }
        private void onStartOptimizationClicked(object sender, RoutedEventArgs e)
        {
            resetPlot();
            var network = NetworkGetter();
            if (network.Nodes.Count == 0)
            {
                showEmptyNetworkMessage();
                return;
            }         
            var parameters = new Optimization.OptimizationParameters
            {
                ScoutCount = ScoutCount.Value,
                BestSolutionsCount = BestCount.Value,
                EliteSolutionsCount = ELiteCount.Value,
                ProcessorsMaxCount = ProcessorMaxCount.Value,
                NodesCount = network.Nodes.Count,
                FitnessCalculator = new FitnessCalculator.FitnessCalculator(network),
                ProgressReporter = this

            };
            Optimization.Optimization.Run(parameters);
        }

        private void showEmptyNetworkMessage()
        {
            MessageBox.Show("Can't optimize empty network");
        }

        private void onStopOptimizationClicked(object sender, RoutedEventArgs e)
        {
            Optimization.Optimization.Stop();
        }
    }
}
