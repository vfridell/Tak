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

namespace TakSOM
{
    /// <summary>
    /// Interaction logic for BoardDisplayTest.xaml
    /// </summary>
    public partial class BoardDisplayTest : Window
    {

        public BoardDisplayTest(IList<Board> boards)
        {
            InitializeComponent();
            BoardListView.ItemsSource = boards;
            BoardListView.SelectionChanged += BoardListViewOnSelectionChanged;
            if (boards != null && boards.Count > 0)
            {
                BoardUserControl.Board = boards[0];
            }
        }

        public BoardDisplayTest(IList<Tuple<IAnalysisResult, Board>> boards)
        {
            InitializeComponent();
            BoardListView.ItemsSource = boards;
            BoardListView.SelectionChanged += BoardAnalysisListViewOnSelectionChanged;
            if (boards != null && boards.Count > 0)
            {
                BoardUserControl.Board = boards[0].Item2;
            }
        }

        private void BoardListViewOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            BoardUserControl.Board = (Board)selectionChangedEventArgs.AddedItems[0];
            BoardUserControl.DrawBoard();
        }

        private void BoardAnalysisListViewOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            BoardUserControl.Board = ((Tuple<IAnalysisResult, Board>)selectionChangedEventArgs.AddedItems[0]).Item2;
            BoardUserControl.DrawBoard();
        }
    }
}
