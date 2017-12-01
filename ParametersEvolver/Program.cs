using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using GeneticSharp.Infrastructure.Framework.Collections;
using TakLib;
using TakLib.AI;

namespace ParametersEvolver
{
    class Program
    {
        static void Main(string[] args)
        {
            //EvolveWeights();

            TrialSet trialSet = new TrialSet();
            trialSet.LoadFiles(5);
            //IBoardAnalyzer analyzer = new BoardStacksAnalyzer(trialSet.BoardSize, BoardAnalysisWeights.bestStackWeights);
            MaximumRatioAnalyzer analyzer = new MaximumRatioAnalyzer(trialSet.BoardSize);
            foreach (Trial trial in trialSet.GetTrials())
            {
                IDictionary<double, HashSet<NegamaxContext>> moveDictionary = MoveSorter.GetSortedAnalysisDictionary(trial.Board, analyzer);
                IDictionary<double, HashSet<AnalysisFactors>> analysisDictionary = MoveSorter.GetSortedAnalysisFactorsDictionary(trial.Board, analyzer);
                int uniqueScores = moveDictionary.Values.Count;
                int totalMoves = moveDictionary.Keys.Select(d => moveDictionary[d].Count).Sum();
                double ratio = (double)totalMoves / uniqueScores;
                if(ratio > 1)
                {
                    Console.WriteLine($"{trial.GameName}: Ratio: {totalMoves}/{uniqueScores} = {ratio:N} ({totalMoves} moves grouped into {uniqueScores} analysis scores)");
                }
            }
        }


        static void EvolveWeights()
        {
            Stopwatch stopwatch = new Stopwatch();
            TrialSet trialSet = new TrialSet();
            trialSet.LoadFiles(5);
            ITakAI AI = new OptimusDeep(1, new BoardStacksAnalyzer(trialSet.BoardSize, BoardAnalysisWeights.bestStackWeights));

            stopwatch.Start();
            trialSet.RunTrials(AI);
            stopwatch.Stop();
            Console.WriteLine($"Single RunTrials takes {stopwatch.Elapsed}");

            var selection = new RouletteWheelSelection();
            var crossover = new TwoPointCrossover();
            var mutation = new UniformMutation();
            var fitness = new WeightsFitness(trialSet, AI);
            var chromosome = new WeightsChromosome();
            var population = new Population(5, 10, chromosome);

            var ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation);
            ga.Termination = new GenerationNumberTermination(100);
            ga.GenerationRan += delegate
            {
                WeightsChromosome bestChromosome = (WeightsChromosome)ga.Population.BestChromosome;
                Console.WriteLine("Generations: {0}", ga.Population.GenerationsNumber);
                Console.WriteLine("Fitness: {0,10}", bestChromosome.Fitness);
                Console.WriteLine("Time: {0}", stopwatch.Elapsed);
                Console.WriteLine(bestChromosome.GetWeights().ToString());
            };

            Console.WriteLine("GA running...");
            stopwatch.Restart();
            ga.Start();
            stopwatch.Stop();

            Console.WriteLine("Best solution found has {0} fitness.", ga.BestChromosome.Fitness);
            Console.WriteLine(((WeightsChromosome)ga.BestChromosome).GetWeights().ToString());
        }

    }
}
