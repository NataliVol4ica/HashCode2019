using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace HashCode2019
{
    static class DataAnalyzer
    {
        public delegate void PathAnalyzer(string path);

        /// <summary>
        /// Checks if tag contain any character except 0..9a..z
        /// </summary>
        public static void CheckTagChars(string path)
        {
            int illegals = 0;
            var input = new InputData();
            input.Read(path);
            foreach (var photo in input.Horizontals)
            {
                foreach (var tag in photo.StringTags)
                {
                    foreach (var c in tag)
                        if (!(char.IsDigit(c) || char.IsLower(c)))
                            illegals++;
                }
            }
            foreach (var photo in input.Verticals)
            {
                foreach (var tag in photo.StringTags)
                {
                    foreach (var c in tag)
                        if (!(char.IsDigit(c) || char.IsLower(c)))
                            illegals++;
                }
            }
            Console.WriteLine("{0} illegals in file {1}", illegals, path);
        }
        /// <summary>
        /// Prints data about test cases
        /// </summary>
        public static void PrintTestData(string path)
        {
            int size;
            int horizontals = 0;
            int verticals = 0;
            int tagNum = 0;
            int MaxTagSize = 0;
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                string line;
                size = Convert.ToInt32(sr.ReadLine());
                while ((line = sr.ReadLine()) != null)
                {
                    string[] data = line.Split(' ');
                    if (data[0] == "H")
                        horizontals++;
                    else
                        verticals++;
                    tagNum += Convert.ToInt32(data[1]);
                    for (int i = 2; i < data.Count(); i++)
                        if (data[i].Length > MaxTagSize)
                            MaxTagSize = data[i].Length;
                }
            }
            Console.WriteLine("Path {0}", path);
            Console.WriteLine("Test size = {0}", size);
            Console.WriteLine("Horizontals = {0}", horizontals);
            Console.WriteLine("Verticals = {0}", verticals);
            Console.WriteLine("AVG tag num = {0}", tagNum / size);
            Console.WriteLine("Max tag size = {0}", MaxTagSize);
        }
        /// <summary>
        /// Analyzes all the test files with a provided method
        /// </summary>
        public static void AnalyzeAllTests(PathAnalyzer analyzer)
        {
            analyzer(pathA);
            analyzer(pathB);
            analyzer(pathC);
            analyzer(pathD);
            analyzer(pathE);
        }
        /// <summary>
        /// Count profit of two tag lists
        /// </summary>       
        public static int CountProfit(List<string> tags1, List<string> tags2)
        {
            int intersectCount = tags1.Intersect(tags2).Count();
            int leftDifCount = tags1.Except(tags2).Count();
            int rightDifCount = tags2.Except(tags1).Count();
            var values = new List<int> { intersectCount, leftDifCount, rightDifCount };
            return values.Min();
        }
        /// <summary>
        /// Analyze pair input data + answer file and print interest amount
        /// </summary>
        public static void AnalyzeAnswer(string testPath, string ansPath)
        {
            int totalProfit = 0;
            List<List<string>> inpData;
            using (StreamReader sr = new StreamReader(testPath))
            {
                int lineNumber = Convert.ToInt32(sr.ReadLine());
                inpData = new List<List<string>>();
                for (int i = 0; i < lineNumber; i++)
                {
                    var data = sr.ReadLine().Split(' ').ToList();
                    inpData.Add(data.GetRange(2, data.Count - 2));
                }
            }
            List<int> ansData;
            using (StreamReader sr = new StreamReader(ansPath))
            {
                int lineNumber = Convert.ToInt32(sr.ReadLine());
                ansData = new List<int>();
                for (int i = 0; i < lineNumber; i++)
                    ansData.Add(Convert.ToInt32(sr.ReadLine()));
            }
            for (int i = 0; i < ansData.Count - 1; i++)
            {
                totalProfit += CountProfit(inpData[ansData[i]], inpData[ansData[i + 1]]);
                if (i % 1000 == 1)
                    Console.WriteLine(i);
            }
            Console.WriteLine("Total profit: {0}", totalProfit);
        }
        /// <summary>
        /// Looks for pairs of photos having profit > 0
        /// </summary>
  
        #region files
        public static string pathA = @"D:\HashCode2019\a_example.txt";
        public static string pathB = @"D:\HashCode2019\b_lovely_landscapes.txt";
        public static string pathC = @"D:\HashCode2019\c_memorable_moments.txt";
        public static string pathD = @"D:\HashCode2019\d_pet_pictures.txt";
        public static string pathE = @"D:\HashCode2019\e_shiny_selfies.txt";

        public static string path = @"D:\HashCode2019\";
        #endregion

    }
}
