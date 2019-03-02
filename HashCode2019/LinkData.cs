using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HashCode2019
{
    class LinkData
    {
        #region vars
        private object intLinksMutex = new object();
        private object repeatsMutex = new object();
        public readonly int VertexNum;
        private List<List<int>> _intLinks = null;
        #endregion

        #region Properties
        private List<Pair> _repeats = null;
        public List<Link> Links { get; private set; } = new List<Link>();
        public List<List<int>> IntLinks
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
        public List<Pair> Repeats
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
        public LinkData(int vertexNum)
        {
            VertexNum = vertexNum;
        }
        public LinkData(string path, int vertexNum)
        {
            VertexNum = vertexNum;
            Links = LinkListFromFile(path);
        }
        #endregion

        public void AddLink(Link l)
        {
            Links.Add(l);
        }
        private void LinksToListOfLists()
        {
            _intLinks = Enumerable.Repeat(new List<int>(), 80000).ToList();
            foreach (var link in Links)
            {
                IntLinks[link.slide1].Add(link.slide2);
                IntLinks[link.slide2].Add(link.slide1);
            }
        }
        private void CountRepeats()
        {
            var flatRepeats = Enumerable.Repeat(0, VertexNum).ToList();
            foreach (var link in Links)
            {
                flatRepeats[link.slide1]++;
                flatRepeats[link.slide2]++;
            }
            _repeats = new List<Pair>();
            for (int i = 0; i < VertexNum; i++)
                _repeats.Add(new Pair { index = i, amount = flatRepeats[i] });
            _repeats = _repeats
                .OrderBy(pair => pair.amount)
                .ThenBy(pair => pair.index)
                .ToList();
        }
        public void PrintRepeatsToTemp()
        {
            using (StreamWriter sw = new StreamWriter(@"D:\HashCode2019\temp.txt"))
            {
                sw.WriteLine(_repeats.Count);
                for (int i = 0; i < 80000; i++)
                    sw.WriteLine("{0} x {1}", _repeats[i].index, _repeats[i].amount);
            }
        }

        public static void OrderLinks(string path)
        {
            var links = LinkListFromFile(path);
            foreach(var link in links)
            {
                if (link.slide1 > link.slide2)
                    Tools.Swap(ref link.slide1, ref link.slide2);
            }
            Tools.SaveLinkList(path,
                links
                .OrderBy(link => link.interest)
                .ThenBy(link => link.slide1)
                .ThenBy(link => link.slide2)
                .ToList());
        }
        public static List<Link> LinkListFromFile(string path)
        {
            var links = new List<Link>();
            using (StreamReader sr = new StreamReader(path))
            {
                int lineNumber = Convert.ToInt32(sr.ReadLine());
                for (int i = 0; i < lineNumber; i++)
                {
                    links.Add(new Link(
                        sr.ReadLine()
                        .Split(' ')
                        .Where(val => !String.IsNullOrEmpty(val))
                        .Select(p => Convert.ToInt32(p))
                        .ToArray()));
                }
            }
            return links;
        }
    }
}