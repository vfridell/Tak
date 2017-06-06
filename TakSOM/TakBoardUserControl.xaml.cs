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
        private double _canvasSize;

        public TakBoardUserControl()
        {
            InitializeComponent();
            CanvasDockPanel.SizeChanged += (sender, args) =>
            {
                if (Board != null) DrawBoard();
            };
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            _canvasSize = Math.Min(MainCanvas.ActualHeight, MainCanvas.ActualWidth);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            if(Board != null) DrawBoard();
        }

        public Board Board { get; set; }

        public void DrawBoard()
        {
            if(Board == null) throw new Exception("Must set Board before calling DrawBoard");
            try
            {
                MainCanvas.Children.Clear();
                _imageToPieceStackMap.Clear();
                double squareSize = _canvasSize / Board.Size;
                BoardGridDrawing boardGrid = new BoardGridDrawing(Board, squareSize);
                                
                foreach (Rectangle r in boardGrid.Rectangles)
                {
                    //Panel.SetZIndex(r, -1);
                    MainCanvas.Children.Add(r);

                    Tuple<Space, PieceStack> sps = (Tuple<Space, PieceStack>)r.DataContext;
                    var pieceStackDrawing = new PieceStackDrawing(sps.Item2, sps.Item1, Board.Size, squareSize);
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
