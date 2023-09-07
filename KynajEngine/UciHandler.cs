using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KynajEngine
{
    public class UciHandler
    {
        private Board board;

        public void handle(string[] tokens)
        {
            string command = tokens[0];

            string response = "";

            List<Move> bestMoves;
            Move move;

            Random random = new();

            switch (command)
            {
                case "uci":
                    response = "uciok";
                    break;
                case "isready":
                    response = "readyok";
                    break;
                case "ucinewgame":
                    response = "isready";
                    break;
                case "position":
                    updatePosition(tokens);
                    break;
                case "go":
                    bestMoves = BestMove.getBestMove(board, 5);
                    move = bestMoves[random.Next(0, bestMoves.Count() - 1)];

                    response = "bestmove " + Notation.getAlgebraicNotation(move);
                    break;
                case "fen":
                    board = new(tokens[1] + " " + tokens[2] + " " + tokens[3] + " " + tokens[4] + " " + tokens[5]);

                    response = board.toString();

                    break;
                case "play":
                    updatePosition(tokens);

                    break;
                case "bestmove":
                    bestMoves = BestMove.getBestMove(board, 5);
                    move = bestMoves[random.Next(0, bestMoves.Count() - 1)];

                    response = Notation.getAlgebraicNotation(move);
                    break;
                case "perft":
                    this.board = new(true);

                    response = getPerft(tokens).ToString();

                    break;
                default:
                    response = $"{command} is not a command";
                    break;
            }

            Console.WriteLine(response);
        }

        private void updatePosition(string[] tokens)
        {
            List<String> moves = new();
            this.board = new(true);

            if (tokens[1] == "startpos")
            {

                for (int i = 3; i < tokens.Count(); i++)
                {
                    moves.Add(tokens[i]);
                }

                this.updateBoard(moves);
            }

            if (tokens[1] == "move")
            {
                moves.Add(tokens[2]);

                this.updateBoard(moves);
            }

            if (tokens[1] == "fen")
            {
                this.board = new(tokens[1] + " " + tokens[2] + " " + tokens[3] + " " + tokens[4] + " " + tokens[5]);

            }
        }

        private void updateBoard(List<String> moves)
        {
            for(int i = 0; i < moves.Count(); i++)
            {
                string move = moves[i];
                char letterFrom = move[0];
                char numberFrom = move[1];
                char letterTo = move[2];
                char numberTo = move[3];
                Piece promotion = Piece.None;

                bool isWhite = i % 2 == 0;

                if (move.Length > 4)
                {
                    promotion = Notation.getPromotionPiece(move[4], isWhite);
                }
                //promotion

                byte indexFrom = (byte) Notation.AlgebraicToIndex(letterFrom, numberFrom);
                byte indexTo = (byte)Notation.AlgebraicToIndex(letterTo, numberTo);

                Move moveToPlay = new(indexFrom, indexTo, promotion);

                this.board.makeMove(moveToPlay);
            }
        }

        private int getPerft(string[] tokens)
        {
            int depth = Int32.Parse(tokens[1]);

            return Perft.handle(board, depth);
        }
    }
}
