using System;
using System.Collections.Generic;
using EnumerableExtensionTask;

namespace _123
{
    class Program
    {
        static void Main(string[] args)
        {
            var intList = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var stringList = new List<string> { "a", "ab", "abc", "abcd", "abcde" };
            var intTest = intList.Where(x => x > 5);
            foreach (var item in intTest)
            {
                Console.WriteLine(item);
            }

            var stringTest = stringList.Where(str => str.Length > 2);
            foreach (var item in stringTest)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
            Console.WriteLine(intList.Count(x => x > 3 && x < 8));

            Console.WriteLine();
            Console.WriteLine(stringList.Count());
            Console.WriteLine();
            var test = intList.Select(x => x * 12.5);
            foreach (var item in test)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();

            intTest = (intList as IEnumerable<int>).Reverse();
            foreach (var item in intTest)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();

            stringTest = stringList.Select(str => str + "TEST");
            foreach (var item in stringTest)
            {
                Console.WriteLine(item);
            }
        }
    }
}
