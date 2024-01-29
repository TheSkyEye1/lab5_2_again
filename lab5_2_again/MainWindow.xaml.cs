using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;

namespace lab5_2_again
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Forest> forests = new List<Forest>();
        public Random rnd = new Random();
        DispatcherTimer timer;
        int iteration = 0;
        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            timer.Tick += Timer_Tick;
        }

        public void drawEllipse(double x, double y, int type)
        {
            Ellipse el = new Ellipse();
            SolidColorBrush cb = new SolidColorBrush();
            if (type == 0) cb.Color = Color.FromArgb(150, 140, 200, 140);
            else if(type == 1) cb.Color = Color.FromArgb(255, 15, 90, 15);
            else cb.Color = Color.FromArgb(255, 15, 90, 15);
            el.Fill = cb;
            el.StrokeThickness = 0;
            el.Stroke = Brushes.Black;
            el.Width = Constants.RADIUS;
            el.Height = Constants.RADIUS;
            el.RenderTransform = new TranslateTransform(x - Constants.RADIUS / 2, y - Constants.RADIUS / 2);
            scene.Children.Add(el);
        }

        public void initForests()
        {
            for (int i = forests.Count; i < Constants.FORESTSMAX; i++)
            {
                forests.Add(new Forest(rnd));
                forests[i].CreateForest();
            }
        }
        public void drawScene()
        {
            scene.Children.Clear();
            for (int i = forests.Count - 1; i >= 0; i--)
            {
                int type = 0;
                if (i == 0) type = 1;
                foreach (Tree t in forests[i].trees)
                {
                    drawEllipse(t.x, t.y, type);
                }
            }
        }

        public void fitness()
        {
            foreach (Forest f in forests)
            {
                f.CalcFitness();
            }

            forests.Sort((x, y) => y.fitness.CompareTo(x.fitness));
        }

        public void breed_my_pretties()
        {
            List<Forest> parents = new List<Forest>();

            for (int i = 1; i < forests.Count / 2 + 1; i++)
            {
                int id = rnd.Next(forests.Count);

                if (parents.Contains(forests[id])) i--;
                else parents.Add(forests[id]);
            }

            parents.Sort((x, y) => y.fitness.CompareTo(x.fitness));

            while (parents.Count > 1)
            {
                int index1 = rnd.Next(parents.Count);
                int index2 = index1;
                while (index1 == index2)
                {
                    index2 = rnd.Next(parents.Count);
                }
                Forest parent1 = parents[index1];
                Forest parent2 = parents[index2];
                forests.Add(parent1.Crossover(parent2));
                parents.Remove(parent2);
                parents.Remove(parent1);
            }
        }

        public void mutate_my_pretties()
        {
            for (int i = 1; i < forests.Count / 4; i++)
            {
                if (rnd.Next(100) > Constants.MUTATE) forests.Add(forests[i].Mutate());
            }
        }

        public void NextIteration()
        {
            iteration++;
            breed_my_pretties();
            mutate_my_pretties();
            fitness();

            while (forests.Count > Constants.FORESTSMAX)
            {
                forests.Remove(forests.Last());
            }

            fitLB.Content = "Best Fit: " + forests[0].fitness;
            iterLB.Content = "Iteration " + iteration; 

            drawScene();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            initForests();
            fitness();
            drawScene();
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            NextIteration();
        }
    }
}
