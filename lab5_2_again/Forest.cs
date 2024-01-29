using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace lab5_2_again
{
    public class Forest
    {
        public List<Tree> trees = new List<Tree>();
        Random rnd;
        public double fitness;

        public Forest(Random rnd) 
        {
            this.rnd = rnd;
        }

        public void CreateForest()
        {
            for(int i = trees.Count(); i<Constants.POPS;  i++)
            {
                double x =  Constants.RADIUS + (Constants.MAXW - Constants.RADIUS) * rnd.NextDouble();
                double y = Constants.RADIUS + (Constants.MAXH - Constants.RADIUS) * rnd.NextDouble();
                trees.Add(new Tree(x, y, rnd));
            }
        }

        public void CalcFitness()
        {
            fitness = 0;
            foreach (Tree tree in trees)
            {
                tree.fitness = 0;
                tree.treesInRadius = new List<Tree>();
            }

            for (int i = 0; i < trees.Count(); i++)
            {
                for (int j = 0; j < trees.Count(); j++)
                {
                    if (i != j)
                    {
                        if (!trees[i].treesInRadius.Contains(trees[j]))
                        {
                            if (trees[i].isInRadius(trees[j]))
                            {
                                trees[i].treesInRadius.Add(trees[j]);
                                trees[j].treesInRadius.Add(trees[i]);
                            }
                        }
                    }
                }
            }

            CalcTreesFitness();
            punish();

            foreach(Tree tree in trees)
            {
                fitness += tree.fitness;
            }

            trees.Sort((x, y) => x.fitness.CompareTo(y.fitness));
        }

        public void CalcTreesFitness()
        {
            foreach (Tree t in trees)
            {
                if (t.treesInRadius.Count() >= Constants.MINTREES)
                {
                    foreach (Tree ttt in t.treesInRadius)
                    {
                        //double num = Constants.MAXDISTANCE - t.getDistance(ttt);
                        t.fitness += 3;
                    }
                }
                else
                {
                    t.fitness -= 100;
                }
            }

        }

        public void punish()
        {
            foreach(Tree t in trees)
            {
                if (t.x < Constants.RADIUS) t.fitness -= 3 *  Constants.RADIUS - t.x;
                if (t.y < Constants.RADIUS) t.fitness -= 3 * Constants.RADIUS - t.y;
                if (t.x > Constants.MAXW - Constants.RADIUS) t.fitness -= 3 * t.x - Constants.MAXW - Constants.RADIUS;
                if (t.x > Constants.MAXH - Constants.RADIUS) t.fitness -= 3 * t.x - Constants.MAXH - Constants.RADIUS;

                foreach (Tree ttt in t.treesInRadius)
                {
                    if(t.getDistance(ttt) <= Constants.RADIUS)
                    {
                        t.fitness -= 100;
                        break;
                    }
                }
            }
        }

        public Forest Mutate()
        {
            Forest f = new Forest(rnd);
            List<Tree> list = new List<Tree>();
            List<Tree> treelist = new List<Tree>();

            for(int i = 0; i< trees.Count; i++)
            {
                treelist.Add(new Tree(trees[i].x, trees[i].y, rnd));
                treelist[i].fitness = trees[i].fitness;
            }

            for(int i = 0; i< treelist.Count; i++)
            {
                if (treelist[i].fitness < 0) list.Add(treelist[i]);
                else break;
            }

            if(list.Count > 0)
            {
                for(int i = 0; i<list.Count;i++)
                {
                    if(rnd.Next(100) > Constants.MUTATE)
                    {
                        treelist[treelist.IndexOf(list[i])].mutateTree();
                    }
                }
            }
            else
            {
                for(int i = 0; i<rnd.Next(4); i++)
                {
                    if (rnd.Next(100) > Constants.MUTATE)
                    {
                        treelist[i].mutateTree();
                    }
                }
            }

            f.trees = treelist;
            return f;
        }

        public Forest Crossover(Forest parent)
        {
            Forest child = new Forest(rnd);
            List<Tree> list = new List<Tree>();
            for(int i = 0; i<Constants.POPS; i++)
            {
                if(rnd.Next(3) == 0)
                {
                    list.Add(new Tree(trees[i].x, trees[i].y, rnd));
                }
                else
                {
                    list.Add(new Tree(parent.trees[i].x, parent.trees[i].y, rnd));
                }
            }
            child.trees = list;
            return child;
        }
    }
}
