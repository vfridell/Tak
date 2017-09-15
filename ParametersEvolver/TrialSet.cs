using System;
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
        public List<Game> Games { get; set; }

        public void LoadFiles()
        {
            string directory = "./TrainingGames/";

            IEnumerable<string> files = Directory.EnumerateFiles(directory, "*.txt", SearchOption.TopDirectoryOnly);

            Games = new List<Game>();
            foreach (string file in files)
            {
                try
                {
                    Games.Add(Game.CreateGameFromTranscript(file));
                    Console.WriteLine($"Loaded file {file}\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading file {file}: {ex.Message}\n");
                }
            }
        }
    }
}
