using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCode2019
{
    struct Pair
    {
        public int index;
        public int amount;
    }

    static class Tools
    {
        public static int CharToInt(char c)
        {
            if (char.IsDigit(c))
                return (int)c - (int)'0';
            return (int)c - (int)'a' + 10;
        }
        public static int CountSameTags(Photo left, Photo right)
        {
            //todo:
            //if not enough checks left return ?
            int ans = 0;
            int i = 0, j = 0;
            while (i < left.IntTags.Count && j < right.IntTags.Count)
            {
                if (left.IntTags[i] < right.IntTags[j])
                    i++;
                else if (right.IntTags[j] < left.IntTags[i])
                    j++;
                else
                {
                    i++;
                    j++;
                    ans++;
                }
            }
            return ans;
        }

        public static int CountInterest(Photo p1, Photo p2)
        {
            int intersec = CountSameTags(p1, p2);
            int leftDif = p1.TagNum - intersec;
            int rightDif = p2.TagNum - intersec;
            int min = intersec > leftDif ? leftDif : intersec;
            min = min > rightDif ? rightDif : min;
            return min;
        }

        public static void SavePhotoAnswer(List<Photo> ans, string path)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(ans.Count);
                foreach (var a in ans)
                    sw.WriteLine(a.Index);
            }
        }
        public static void SaveIntAnswer(List<int> ans, string path)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(ans.Count);
                foreach (var a in ans)
                    sw.WriteLine(a);
            }
        }

        public static void Swap<T>(ref T a, ref T b)
        {
            T c = a;
            a = b;
            b = c;
        }

        static Random random = new Random();
        public static IEnumerable<T> RandomPermutation<T>(IEnumerable<T> sequence)
        {
            T[] retArray = sequence.ToArray();


            for (int i = 0; i < retArray.Length - 1; i += 1)
            {
                int swapIndex = random.Next(i, retArray.Length);
                if (swapIndex != i)
                {
                    T temp = retArray[i];
                    retArray[i] = retArray[swapIndex];
                    retArray[swapIndex] = temp;
                }
            }

            return retArray;
        }
        public static void SaveLinkList(string path, List<Link> links)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(links.Count);
                foreach (var link in links)
                    sw.WriteLine(link);
            }
        }
    }
}
