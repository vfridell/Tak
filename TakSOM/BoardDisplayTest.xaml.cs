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

        public BoardDisplayTest(Board board)
        {
            InitializeComponent();
            BoardUserControl.Board = board;
        }
    }
}
