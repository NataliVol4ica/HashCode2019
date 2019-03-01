using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HashCode2019
{
    class Photo
    {
        #region Properties
        public int Index { get; private set; }
        public List<long> IntTags { get; private set; }
        public List<string> StringTags { get; private set; }
        public bool IsHorizontal { get; private set; }
        public List<Photo>  Neighbours { get; private set; }
        public long Min { get; private set; }
        public long Max { get; private set; }
        public int TagNum { get; private set; }
        public bool IsUsed { get; set; } // legacy
        #endregion

        public Photo(int index)
        {
            IntTags = new List<long>();
            StringTags = new List<string>();
            IsUsed = false;
            Index = index;           
        }
        public void ParseLine(string line)
        {
            string[] data = line.Split(' ');
            IsHorizontal = data[0] == "H" ? true : false;
            TagNum = Convert.ToInt32(data[1]);
            for (int i = 2; i < data.Count(); i++)
            {
                StringTags.Add(data[i]);
                IntTags.Add(TagToInt(data[i]));
            }
            IntTags = IntTags.OrderBy(num => num).ToList();
            Min = IntTags[0];
            Max = IntTags[IntTags.Count - 1];
        }

        public void FoundNeighbour(Photo p)
        {
            Neighbours.Add(p);
        }       
        #region static
        static long TagToInt(string tag)
        {
            long ans = 0;
            foreach (char c in tag)
                ans = (ans << 8) + (int)c;
            return ans;
        }
        #endregion
    }
}