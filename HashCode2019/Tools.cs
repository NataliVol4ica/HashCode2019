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

        public static void PrintAnswer(List<Photo> ans, string path)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(ans.Count);
                foreach (var a in ans)
                    sw.WriteLine(a.Index);
            }
        }
    }
}
