using System.Threading;
using System.Threading.Tasks;

namespace TakLib
{
    public class Player
    {
        public virtual string Name { get; set; }
    }

    public interface ITakAI
    {
        void BeginNewGame(bool PlayingWhite, int boardSize);
        Move MakeBestMove(Game game);
        Move PickBestMove(Board board);
        Task<Move> PickBestMoveAsync(Board board, CancellationToken aiCancelToken);
        bool playingWhite { get; }
        string Name { get; }
    }
}