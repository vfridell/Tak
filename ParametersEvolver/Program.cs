using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakLib;

namespace ParametersEvolver
{
    class Program
    {
        static void Main(string[] args)
        {
            BoardAnalysisWeights startingWeights = BoardAnalysisWeights.bestStackWeights;


            var games = LoadFiles();
            foreach (Game game in games)
            {
                //BoardAnalyzer analyzer = new BoardAnalyzer(game.CurrentBoard.Size, BoardAnalysisWeights.bestWeights);
                //_boardsToAnalyze.AddRange(game.Boards.Select(b => new Tuple<BoardAnalysisData, Board>((BoardAnalysisData)analyzer.Analyze(b), b)));
            }
        }

        static List<Game> LoadFiles()
        {
            string directory = "./TrainingGames/";

            IEnumerable<string> files = Directory.EnumerateFiles(directory, "*.txt", SearchOption.TopDirectoryOnly);

            List<Game> games = new List<Game>();
            foreach (string file in files)
            {
                try
                {
                    games.Add(Game.CreateGameFromTranscript(file));
                    Console.WriteLine($"Loaded file {file}\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading file {file}: {ex.Message}\n");
                }
            }

            return games;

        }
    }
}
