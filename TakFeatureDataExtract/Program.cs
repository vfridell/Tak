using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakLib;

namespace TakFeatureDataExtract
{
    class Program
    {
        static void Main(string[] args)
        {
            string directory = "./TrainingGames/";
            List<Game> games = LoadFiles(5, directory);


        }

        public static List<Game> LoadFiles(int boardSize, string directory)
        {
            IEnumerable<string> files = Directory.EnumerateFiles(directory, "*.txt", SearchOption.TopDirectoryOnly);

            var games = new List<Game>();
            int i = 0;
            foreach (string file in files)
            {
                try
                {
                    Game game = Game.CreateGameFromTranscript(file);
                    Console.WriteLine($"Loaded file {file}");
                    if (game.Boards[0].Size == boardSize)
                    {
                        games.Add(game);
                        i++;
                    }
                    else
                    {
                        Console.WriteLine(
                            $"Skipping file {file} due to board size mismatch.  Expected {boardSize} saw {game.Boards[0].Size}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading file {file}: {ex.Message}");
                }
            }
            Console.WriteLine($"Loaded {i} files");
            return games;
        }
    }
}
