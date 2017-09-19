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

        public IEnumerable<Trial> GetTrials()
        {
            if(null == Games || !Games.Any()) throw new Exception("No games!");

            foreach (Game game in Games)
            {
                if(!game.WinnerColor.HasValue) continue;
                bool WhiteTrial = game.WinnerColor.Value == PieceColor.White;
                for(int i=0; i<game.Boards.Count - 1; i++)
                {
                    if (i == 0)
                    {
                        if(WhiteTrial)
                            yield return new Trial(game.GetInitialBoard(), null, game.movesMade[0]);
                    }
                    else if (i == 1)
                    {
                        if(!WhiteTrial)
                            yield return new Trial(game.Boards[0], game.movesMade[0], game.movesMade[1]);
                    }
                    else
                    {
                        if (game.Boards[i-1].WhiteToPlay == WhiteTrial)
                        {
                            yield return new Trial(game.Boards[i-1], game.movesMade[i-1], game.movesMade[i]);
                        }
                    }
                }
            }
        }

        
    }
}
