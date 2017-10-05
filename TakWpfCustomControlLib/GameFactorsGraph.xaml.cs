using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using TakLib;

namespace TakWpfControls
{
    /// <summary>
    /// Interaction logic for GameFactorsGraph.xaml
    /// </summary>
    public partial class GameFactorsGraph : UserControl
    {
        private Game _game;

        public Game Game => _game;

        public void Initialize(Game game)
        {
            InitializeComponent();
            _game = game;
            List<double> turns = new List<double>();
            MaximumRatioAnalyzer analyzer = new MaximumRatioAnalyzer(game.Boards[0].Size);
            ObservableCollection<AnalysisFactors> factors = new ObservableCollection<AnalysisFactors>();
            int i = 0;
            foreach (Board board in Game.Boards)
            {
                turns.Add(board.Round);
                analyzer.Analyze(board);
                factors.Add(analyzer.GetCurrentAnalysisFactors.Clone());
                i++;
            }

            // Create data sources:
            EnumerableDataSource<AnalysisFactors> dataSource = new EnumerableDataSource<AnalysisFactors>(factors);
            dataSource.SetXMapping(f => f["roundNumber"].Value);
            dataSource.SetYMapping(f => f["flatScore"].CalculatedResult);
            Chart.AddLineGraph(dataSource, Colors.Red, 2, "Flat Score");
            EnumerableDataSource<AnalysisFactors> dataSource2 = new EnumerableDataSource<AnalysisFactors>(factors);
            dataSource2.SetXMapping(f => f["roundNumber"].Value);
            dataSource2.SetYMapping(f => f["stacksAdvantageDiff"].CalculatedResult);
            Chart.AddLineGraph(dataSource2, Colors.Blue, 2, "Stacks Advantage");

            //var y2DataSource = factors.Select(f => f.Values.FirstOrDefault(v => v.Name == "stacksAdvantageDiff")).Select(f => f.CalculatedResult).AsYDataSource();
            //var y3DataSource = factors.Select(f => f.Values.FirstOrDefault(v => v.Name == "longestSubGraphDiff")).Select(f => f.CalculatedResult).AsYDataSource();
            //var y4DataSource = factors.Select(f => f.Values.FirstOrDefault(v => v.Name == "averageSubGraphDiff")).Select(f => f.CalculatedResult).AsYDataSource();

            //var factory = new EnumerableDataSourceFactory();
            //var blah = factory.TryBuild(factors);

            // adding graph to plotter

            // Force evertyhing plotted to be visible
            Chart.FitToView();
        }
    }

}
