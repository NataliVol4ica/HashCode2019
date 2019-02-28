using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HashCode2019
{
    class Photo
    {
        public int Index { get; private set; }
        public List<string> StringTags { get; private set; }
        public List<long> IntTags { get; private set; }

        public bool IsHorizontal { get; private set; }
        public bool IsUsed { get; set; }

        public Photo(string type, int index)
        {
            IsUsed = false;
            Index = index;
            if (type == "H")
                IsHorizontal = true;
            else
                IsHorizontal = false;
            IntTags = new List<long>();
            StringTags = new List<string>();
        }
    }
}