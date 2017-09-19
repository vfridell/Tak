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

            TrialSet trialSet = new TrialSet();
            trialSet.LoadFiles();
            int i = 0;
            foreach (Trial trial in trialSet.GetTrials())
            {
                Console.WriteLine($"{trial.PriorMove} -> {trial.CorrectMove}");
                i++;
            }
        }


    }
}
