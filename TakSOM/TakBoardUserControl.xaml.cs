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
                foreach (Coordinate c in new CoordinateEnumerable(board.Size))
                {
                    HexagonDrawing hexWithImage = HexagonDrawing.GetHexagonDrawing(kvp.Value, _drawSize, kvp.Key, _mainCanvasOffsetPoint);
                    MainCanvas.Children.Add(hexWithImage.image);
                    MainCanvas.Children.Add(hexWithImage.polygon);
                    _imageToPieceStackMap[hexWithImage.polygon] = kvp.Key;
                    hexWithImage.polygon.MouseLeftButtonDown += polygon_MouseLeftButtonDown;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
