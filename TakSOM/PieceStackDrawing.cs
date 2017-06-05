using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TakLib;

namespace TakSOM
{
    public class PieceStackDrawing
    {
        private List<Shape> _shapes;
        private PieceStack _pieceStack;

        public List<Shape> Shapes => _shapes;

        private Brush[] _colorBrushes = new Brush[2] { Brushes.Orange, Brushes.Navy };

        public PieceStack PieceStack => _pieceStack;

        public PieceStackDrawing(PieceStack pieceStack, Space space, int boardSize, double squareSize)
        {
            _shapes = new List<Shape>();
            if (pieceStack == null) return;
            _pieceStack = pieceStack;
            foreach(Piece p in _pieceStack.Reverse())
            {
                Brush fill = _colorBrushes[(int)p.Color];
                Shape shape;
                if (p.Type == PieceType.CapStone) shape = new Ellipse() { Height = squareSize - 10, Width = squareSize - 10, Margin = new Thickness(5) };
                else if (p.Type == PieceType.Wall) shape = new Rectangle() { Height = squareSize - 10, Width = squareSize - 20, Margin = new Thickness(5) };
                else shape = new Rectangle() { Height = squareSize - 10, Width = squareSize - 10, Margin = new Thickness(5) };
                shape.Fill = fill;
                shape.Stroke = Brushes.Black;
                Canvas.SetLeft(shape, space.Coordinate.Column * squareSize);
                Canvas.SetTop(shape, ((boardSize - 1) * squareSize) - space.Coordinate.Row * squareSize);

                _shapes.Add(shape);
            }
        }
    }
}
