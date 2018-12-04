using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace MergeSort
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Test of Merge Sort vs Threaded Merge Sort...");
            Console.WriteLine("There will be Four Test, with List Sizes of 8, 64, 256, and 1024.");
            Random random = new Random();
            int[] listSizes = new int[] { 8, 64, 256, 1024 };
            foreach (int size in listSizes)
            {
                Console.WriteLine($"Starting Test for List size of {size} - Test Results:");
                List<int> list1 = new List<int>();
                List<int> list2 = new List<int>();
                for (int i = 0; i < size; i++)
                {
                    int num1 = random.Next(0, Int32.MaxValue);
                    list1.Add(num1);
                    int num2 = random.Next(0, Int32.MaxValue);
                    list2.Add(num2);
                }
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                MergeSort(list1, 0, list1.Count - 1);
                stopwatch.Stop();
                double time = stopwatch.Elapsed.TotalMilliseconds;
                Console.WriteLine($"Normal Sort Time (ms): {time}");
                stopwatch.Restart();
                ThreadMergeSort(list2, 0, list2.Count - 1);
                stopwatch.Stop();
                time = stopwatch.ElapsedMilliseconds;
                Console.WriteLine($"Threaded Sort Time (ms): {time}");
            }
        }
        public static void ThreadMergeSort(List<int> list, int left, int right)
        {
            if (left < right)
            {
                //Middle index of list
                int middle = (left + right) / 2;
                // Sort first and second halves of list by threading the recursive call
                Thread thread1 = new Thread(() => ThreadMergeSort(list, left, middle));
                Thread thread2 = new Thread(() => ThreadMergeSort(list, middle + 1, right));
                thread1.Start();
                thread2.Start();
                //Waits for thread1 and thread2 to finish and then Merges the list
                thread1.Join();
                thread2.Join();
                Merge(list, left, middle, right);
            }
        }

        public static void MergeSort(List<int> list, int left, int right)
        {
            if (left < right)
            {
                //Middle index of list
                int middle = (left+right) / 2;
                // Sort first and second halves of list
                MergeSort(list, left, middle);
                MergeSort(list, middle + 1, right);
                //Merges the list
                Merge(list, left, middle, right);
            }
        }

        public static void Merge(List<int> list, int left, int middle, int right)
        {
            //Temporary array sizes
            int leftSize = middle - left + 1;
            int rightSize = right - middle;
            //Temporary Arrays
            int[] leftArr = new int[leftSize];
            int[] rightArr = new int[rightSize];
            //Copies values to Temporary leftArr
            int leftIndex = 0;
            for (; leftIndex < leftSize; leftIndex++)
            {
                leftArr[leftIndex] = list[left + leftIndex];
            }
            leftIndex = 0;
            //Copies values to Temporary rightArr
            int rightIndex = 0;
            for (; rightIndex < rightSize; rightIndex++)
            {
                rightArr[rightIndex] = list[middle + rightIndex + 1];
            }
            rightIndex = 0;
            //Merges the two temporary array's values back into original list
            int index = left;
            while (leftIndex < leftSize && rightIndex < rightSize)
            {
                if (leftArr[leftIndex] <= rightArr[rightIndex])
                {
                    list[index++] = leftArr[leftIndex++];
                }
                else
                {
                    list[index++] = rightArr[rightIndex++];
                }
            }
            //Copy the remaining values in leftArr's, if there are any, into the list
            while (leftIndex < leftSize)
            {
                list[index++] = leftArr[leftIndex++];
            }
            //Copy the remaining values in rightArr's, if there are any, into the list
            while (rightIndex < rightSize)
            {
                list[index++] = rightArr[rightIndex++];
            }
        }
    }
}
