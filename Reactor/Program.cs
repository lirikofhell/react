using System;
using System.Threading;

namespace Reactor
{
    class Program
    {
        static void Main(string[] args)
        {
            Boolean enabled = true;
            MyReactor myReactor = new MyReactor(enabled);
            Thread ThUran = new Thread(new ThreadStart(myReactor.uran));
            Thread ThAbsorb = new Thread(new ThreadStart(myReactor.absorb));
            Thread ThReactorMonitor = new Thread(new ThreadStart(myReactor.reactorMonitor));
            Thread ThChumba = new Thread(new ThreadStart(myReactor.Chumba));
            ThUran.Start();
            ThAbsorb.Start();
            ThReactorMonitor.Start();
            ThChumba.Start();
        }
    }

    class MyReactor
    {
        private readonly object ReactorLock = new object();
        Int32 temp = 30;
        Int32 targetTemp = 80;
        Boolean enabled = true;

        public MyReactor(Boolean enabled) { this.enabled = enabled; }

        public void uran()
        {
            while (enabled)
            {
                lock (ReactorLock)
                {
                    temp++;
                    Console.WriteLine(temp);
                }
                Thread.Sleep(100);
            }
        }

        public void reactorMonitor()
        {
            while (enabled || temp > -1)
            {
                Console.WriteLine("TEMP:" + temp);
                if (temp >= 100)
                {
                    Console.Clear();
                    Console.WriteLine("BOOM");
                }
                else if (temp <= 0)
                {
                    Console.Clear();
                    Console.WriteLine("STOPPED");
                }
                Thread.Sleep(1000);
            }
        }

        public void absorb()
        {
            while(enabled || temp > 0)
            {
                lock (ReactorLock)
                {
                    if(temp > targetTemp)
                    {
                        int diff = Math.Abs(targetTemp - temp);
                        if (diff < 10) temp -= 2;
                        else if (diff > 10 && diff < 20) temp -= 3;
                        else if (diff > 20) temp -= 4;
                    }
                }
                Thread.Sleep(100);
            }
        }

        public void Chumba()
        {
            while (enabled)
            {
                var s = Console.ReadLine();
                var line = s;
                if (line.Equals("stop")) Environment.Exit(0);
                else targetTemp = int.Parse(line);
                Thread.Sleep(100);
            } 
        }
    }
}
