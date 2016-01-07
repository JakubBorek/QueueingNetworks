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

        public FitnessCalculator(QueueingNetworks.Network network)
        {
            this.network = network;
            liczbaWezlow = network.Nodes.Count;
            liczbaKlas = network.ClassCount;
        }
        public double CalculateFitness(int[] solution)
        {
            //TO DO: Przeniesc do konstruktora + refactor
            double[,] macierzE = new double[liczbaWezlow * liczbaKlas, liczbaWezlow * liczbaKlas];
            double[] wyrazyWolne = new double[liczbaWezlow * liczbaKlas];
            double[] eIR = new double[liczbaWezlow * liczbaKlas];
            double[] lambdaIR = new double[liczbaWezlow * liczbaKlas];
            double[] rhoIR = new double[liczbaWezlow * liczbaKlas];
            double[] rhoI = new double[liczbaWezlow];
            double[] Pmi = new double[liczbaWezlow * liczbaKlas];
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
            int K = 100; //LICZBA ZGLOSZEN - powinna być podawana

            for (int i = 0; i < liczbaKlas; i++)
            {
                lambdaR1[i] = 0.00001;
            }
      

            //STEP1: Wyznaczanie eIR.. tylko raz potrzebne
            macierzE = liczenieMacierzyE();
            eIR = GaussElimination(macierzE, wyrazyWolne, liczbaWezlow * liczbaKlas);

            //STEP2: Parametry (zależne od iteracji i od aktualnego rozwiązania)
            do
            {
                lambdaIR = liczenieLambdaIR(eIR, lambdaR1);
                rhoIR = liczenieRhoIR(solution, lambdaIR);
                rhoI = liczenieRhoI(rhoIR);
                Pmi = liczeniePmi(solution, rhoI);
                kIR = liczenieKIR(solution, rhoIR, rhoI, K, Pmi);
                kR = liczenieSumKir(kIR);
                fixIR = liczeniefixIR(eIR, rhoIR, rhoI, K, Pmi, solution);
                fixR = liczenieSumFixR(fixIR);
                lambdaR2 = liczenieLambdaR(fixR, kR);
                wartBlad = blad(lambdaR1, lambdaR2);

                lambdaR1 = lambdaR2;
            } while (wartBlad > epsilon);

            //STEP3: Do funkcji celu
            niezajeteI = liczbaNieZajetychKanalowWStacji(rhoI, solution);
            pi0I = prawdopodobodobienstwoPi0I(solution, rhoI);
            srDlKolejkiI = sredniaDlugoscKolejkiI(solution, rhoI, pi0I);

            return funkcjaCelu(srDlKolejkiI, niezajeteI);
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
                                macierzE[i, j + k * liczbaWezlow] = network.Nodes[j].GetTransitionProbability(i % liczbaWezlow, k) - 1;
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
            double[] Pmi = new double[liczbaWezlow * liczbaKlas];

            double tempSum;

            for (int i = 0; i < liczbaWezlow; i++)
            {
                tempSum = 0;
                for (int k = 0; k < m[i] - 1; k++)
                {
                    tempSum = tempSum + Math.Pow(m[i] * rho[i], k) / silnia(k);
                                
                }

                Pmi[i] = (Math.Pow(rho[i] * m[i], m[i]) / (silnia(m[i]) * (1 - rho[i]))) *
                            1 / (tempSum + Math.Pow(m[i] * rho[i], m[i]) / silnia(m[i]) *
                                    (1 / (1 - rho[i])));
            }

            return Pmi;
        }

        public double[] liczenieKIR(int[]m,double[] rhoIR, double[] rho, int K, double[] Pmi)
        {
            double[] kIR = new double[liczbaWezlow * liczbaKlas];

            for (int i = 0; i < liczbaWezlow * liczbaKlas; i++)
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
                lambdaR[i] = kR[i] + fixR[i];
            }

            return kR;
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
                niezajeteI[i] = m[i] * (1 - rhoI[i]);
            }

            return niezajeteI;
        }

        public double[] prawdopodobodobienstwoPi0I(int[] m, double[] rhoI)
        {
            double[] pi0I = new double[liczbaWezlow];
            double tempSum;

            for (int i = 0; i < liczbaWezlow; i++)
            {
                tempSum = 0;
                for (int k = 0; k < m[i] - 1; k++)
                {
                    tempSum = tempSum + Math.Pow(rhoI[i], k) / silnia(k);
                }
                pi0I[i] = 1 / (tempSum +(Math.Pow(rhoI[i], m[i])) / (silnia(m[i] - 1) * (m[i] - rhoI[i])));
            }

            return pi0I;
        }

        public double[] sredniaDlugoscKolejkiI(int[] m, double[] rhoI, double[] pi0I)
        {
            double[] sredniaDlugoscKolejkiI = new double[liczbaWezlow];


            for (int i = 0; i < liczbaWezlow; i++)
            {
                sredniaDlugoscKolejkiI[i] = (Math.Pow(rhoI[i], m[i] + 1) / (Math.Pow(m[i] - rhoI[i], 2) * silnia(m[i] - 1))) * pi0I[i];
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
