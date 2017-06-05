using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TakLib;

namespace TakSOM
{
    public class BoardGridDrawing
    {
        private List<Rectangle> _rectangles = new List<Rectangle>();
        public IReadOnlyList<Rectangle> Rectangles => _rectangles;

        private Brush[] _gridBrushes = new Brush[2];

        public BoardGridDrawing(Board board, double squareSize)
        {
            if (board.Size % 2 == 0) { _gridBrushes[1] = Brushes.WhiteSmoke; _gridBrushes[0] = Brushes.DimGray; }
            else { _gridBrushes[0] = Brushes.WhiteSmoke; _gridBrushes[1] = Brushes.DimGray; }

            int squareFillModNum = 2;
            foreach(Coordinate c in new CoordinateEnumerable(board.Size))
            {
                Rectangle r = new Rectangle() { Height = squareSize, Width = squareSize };
                r.Stroke = Brushes.Black;
                r.Fill = _gridBrushes[squareFillModNum % 2];
                r.Name = $"{c.ColumnChar}{c.RowChar}";
                Space s = board.GetSpace(c);
                PieceStack ps = null;
                if (!s.IsEmpty) ps = board.GetPieceStack(c);
                r.DataContext = new Tuple<Space, PieceStack> (s, ps);
                Canvas.SetLeft(r, c.Column * squareSize);
                Canvas.SetTop(r, ((board.Size - 1) * squareSize) - c.Row * squareSize);
                _rectangles.Add(r);
                squareFillModNum++;
            }
        }
    }
}
