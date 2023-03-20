using System;
using System.Threading;
using System.Collections.Generic;

namespace HorseRacing
{
    class Program
    {
        //Todo: 1) Create 3 threads (one per horse)
        //Todo: 2) establish 3 AutoResetEvents  (one for each child thread).
        //Todo: 3) The main thread will then send a pulse out to all 3 threads by calling .Set()
        //Todo: 4) Each Horse thread will wait for a signal from main, and upon hearing it:
        //      4a) run a lottery 1/1000
        //      4b) if lottery won, then advance their horse 1 space (put a '-' on the screen)
        //Todo: 5) The first horse to travel 50 spaces is pronounced the winner
        //Todo: 6) 2 - All kids die appropriately  (hint - how did we do this in Matrix??)

        private static AutoResetEvent horseCanGo1 = new AutoResetEvent(true);
        private static AutoResetEvent horseCanGo2 = new AutoResetEvent(true);
        private static AutoResetEvent horseCanGo3 = new AutoResetEvent(true);

        private static AutoResetEvent mainCanGo1 = new AutoResetEvent(true);
        private static AutoResetEvent mainCanGo2 = new AutoResetEvent(true);
        private static AutoResetEvent mainCanGo3 = new AutoResetEvent(true);

        static int dist1 = 0;
        static int dist2 = 0;
        static int dist3 = 0;

        public static object Lock = new Object();

        public static void DrawScreen()
        {
            string s1 = "";
            string s2 = "";
            string s3 = "";

            for (int i = 0; i < dist1; i++)
            {
                s1 += '-';
            }
            for (int i = 0; i < dist2; i++)
            {
                s2 += '-';
            }
            for (int i = 0; i < dist3; i++)
            {
                s3 += '-';
            }

            Console.SetCursorPosition(0, 1);
            Console.WriteLine("HorseA: [" + s1 + ']');
            Console.WriteLine("HorseB: [" + s2 + ']');
            Console.WriteLine("HorseC: [" + s3 + ']');
        }
        static void Main(string[] args)
        {
            bool gameRunning = true;

            Thread horseA = new Thread(goHorse);
            Thread horseB = new Thread(goHorse);
            Thread horseC = new Thread(goHorse);
            horseA.Start();
            horseB.Start();
            horseC.Start();

            while (gameRunning)
            {
                horseCanGo1.Set();
                horseCanGo2.Set();
                horseCanGo3.Set();

                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Welcome to the Racing Program:");
                DrawScreen();


                mainCanGo1.WaitOne();
                mainCanGo2.WaitOne();
                mainCanGo3.WaitOne();

                if (checkWinner().Count > 0)
                {
                    foreach (int T in checkWinner())
                    {
                        Console.WriteLine("The winner of this race was " + T);
                    }
                    gameRunning = false;
                    
                    // horseA.Abort();
                    // horseB.Abort();
                    // horseC.Abort();
                }
            }
        }

        public static void goHorse(object ID)
        {
            int threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
            int objectID = (int)ID;

            while (true)
            {
                horseCanGo1.WaitOne();
                horseCanGo2.WaitOne();
                horseCanGo3.WaitOne();

                //create random value
                Random rand = new Random();
                int value = rand.Next(1, 2);

                //how to increase correct distance?


                if (value == 2)
                {
                    if (objectID == 0)
                    {
                        dist1++;
                    }
                    if (objectID == 1)
                    {
                        dist2++;
                    }
                    if (objectID == 2)
                    {
                        dist3++;
                    }
                    //increase the distance
                }
                mainCanGo1.Set();
                mainCanGo2.Set();
                mainCanGo3.Set();

            }
        }

        static List<int> checkWinner()
        {
            List<int> winners = new List<int>();

            if (dist1 == 50)
            {
                winners.Add(1);
            }
            if (dist2 == 50)
            {
                winners.Add(2);
            }
            if (dist3 == 50)
            {
                winners.Add(3);
            }

            return winners;
        }
    }
}
