﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KynajEngine
{
    static class UciHandler
    {
        private static Board board = new();

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
                    List<Move> moves = Perft.getPossibleMovesList(board, true);
                    Random random = new();

                    Move randomMove = moves[random.Next(moves.Count() - 1)];

                    string move = randomMove.getUciNotation();

                    response = $"bestmove {move} - from: {randomMove.fromIndex} - to: {randomMove.toIndex} ";
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
            Board board = new();

            if (tokens[1] == "startpos")
            {
                for (int i = 3; i < tokens.Count(); i++)
                {
                    moves.Add(tokens[i]);
                }

                return updateBoard(board, moves);
            }

            return new Board();
        }

        private static Board updateBoard(Board board, List<String> moves)
        {
            for(int i = 0; i < moves.Count(); i++)
            {
                string move = moves[i];

                bool isWhite = i % 2 == 0;

                char letterFrom = move[0];
                char numberFrom = move[1];
                char letterTo = move[2];
                char numberTo = move[3];
                Piece promotion = Piece.None;

                if (move.Length > 4)
                {
                    promotion = Notation.getPromotionPiece(move[4], isWhite);
                }
                //promotion

                byte indexFrom = (byte) Notation.AlgebraicToIndex(letterFrom, numberFrom);
                byte indexTo = (byte)Notation.AlgebraicToIndex(letterTo, numberTo);

                Move moveToPlay = new(indexFrom, indexTo, promotion);

                board.makeMove(moveToPlay);

                Console.WriteLine(board.toString());
            }

            return board;
        }
    }
}
