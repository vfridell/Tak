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
            BoardListView.SelectionChanged += BoardListViewOnSelectionChanged;
            if (boards != null && boards.Count > 0)
            {
                BoardUserControl.Board = boards[0];
                BoardListView.DataContext = boards;
            }
        }

        private void BoardListViewOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            BoardUserControl.Board = (Board)selectionChangedEventArgs.AddedItems[0];
        }
    }
}
