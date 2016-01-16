using FitnessCalculator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Optimization
{
    internal class BeesAlgorithm
    {
        public static int ScoutsAmount;
        private static int nodesAmount;
        private static List<Scout> scouts;
        private static int processorsMaxAmount;
        private static int elisteSolutionsAmount;
        private static int bestSolutionsAmount;
        private static bool cancelPending;
        public static bool Running { get; private set; }
        public static Tuple<int[], double, double[]> BestSolution { get; private set; }

        public static IFitnessCalculator FitnessCalculator { get; set; }

        public BeesAlgorithm()
        {
            cancelPending = false;
            // Tworzymy skautów
            scouts = Utilities.createScouts(ScoutsAmount);
            // Wysy³amy skautow na losowe poszukiwania kwiatów 
            Utilities.sendScouts(scouts);
            // Sortujemy skautów pod wzglêdem atrakcyjnoœci kwwiatów, które znaleŸli
            scouts.Sort();

            Running = true;
            while (!isStopConditionFulfilled())
            {
                var bestFlower = scouts.First().getFlower();
                BestSolution = Tuple.Create((int[])bestFlower.getSolution().Clone(), bestFlower.getSolutionValue(), bestFlower.Kir);
                //wybieramy elitarnych Skautów - s¹ to Skauci z elitarnymi kwiatami 
                List<Scout> scoutsWithEliteFlowers = scouts.GetRange(0, elisteSolutionsAmount);
                //wybieramy najlpeszych Skautów - s¹ to Skauci z najlepszymi kwiatami 
                List<Scout> scoutsWithBestFlowers = scouts.GetRange(elisteSolutionsAmount, bestSolutionsAmount);
                //grupujemy pozosta³ych skautów 
                List<Scout> restOfScouts = scouts.GetRange(bestSolutionsAmount, ScoutsAmount - bestSolutionsAmount - elisteSolutionsAmount);
                // Intensywnie przeszukujemy s¹siedztwo elitarnych kwiatow 
                Utilities.searchEliteFlowersNeigberhood(scoutsWithEliteFlowers);
                // Mniej intensywnie przeszukujemy s¹siedztwo najlepszych kwiatów 
                Utilities.searchBestFlowersNeigberhood(scoutsWithBestFlowers);
                // Dla najgorszych kwiatów generujemy nowe
                Utilities.sendScouts(restOfScouts);
                //Sortujemy wszystkie kwiaty pod wzglêdem atrakcyjnoœci 
                scouts.Sort();
            }
            Running = false;
        }

        public static void Cancel()
        {
            cancelPending = true;
        }

        private static bool isStopConditionFulfilled()
        {
            //TODO wzi¹Ÿæ posortowany wektor skautów, wzi¹Ÿæ najlepszego skauta i na jego podstawie sprawdziæ czy warunek jest spe³niony
            // zwracamy false - na cele testów
            return cancelPending;
        }

        public static int getNodesAmount()
        {
            return nodesAmount;
        }

        public static void setNodesAmount(int nodesAmount)
        {
            BeesAlgorithm.nodesAmount = nodesAmount;
        }

        public static List<Scout> getScouts()
        {
            return scouts;
        }

        public static void setScouts(List<Scout> scouts)
        {
            BeesAlgorithm.scouts = scouts;
        }

        public static int getProcessorsMaxAmount()
        {
            return processorsMaxAmount;
        }

        public static void setProcessorsMaxAmount(int processorsMaxAmount)
        {
            BeesAlgorithm.processorsMaxAmount = processorsMaxAmount;
        }

        public static int getElisteSolutionsAmount()
        {
            return elisteSolutionsAmount;
        }

        public static void setElisteSolutionsAmount(int elisteSolutionsAmount)
        {
            BeesAlgorithm.elisteSolutionsAmount = elisteSolutionsAmount;
        }

        public static int getBestSolutionsAmount()
        {
            return bestSolutionsAmount;
        }

        public static void setBestSolutionsAmount(int bestSolutionsAmount)
        {
            BeesAlgorithm.bestSolutionsAmount = bestSolutionsAmount;
        }
    }
}