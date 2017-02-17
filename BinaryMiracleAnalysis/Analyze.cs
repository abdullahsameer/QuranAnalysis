using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuranLibrary;
using BinaryMiracleAnalysis;

namespace BinaryMiracleAnalysis
{
    public class Analyze
    {
        public static int Count { get; set; } = 1;

        private QuranLibrary.QuranLibrary _library = new QuranLibrary.QuranLibrary();

        //groups of oddeven/oddodd/etc
        //27, 30, 27, 30 
        //distinct numer of ayah counts in each
        //20, 28, 24, 25

        //possible number of orders
        //27c20 = 2.1604899703211E+24
        //30c28 = 1.32626429906096E+32
        //27c24 = 1.81481157506973E+27
        //30c25 = 2.21044049843493E+30

        public void Go()
        {
            /*get a list of 4 buckets
             * odd surahs with odd number of verses 
             * odd surahs with even number of verses
             * even surahs with odd number of verses
             * even surah with even number of verses
             */

            var oddOdd = new List<SuraMetaData>();
            var oddEven = new List<SuraMetaData>();
            var evenOdd = new List<SuraMetaData>();
            var evenEven = new List<SuraMetaData>();
            var oddMagicNumbers = new List<SuraMetaData>();
            var evenMagicNumbers = new List<SuraMetaData>();

            foreach (var surah in _library.Surahs)
            {
                if (surah.MagicNumber.IsOdd())
                    oddMagicNumbers.Add(surah);
                else
                    evenMagicNumbers.Add(surah);

                if (surah.SurahNumber.IsOdd() && surah.AyahCount.IsOdd())
                {
                    oddOdd.Add(surah);
                }
                else if (surah.SurahNumber.IsOdd() && surah.AyahCount.IsEven())
                {
                    oddEven.Add(surah);
                }
                else if (surah.SurahNumber.IsEven() && surah.AyahCount.IsEven())
                {
                    evenEven.Add(surah);
                }
                else if (surah.SurahNumber.IsEven() && surah.AyahCount.IsOdd())
                {
                    evenOdd.Add(surah);
                }
            }

            //Console.WriteLine("TotalOddMagicNumbers TotalEvenMagicNumbers");
            //Console.WriteLine($"{oddMagicNumbers.Sum(o=>o.MagicNumber)} {evenMagicNumbers.Sum(o => o.MagicNumber)}");

            //Console.WriteLine("distinct ayahNumber count: " + oddOdd.Select(s => s.AyahCount).Distinct().Count());
            //Console.WriteLine("distinct ayahNumber count: " + oddEven.Select(s => s.AyahCount).Distinct().Count());
            //Console.WriteLine("distinct ayahNumber count: " + evenOdd.Select(s => s.AyahCount).Distinct().Count());
            //Console.WriteLine("distinct ayahNumber count: " + evenEven.Select(s => s.AyahCount).Distinct().Count());
            //Console.WriteLine("Magic#s");
            //Console.WriteLine("SurahNumber\tAyahCount\tMagicNumber");
            //foreach (var surah in oddMagicNumbers)
            //{
            //    Console.WriteLine($"{surah.SurahNumber}\t{surah.AyahCount}\t{surah.MagicNumber}");
            //}

            //var oddOddShuffled = new List<SuraMetaData>();
            //var oddEvenShuffled = new List<SuraMetaData>();
            //var evenOddShuffled = new List<SuraMetaData>();
            //var evenEvenShuffled = new List<SuraMetaData>();
            oddOdd.ShuffleAyahCounts();
            oddEven.ShuffleAyahCounts();
            evenOdd.ShuffleAyahCounts();
            evenEven.ShuffleAyahCounts();
            Console.WriteLine();

            //Console.WriteLine("SurahNumber\tAyahCount\tMagicNumber");
            var allSurahs = oddOdd.Union(oddEven).Union(evenOdd).Union(evenEven).OrderBy(t => t.SurahNumber);
            //var outputString = new StringBuilder(115);
            //outputString.AppendLine("SurahNumber,AyahCount,MagicNumber");
            foreach (var surah in allSurahs)
            {
                var resultString = $"{surah.SurahNumber},{surah.AyahCount},{surah.MagicNumber}";
                Console.WriteLine(resultString);
                //outputString.AppendLine(resultString);
            }


            Console.WriteLine("Post Shuffle\nTotalOddMagicNumbers TotalEvenMagicNumbers");
            Console.WriteLine($"{allSurahs.Where(s => s.MagicNumber.IsOdd()).Sum(o => o.MagicNumber)} {allSurahs.Where(s => s.MagicNumber.IsEven()).Sum(o => o.MagicNumber)}");
            Console.WriteLine();


            //Console.WriteLine("TotalOddMagicNumbers TotalEvenMagicNumbers");
            //Console.WriteLine($"{oddMagicNumbers.Sum(o => o.MagicNumber)} {evenMagicNumbers.Sum(o => o.MagicNumber)}");
            //Console.WriteLine();

            //Console.WriteLine("TotalOddMagicNumbers TotalEvenMagicNumbers");
            //Console.WriteLine($"{oddMagicNumbers.Sum(o => o.MagicNumber)} {evenMagicNumbers.Sum(o => o.MagicNumber)}");
            //Console.WriteLine();
            //var hash = 19;
            //foreach (var foo in allSurahs)
            //{
            //    hash = hash * 31 + foo.GetHashCode();
            //}
            //System.IO.File.WriteAllText(@"c:\temp\QuranShuffle" + hash + ".csv",outputString.ToString());
        }
    }
}
