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

        public static AutoResetEvent[] horseCanGo = new AutoResetEvent[3];
        public static AutoResetEvent[] mainCanGo = new AutoResetEvent[3];

        static List<int> winners = new List<int>();

        public static Thread[] threads = new Thread[3];

        static int dist1 = 0;
        static int dist2 = 0;
        static int dist3 = 0;

        public static object Lock = new Object();
        static bool gameRunning = true;

        static void Main(string[] args)
        {

            for (int i = 0; i <= 2; i++)
            {
                horseCanGo[i] = new AutoResetEvent(false);
                mainCanGo[i] = new AutoResetEvent(false);
                threads[i] = new Thread(goHorse);

                threads[i].Start(i);
            }


            while (gameRunning)
            {
                for (int i = 0; i <= 2; i++)
                {
                    horseCanGo[i].Set();
                }
                for (int i = 0; i <= 2; i++)
                {
                    mainCanGo[i].WaitOne();
                }
            }

            for (int i = 0; i <= 2; i++)
            {
                horseCanGo[i].Set();
            }

            for (int i = 0; i <= 2; i++)
            {
                threads[i].Join();
            }

            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Welcome to the Racing Program:");
            DrawScreen();

            if (dist1 == 50 || dist2 == 50 || dist3 == 50)
            {
                gameRunning = false;
                winners.Clear();

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

                foreach (int T in winners)
                {
                    Console.WriteLine("The Winner was Horse " + T);
                }

            }
        }
        public static void DrawScreen()
        {
            Console.Clear();
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


        public static void goHorse(object ID)
        {
            int objectID = (int)ID;

            Random rand = new Random();

            while (gameRunning)
            {
            int value = rand.Next(1, 1000);
                horseCanGo[objectID].WaitOne();


                if ( value == 5)
                {
                    {
                        if (objectID == 0)
                        {
                            dist1++;
                        }
                        else if (objectID == 1)
                        {
                            dist2++;
                        }
                        else if (objectID == 2)
                        {
                            dist3++;
                        }
                        //increase the distance

                    if (dist1 == 50 || dist2 == 50 || dist3 == 50)
                    {
                        gameRunning = false;
                        DrawScreen();
                    }
                    }
                }
                mainCanGo[objectID].Set();
            }
        }

    }
}
