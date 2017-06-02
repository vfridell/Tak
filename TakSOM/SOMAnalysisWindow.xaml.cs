using System;
using System.Collections.Generic;
using System.Globalization;
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
using TakLib.AI.Helpers;

namespace TakSOM
{
    /// <summary>
    /// Interaction logic for SOMAnalysisWindow.xaml
    /// </summary>
    public partial class SOMAnalysisWindow : Window
    {
        public double MaxWeight = Double.MaxValue;
        public double MinWeight = 0;

        private SOMLattice _lattice;
        private Dictionary<Coordinate, List<Board>> _analysisDictionary = new Dictionary<Coordinate, List<Board>>();

        public SOMAnalysisWindow(SOMLattice lattice)
        {
            InitializeComponent();
            _lattice = lattice;
        }

        public void CategorizeData(IEnumerable<Board> data)
        {
            foreach (Board item in data)
            {
                MarkAndStoreDataPoint(item);
            }
        }

        public void MarkAndStoreDataPoint(Board board)
        {
            SOMWeightsVector vector = _lattice.Analyzer.GetSomWeightsVector(board);
            SOMNode bestNode = _lattice.GetBestMatchingUnitNode(vector);
            var loc = new Coordinate(bestNode.X, bestNode.Y);
            if (!_analysisDictionary.ContainsKey(loc)) _analysisDictionary.Add(loc, new List<Board>());
            _analysisDictionary[loc].Add(board);
        }

        public void RenderColors()
        {
            foreach(Coordinate c in new CoordinateEnumerable(_lattice.Size))
            {
                SOMNode node = _lattice.GetNode(c);
                DrawNode(node);
            }
        }

        public void RenderCounts()
        {
            foreach (var kvp in _analysisDictionary)
            {
                DrawNode(kvp.Key, kvp.Value);
            }
        }

        private void DrawNode(Coordinate loc, List<Board> fileAnalysisDataList)
        {
            TextBlock tb = new TextBlock();
            tb.Text = fileAnalysisDataList.Count.ToString();
            tb.Foreground = new SolidColorBrush(Colors.White);
            tb.FontSize = 10.0;

            Canvas.SetZIndex(tb, 99);
            Canvas.SetLeft(tb, (loc.Column * 11) + 2);
            Canvas.SetBottom(tb, (loc.Row * 11) - 2);
            tb.MouseLeftButtonDown += (sender, args) => { detailsTextBlock.Text = fileAnalysisDataList.Aggregate("", (result, f) => $"{f.Filename.Substring(f.Filename.LastIndexOf(@"\"))}\n" + result); };

            MainCanvas.Children.Add(tb);

        }

        private void DrawNode(SOMNode node)
        {
            int rectSize = 10;
            Rectangle rect = new Rectangle();
            rect.Width = rectSize;
            rect.Height = rectSize;

            byte red = GetColorFromWeight(node.GetWeight(0));
            byte green = GetColorFromWeight(node.GetWeight(1));
            byte blue = GetColorFromWeight(node.GetWeight(2));
            rect.Fill = new SolidColorBrush(Color.FromRgb(red, green, blue));

            //rect.Fill = Brushes.Tan;
            //double imageXOffset = rect.Width / 2;
            //double imageYOffset = rect.Height / 2;

            Canvas.SetZIndex(rect, -1);
            Canvas.SetLeft(rect, node.X * 11);
            Canvas.SetBottom(rect, node.Y * 11);
            rect.MouseLeftButtonDown += (sender, args) => { detailsTextBlock.Text = $"{node.X},{node.Y}\n" + node.WeightsVector.Aggregate("", (result, d) => $"{result}\n{d.ToString(CultureInfo.InvariantCulture)}"); };
            MainCanvas.Children.Add(rect);
        }

        private byte GetColorFromWeight(double weight)
        {
            int squashedValue = (int)(1.0 / (1.0 + Math.Exp(5.0 - (5.0 * weight / 128.0))));
            byte b = (byte)squashedValue;
            //double expandedValue = weight * 256.0;
            //byte b = (byte) expandedValue;
            return b;
        }
    }
}
