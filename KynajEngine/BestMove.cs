using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KynajEngine
{
    public static class BestMove
    {
        public static List<Move> getBestMove(Board Fboard, int depth)
        {
            Board board = (Board) Fboard.Clone();
            List<Move> bestMove = new List<Move>();
            int score = int.MinValue;

            bool isWhite = board.isWhiteMove;

            foreach (Move move in Perft.getPossibleMovesList(board, isWhite))
            {
                board.makeMove(move);

                int moveScore = minimax(board, depth - 1, int.MinValue, int.MaxValue, true, isWhite);

                if (moveScore > score)
                {
                    score = moveScore;

                    bestMove.Clear();

                    bestMove.Add(move);
                }
                else if(moveScore == score)
                    bestMove.Add(move);


                Console.WriteLine(Notation.getAlgebraicNotation(move) + " - " + moveScore);

                board.undoMove(move);
            }

            return bestMove;
        }

        private static int minimax(Board board, int depth, int alpha, int beta, bool maximazing, bool MaxisWhite)
        {
            if(depth == 0)
            {
                return Eval.getEval(board, MaxisWhite);
            }

            if (maximazing)
            {
                List<Move> moves = Perft.getPossibleMovesList(board, MaxisWhite);

                int maxEval = int.MinValue;
                foreach(Move move in moves)
                {
                    board.makeMove(move);

                    int eval = minimax(board, depth - 1, alpha, beta, false, MaxisWhite);

                    board.undoMove(move);

                    if(eval > maxEval)
                    {
                        maxEval = eval;
                    }
                    alpha = Math.Max(alpha, eval);
                    if (beta <= alpha)
                        break;
                }

                return maxEval;
            } 
            else
            {
                List<Move> moves = Perft.getPossibleMovesList(board, !MaxisWhite);

                int minEval = int.MaxValue;
                foreach (Move move in moves)
                {
                    board.makeMove(move);

                    int eval = minimax(board, depth - 1, alpha, beta, true, !MaxisWhite);

                    board.undoMove(move);

                    if (eval < minEval)
                    {
                        minEval = eval;
                    }
                    beta = Math.Min(beta, eval);
                    if (beta <= alpha)
                        break;
                }

                return minEval;
            }
        }
    }
}
