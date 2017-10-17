using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using TakLib;
using TakLib.AI;

namespace TakFeaturesExplorer
{
    /// <summary>
    /// Interaction logic for NextMovesWindows.xaml
    /// </summary>
    public partial class NextMovesWindows : Window
    {
        private Board _board;

        public NextMovesWindows(Board board)
        {
            InitializeComponent();
            _board = board;
            BoardListView.SelectionChanged += BoardAnalysisListViewOnSelectionChanged;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BoardListView.ItemsSource = null;
            MaximumRatioAnalyzer analyzer = new MaximumRatioAnalyzer(_board.Size);
            IDictionary<double, HashSet<NegamaxContext>> analysisDictionary = MoveSorter.GetSortedAnalysisDictionary(_board, analyzer);
            var src = analysisDictionary.SelectMany<KeyValuePair<double, HashSet<NegamaxContext>>, NegamaxContext>(kvp => kvp.Value);

            BoardListView.ItemsSource = src;
            if (src != null && src.Count() > 0)
            {
                BoardUserControl.Board = src.First().Board;
            }
        }

        private void BoardAnalysisListViewOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (selectionChangedEventArgs.AddedItems.Count > 0)
            {
                BoardUserControl.Board = ((NegamaxContext)selectionChangedEventArgs.AddedItems[0]).Board;
                MaximumRatioAnalyzer analyzer = new MaximumRatioAnalyzer(_board.Size);
                var analysisData = (MaximumRatioAnalysisData)analyzer.Analyze(BoardUserControl.Board);
                BoardUserControl.DrawBoard(analysisData);
            }
        }
    }
}
