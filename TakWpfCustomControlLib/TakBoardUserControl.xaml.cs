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

namespace TakWpfControls
{
    /// <summary>
    /// Interaction logic for TakBoardUserControl.xaml
    /// </summary>
    public partial class TakBoardUserControl : UserControl
    {
        private double _canvasSize;

        public TakBoardUserControl()
        {
            InitializeComponent();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            _canvasSize = Math.Min(MainCanvas.ActualHeight, MainCanvas.ActualWidth);
            if(Board != null && Data != null)
                DrawBoard(Data);
            else if (Board != null) DrawBoard();
        }

        public Board Board { get; set; }
        public BoardAnalysisData Data { get; set; }
        public MaximumRatioAnalysisData AltData { get; set; }

        public void DrawBoard()
        {
            if(Board == null) throw new Exception("Must set Board before calling DrawBoard");
            try
            {
                MainCanvas.Children.Clear();
                double squareSize = _canvasSize / Board.Size;
                BoardGridDrawing boardGrid = new BoardGridDrawing(Board, squareSize);
                                
                foreach (Rectangle r in boardGrid.Rectangles)
                {
                    MainCanvas.Children.Add(r);

                    Tuple<Space, PieceStack> sps = (Tuple<Space, PieceStack>)r.DataContext;
                    var pieceStackDrawing = new PieceStackDrawing(sps.Item2, sps.Item1, Board.Size, squareSize);
                    foreach (Shape s in pieceStackDrawing.Shapes)
                        MainCanvas.Children.Add(s);
                }

                WriteInfo();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void DrawBoard(BoardAnalysisData data)
        {
            DrawBoard();
            Data = data;

            StringBuilder generalSB = new StringBuilder();
            generalSB.AppendLine($"avgSubgraphDiff: {data.averageSubGraphDiff}");
            generalSB.AppendLine($"capStoneDiff: {data.capStoneDiff}");
            generalSB.AppendLine($"longestSubGraphDiff: {data.longestSubGraphDiff}");
            generalSB.AppendLine($"numberOfSubGraphsDiff: {data.numberOfSubGraphsDiff}");
            generalSB.AppendLine($"possibleMovesDiff: {data.possibleMovesDiff}");
            generalSB.AppendLine($"wallCountDiff: {data.wallCountDiff}");
            generalSB.AppendLine($"winningResultDiff: {data.winningResultDiff}");
            //GeneralInfo.Text += generalSB.ToString();
            //GeneralInfo.FontSize = GetFontSize();
        }

        public void DrawBoard(MaximumRatioAnalysisData data)
        {
            DrawBoard();
            UpdateAnalysisDataGrid();
        }

        private void UpdateAnalysisDataGrid()
        {
            MaximumRatioAnalyzer analyzer = new MaximumRatioAnalyzer(Board.Size);
            var result = analyzer.Analyze(Board);

            FactorsDG.DataContext = analyzer.GetCurrentAnalysisFactors.Values;

            StringBuilder generalSB = new StringBuilder();
            foreach (AnalysisFactor factor in analyzer.GetCurrentAnalysisFactors.Values)
            {
                double calc = analyzer.GetCurrentAnalysisFactors.CalculateNamedFactor(factor.Name, Board.Turn);
                generalSB.AppendLine($"{factor.Name}: Base {factor.Value:g2}  Calc {calc:g2}");
            }
            generalSB.AppendLine($"Advantage: {result.whiteAdvantage:G}");
        }

        private void WriteInfo()
        {
            StringBuilder generalSB = new StringBuilder();
            StringBuilder whiteSB = new StringBuilder();
            StringBuilder blackSB = new StringBuilder();
            generalSB.AppendLine($"FlatScore: {Board.FlatScore}");
            generalSB.AppendLine($"Result: {Board.GameResult}");

            whiteSB.AppendLine($"Stones Held: {Board.StonesInHand(PieceColor.White)}");
            whiteSB.AppendLine($"CapStones Held: {Board.CapStonesInHand(PieceColor.White)}");
            
            blackSB.AppendLine($"Black");
            blackSB.AppendLine($"Stones Held: {Board.StonesInHand(PieceColor.Black)}");
            blackSB.AppendLine($"CapStones Held: {Board.CapStonesInHand(PieceColor.Black)}");

            //GeneralInfo.Text = generalSB.ToString();
            //GeneralInfo.FontSize = GetFontSize();
            WhiteInfo.Text = whiteSB.ToString();
            WhiteInfo.FontSize = GetFontSize();
            BlackInfo.Text = blackSB.ToString();
            BlackInfo.FontSize = GetFontSize();
        }

        private double GetFontSize()
        {
            if (_canvasSize > 600)
                return 16.0;
            else if (_canvasSize > 400)
                return 14.0;
            else if (_canvasSize > 200)
                return 12.0;
            else if (_canvasSize > 100)
                return 10.0;
            else
                return 8.0;
        }

        private void FactorsDG_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(System.Double))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "0000.00";
        }
    }
}
