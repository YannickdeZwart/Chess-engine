using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KynajEngine
{
    //    A  B  C  D  E  F  G  H
    // 8  00 01 02 03 04 05 06 07
    // 7  08 09 10 11 12 13 14 15
    // 6  16 17 18 19 20 21 22 23
    // 5  24 25 26 27 28 29 30 31
    // 4  32 33 34 35 36 37 38 39
    // 3  40 41 42 43 44 45 46 47
    // 2  48 49 50 51 52 53 54 55
    // 1  56 57 58 59 60 61 62 63
    //

    public class Board : ICloneable
    {
        private Piece[] state = new Piece[64];

        public Boolean isWhiteMove;

        // for clonability
        public object Clone()
        {
            return this.MemberwiseClone();
        }


        // Setup's a normal chess board
        public Board(Boolean isWhiteMove)
        {
            this.isWhiteMove = isWhiteMove;

            //setup black pawns
            for (int i = 8; i <= 15; i++)
            {
                state[i] = Piece.BlackPawn;
            }

            //setup black rooks
            state[0] = Piece.BlackRook;
            state[7] = Piece.BlackRook;

            //setup black knights
            state[1] = Piece.BlackKnight;
            state[6] = Piece.BlackKnight;

            //setup black bishops
            state[2] = Piece.BlackBishop;
            state[5] = Piece.BlackBishop;

            //setup black king
            state[3] = Piece.BlackQueen;

            //setup black queen
            state[4] = Piece.BlackKing;



            //setup white pawns
            for (int i = 48; i <= 55; i++)
            {
                state[i] = Piece.WhitePawn;
            }

            //setup white rooks
            state[56] = Piece.WhiteRook;
            state[63] = Piece.WhiteRook;

            //setup white knights
            state[57] = Piece.WhiteKnight;
            state[62] = Piece.WhiteKnight;

            //setup white bishops
            state[58] = Piece.WhiteBishop;
            state[61] = Piece.WhiteBishop;

            //setup white queen
            state[59] = Piece.WhiteQueen;

            //setup white king
            state[60] = Piece.WhiteKing;
        }

        public Board(string fen)
        {
            string[] split = fen.Split(' ');

            string boardState = split[0].Replace("/", "");

            string startPlayer = split[1];

            this.isWhiteMove = startPlayer == "w";

            int skipcounter = 0;
            for(int i = 0; i < boardState.Count(); i++)
            {
                char symbol = boardState[i];

                switch(symbol)
                {
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                        int number = (int)Char.GetNumericValue(symbol);
                        skipcounter += number - 1;
                        break;
                    default:
                        state[i + skipcounter] = Notation.notationToPiece(symbol); 
                        break;
                }
            }
        }

        public bool squareEmpty(byte index)
        {
            return this.state[index] == Piece.None;
        }

        public bool squareIsWhite(byte index)
        {
            return (int)this.state[index] < 7;
        }

        public bool squareIsEnemy(byte index, bool isWhite)
        {
            return isWhite ? !this.squareIsWhite(index) : this.squareIsWhite(index);
        }

        public Piece getindexPiece(byte index)
        {
            return this.state[index];
        }

        public Piece[] getState()
        {
            return this.state;
        }

        public void makeMove(Move move)
        {
            // Add capture for undo
            move.capture = this.state[move.toIndex];

            // First whe set the piece of the new square to the piece of the old square
            this.state[move.toIndex] = this.state[move.fromIndex];

            // Now whe reset the old square
            this.state[move.fromIndex] = Piece.None;



            // Check for promotion and add
            if(move.promotion != Piece.None)
            {
                this.state[move.toIndex] = move.promotion;
            }
        }

        public void undoMove(Move move)
        {
            // First whe set the piece of the old square to the piece of the new square
            this.state[move.fromIndex] = this.state[move.toIndex];

            // Now whe reset the new square
            this.state[move.toIndex] = move.capture;

            // Check for promotion and remove
            if (move.promotion != Piece.None)
                if(move.toIndex > 8)
                    this.state[move.fromIndex] = Piece.BlackPawn;
                else
                    this.state[move.fromIndex] = Piece.WhitePawn; 
        }

        public string toString()
        {
            string boardString = "";

            for (int i = 0; i < this.state.Length; i++)
            {
                if (i % 8 == 0)
                    boardString += "\n";

                boardString += Notation.pieceNotation(state[i]);

                boardString += " | ";
            }

            return boardString;
        }
    }
    public class Move
    {
        public byte fromIndex;
        public byte toIndex;
        public Piece promotion;

        public Piece capture = Piece.None;

        public Move(byte fromIndex, byte toIndex, Piece promotion)
        {
            this.fromIndex = fromIndex;
            this.toIndex = toIndex;
            this.promotion = promotion;
        }

        public string getUciNotation()
        {
            return Notation.getAlgebraicNotation(new Move(this.fromIndex, this.toIndex, this.promotion));
        }
    }

    public enum Piece
    {
        None = 0,
        WhitePawn = 1,
        WhiteKnight = 2,
        WhiteBishop = 3,
        WhiteRook = 4,
        WhiteQueen = 5,
        WhiteKing = 6,

        BlackPawn = 7,
        BlackKnight = 8,
        BlackBishop = 9,
        BlackRook = 10,
        BlackQueen = 11,
        BlackKing = 12,
    }
}
