﻿using System;
using System.Text.RegularExpressions;
using TakLib;

namespace PlayTakConsole
{
    class Program
    {
        public enum YesNo { Yes, No };

        static void Main(string[] args)
        {
            int gamesToPlay = 15;
            bool AIWhite = true;
            do
            {
                GameSetup gameSetup = new GameSetup()
                {
                    BoardSize = 5
                };

                //ITakAI AI = new JohnnyDeep(3, new BoardAnalyzer(gameSetup.BoardSize, BoardAnalysisWeights.bestWeights));
                ITakAI AI = new OptimusDeep(4,
                    new BoardStacksAnalyzer(gameSetup.BoardSize, BoardAnalysisWeights.bestStackWeights));
                ITakAI AI2 = new OptimusDeep(4, new MaximumRatioAnalyzer(gameSetup.BoardSize), "Optimus4MaxRatio");
                //ITakAI AI2 = new SimpleJack(4, gameSetup.BoardSize);
                //ITakAI AI2 = new RandomAI();

                YesNo yn = YesNo.Yes;

                Game game;
                if (AIWhite)
                {
                    gameSetup.WhitePlayer = (Player) AI;
                    gameSetup.BlackPlayer = (Player) AI2;
                }
                else
                {
                    gameSetup.WhitePlayer = (Player) AI2;
                    gameSetup.BlackPlayer = (Player) AI;
                }

                game = Game.GetNewGame(gameSetup);

                AI.BeginNewGame(AIWhite, gameSetup.BoardSize);
                AI2.BeginNewGame(!AIWhite, gameSetup.BoardSize);

                do
                {
                    Board currentBoard = game.CurrentBoard;
                    Move move;
                    ITakAI currentAI;
                    if (game.WhiteToPlay == AI.playingWhite)
                    {
                        currentAI = AI;
                    }
                    else
                    {
                        currentAI = AI2;
                    }
                    DateTime beginTimestamp = DateTime.Now;
                    move = currentAI.MakeBestMove(game);
                    TimeSpan timespan = DateTime.Now.Subtract(beginTimestamp);
                    Console.WriteLine($"{timespan.TotalSeconds} seconds {currentAI.Name} Moved: {move}");
                } while (game.GameResult == GameResult.Incomplete);

                Console.WriteLine(GetWinnerString(game));
                string filename = Game.WriteGameTranscript(game);
                Console.WriteLine("Written to " + filename);

                AIWhite = !AIWhite;
            } while (gamesToPlay-- > 0);
        }

        private static string GetWinnerString(Game game)
        {
            switch (game.GameResult)
            {
                case GameResult.Draw:
                    return "Draw";
                case GameResult.Incomplete:
                    return "Draw (negotiated)";
                case GameResult.WhiteRoad:
                case GameResult.WhiteFlat:
                    return $"White ({game.WhitePlayer.Name}) wins! {game.GameResult}";
                case GameResult.BlackRoad:
                case GameResult.BlackFlat:
                    return $"Black ({game.BlackPlayer.Name}) wins! {game.GameResult}";
                default:
                    throw new Exception("Bad game result");
            }
        }

        static YesNo PromptYesOrNo(string prompt)
        {
            Regex yesRegex = new Regex("y|yes", RegexOptions.IgnoreCase);
            Regex noRegex = new Regex("n|no", RegexOptions.IgnoreCase);
            string response;
            do
            {
                Console.WriteLine(prompt);
                response = Console.ReadLine();

            } while (!yesRegex.IsMatch(response) && !noRegex.IsMatch(response));
            if (yesRegex.IsMatch(response))
                return YesNo.Yes;
            else
                return YesNo.No;
        }


        static string PromptForString(string prompt)
        {
            string response;
            do
            {
                Console.WriteLine(prompt);
                response = Console.ReadLine();

            } while (response.Length == 0);
            return response;
        }

        //static Move PromptForMove(string prompt, Game game)
        //{
        //    int turnNumber = game.turnNumber;
        //    Move move;
        //    do
        //    {
        //        Console.WriteLine(prompt);
        //        string response = Console.ReadLine();
        //        if (!Move.TryGetMove(response, out move))
        //        {
        //            Console.WriteLine("Invalid move notation");
        //            continue;
        //        }
        //        if (!game.TryMakeMove(move))
        //        {
        //            Console.WriteLine(string.Format("Invalid move: {0}", game.lastError));
        //            continue;
        //        }
        //    } while (turnNumber == game.turnNumber && game.gameResult == GameResult.Incomplete);
        //    return move;
        //}
    }
}
