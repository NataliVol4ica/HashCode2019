using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HashCode2019
{
    //todo: make it contain both one slide and two slides
    //todo: make all algorithms work only with slides
    class Slide
    {
        static int _indexCounter = 0;
        public int Index { get; private set; }
        public int NumOfTags { get; private set; }
        public long Min { get; private set; }
        public long Max { get; private set; }
        public List<long> IntTags { get; private set; }

        public enum SlideOrientation
        {
            Horizontal = 0,
            Vertical
        }
        public SlideOrientation Orientation { get; private set; }
        public List<long> Tags { get; private set; }
        public List<int> Photos { get; private set; }
        public Slide(Photo p)
        {
            Index = _indexCounter++;
            Orientation = SlideOrientation.Horizontal;
            Photos = new List<int>() { p.Index };
            IntTags = new List<long>(p.IntTags);
            NumOfTags = p.NumOfTags;
            Min = p.Min;
            Max = p.Max;
        }
        public Slide(Photo p1, Photo p2)
        {
            Index = _indexCounter++;
            Orientation = SlideOrientation.Vertical;
            Photos = new List<int>() { p1.Index, p2.Index };
            IntTags = p1.IntTags.Union(p2.IntTags).OrderBy(tag => tag).ToList();
            NumOfTags = IntTags.Count;
            Min = IntTags.First();
            Max = IntTags.Last();
        }
        public Slide(SlideOrientation orient, List<int> photos, int tagNum, List<long> tags)
        {
            Orientation = orient;
            NumOfTags = tagNum;
            IntTags = tags;
            Photos = photos;
        }

        public string ToLogString()
        {
            string str;
            str = Orientation == SlideOrientation.Horizontal ? "H " : "V ";
            str += this.ToString();
            str += " " + IntTags.Count + " ";
            foreach (var tag in IntTags)
                str += tag + " ";
            return str;
        }
        public override string ToString()
        {
            if (Photos.Count == 1)
                return Photos[0].ToString();
            return Photos[0].ToString() + " " + Photos[1].ToString();
        }
    }
}