using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCode2019
{
    abstract class TagContainer
    {
        public int NumOfTags { get; protected set; }
        public long Min { get; protected set; }
        public long Max { get; protected set; }
        public List<long> Tags { get; protected set; }
    }
}
