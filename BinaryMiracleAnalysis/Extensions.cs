using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuranLibrary;

namespace BinaryMiracleAnalysis
{
    public static class Extensions
    {
        public static bool IsOdd(this int a)
        {
            return a%2 == 1;
        }
        public static bool IsEven(this int a)
        {
            return a % 2 == 0;
        }

        private static Random _rng = new Random();

        //public static void Shuffle<T>(this IList<T> list)
        //{
        //    int n = list.Count;
        //    while (n > 1)
        //    {
        //        n--;
        //        int k = _rng.Next(n + 1);
        //        T value = list[k];
        //        list[k] = list[n];
        //        list[n] = value;
        //    }
        //}

        //see about generating a deterministic list of permutations : http://stackoverflow.com/questions/756055/listing-all-permutations-of-a-string-integer
        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });

            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        public static void ShuffleAyahCounts(this IList<SuraMetaData> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = _rng.Next(n + 1);
                var value = list[k].AyahCount;
                list[k].AyahCount = list[n].AyahCount;
                list[n].AyahCount = value;
            }
        }
    }
}
