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
        private double nextPointX;

        public IList<OxyPlot.DataPoint> PlotPoints { get; private set; }
        public OptimizationControl()
        {
            PlotPoints = new List<OxyPlot.DataPoint>();
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

        public void ReportProgress(double fitness, int[] solution, double[][] kir)
        {
            PlotPoints.Add(new OxyPlot.DataPoint(nextPointX, fitness));
            var solutionText = getSolutionText(solution);
            try
            {
                Dispatcher.Invoke(() =>
                {
                    SolutionLabel.Content = solutionText;
                    updateFitness(fitness);
                    updateKir(kir);
                    Plot.InvalidatePlot();
                });
            }
            catch (TaskCanceledException) { }
            nextPointX += 0.5;
        }

        private void updateKir(double[][] kir)
        {
            var kirList = KirListBuilder.FromMatrix(kir);
            KirStackPanel.Children.Clear();
            foreach (var e in kirList)
            {
                KirStackPanel.Children.Add(e);
            }
        }
        private void updateFitness(double fitness)
        {
            FitnessLabel.Content = fitness;
        }
        private string getSolutionText(int[] solution)
        {
            var builder = new StringBuilder();
            foreach (var i in solution)
            {
                builder.Append(i);
                builder.Append(" ");
            }
            return builder.ToString();
        }

        private void resetPlot()
        {
            PlotPoints.Clear();
            nextPointX = 0;
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
