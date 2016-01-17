using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCalculator
{
    public class FitnessCalculator : IFitnessCalculator
    {
        QueueingNetworks.Network network;
        public int liczbaWezlow;
        public int liczbaKlas;
        public int K;

        public FitnessCalculator(QueueingNetworks.Network network)
        {
            this.network = network;
            liczbaWezlow = network.Nodes.Count;
            liczbaKlas = network.ClassCount;
            K = 0;

            for (int i=0; i < liczbaKlas; i++)
            {
                K = K + network.ClassMembersCounts[i];
            }
          
        }
        public Tuple<double,double []> CalculateFitness(int[] solution)
        {
            //TO DO: Przeniesc do konstruktora + refactor
            double[,] macierzE = new double[liczbaWezlow * liczbaKlas, liczbaWezlow * liczbaKlas];
            double[] wyrazyWolne = new double[liczbaWezlow * liczbaKlas];
            double[] eIR = new double[liczbaWezlow * liczbaKlas];
            double[] lambdaIR = new double[liczbaWezlow * liczbaKlas];
            double[] rhoIR = new double[liczbaWezlow * liczbaKlas];
            double[] rhoI = new double[liczbaWezlow];
            double[] Pmi = new double[liczbaWezlow];
            double[] kIR = new double[liczbaWezlow * liczbaKlas];
            double[] kR = new double[liczbaKlas];
            double[] fixIR = new double[liczbaWezlow * liczbaKlas];
            double[] fixR = new double[liczbaKlas];
            double[] lambdaR1 = new double[liczbaKlas];
            double[] lambdaR2 = new double[liczbaKlas];
            double[] niezajeteI = new double[liczbaWezlow];
            double[] pi0I = new double[liczbaWezlow];
            double[] srDlKolejkiI = new double[liczbaWezlow];
            double wartBlad;
            double epsilon = 0.00001;

            for(int i = 0; i < liczbaWezlow; i++)
            {
                if(network.Nodes[i].Type == QueueingNetworks.Node.NodeType.Type1)
                {
                    if (solution[i] >= K)
                    {
                        return Tuple.Create(double.PositiveInfinity, kIR);
                    }
                }
                
            }

            for (int i = 0; i < liczbaKlas; i++)
            {
                lambdaR1[i] = 0.00001;
            }

            for (int i = 0; i < liczbaKlas*liczbaWezlow; i++)
            {
                if (i % liczbaWezlow == 0)
                {
                    wyrazyWolne[i] = 1;
                }       
            }


            //STEP1: Wyznaczanie eIR.. tylko raz potrzebne
            macierzE = liczenieMacierzyE();

            var A = Matrix<double>.Build.DenseOfArray(macierzE);
            var b = Vector<double>.Build.Dense(wyrazyWolne);
            var x = A.Solve(b);
            eIR = x.ToArray();

            //STEP2: Parametry (zależne od iteracji i od aktualnego rozwiązania)
            do
            {
                lambdaIR = liczenieLambdaIR(eIR, lambdaR1);
                rhoIR = liczenieRhoIR(solution, lambdaIR);
                rhoI = liczenieRhoI(rhoIR);
                Pmi = liczeniePmi(solution, rhoI);
                kIR = liczenieKIR(solution, rhoIR, rhoI, K, Pmi, lambdaIR); //  tylko debug
                kR = liczenieSumKir(kIR);                         //  tylko debug
                fixIR = liczeniefixIR(eIR, rhoIR, rhoI, K, Pmi, solution);
                fixR = liczenieSumFixR(fixIR);
                lambdaR2 = liczenieLambdaR(fixR, kR);
                wartBlad = blad(lambdaR1, lambdaR2);

                lambdaR1 = lambdaR2;
            } while (wartBlad > epsilon);

            //STEP3: Do funkcji celu
            lambdaIR = liczenieLambdaIR(eIR, lambdaR1);
            rhoIR = liczenieRhoIR(solution, lambdaIR);
            rhoI = liczenieRhoI(rhoIR);
            Pmi = liczeniePmi(solution, rhoI);
            kIR = liczenieKIR(solution, rhoIR, rhoI, K, Pmi, lambdaIR);
            kR = liczenieSumKir(kIR);
            niezajeteI = liczbaNieZajetychKanalowWStacji(rhoI, solution);
            pi0I = prawdopodobodobienstwoPi0I(solution, rhoI);
            srDlKolejkiI = sredniaDlugoscKolejkiI(solution, rhoI, pi0I);

            for(int i = 0; i < liczbaWezlow; i++)
            {
                if (pi0I[i] < 0)
                {
                    return Tuple.Create(double.PositiveInfinity, kIR);
                }       
            }

            return Tuple.Create(funkcjaCelu(srDlKolejkiI, niezajeteI),kIR);
        }

        public double[] GaussaSeidela(double[,] A, double[] b, int n, double eps)
        {
            double tmp1;
            double tmp2;
            double[] x = new double[n];
            double[] x1 = new double[n];
            double sumx, sumx1;
            int count = 0;
            for (int i = 0; i < n; i++)
            {
                x1[i] = b[i] / A[i, i];
            }

            do
            {
                for (int i = 0; i < n; i++)
                {
                    x[i] = x1[i];
                }

                for (int i = 0; i < n; i++)
                {
                    tmp1 = 0;
                    tmp2 = 0;
                    for (int j = 0; j < i; j++)
                    {
                        tmp1 += A[i, j] * x1[j];
                    }
                    for (int j = i + 1; j < n; j++)
                    {
                        tmp2 += A[i, j] * x[j];
                    }
                    x1[i] = (1.0 / A[i, i]) * (b[i] - tmp1 - tmp2);
                }

                sumx = 0;
                sumx1 = 0;
                for (int i = 0; i < n; i++)
                {
                    sumx += x[i];
                    sumx1 += x1[i];
                }

                ++count;
            } while (Math.Abs(sumx - sumx1) > eps);

            return x1;
        }

        public int silnia(int i)
        {
            if (i < 1)
            {
                return 1;
            }
            else
            {
                return i * silnia(i - 1);
            }
        }

        public double[,] liczenieMacierzyE()
        {
            double[,] macierzE = new double[liczbaWezlow * liczbaKlas, liczbaWezlow * liczbaKlas];

            for (int i = 0; i < liczbaWezlow * liczbaKlas; i++)
            {
                for (int k = 0; k < liczbaKlas; k++)
                {
                    for (int j = 0; j < liczbaWezlow; j++)
                    {
                        if (k == (int)i / liczbaWezlow)
                        {
                            if (i == j + k * liczbaWezlow)
                            {
                                if (i % liczbaWezlow == 0)
                                {
                                    macierzE[i, j + k * liczbaWezlow] = network.Nodes[j].GetTransitionProbability(i % liczbaWezlow, k);
                                }
                                else
                                {
                                    macierzE[i, j + k * liczbaWezlow] = network.Nodes[j].GetTransitionProbability(i % liczbaWezlow, k) - 1;
                                }
                            }
                            else
                            {
                                macierzE[i, j + k * liczbaWezlow] = network.Nodes[j].GetTransitionProbability(i % liczbaWezlow, k);
                            }
                        }
                    }
                }
            }
            return macierzE;
        }

        public double[] liczenieLambdaIR(double[] eIR, double[] lamdaR)
        {
            double[] lambdaIR = new double[liczbaWezlow * liczbaKlas];

            for (int i = 0; i < liczbaWezlow * liczbaKlas; i++)
            {
                lambdaIR[i] = eIR[i] * lamdaR[i / liczbaWezlow];
            }

            return lambdaIR;
        }

        public double[] liczenieRhoIR(int[] m, double[] lambdaIR)
        {
            double[] rhoIR = new double[liczbaWezlow * liczbaKlas];

            for (int i = 0; i < liczbaWezlow * liczbaKlas; i++)
            {
                rhoIR[i] = lambdaIR[i] / (network.Nodes[i%liczbaWezlow].Mi[i/liczbaWezlow] * m[i % liczbaWezlow]);
            }

            return rhoIR;
        }

        public double[] liczenieRhoI(double[] rhoIR)
        {
            double[] rhoI = new double[liczbaWezlow];

            for (int i = 0; i < liczbaWezlow * liczbaKlas; i++)
            {
                rhoI[i % liczbaWezlow] = rhoI[i % liczbaWezlow] + rhoIR[i];
            }

            return rhoI;
        }

        public double[] liczeniePmi(int[] m, double[] rho)
        {
            double[] Pmi = new double[liczbaWezlow];

            double tempSum;

            for (int i = 0; i < liczbaWezlow; i++)
            {
                if (network.Nodes[i % liczbaWezlow].Type == QueueingNetworks.Node.NodeType.Type1)
                {
                    tempSum = 0;
                    for (int k = 0; k <= m[i] - 1; k++)
                    {
                        tempSum = tempSum + Math.Pow(m[i] * rho[i], k) / silnia(k);

                    }

                    Pmi[i] = (Math.Pow(rho[i] * m[i], m[i]) / (silnia(m[i]) * (1 - rho[i]))) *
                                (1 / (tempSum + (Math.Pow(m[i] * rho[i], m[i]) / silnia(m[i])) *
                                        (1 / (1 - rho[i]))));
                }
                else
                {
                    Pmi[i] = 0;
                }
                    
            }

            return Pmi;
        }

        public double[] liczenieKIR(int[]m,double[] rhoIR, double[] rho, int K, double[] Pmi, double[] lambdaIR)
        {
            double[] kIR = new double[liczbaWezlow * liczbaKlas];

            for (int i = 0; i < liczbaWezlow * liczbaKlas; i++)
            {
                if(network.Nodes[i % liczbaWezlow].Type == QueueingNetworks.Node.NodeType.Type1)
                {
                    if (m[i % liczbaWezlow] == 1)
                    {
                        kIR[i] = rhoIR[i] / (1 - (((K - 1) / K) * rho[i % liczbaWezlow]));
                    }
                    else
                    {
                        kIR[i] = m[i % liczbaWezlow] * rhoIR[i] +
                                    (rhoIR[i] / (1 - (((K - m[i % liczbaWezlow] - 1) / (K - m[i % liczbaWezlow])) * rho[i % liczbaWezlow]))) * Pmi[i % liczbaWezlow];
                    }
                }
                else
                {
                    kIR[i] = lambdaIR[i] / network.Nodes[i % liczbaWezlow].Mi[i / liczbaWezlow];
                } 
            }

            return kIR;
        }

        public double[] liczenieSumKir(double[] kIR)
        {
            double[] kR = new double[liczbaKlas];

            for (int i = 0; i < liczbaWezlow * liczbaKlas; i++)
            {
                kR[i / liczbaWezlow] = kR[i / liczbaWezlow] + kIR[i];
            }

            return kR;
        }

        public double[] liczeniefixIR(double[] eIR, double[] rhoIR, double[] rho, int K, double[] Pmi, int[] m)
        {
            double[] fixIR = new double[liczbaWezlow * liczbaKlas];

            for (int i = 0; i < liczbaWezlow * liczbaKlas; i++)
            {
                if (network.Nodes[i % liczbaWezlow].Type == QueueingNetworks.Node.NodeType.Type1)
                {
                    if (m[i % liczbaWezlow] == 1)
                    {
                        fixIR[i] = (eIR[i] / network.Nodes[i % liczbaWezlow].Mi[i / liczbaWezlow]) / (1 - (((K - 1) / K) * rho[i % liczbaWezlow]));
                    }
                    else
                    {
                        fixIR[i] = (eIR[i] / network.Nodes[i % liczbaWezlow].Mi[i / liczbaWezlow]) +
                                    ((eIR[i] / (m[i % liczbaWezlow] * network.Nodes[i % liczbaWezlow].Mi[i / liczbaWezlow])) /
                                        (1 - (((K - m[i % liczbaWezlow] - 1) / (K - m[i % liczbaWezlow])) * rho[i % liczbaWezlow]))) * Pmi[i % liczbaWezlow];
                    }
                }
                else
                {
                    fixIR[i] = eIR[i] / network.Nodes[i % liczbaWezlow].Mi[i / liczbaWezlow];
                }
                    

            }

            return fixIR;
        }

        public double[] liczenieSumFixR(double[] fixIR)
        {
            double[] fixR = new double[liczbaKlas];

            for (int i = 0; i < liczbaWezlow * liczbaKlas; i++)
            {
                fixR[i / liczbaWezlow] = fixR[i / liczbaWezlow] + fixIR[i];
            }

            return fixR;
        }

        public double[] liczenieLambdaR(double[] fixR, double[] kR)
        {
            double[] lambdaR = new double[liczbaKlas];

            for (int i = 0; i < liczbaKlas; i++)
            {
                //lambdaR[i] = kR[i] / fixR[i];
                lambdaR[i] = network.ClassMembersCounts[i] / fixR[i]; ;
            }

            return lambdaR;
        }

        public double blad(double[] lambda1R, double[] lambda2R)
        {
            double bladR = 0;

            for (int i = 0; i < liczbaKlas; i++)
            {
                bladR = bladR + Math.Pow(lambda1R[i] - lambda2R[i], 2);
            }


            return Math.Sqrt(bladR);
        }

        public double[] liczbaNieZajetychKanalowWStacji(double[] rhoI, int[] m)
        {
            double[] niezajeteI = new double[liczbaWezlow];

            for (int i = 0; i < liczbaWezlow; i++)
            {
                if (network.Nodes[i].Type == QueueingNetworks.Node.NodeType.Type1)
                {
                    niezajeteI[i] = m[i] * (1 - rhoI[i]);
                }
                else
                {
                    niezajeteI[i] = 0;
                }
            }

            return niezajeteI;
        }

        public double[] prawdopodobodobienstwoPi0I(int[] m, double[] rhoI)
        {
            double[] pi0I = new double[liczbaWezlow];
            double tempSum;

            for (int i = 0; i < liczbaWezlow; i++)
            {
                if (network.Nodes[i].Type == QueueingNetworks.Node.NodeType.Type1)
                {
                    tempSum = 0;
                    for (int k = 0; k <= m[i] - 1; k++)
                    {
                        tempSum = tempSum + Math.Pow(rhoI[i], k) / silnia(k);
                    }
                    pi0I[i] = 1 / (tempSum + (Math.Pow(rhoI[i], m[i])) / (silnia(m[i] - 1) * (m[i] - rhoI[i])));
                }
                else
                {
                    pi0I[i] = 0;
                }
                    
            }

            return pi0I;
        }

        public double[] sredniaDlugoscKolejkiI(int[] m, double[] rhoI, double[] pi0I)
        {
            double[] sredniaDlugoscKolejkiI = new double[liczbaWezlow];


            for (int i = 0; i < liczbaWezlow; i++)
            {
                if (network.Nodes[i].Type == QueueingNetworks.Node.NodeType.Type1)
                {
                    sredniaDlugoscKolejkiI[i] = (Math.Pow(rhoI[i], m[i] + 1) / (Math.Pow(m[i] - rhoI[i], 2) * silnia(m[i] - 1))) * pi0I[i];
                }
                else
                {
                    sredniaDlugoscKolejkiI[i] = 0;
                }
            }

            return sredniaDlugoscKolejkiI;
        }

        public double funkcjaCelu(double[] sredniaDlugoscKolejkiI, double[] niezajeteI)
        {
            double wartoscFkcjiCelu = 0;
            for (int i = 0; i < liczbaWezlow; i++)
            {
                wartoscFkcjiCelu = wartoscFkcjiCelu + 5 * sredniaDlugoscKolejkiI[i] + 15 * niezajeteI[i];
            }

            return wartoscFkcjiCelu;
        }

        public double[] GaussElimination(double[,] A, double[] b, int n)
        {
            double[] x = new double[n];

            double[,] tmpA = new double[n, n + 1];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    tmpA[i, j] = A[i, j];
                }
                tmpA[i, n] = b[i];
            }

            double tmp = 0;

            for (int k = 0; k < n - 1; k++)
            {
                for (int i = k + 1; i < n; i++)
                {
                    tmp = tmpA[i, k] / tmpA[k, k];
                    for (int j = k; j < n + 1; j++)
                    {
                        tmpA[i, j] -= tmp * tmpA[k, j];
                    }
                }
            }

            for (int k = n - 1; k >= 0; k--)
            {
                tmp = 0;
                for (int j = k + 1; j < n; j++)
                {
                    tmp += tmpA[k, j] * x[j];
                }
                x[k] = (tmpA[k, n] - tmp) / tmpA[k, k];
            }

            return x;
        }

    }
}
