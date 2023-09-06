using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KynajEngine
{
    static class Notation
    {
        public static char[] letters = new char[8] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };
        public static char[] numbers = new char[8] { '8', '7', '6', '5', '4', '3', '2', '1' };
        public static string pieceNotation(Piece piece)
        {
            switch (piece)
            {
                case Piece.None:
                    return " ";

                // black pieces
                case Piece.BlackPawn:
                    return "p";
                case Piece.BlackKnight:
                    return "n";
                case Piece.BlackBishop:
                    return "b";
                case Piece.BlackRook:
                    return "r";
                case Piece.BlackQueen:
                    return "q";
                case Piece.BlackKing:
                    return "k";

                // white pieces
                case Piece.WhitePawn:
                    return "P";
                case Piece.WhiteKnight:
                    return "N";
                case Piece.WhiteBishop:
                    return "B";
                case Piece.WhiteRook:
                    return "R";
                case Piece.WhiteQueen:
                    return "Q";
                case Piece.WhiteKing:
                    return "K";

                default:
                    throw new Exception();
            }
        }
      
        public static Piece getPromotionPiece(char piece, bool isWhite)
        {
            switch(piece)
            {
                case 'n':
                    return isWhite ? Piece.WhiteKnight : Piece.BlackKnight;

                case 'b':
                    return isWhite ? Piece.WhiteBishop : Piece.BlackBishop;

                case 'r':
                    return isWhite ? Piece.WhiteRook : Piece.BlackRook;
                case 'q':
                    return isWhite ? Piece.WhiteQueen : Piece.BlackQueen;
                default:
                    throw new Exception();
            }
        }
      
        public static Piece notationToPiece(char piece)
        {
            switch (piece)
            {
                case 'p':
                    return Piece.BlackPawn;
                case 'n':
                    return Piece.BlackKnight;
                case 'b':
                    return Piece.BlackBishop;
                case 'r':
                    return Piece.BlackRook;
                case 'q':
                    return Piece.BlackQueen;
                case 'k':
                    return Piece.BlackKing;

                case 'P':
                    return Piece.WhitePawn;
                case 'N':
                    return Piece.WhiteKnight;
                case 'B':
                    return Piece.WhiteBishop;
                case 'R':
                    return Piece.WhiteRook;
                case 'Q':
                    return Piece.WhiteQueen;
                case 'K':
                    return Piece.WhiteKing;
                default:
                    throw new Exception();
            }
        }

        public static char getPromotionLetter(Piece piece)
        {
            switch(piece)
            {
                case Piece.BlackKnight:
                case Piece.WhiteKnight:
                    return 'n';

                case Piece.BlackBishop:
                case Piece.WhiteBishop:
                    return 'b';

                case Piece.BlackRook:
                case Piece.WhiteRook:
                    return 'r';

                case Piece.BlackQueen:
                case Piece.WhiteQueen:
                    return 'q';

                default:
                    throw new Exception();
            }
        }

        public static string getAlgebraicNotation(Move move)
        {
            string notation = "";

            notation += IndexToAlgebraic(move.fromIndex);
            notation += IndexToAlgebraic(move.toIndex);

            if(move.promotion != Piece.None)
            {
                notation += getPromotionLetter(move.promotion);
            }

            return notation;
        }

        private static string IndexToAlgebraic(byte index)
        {
            // 

            char number = numbers[(index - (index % 8) ) / 8];
            char letter = letters[index % 8];

            return letter.ToString() + number.ToString();
        }

        public static int AlgebraicToIndex(char letter, char number)
        {
            byte letterIndex = 0;

            for (byte i = 0; i < letters.Count(); i++)
            {
                if (letters[i] == letter)
                {
                    letterIndex = i;

                    break;
                }
            }

            for(int i = 0; i < numbers.Count(); i++)
            {
                if(number == numbers[i])
                {
                    return i * 8 + letterIndex;
                }
            }

            throw new Exception();
        }
    }
}
