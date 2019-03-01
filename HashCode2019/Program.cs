using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCode2019
{
    class Program
    {
        #region files
        static string pathA = @"D:\HashCode2019\a_example.txt";
        static string pathB = @"D:\HashCode2019\b_lovely_landscapes.txt";
        static string pathC = @"D:\HashCode2019\c_memorable_moments.txt";
        static string pathD = @"D:\HashCode2019\d_pet_pictures.txt";
        static string pathE = @"D:\HashCode2019\e_shiny_selfies.txt";

        static string ansA = @"D:\HashCode2019\a_ans.txt";
        static string ansB = @"D:\HashCode2019\b_ans.txt";
        static string ansC = @"D:\HashCode2019\c_ans.txt";
        static string ansD = @"D:\HashCode2019\d_ans.txt";
        static string ansE = @"D:\HashCode2019\e_ans.txt";
        #endregion

        static void PrintData(string path)
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
            Console.WriteLine("AVG tag num = {0}", (int)(tagNum/size));
            Console.WriteLine("Max tag size = {0}", MaxTagSize);
        }

        static void LegacyDataPrint()
        {
            PrintData(pathA);
            PrintData(pathB);
            PrintData(pathC);
            PrintData(pathD);
            PrintData(pathE);
        }       

        static List<Photo> LegacyCalc(IEnumerable<Photo> Photos, int tagNum)
        {
            var answer = new List<Photo>();
            int halfTagNum = tagNum / 2;
            bool foundMatch = false;
            try
            {
                while (true)
                {
                    Photo current = Photos.Where(val => val.IsUsed == false).First();
                    current.IsUsed = true;
                    answer.Add(current);
                    while (true)
                    {
                        foreach (var p in Photos)
                            if (!p.IsUsed && 
                                Math.Abs(tagNum - Tools.CountSameTags(current, p) - halfTagNum) <= 1)
                            {
                                foundMatch = true;
                                answer.Add(p);
                                p.IsUsed = true;
                                current = p;
                                break;
                            }
                        if (!foundMatch)
                            break;
                      }
                }
            }
            catch
            {
                Console.WriteLine("Finished for {0}", tagNum);
            }
            return answer;
        }

        public static Task<List<Photo>> CalculateTagGroupTask()
        {
            return Task.Run(() =>
            {
                List<Photo> answer = new List<Photo>();
                return answer;

            });
        }
        static void AsyncCalculations(InputData input, List<TagInfo> tagNums )
        {
            var tasks = new List<Task>();
            for (int i = 0; i < tagNums.Count; i++)
            {
                tasks.Add(CalculateTagGroupTask());
            }
            Task.WaitAll(tasks.ToArray());
        }

        static public List<Photo> CalculateTagGroup(List<Photo> photos, int tagNum)
        {
            var answer = new List<Photo>();
            Console.WriteLine("Began calculations for {0} photos with tag number {1}", photos.Count, tagNum);
            answer = LegacyCalc(photos, tagNum);
            Console.WriteLine("Finished {0} photos with tag number {1}", photos.Count, tagNum);
            return answer;
        }
        static List<Photo> ParallelCalculations(InputData input, List<int> tagNums)
        {
            Console.WriteLine("I am at parallel");
            //            var midAnswer = Enumerable.Repeat(new List<Photo>(), tagNums.Count).ToList();
            var midAnswer = tagNums
                .Select(n => Tuple.Create(input.AllPhotos.Where(photo => photo.TagNum == n), n))
                .AsParallel()
                .Select(tuple => CalculateTagGroup(tuple.Item1.ToList(), tuple.Item2))
                .ToList();
            var answer = midAnswer
                .SelectMany(ans => ans.Select(n=>n))
                .ToList();
            return answer;
        }

        public struct TagInfo
        {
            public int tagNum;
            public int index;
        };

        static void Main()
        {
            var input = new InputData();
            input.Read(pathB);
            Console.WriteLine(input.Count);

            /*var tagNums = input.AllPhotos
                .Select((photo, i) => new TagInfo { tagNum = photo.TagNum, index = i })
                .Distinct()
                .OrderByDescending(tagNum => tagNum)
                .ToList();*/
            var tagNums1 = input.AllPhotos
               .Select(photo =>photo.TagNum)
               .Distinct()
               .OrderByDescending(tagNum => tagNum)
               .ToList();
            List<List<Photo>> answer = Enumerable.Repeat(new List<Photo>(), tagNums1.Count).ToList();
            var ans = ParallelCalculations(input, tagNums1);
            Tools.PrintAnswer(ans, ansB);
            Console.ReadLine();
        }
    }
}
