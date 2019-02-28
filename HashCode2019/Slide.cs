using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HashCode2019
{
    class Slide
    {
        public bool Orientation { get; private set; }

        public List<long> Tags { get; private set; }

        public Photo HPhoto { get; private set; }
        public List<Photo> VPhotos { get; set; }
    }
}