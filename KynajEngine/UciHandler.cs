using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KynajEngine
{
    static class UciHandler
    {
        private static Board board = new(true);

        public static void handle(string[] tokens)
        {
            string command = tokens[0];

            string response = "";

            switch(command)
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
                    board = updatePosition(tokens);
                    break;
                case "go":
                    List<Move> moves = Perft.getPossibleMovesList(board, board.isWhiteMove);
                    Random random = new();

                    Console.WriteLine(board.toString());

                    Move randomMove = moves[random.Next(moves.Count() - 1)];

                    string move = randomMove.getUciNotation();

                    response = $"bestmove {move} - from: {randomMove.fromIndex} - to: {randomMove.toIndex} ";
                    break;

                case "fen":
                    board = new(tokens[1] + " " + tokens[2] + " " + tokens[3] + " " + tokens[4] + " " + tokens[5]);

                    response = board.toString();

                    break;
                case "play":
                    board = updatePosition(tokens);

                    break;


                case "perft":
                    response = getPerft(tokens).ToString();

                    break;
                default:
                    response = $"{command} is not a command";
                    break;
            }

            Console.WriteLine(response);
        }

        private static Board updatePosition(string[] tokens)
        {
            List<String> moves = new();

            if (tokens[1] == "startpos")
            {
                Board board = new(true);

                for (int i = 3; i < tokens.Count(); i++)
                {
                    moves.Add(tokens[i]);
                }

                return updateBoard(board, moves);
            }

            if (tokens[1] == "move")
            {
                moves.Add(tokens[2]);

                return updateBoard(board, moves);
            }

            return new(true);
        }

        private static Board updateBoard(Board board, List<String> moves)
        {
            board.isWhiteMove = moves.Count() % 2 == 0;

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

                board.makeMove(moveToPlay);
            }

            return board;
        }

        private static int getPerft(string[] tokens)
        {
            int depth = Int32.Parse(tokens[1]);

            return Perft.handle(board, depth);
        }
    }
}
