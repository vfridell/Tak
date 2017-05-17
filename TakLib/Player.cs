using System.Threading;
using System.Threading.Tasks;

namespace TakLib
{
    public class Player
    {
        public virtual string Name { get; set; }
        public virtual bool IsAI => false;
    }

    public interface ITakAI
    {
        void BeginNewGame(bool PlayingWhite);
        Move MakeBestMove(Game game);
        Move PickBestMove(Board board);
        Task<Move> PickBestMoveAsync(Board board, CancellationToken aiCancelToken);
        bool playingWhite { get; }
    }
}