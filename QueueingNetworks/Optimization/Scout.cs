using System;

namespace Optimization
{
    class Scout : IComparable<Scout>
    {
        private Flower flower = new Flower();

        public Scout() { }

        public Scout(Flower flower)
        {
            this.flower = flower;
        }

        public void choseFlower()
        {
            flower.choseFlower();
        }    

        public Flower getFlower()
        {
            return flower;
        }

        public void setFlower(Flower flower)
        {
            this.flower = flower;
        }

        public int CompareTo(Scout other)
        {
            return flower.getSolutionValue().CompareTo(other.flower.getSolutionValue());
        }
    }
}