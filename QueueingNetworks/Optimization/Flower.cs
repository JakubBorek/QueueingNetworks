using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimization
{
    class Flower : IComparable<Flower>
    {
        private int processorsMaxAmount = BeesAlgorithm.getProcessorsMaxAmount();
        private int nodeAmounts = BeesAlgorithm.getNodesAmount();
        private int[] solution;
        private double solutionValue;

        public Flower()
        {
            this.solution = new int[nodeAmounts];
        }

        public void choseFlower()
        {
            Random rn = new Random();
            for (int i = 0; i < nodeAmounts; i++)
            {
                solution[i] = rn.Next() % processorsMaxAmount;
            }
            computeSolutionValue();
        }

        public Flower choseFlowerNeighbor()
        {
            //TODO zaimplementowaæ to
            Flower flower = new Flower();
            flower.computeSolutionValue();
            return flower;
        }

        private void computeSolutionValue()
        {
            var fitness = BeesAlgorithm.FitnessCalculator.CalculateFitness(solution);
            setSolutionValue(fitness);
        }

        public double getSolutionValue()
        {
            return solutionValue;
        }

        public int[] getSolution()
        {
            return solution;
        }

        public void setSolutionValue(double solutionValue)
        {
            this.solutionValue = solutionValue;
        }

        public int CompareTo(Flower other)
        {
            return getSolutionValue().CompareTo(other.getSolutionValue());
        }
    }
}
