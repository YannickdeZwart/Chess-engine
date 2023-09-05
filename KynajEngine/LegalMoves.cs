using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KynajEngine
{
    static class LegalMoves
    {
        private static byte[] AFile = new byte[8]{0, 1, 2, 3, 4, 5, 6, 7};
        private static byte[] BFile = new byte[8]{8, 9, 10, 11, 12, 13, 14, 15};
        private static byte[] CFile = new byte[8]{16, 17, 18, 19, 20, 21, 22, 23};
        private static byte[] DFile = new byte[8]{24, 25, 26, 27, 28, 29, 30, 31};
        private static byte[] EFile = new byte[8]{32, 33, 34, 35, 36, 37, 38, 39};
        private static byte[] FFile = new byte[8]{40, 41, 42, 43, 44, 45, 46, 47};
        private static byte[] GFile = new byte[8]{48, 49, 50, 51, 52, 53, 54, 55};
        private static byte[] HFile = new byte[8]{56, 57, 58, 59, 60, 61, 62, 63};

        private static byte[] RowOne = new byte[8] { 0, 8, 16, 24, 32, 40, 48, 56 };
        private static byte[] RowTwo = new byte[8] { 1, 9, 17, 25, 33, 41, 49, 57 };
        private static byte[] RowThree = new byte[8] { 2, 10, 18, 26, 34, 42, 50, 58 };
        private static byte[] RowFour = new byte[8] { 3, 11, 19, 27, 35, 43, 51, 59 };
        private static byte[] RowFive = new byte[8] { 4, 12, 20, 28, 36, 44, 52, 60 };
        private static byte[] RowSix = new byte[8] { 5, 13, 21, 29, 37, 45, 53, 61 };
        private static byte[] RowSeven = new byte[8] { 6, 14, 22, 30, 38, 46, 54, 62 };
        private static byte[] RowEight = new byte[8] { 7, 15, 23, 31, 39, 47, 55, 63 };

        private static int[] KnightMoves = new int[8] { -17, -15, -10, -6, 6, 10, 15, 17 };
        private static int[] BishopMoves = new int[4] { -9, -7, 7, 9 };
        private static int[] RookMoves = new int[4] { -8, -1, 1, 8 };
        private static int[] KingMoves = new int[8] { -9, -8, -7, -1, 1, 7, 8, 9 };


        public static List<Byte> getMoves(Board board, byte index)
        {
            Piece piece = board.getindexPiece(index);

            switch (piece)
            {
                case Piece.BlackPawn:
                    return getMovesPawn(board, index, false);
                case Piece.WhitePawn:
                    return getMovesPawn(board, index, true);

                case Piece.BlackKnight:
                    return getMovesKnight(board, index, false);
                case Piece.WhiteKnight:
                    return getMovesKnight(board, index, true);

                case Piece.WhiteBishop:
                    return getMovesBishop(board, index, true);
                case Piece.BlackBishop:
                    return getMovesBishop(board, index, false);

                case Piece.WhiteRook:
                    return getMovesRook(board, index, true);
                case Piece.BlackRook:
                    return getMovesRook(board, index, false);

                case Piece.WhiteQueen:
                    return getMovesQueen(board, index, true);
                case Piece.BlackQueen:
                    return getMovesQueen(board, index, true);

                case Piece.WhiteKing:
                    return getMovesKing(board, index, true);
                case Piece.BlackKing:
                    return getMovesKing(board, index, false);
                default:
                    throw new Exception();
            }
        }

        //
        // Pawn generator moves
        // TODO: add en-passent + promotion
        //
        private static List<Byte> getMovesPawn(Board board, byte index, Boolean isWhite)
        {
            List<Byte> moves = new List<byte>();

            byte nextSquareIndex = (byte) ( isWhite ? index - 8 : index + 8 );
            byte doubleNextSquareIndex = (byte)(isWhite ? index - 16 : index + 16);

            bool nextSquareEmpty = board.squareEmpty(nextSquareIndex);
            bool doubleNextSquareEmpty = board.squareEmpty(doubleNextSquareIndex);

            if (nextSquareEmpty)
            {
                // Pawn 1 square forward
                moves.Add(nextSquareIndex);

                if (doubleNextSquareEmpty && pawnFirstMove(index, isWhite))
                {
                    // Pawn 2 squares forward
                    moves.Add(doubleNextSquareIndex);
                }
            }

            // calculate the index depending on the color
            byte leftDiagnalIndex = (byte) ( isWhite ? index - 9 : index + 7 );
            byte rightDiagnalIndex = (byte) ( isWhite ? index - 7 : index + 9 );


            // checks if the piece is on row 1 and therefor can not move left
            if (!RowOne.Contains(leftDiagnalIndex))
            {
                // checks if there is an enemy piece on the left diagnal
                if (board.squareIsEnemy(leftDiagnalIndex, isWhite))
                {
                    moves.Add(leftDiagnalIndex);
                }
            }

            // checks if the piece is on row 8 and therefor can not move right
            if (!RowEight.Contains(rightDiagnalIndex))
            {
                // checks if there is an enemy piece on the right diagnal
                if (board.squareIsEnemy(rightDiagnalIndex, isWhite))
                {
                    moves.Add(rightDiagnalIndex);
                }
            }

            return moves;
        }

        private static Boolean pawnFirstMove(byte index, Boolean isWhite)
        {
            if(isWhite)
            {
                return index >= 48 && index <= 55;
            } else
            {
                return index >= 8 && index <= 15;
            }
        }

        //
        // Knight generator moves
        //

        private static List<Byte> getMovesKnight(Board board, byte index, Boolean isWhite)
        {
            List<Byte> moves = new List<byte>();
            int[] moveindexes = KnightMoves;

            Boolean firstRowException = RowOne.Contains(index);
            Boolean secondRowException = RowTwo.Contains(index);
            Boolean secondLastRowException = RowSeven.Contains(index);
            Boolean lastRowException = RowEight.Contains(index);

            foreach(int moveindex in moveindexes)
            {
                // first row exception
                if(firstRowException)
                {
                    if(moveindex == -17 || moveindex == -10 || moveindex == 6 || moveindex == 15)
                    {
                        continue;
                    }
                }

                // second row exception
                if (secondRowException)
                {
                    if (moveindex == -10 || moveindex == 6)
                    {
                        continue;
                    }
                }

                // second last row exception
                if (secondLastRowException)
                {
                    if (moveindex == 10 || moveindex == -6)
                    {
                        continue;
                    }
                }

                // last row exception
                if (lastRowException)
                {
                    if (moveindex == 17 || moveindex == 10 || moveindex == -6 || moveindex == -15)
                    {
                        continue;
                    }
                }

                int finalindex = index + moveindex;

                // check if move is out of the board so illegal
                if (OutOfBound((byte) finalindex))
                    continue;

                if(finalindex > 64 || finalindex < 0)
                {
                    continue;
                }

                // check if move is on a friendly piece so illegal
                if (!board.squareEmpty((byte)finalindex))
                    if (!board.squareIsEnemy((byte) finalindex, isWhite))  
                        continue;

                moves.Add((byte) finalindex);
            }

            return moves;
        }
        private static List<Byte> getMovesBishop(Board board, byte index, Boolean isWhite)
        {
            List<Byte> moves = new List<byte>();
            int[] moveIndexes = BishopMoves;

            foreach(int moveIndex in moveIndexes)
            {
                for(int i = 1; i < 10; i++)
                {
                    int newIndexFull = index + (moveIndex * i);

                    // Checks if position is out of bounds for the board
                    if (OutOfBound(newIndexFull))
                        break;

                    byte newIndex = (byte) newIndexFull;

                    if (board.squareEmpty(newIndex))
                    {
                        moves.Add(newIndex);

                        // Checks if direction reached a corner and stops the search
                        if (RowOne.Contains(newIndex))
                            if(moveIndex == -9 || moveIndex == 7)
                                break;

                        if (RowEight.Contains(newIndex))
                            if (moveIndex == 9 || moveIndex == -7)
                                break;

                        continue;
                    }



                    // breaks current direction search stop because blocked by enemy piece
                    if (board.squareIsEnemy(newIndex, isWhite))
                    {
                        moves.Add(newIndex);
                        break;
                    }


                    // breaks current direction search stop because blocked by friendly piece
                    if (!board.squareIsEnemy(newIndex, isWhite))
                        break;
                }
            }

            return moves;
        }

        private static List<Byte> getMovesRook(Board board, byte index, Boolean isWhite)
        {
            List<Byte> moves = new List<byte>();
            int[] moveIndexes = RookMoves;

            foreach (int moveIndex in moveIndexes)
            {
                for (int i = 1; i < 10; i++)
                {
                    int newIndexFull = index + (moveIndex * i);

                    // Checks if position is out of bounds for the board
                    if (OutOfBound(newIndexFull))
                        break;

                    byte newIndex = (byte)newIndexFull;

                    if (board.squareEmpty(newIndex))
                    {
                        moves.Add(newIndex);

                        // Checks if direction reached a corner and stops the search
                        if (RowOne.Contains(newIndex))
                            if (moveIndex == -1)
                                break;

                        if (RowEight.Contains(newIndex))
                            if (moveIndex == 1)
                                break;

                        continue;
                    }



                    // breaks current direction search stop because blocked by enemy piece
                    if (board.squareIsEnemy(newIndex, isWhite))
                    {
                        moves.Add(newIndex);
                        break;
                    }


                    // breaks current direction search stop because blocked by friendly piece
                    if (!board.squareIsEnemy(newIndex, isWhite))
                        break;
                }
            }

            return moves;
        }

        private static List<Byte> getMovesQueen(Board board, byte index, Boolean isWhite)
        {
            List<Byte> moves = new List<byte>();

            foreach (byte move in getMovesBishop(board, index, isWhite))
            {
                moves.Add(move);
            }

            foreach (byte move in getMovesRook(board, index, isWhite))
            {
                moves.Add(move);
            }

            return moves;
        }

        private static List<Byte> getMovesKing(Board board, byte index, Boolean isWhite)
        {
            List<Byte> moves = new List<byte>();
            int[] moveIndexes = KingMoves;

            foreach (int moveIndex in moveIndexes)
            {
                int newIndexFull = index + moveIndex;

                if (OutOfBound(newIndexFull))
                    continue;

                byte newIndex = (byte)newIndexFull;

                // Checks if direction reached a corner and stops the search
                if (RowOne.Contains(newIndex))
                    if (moveIndex == -9 || moveIndex == -1 || moveIndex == 7)
                        continue;

                if (RowEight.Contains(newIndex))
                    if (moveIndex == 9 || moveIndex == 1 || moveIndex == -7)
                        continue;

                if (board.squareEmpty(newIndex))
                {
                    moves.Add(newIndex);
                    continue;
                }

                if (!board.squareIsEnemy(newIndex, isWhite))
                {
                    continue;
                }

                moves.Add(newIndex);
            }

            return moves;
        }

        private static Boolean OutOfBound(int index)
        {
            return index > 63 || index < 0;
        }
    }
}
