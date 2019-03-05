using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Text;

namespace HashCode2019
{
    /// <summary>
    /// Info about all provided links
    /// </summary>
    class LinkData
    {
        #region Nested Stuff
        /// <summary>
        /// Info about number of repeats of vertexes
        /// </summary>
        public struct RepeatInfo
        {
            public int index;
            public int amount;
        }
        /// <summary>
        /// Info about slide interest by index
        /// </summary>
        public class IntLinkData : IComparable<IntLinkData>
        {
            public int slideIndex;
            public int interest;
            public IntLinkData(int idx, int inter)
            {
                slideIndex = idx;
                interest = inter;
            }
            public int CompareTo(IntLinkData right)
            {
                int cmp = right.interest - this.interest;
                if (cmp == 0)
                    cmp = this.slideIndex - right.slideIndex;
                return cmp;
            }
        }
        #endregion

        #region vars
        private readonly object intLinksMutex = new object();
        private readonly object repeatsMutex = new object();
        public readonly int VertexNum;
        private List<RepeatInfo> _repeats = null;
        private List<SortedSet<IntLinkData>> _intLinks = null;
        #endregion

        #region Properties
        public string fileName;
        public List<Link> Links { get; private set; }
        public List<SortedSet<IntLinkData>> IntLinks
        {
            get
            {
                lock (intLinksMutex)
                {
                    if (_intLinks is null)
                        LinksToListOfLists();
                }
                return _intLinks;
            }
        }
        public List<RepeatInfo> Repeats
        {
            get
            {
                lock (repeatsMutex)
                {
                    if (_repeats is null)
                        CountRepeats();
                }
                return _repeats;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Reads links from file
        /// </summary>
        public LinkData(string testName, int VertexNum)
        {
            Links = new List<Link>();
            fileName = testName;
            this.VertexNum = VertexNum;
            ReadLinksFromFile();
        }
        /// <summary>
        /// Creates links from Slides
        /// </summary>        
        public LinkData(string testName, Slideshow slideshow)
        {
            Links = new List<Link>();
            fileName = testName;
            this.VertexNum = slideshow.Slides.Count;
            int interest;
            var sw = new Stopwatch();
            sw.Start();
            int i_s = 0;
            var indexedSlides = slideshow.Slides.Select(slide => new { Index = i_s++, Slide = slide }).ToList();
            var midLinks = indexedSlides
                .AsParallel()
                .Select(l =>
            {
                int i = l.Index;
                var left = l.Slide;
                var midList = new List<Link>();
                for (int j = i + 1; j < indexedSlides.Count; j++)
                {
                    var right = slideshow.Slides[j];
                    //because photo array is ordered by first tag
                    if (left.Max < right.Min)
                        break;
                    if ((interest = Tools.CountInterest(left, right)) > 0)
                        midList.Add(new Link(i, j, interest)); //change to i and j, or...
                }
                if (i % 100 == 1)
                {
                    sw.Stop();
                    Console.WriteLine("{0} / {1} | {2}", i, slideshow.Slides.Count - 1, sw.Elapsed);
                    sw.Restart();
                }
                i++;
                return midList;
            })
                .ToList();
            Console.WriteLine("Ordering the link list");
            Links = midLinks
                .SelectMany(list => list)
                .OrderByDescending(link => link.interest)
                .ToList();
            Console.WriteLine("Writing to file");
        }
        #endregion
        private void LinksToListOfLists()
        {
            Console.WriteLine("Creating int lists");
            _intLinks = new List<SortedSet<IntLinkData>>();
            for (int i = 0; i < VertexNum; i++)
                _intLinks.Add(new SortedSet<IntLinkData>());
            foreach (var link in Links)
            {
                IntLinks[link.slide1].Add(new IntLinkData(link.slide2, link.interest));
                IntLinks[link.slide2].Add(new IntLinkData(link.slide1, link.interest));
            }
            Console.WriteLine("Finished");
        }
        private void CountRepeats()
        {
            var flatRepeats = Enumerable.Repeat(0, VertexNum).ToList();
            foreach (var link in Links)
            {
                flatRepeats[link.slide1]++;
                flatRepeats[link.slide2]++;
            }
            _repeats = new List<RepeatInfo>();
            for (int i = 0; i < VertexNum; i++)
                _repeats.Add(new RepeatInfo { index = i, amount = flatRepeats[i] });
            _repeats = _repeats
                .OrderBy(pair => pair.amount)
                .ThenBy(pair => pair.index)
                .ToList();
        }

        #region IO
        private void ReadLinksFromFile()
        {
            string path = DataAnalyzer.path + fileName + "_links.txt";
            using (StreamReader sr = new StreamReader(path))
            {
                int lineNumber = Convert.ToInt32(sr.ReadLine());
                for (int i = 0; i < lineNumber; i++)
                {
                    Links.Add(new Link(
                        sr.ReadLine()
                        .Split(' ')
                        .Where(val => !String.IsNullOrEmpty(val))
                        .Select(p => Convert.ToInt32(p))
                        .ToArray()));
                }
            }
        }
        public void SaveRepeatsToFile()
        {
            string path = DataAnalyzer.path + fileName + "_repeats.txt";
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(_repeats.Count);
                for (int i = 0; i < 80000; i++)
                    sw.WriteLine("{0} x {1}", _repeats[i].index, _repeats[i].amount);
            }
        }
        public void SaveLinksToFile()
        {
            string path = DataAnalyzer.path + fileName + "_links.txt";
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(Links.Count);
                foreach (var link in Links)
                    sw.WriteLine(link);
            }
        }
        #endregion
    }
}