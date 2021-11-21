using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Benchmarks.EmptyChecks
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<CheckEmptyListsBenchmark>();
        }
    }

    public static class Helper
    {
        private static Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static IEnumerable<string> GenerateRandomStringList(int length)
        {
            for (int i = 0; i < length; i++)
                yield return new string(Enumerable.Repeat(chars, 5)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }

    [RankColumn]
    public class CheckEmptyListsBenchmark
    {
        IEnumerable<string> listIEnumerable;
        List<string> list;

        [Params(0, 100, 1000)]
        public int listSize;

        [GlobalSetup]
        public void Setup()
        {
            listIEnumerable = Helper.GenerateRandomStringList(listSize);
            list = listIEnumerable.ToList();
        }

        [Benchmark]
        public bool Count_Property_List()
        {
            return list.Count == 0;
        }

               [Benchmark]
        public bool Count_Method_List()
        {
            return list.Count() == 0;
        }

        [Benchmark]
        public bool Linq_FirstOrDefault_List()
        {
            return list.FirstOrDefault() == null;
        }

        [Benchmark]
        public bool Linq_Any_List()
        {
            return list.Any();
        }

        [Benchmark]
        public bool Count_Method_IEnumerable()
        {
            return listIEnumerable.Count() == 0;
        }

        [Benchmark]
        public bool Linq_Any_IEnumerable()
        {
            return listIEnumerable.Any();
        }

        [Benchmark]
        public bool Linq_FirstOrDefault_IEnumerable()
        {
            return listIEnumerable.FirstOrDefault() == null;
        }
    }
}