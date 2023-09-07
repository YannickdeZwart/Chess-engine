using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KynajEngine
{
    public static class Eval
    {
        public static int getEval(Board board, bool isWhite)
        {
            int eval = 0;
            foreach(Piece piece in board.getState())
            {
                if(piece != Piece.None)
                    eval += getPieceScore(piece, isWhite);
            }

            return eval;
        }

        private static int getPieceScore(Piece piece, bool isWhite)
        {
            if(isWhite)
                switch(piece)
                {
                    case Piece.WhitePawn: return 10;
                    case Piece.WhiteKnight: return 30;
                    case Piece.WhiteBishop: return 30;
                    case Piece.WhiteRook: return 50;
                    case Piece.WhiteQueen: return 90;
                    case Piece.WhiteKing: return 900;
                    
                    case Piece.BlackPawn: return -10;
                    case Piece.BlackKnight: return -30;
                    case Piece.BlackBishop: return -30;
                    case Piece.BlackRook: return -50;
                    case Piece.BlackQueen: return -90;
                    case Piece.BlackKing: return -900;
                }
            else
                switch (piece)
                {
                    case Piece.WhitePawn: return -10;
                    case Piece.WhiteKnight: return -30;
                    case Piece.WhiteBishop: return -30;
                    case Piece.WhiteRook: return -50;
                    case Piece.WhiteQueen: return -90;
                    case Piece.WhiteKing: return -900;

                    case Piece.BlackPawn: return 10;
                    case Piece.BlackKnight: return 30;
                    case Piece.BlackBishop: return 30;
                    case Piece.BlackRook: return 50;
                    case Piece.BlackQueen: return 90;
                    case Piece.BlackKing: return 900;
                }
            throw new Exception();
        }       
    }
}
