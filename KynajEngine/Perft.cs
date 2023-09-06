using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static KynajEngine.Board;

namespace KynajEngine
{
    public static class Perft
    {
        public static int handle(Board board, int depth, int level = 0)
        {
            if(depth == 0) 
                return 1;

            int nodes = 0;

            foreach(Move move in getPossibleMovesList(board, board.isWhiteMove))
            {
                board.makeMove(move);

                int nodeCount = handle(board, depth - 1, level + 1);

                if(level == 0)
                    Console.WriteLine(Notation.getAlgebraicNotation(move) + " - " + nodeCount);


                nodes += nodeCount;

                board.undoMove(move);
            }

            return nodes;
        }

        public static List<Move> getPossibleMovesList(Board board, bool isWhite)
        {
            Piece[] boardState = board.getState();
            List<Move> moves = new List<Move>();

            // Loops trough all the pieces on the board
            for (byte index = 0; index < boardState.Length; index++) 
            {
                if (boardState[index] != Piece.None)
                    if (isWhite == board.squareIsWhite(index))
                    {
                    // The legal moves for the current piece
                    List<Move> legalMoves = LegalMoves.getMoves(board, index);

                    foreach(Move move in legalMoves)
                    {
                        board.makeMove(move);

                        if (isChecked(isWhite, board))
                        {
                            // Move resulted in a self check so is illegal

                            board.undoMove(move);
                            continue;
                        }

                        
                        board.undoMove(move);

                        // Move is legal and is added
                        moves.Add(move);
                    }
                }
            }

            return moves;
        }

        public static List<Move> getPossibleMovesListWithoutCheck(Board board, bool isWhite)
        {
            Piece[] boardState = board.getState();
            List<Move> moves = new List<Move>();

            // Loops trough all the pieces on the board
            for (byte index = 0; index < boardState.Length; index++)
            {
                if (boardState[index] != Piece.None)
                    if (isWhite == board.squareIsWhite(index))
                    {
                        // The legal moves for the current piece
                        List<Move> legalMoves = LegalMoves.getMoves(board, index);

                        foreach (Move move in legalMoves)
                            moves.Add(move);
                        }
            }

            return moves;
        }

        public static int getPossibleMovesCount(Board board)
        {
            return getPossibleMovesList(board, true).Count();
        }

        public static int PerftF(Board board, int depth, bool isWhite = true)
        {
            int nodes = 0;

            if (depth == 0)
                return 1;

            List<Move> moves = getPossibleMovesList(board, isWhite);

            foreach(Move move in moves)
            {
                board.makeMove(move);

                nodes += PerftF(board, depth - 1, !isWhite);

                board.undoMove(move);
            }

            return nodes;
        }

        public static int PerftFWithNotation(Board board, int depth, bool isWhite = true)
        {
            Board tempBoard = (Board) board.Clone();

            int nodes = 0;

            if (depth == 0)
                return 1;

            List<Move> moves = getPossibleMovesList(tempBoard, isWhite);

            foreach (Move move in moves)
            {
                tempBoard.makeMove(move);

                nodes += PerftF(tempBoard, depth - 1, !isWhite);

                Console.WriteLine("MOVE: " + board.toString());

                tempBoard.undoMove(move);

                Console.WriteLine("UNDO: " + tempBoard.toString());
            }

            return nodes;
        }


        // Checking

        private static Boolean isChecked(Boolean isWhite, Board board)
        {
            Piece kingPiece = isWhite ? Piece.WhiteKing : Piece.BlackKing;
            byte kingIndex = 0;

            for (byte i = 0; i < board.getState().Count(); i++)
            {
                if (board.getState()[i] == kingPiece)
                    kingIndex = i;
            }

            List<Move> legalMovesOppisite = getPossibleMovesListWithoutCheck(board, !isWhite);

            foreach (Move move in legalMovesOppisite)
                if (move.toIndex == kingIndex)
                    return true;

            return false;
        }
    }
}
