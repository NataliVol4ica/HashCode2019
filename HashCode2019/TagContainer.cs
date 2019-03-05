using System.Collections.Generic;

namespace HashCode2019
{
    abstract class TagContainer
    {
        public int NumOfTags { get; protected set; }
        public long Min { get; protected set; }
        public long Max { get; protected set; }
        /// <summary>
        /// Ordered tags
        /// </summary>
        public List<long> Tags { get; protected set; }
    }
}
