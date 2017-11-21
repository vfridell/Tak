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
        private ObservableCollection<AnalysisFactors> _factors;
        private int _colorIndex = 0;

        public Game Game => _game;

        public void Initialize(Game game)
        {
            InitializeComponent();
            _game = game;
            List<double> turns = new List<double>();
            MaximumRatioAnalyzer analyzer = new MaximumRatioAnalyzer(game.Boards[0].Size);
            _factors = new ObservableCollection<AnalysisFactors>();
            int i = 0;
            foreach (Board board in Game.Boards)
            {
                turns.Add(board.Round);
                analyzer.Analyze(board);
                _factors.Add(analyzer.GetCurrentAnalysisFactors.Clone());
                i++;
            }

            i = 0;
            foreach (string featureIndex in _factors[0].Keys)
            {
                if (_factors[0][featureIndex].Weight == 0) continue;
                // Create data sources:
                AddFeatureGraph(featureIndex, GetColor());
                CheckBox newCheckBox = new CheckBox() { IsChecked = true, Content = FeatureIndexToName(featureIndex), Name = featureIndex};
                RadioStackPanel.Children.Add(newCheckBox);
                newCheckBox.Checked += (sender, args) =>
                {
                    AddFeatureGraph(newCheckBox.Name, GetColor());
                };
                newCheckBox.Unchecked +=
                    (sender, args) =>
                    {
                        Chart.Children.Where(e => e is LineGraph)
                            .FirstOrDefault(l => l.ToString() == FeatureIndexToName(featureIndex))?.Remove();
                    };
                i++;
            }

            Chart.Legend.LegendLeft = 5;

            // Force evertyhing plotted to be visible
            Chart.FitToView();
        }

        public void AddFeatureGraph(string featureIndex, Color color)
        {
            EnumerableDataSource<AnalysisFactors> dataSource = new EnumerableDataSource<AnalysisFactors>(_factors);
            dataSource.SetXMapping(f => f["roundNumber"].Value);
            dataSource.SetYMapping(f => f[featureIndex].CalculatedResult);
            Chart.AddLineGraph(dataSource, color, 2, FeatureIndexToName(featureIndex));
        }

        public static string FeatureIndexToName(string featureIndex)
        {
            int i = 0;
            string name = "";
            foreach (char c in featureIndex)
            {
                char newC = c;
                if (i == 0) newC = Char.ToUpper(c);
                if (Char.IsUpper(newC) && i > 0)
                {
                    name = name + ' ';
                }
                name = name + newC;
                i++;
            }
            name = name.Replace("Diff", "").Trim();
            return name;
        }

        private Color GetColor()
        {
            _colorIndex++;
            return _colors[_colorIndex % 16];
        }
        private static Dictionary<int, Color> _colors = new Dictionary<int, Color>()
        {
            {0,Colors.Blue},
            {1,Colors.Red},
            {2,Colors.Green},
            {3,Colors.Orange},
            {4,Colors.Black},
            {5,Colors.DeepPink},
            {6,Colors.DarkSlateGray},
            {7,Colors.Tomato},
            {8,Colors.Lime},
            {9,Colors.Chocolate},
            {10,Colors.Brown},
            {11,Colors.CadetBlue},
            {12,Colors.BlueViolet},
            {13,Colors.GreenYellow},
            {14,Colors.Firebrick},
            {15,Colors.DarkSalmon},
        };
    }

}
