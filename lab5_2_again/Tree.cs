using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab5_2_again
{
    public class Tree
    {
        public double x;
        public double y;
        Random rnd;
        public double fitness = 0;
        public List<Tree> treesInRadius = new List<Tree>();


        public Tree(double x, double y, Random rnd)
        {
            this.x = x;
            this.y = y;
            this.rnd = rnd;
        }

        public double getDistance(Tree tree1)
        {
            return Math.Sqrt(Math.Pow(x - tree1.x, 2) + Math.Pow(y - tree1.y, 2));
        }

        public bool isInRadius(Tree tree1)
        {
            double distance = getDistance(tree1);

            if (distance < 0) return true;
            if (distance == 0) return true;
            if (distance <= Constants.MAXDISTANCE) return true;
            return false;
        }

        public void mutateTree()
        {
            double x1 = 1 + (Constants.RADIUS - 1) * rnd.NextDouble();
            double y1 = 1 + (Constants.RADIUS - 1) * rnd.NextDouble();
            if (rnd.Next(2) == 0) x1 *= -1;
            if (rnd.Next(2) == 0) y1 *= -1;

            x += x1;
            y += y1;
        }
    }
}
