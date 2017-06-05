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

            int stackHeight = 0;
            foreach(Piece p in _pieceStack.Reverse())
            {
                Brush fill = _colorBrushes[(int)p.Color];
                Shape shape;
                if (p.Type == PieceType.CapStone) shape = MakeCapstoneShape(space, boardSize, squareSize, stackHeight);
                else if (p.Type == PieceType.Wall) shape = MakeWallShape(space, boardSize, squareSize, stackHeight);
                else shape = MakeStoneShape(space, boardSize, squareSize, stackHeight);
                shape.Fill = fill;
                shape.Stroke = Brushes.Black;
                Canvas.SetLeft(shape, space.Coordinate.Column * squareSize);
                Canvas.SetTop(shape, ((boardSize - 1) * squareSize) - space.Coordinate.Row * squareSize);

                _shapes.Add(shape);
                stackHeight++;
            }
        }

        private Rectangle MakeWallShape(Space space, int boardSize, double squareSize, int stackHeight)
        {
            double height = (squareSize - 10) / 1.5d;
            double wallWidth = (squareSize - 10) / 4d;
            double sideMargin = (squareSize - wallWidth) / 2;
            double topMargin = (squareSize - height) / 2;
            Rectangle r = new Rectangle() { Height = height, Width = wallWidth, Margin = new Thickness(sideMargin, topMargin, sideMargin, topMargin) };
            Canvas.SetLeft(r, space.Coordinate.Column * squareSize);
            Canvas.SetTop(r, ((boardSize - 1) * squareSize) - space.Coordinate.Row * squareSize);
            Matrix m = r.RenderTransform.Value;
            m.RotateAt(45, wallWidth / 2, height/ 2);
            r.RenderTransform = new MatrixTransform(m);
            return r;
        }

        private Ellipse MakeCapstoneShape(Space space, int boardSize, double squareSize, int stackHeight)
        {
            double height = (squareSize - 10) / 1.5d;
            double width = (squareSize - 10) / 1.5d;
            double margin = (squareSize - height) / 2;
            Ellipse e = new Ellipse() { Height = height, Width = width, Margin = new Thickness(margin) };
            Canvas.SetLeft(e, space.Coordinate.Column * squareSize);
            Canvas.SetTop(e, ((boardSize - 1) * squareSize) - space.Coordinate.Row * squareSize);
            return e;
        }

        private Rectangle MakeStoneShape(Space space, int boardSize, double squareSize, int stackHeight)
        {
            double height = (squareSize - 10) / 1.5d;
            double width = (squareSize - 10) / 1.5d;
            double margin = (squareSize - height) / 2;
            Rectangle r = new Rectangle() { Height = height, Width = width, Margin = new Thickness(margin)};
            Canvas.SetLeft(r, space.Coordinate.Column * squareSize);
            Canvas.SetTop(r, ((boardSize - 1) * squareSize) - space.Coordinate.Row * squareSize);
            return r;
        }
    }
}
