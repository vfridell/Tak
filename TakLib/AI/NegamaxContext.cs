namespace TakLib
{
    public struct NegamaxContext
    {
        public NegamaxContext(Move move, Board board, double score, bool calculated)
        {
            Move = move;
            Board = board;
            Score = score;
            ScoreCalculated = calculated;
        }

        public Move Move;
        public Board Board;
        public double Score;
        public bool ScoreCalculated;
    }
}
