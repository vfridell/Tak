﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakLib;

namespace ParametersEvolver
{
    public class TrialSet
    {
        public Dictionary<string, Game> Games { get; set; }
        public int NumberOfTrialsRun { get; protected set; }
        public int NumberOfCorrectPicks { get; protected set; }
        public int BoardSize { get; protected set; }

        public void LoadFiles(int boardSize)
        {
            BoardSize = boardSize;
            string directory = "./TrainingGames/";

            IEnumerable<string> files = Directory.EnumerateFiles(directory, "*.txt", SearchOption.TopDirectoryOnly);

            Games = new Dictionary<string, Game>();
            int i = 0;
            foreach (string file in files)
            {
                try
                {
                    Game game = Game.CreateGameFromTranscript(file);
                    Console.WriteLine($"Loaded file {file}");
                    if (game.Boards[0].Size == BoardSize)
                    {
                        Games.Add(file, game);
                        i++;
                    }
                    else
                    {
                        Console.WriteLine(
                            $"Skipping file {file} due to board size mismatch.  Expected {BoardSize} saw {game.Boards[0].Size}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading file {file}: {ex.Message}");
                }
            }
            Console.WriteLine($"Loaded {i} files");
        }

        public IEnumerable<Trial> GetTrials()
        {
            if(null == Games || !Games.Any()) throw new Exception("No games!");

            foreach (var kvp in Games)
            {
                Game game = kvp.Value;
                if(!game.WinnerColor.HasValue) continue;
                bool WhiteTrial = game.WinnerColor.Value == PieceColor.White;
                for(int i=0; i<game.Boards.Count - 1; i++)
                {
                    if (i == 0)
                    {
                        if(WhiteTrial)
                            yield return new Trial(kvp.Key, game.GetInitialBoard(), null, game.movesMade[0]);
                    }
                    else if (i == 1)
                    {
                        if(!WhiteTrial)
                            yield return new Trial(kvp.Key, game.Boards[0], game.movesMade[0], game.movesMade[1]);
                    }
                    else
                    {
                        if (game.Boards[i-1].WhiteToPlay == WhiteTrial)
                        {
                            yield return new Trial(kvp.Key, game.Boards[i-1], game.movesMade[i-1], game.movesMade[i]);
                        }
                    }
                }
            }
        }

        public double RunTrials(ITakAI AI)
        {
            NumberOfTrialsRun = 0;
            NumberOfCorrectPicks = 0;
            foreach (Trial trial in GetTrials())
            {
                //Console.WriteLine($"{trial.PriorMove} -> {trial.CorrectMove}");
                NumberOfTrialsRun++;

                Move movePicked = AI.PickBestMove(trial.Board);
                Board futureBoard = Board.ComputeFutureBoard(trial.Board, movePicked);
                Board correctFutureBoard = Board.ComputeFutureBoard(trial.Board, trial.CorrectMove);
                if (movePicked.ToString() == trial.CorrectMove.ToString() ||
                    Board.IsCongruent(futureBoard, correctFutureBoard)
                )
                {
                    NumberOfCorrectPicks++;
                }
            }

            double result = (double) NumberOfCorrectPicks / NumberOfTrialsRun;
            return result;
        }
    }
}
