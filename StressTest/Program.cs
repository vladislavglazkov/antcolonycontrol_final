using System;
using AntAlgo;
namespace StressTest
{
    class Callback : EventsCallback
    {
        public void Handle(double time, Ant ant, int type)
        {
            
        }
    }

    class Program
    {
        static Map map = new Map();

        static void Main(string[] args)
        {
            AntColonyControl colony = new AntColonyControl();
            map.Init(GlobalConstraints.XSize, GlobalConstraints.XSize, 10);
            for (int i = 0; i < GlobalConstraints.XSize; i++)
            {
                for (int j = 0; j < GlobalConstraints.XSize; j++)
                {
                    map.Set(2, i, j, -1);
                    map.Set(4, i, j, -1);
                }
            }
            for (int i = 0; i < 100; i++)
            {
                Callback cb = new Callback();
                Ant nw = new Ant();
                DecisionMakeStandard dm = new DecisionMakeStandard();

                nw.Init(1, 1, 25, Math.PI / 2, 5, map, dm, cb, new Random().Next());
                colony.Add(nw);
                Console.WriteLine(nw.id);
            }
            /*var rev = new Ant();
            DecisionMakeStandard dm2 = new DecisionMakeStandard();
            dm2.state = 1;
            rev.Init(8.9, 8.9, 1, Math.PI / 2, 5, map, dm2, cb, new Random().Next());
            rev.angle = Math.PI;
            Console.WriteLine(rev.id);*/
            for (int i = 0; i < GlobalConstraints.XSize; i++)
            {
                Set1(0, i, 1);
                Set1(GlobalConstraints.XSize - 1, i, 1);
                Set1(i, 0, 1);
                Set1(i, GlobalConstraints.XSize - 1, 1);

            }
            /* Set1(2, 0);
             Set1(3, 0);
             Set1(2, 1);*/
            Set1(55, 55, 2);
            Set1(77, 77, 2);
            Set1(71, 71, 2);
            Set1(72, 72, 1);
            Set1(70, 70, 1);
            Set1(1, 1, 3);
            //colony.Add(rev);
            int time = 0;
            while (true)
            {
                time++;
                colony.Move(time);
                Console.WriteLine($"At {time}");
            }
        }
        
        static void Set1(int x,int y,int type)
        {
            map.Set(0, x, y, type);

        }
    }
}
