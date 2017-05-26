using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakLib
{
    public class GameResultService
    {
        public static GameResult GetGameResult(Board CurrentBoard)
        {
            var currentColor = CurrentBoard.WhiteToPlay ? PieceColor.White : PieceColor.Black;
            var otherColor = CurrentBoard.WhiteToPlay ? PieceColor.Black : PieceColor.White;

            RoadFinder roadFinder = new RoadFinder(CurrentBoard.Size);
            roadFinder.Analyze(CurrentBoard, currentColor);
            if (roadFinder.Roads.Count > 0)
            {
                return currentColor == PieceColor.White ? GameResult.WhiteRoad : GameResult.BlackRoad;
            }
            roadFinder.Analyze(CurrentBoard, otherColor);
            if (roadFinder.Roads.Count > 0)
            {
                return otherColor == PieceColor.White ? GameResult.WhiteRoad : GameResult.BlackRoad;
            }

            if (CurrentBoard.EmptySpaces == 0 || CurrentBoard.EitherPlayerOutOfPieces)
            {
                if (CurrentBoard.FlatScore == 0) return GameResult.Draw;
                return CurrentBoard.FlatScore > 0 ? GameResult.WhiteFlat : GameResult.BlackFlat;
            }

            return GameResult.Incomplete;
        }
    }
}
