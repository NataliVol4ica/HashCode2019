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
        static Random random = new Random();
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
        public static int CountSameTags(Photo left, Photo right)
        {
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
            int leftDif = p1.NumOfTags - intersec;
            int rightDif = p2.NumOfTags - intersec;
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
