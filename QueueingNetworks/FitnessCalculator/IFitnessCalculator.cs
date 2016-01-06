﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCalculator
{
    public interface IFitnessCalculator
    {
        double CalculateFitness(int[] solution);
    }

    public class FitnessCalculatorStub : IFitnessCalculator
    {
        public double CalculateFitness(int[] solution)
        {
            return solution.Sum();
        }
    }
}
