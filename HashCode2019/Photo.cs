using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HashCode2019
{
    class Photo : TagContainer
    {
        #region Properties
        public bool IsHorizontal { get; private set; }
        public int Index { get; private set; }      
        public List<string> StringTags { get; private set; }
        #endregion

        public Photo(int index)
        {
            Tags = new List<long>();
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
                Tags.Add(Tools.TagToInt(data[i]));
            }
            Tags = Tags.OrderBy(num => num).ToList();
            Min = Tags.First();
            Max = Tags.Last();
        }

    }
}