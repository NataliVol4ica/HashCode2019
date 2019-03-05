using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCode2019
{
   //todo:
   //change saving path only to name of test
    static class Tools
    {
        private static readonly Random random = new Random();
        static int CharToInt(char c)
        {
            if (char.IsDigit(c))
                return (int)c - (int)'0';
            return (int)c - (int)'a' + 10;
        }
        public static long TagToInt(string tag)
        {
            long ans = 0;
            foreach (char c in tag)
                ans = (ans << 6) + Tools.CharToInt(c);
            return ans;
        }
        public static void Swap<T>(ref T a, ref T b)
        {
            T c = a;
            a = b;
            b = c;
        }
        public static int CountSameTags(TagContainer left, TagContainer right, int stopAt = -1)
        {
            int ans = 0;
            int i = 0, j = 0;
            while (i < left.Tags.Count && j < right.Tags.Count)
            {
                if (left.Tags[i] < right.Tags[j])
                    i++;
                else if (right.Tags[j] < left.Tags[i])
                    j++;
                else
                {
                    i++;
                    j++;
                    ans++;
                }
                if (ans == stopAt)
                    return -1;
            }
            return ans;
        }
        public static int CountInterest(TagContainer tc1, TagContainer tc2)
        {
            int intersec = CountSameTags(tc1, tc2);
            int leftDif = tc1.NumOfTags - intersec;
            int rightDif = tc2.NumOfTags - intersec;
            int min = intersec > leftDif ? leftDif : intersec;
            min = min > rightDif ? rightDif : min;
            return min;
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
    }
}
