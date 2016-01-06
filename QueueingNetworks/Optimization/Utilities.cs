using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimization
{
    static class Utilities
    {
        public static List<Scout> createScouts(int scoutsAmount)
        {
            List<Scout> scouts = new List<Scout>();
            for (int i = 0; i < scoutsAmount; i++)
            {
                scouts.Add(new Scout());
            }
            return scouts;
        }

        public static void sendScouts(List<Scout> scouts)
        {
            foreach (Scout scout in scouts)
            {
                scout.choseFlower();
            }
        }

        public static void searchEliteFlowersNeigberhood(List<Scout> scoutsWithEliteFlowers)
        {
            Flower bestNeighbor;
            for(int i = 0; i < scoutsWithEliteFlowers.Count; i++)
            {
                bestNeighbor = chooseBestNeighbor(scoutsWithEliteFlowers[i].getFlower(), BeesAlgorithm.getElisteSolutionsAmount());
                scoutsWithEliteFlowers[i] = new Scout(bestNeighbor);
            }
        }


        public static void searchBestFlowersNeigberhood(List<Scout> scoutsWithBestFlowers)
        {
            Flower bestNeighbor;
            for (int i = 0; i < scoutsWithBestFlowers.Count; i++)
            {
                bestNeighbor = chooseBestNeighbor(scoutsWithBestFlowers[i].getFlower(), BeesAlgorithm.getBestSolutionsAmount());
                scoutsWithBestFlowers[i] = new Scout(bestNeighbor);
            }
        }

        private static Flower chooseBestNeighbor(Flower flower, int neigborsAmount)
        {
            List<Flower> neighborhood = new List<Flower>();
            for (int i = 0; i < neigborsAmount; i++)
            {
                Flower neighbor = flower.choseFlowerNeighbor();
                neighborhood.Add(neighbor);
            }
            neighborhood.Sort();
            return neighborhood.First();
        }
    }
}
