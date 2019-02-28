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
            int ans = 0;
            foreach (var tag1 in p1.IntTags)
                foreach (var tag2 in p2.IntTags)
                    if (tag1 == tag2)
                    {
                        ans++;
                        break;
                    }
            return ans;
        }

        public static void PrintAnswer(List<Photo> ans, string path)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(ans.Count);
                foreach (var a in ans)
                    Console.WriteLine(a.Index);
            }
        }
    }
}
