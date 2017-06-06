using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TakLib;

namespace TakSOM
{
    public class BoardInfoSidebarDrawing
    {
        public BoardInfoSidebarDrawing(Board board)
        {
            TextBlock flatScoreTB = new TextBlock();
            flatScoreTB.Text = $"FlatScore: {board.FlatScore}";
            TextBlock _whiteCapStonesTB = new TextBlock();
            _whiteCapStonesTB.Text = $"CapStones: {board.CapStonesInHand(PieceColor.White)}";
            TextBlock _blackCapStonesTB = new TextBlock();
            _blackCapStonesTB.Text = $"CapStones: {board.CapStonesInHand(PieceColor.Black)}";
            TextBlock _whiteStonesTB = new TextBlock();
            _whiteStonesTB.Text = $"CapStones: {board.StonesInHand(PieceColor.White)}";
            TextBlock _blackStonesTB = new TextBlock();
            _blackStonesTB.Text = $"CapStones: {board.StonesInHand(PieceColor.Black)}";
        }
    }
}
