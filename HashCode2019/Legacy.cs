using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCode2019
{
    static class Legacy
    {
        const int MaxGroupSize = 1000;
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
                Console.WriteLine("Done");
            }
            return answer;
        }
        static public List<Photo> CalculateTagGroup(List<Photo> photos, int tagNum)
        {
            var answer = new List<Photo>();
            Console.WriteLine("{1} X {0} Started", photos.Count, tagNum);
            answer = LegacyCalc(photos, tagNum);
            Console.WriteLine(">> {1} X {0} Finished", photos.Count, tagNum);
            return answer;
        }
        static List<Photo> ParallelCalculations(InputData input, List<int> tagNums)
        {
            Console.WriteLine("I am at parallel");
            var midAnswer = tagNums
                .Select((n) => {
                    int i = 0;
                    var splitGroups =
                        from photos in input.AllPhotos
                        where photos.TagNum == n
                        group photos by i++ / MaxGroupSize into part
                        select part.AsEnumerable();
                    return splitGroups.Select(subGroup => Tuple.Create(subGroup, n));
                })
                .SelectMany(group => group.Select(n => n))
                .AsParallel()
                .Select(tuple => CalculateTagGroup(tuple.Item1.ToList(), tuple.Item2))
                .ToList();
            var answer = midAnswer
                .SelectMany(ans => ans.Select(n => n))
                .ToList();
            return answer;
        }
        /*static List<Photo> ParallelCalculations2(InputData input, List<int> tagNums)
        {
            int i = 0;
            var sortedData = input.AllPhotos
                .OrderBy(photo => photo.TagNum);
            var splitGroups =
                 from photos in sortedData
                 group photos by i++ / MaxGroupSize into part
                  select part.AsEnumerable();
            var midAnswer = splitGroups     
                .AsParallel()
                .Select(group => LegacyCalc(group))
                .ToList();
            var answer = midAnswer
                .SelectMany(ans => ans.Select(n => n))
                .ToList();
            return answer;
        }
        */
        public static void Algo(string testPath, string ansPath)
        {
            var input = new InputData();
            input.Read(testPath);
            Console.WriteLine(input.Count);
            var tagNums1 = input.AllPhotos
               .Select(photo => photo.TagNum)
               .Distinct()
               .OrderByDescending(tagNum => tagNum)
               .ToList();
            List<List<Photo>> answer = Enumerable.Repeat(new List<Photo>(), tagNums1.Count).ToList();
            var ans = ParallelCalculations(input, tagNums1);
            //List<Photo> ans = RandomPermutation(input.AllPhotos).ToList();
            Tools.SaveAnswer(ans, ansPath);
            DataAnalyzer.AnalyzeAnswer(testPath, ansPath);
        }
    }
}
