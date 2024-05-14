using System;
using System.Threading;

namespace Lab2_paralel
{
    class Program
    {
        private static readonly int arrayLength = 100; 
        private static readonly int threadCount = 4; 
        private readonly Thread[] threads = new Thread[threadCount];
        private readonly int[] array = new int[arrayLength]; 
        private int minIndex = -1; 
        private int minValue = int.MaxValue; 

        static void Main(string[] args)
        {
            Console.OutputEncoding=System.Text.Encoding.UTF8;
            Program main = new Program();
            main.InitArray(); 
            main.RunThreads(); 
            main.FindMinElement(); 
            Console.WriteLine("Мінімальний елемент: " + main.minValue + ", його індекс: " + main.minIndex); 
            Console.ReadKey();
        }

        private void InitArray()
        {
            Random rnd = new Random();
            for (int i = 0; i < arrayLength; i++)
            {
                array[i] = rnd.Next(-1000, 1000); 
            }
        }

        private void RunThreads()
        {
            int partSize = arrayLength / threadCount;
            for (int i = 0; i < threadCount; i++)
            {
                int start = i * partSize; 
                int end = (i == threadCount - 1) ? arrayLength : (i + 1) * partSize; 
                threads[i] = new Thread(() => FindMinInPart(start, end)); 
                threads[i].Start();
            }
        }

        private void FindMinInPart(int start, int end)
        {
            int minVal = int.MaxValue;
            int minIdx = -1;
            for (int i = start; i < end; i++)
            {
                if (array[i] < minVal)
                {
                    minVal = array[i];
                    minIdx = i;
                }
            }
            lock (this)
            {
                if (minVal < minValue)
                {
                    minValue = minVal;
                    minIndex = minIdx;
                }
            }
        }

        private void FindMinElement()
        {
            
            foreach (var thread in threads)
            {
                thread.Join();
            }
        }
    }
}
