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
        public bool IsHorizontal { get; private set; }
        public int Index { get; private set; }
        public int NumOfTags { get; private set; }
        public long Min { get; private set; }
        public long Max { get; private set; }
        public List<long> IntTags { get; private set; }
        public List<string> StringTags { get; private set; }
        #endregion

        public Photo(int index)
        {
            IntTags = new List<long>();
            StringTags = new List<string>();
            Index = index;           
        }

        public void ParseLine(string line)
        {
            string[] data = line.Split(' ');
            IsHorizontal = data[0] == "H" ? true : false;
            NumOfTags = Convert.ToInt32(data[1]);
            for (int i = 2; i < data.Count(); i++)
            {
                StringTags.Add(data[i]);
                IntTags.Add(Tools.TagToInt(data[i]));
            }
            IntTags = IntTags.OrderBy(num => num).ToList();
            Min = IntTags.First();
            Max = IntTags.Last();
        }

    }
}