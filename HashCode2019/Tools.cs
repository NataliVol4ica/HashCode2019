using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCode2019
{
    static class Tools
    {
        public static int CountSameTags(Photo p1, Photo p2)
        {
            if (p1.Max < p2.Min || p2.Max < p1.Min)
                return 0;
            int ans = 0;
            int i = 0, j = 0;
            while (i < p1.IntTags.Count && j < p1.IntTags.Count)
            {
                if (p1.IntTags[i] < p2.IntTags[j])
                    i++;
                else if (p2.IntTags[j] < p1.IntTags[i])
                    j++;
                else
                {
                    i++;
                    j++;
                    ans++;
                }
            }
            /*foreach (var tag1 in p1.IntTags)
                foreach (var tag2 in p2.IntTags)
                    if (tag1 == tag2)
                    {
                        ans++;
                        break;
                    }*/
            return ans;
        }

        public static void SaveAnswer(List<Photo> ans, string path)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(ans.Count);
                foreach (var a in ans)
                    sw.WriteLine(a.Index);
            }
        }

        public static int CountProfit(List<string> tags1, List<string> tags2)
        {
            int intersectCount = tags1.Intersect(tags2).Count();
            int leftDifCount = tags1.Except(tags2).Count();
            int rightDifCount = tags2.Except(tags1).Count();
            var values = new List<int> { intersectCount, leftDifCount, rightDifCount };
            return values.Min();
        }

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
    }
}
