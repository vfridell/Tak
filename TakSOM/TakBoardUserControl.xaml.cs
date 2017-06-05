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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TakLib;

namespace TakSOM
{
    /// <summary>
    /// Interaction logic for TakBoardUserControl.xaml
    /// </summary>
    public partial class TakBoardUserControl : UserControl
    {
        Dictionary<Rectangle, PieceStack> _imageToPieceStackMap = new Dictionary<Rectangle, PieceStack>();
        Dictionary<Rectangle, Space> _imageToSpaceMap = new Dictionary<Rectangle, Space>();

        public TakBoardUserControl()
        {
            InitializeComponent();
        }

        public void DrawBoard(Board board)
        {
            try
            {
                MainCanvas.Children.Clear();
                _imageToPieceStackMap.Clear();
                double squareSize = Math.Min(CanvasDockPanel.ActualHeight - 50, CanvasDockPanel.ActualWidth - 50) / board.Size;
                BoardGridDrawing boardGrid = new BoardGridDrawing(board, squareSize);
                                
                foreach (Rectangle r in boardGrid.Rectangles)
                {
                    MainCanvas.Children.Add(r);

                    Tuple<Space, PieceStack> sps = (Tuple<Space, PieceStack>)r.DataContext;
                    var pieceStackDrawing = new PieceStackDrawing(sps.Item2, sps.Item1, board.Size, squareSize);
                    foreach (Shape s in pieceStackDrawing.Shapes)
                        MainCanvas.Children.Add(s);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
