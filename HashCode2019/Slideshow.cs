using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCode2019
{
    class Slideshow
    {
        #region Nested stuff
        public enum VertexState
        {
            unvisited = 0,
            enqueued,
            visited
        }
        public class VertexData : IComparable<VertexData>
        {
            public int index;
            public VertexState state;
            public int totalInterest;
            public int parent;
            public bool isTaken;
            public VertexData(int index, VertexState state, int totalInterest, int parent)
            {
                this.isTaken = false;
                this.index = index;
                this.state = state;
                this.totalInterest = totalInterest;
                this.parent = parent;
            }
            public int CompareTo(VertexData right) 
            {
                if (index == right.index)
                    return 0;
                int cmp = totalInterest - right.totalInterest;
                if (cmp == 0)
                    cmp = index - right.index;
                return cmp;
            }
            public override string ToString()
            {
                return index + " " + state + " " + totalInterest;
            }
        }
        #endregion

        private readonly string fileName;
        public int VertexNum { get; private set; }
        public List<Slide> Slides { get; private set; }
        public List<int> SlideShow { get; private set; }
        public Slideshow(string testName)
        {
            //linkData = new LinkData();
            fileName = testName;
            Slides = new List<Slide>();
            SlideShow = new List<int>();
        }
        /// <summary>
        /// Parses horizontal and vertical photos into slides
        /// </summary>
        public void ParseDataIntoSlides(InputData data)
        {
            Slides.AddRange(data.Horizontals.Select(photo => new Slide(photo)).ToList());
            data.Verticals = data.Verticals
                .OrderByDescending(photo => photo.NumOfTags)
                .ThenByDescending(photo => photo.Min)
                .ThenByDescending(photo => photo.Max)
                .ToList();
            var added = Enumerable.Repeat(false, data.Verticals.Count).ToList();
            //add slides with min difference
            for (int i = 0; i < data.Verticals.Count - 1; i++)
            {
                if (added[i])
                    continue;
                int minFit = data.Verticals[i].NumOfTags + 1;
                int fitIndex = -1;
                for (int j = i + 1; j < data.Verticals.Count; j++)
                {
                    if (added[j])
                        continue;
                    if (data.Verticals[i].Min > data.Verticals[j].Max)
                    {
                        minFit = -1;
                        added[i] = true;
                        added[j] = true;
                        Slides.Add(new Slide(data.Verticals[i], data.Verticals[j]));
                        break;
                    }
                    int sameTags = Tools.CountSameTags(data.Verticals[i], data.Verticals[j], minFit);
                    if (sameTags >= 0 && sameTags < minFit)
                    {
                        minFit = sameTags;
                        fitIndex = j;
                        if (sameTags == 0)
                            break;
                    }
                }
                if (minFit >= 0 && minFit <= data.Verticals[i].NumOfTags)
                {
                    added[i] = true;
                    added[fitIndex] = true;
                    Slides.Add(new Slide(data.Verticals[i], data.Verticals[fitIndex]));
                }
            }
            VertexNum = Slides.Count;
        }
        public void OrderSlidesByFirstTag()
        {
            Slides = Slides.OrderBy(slide => slide.Tags[0]).ToList();
        }

        #region Algo       
        public void SaveSlidesToFile()
        {
            string path = DataAnalyzer.path + fileName + "_slides.txt";
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(Slides.Count);
                foreach (var slide in Slides)
                    sw.WriteLine(slide.ToLogString());
            }
        }
        public void SaveAnsToFile()
        {
            string path = DataAnalyzer.path + fileName + "_ans.txt";
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(Slides.Count);
                foreach (var slide in Slides)
                    sw.WriteLine(slide);
            }
        }
        public void ReadSlidesFromFile()
        {
            Slides = new List<Slide>();
            Slide.RestartIndexCounter();
            string path = DataAnalyzer.path + fileName + "_slides.txt";
            using (StreamReader sr = new StreamReader(path))
            {
                int lineNumber = Convert.ToInt32(sr.ReadLine());
                VertexNum = lineNumber;
                for (int i = 0; i < lineNumber; i++)
                {
                    var vals = sr.ReadLine()
                        .Split(' ')
                        .Where(val => !String.IsNullOrEmpty(val))
                        .ToList();
                    var photos = new List<int> { Convert.ToInt32(vals[1]) };
                    var orient = vals[0] == "H" ?
                        Slide.SlideOrientation.Horizontal :
                        Slide.SlideOrientation.Vertical; 
                    int from = vals[0] == "H" ? 2 : 3;
                    if (from == 3)
                        photos.Add(Convert.ToInt32(vals[2]));
                    int size = Convert.ToInt32(vals[from++]);
                    int j = 0;
                    var tags = vals.Where(str => j++ >= from).Select(str => Convert.ToInt64(str)).ToList();
                    Slides.Add(new Slide(orient, photos, size, tags));
                }
            }
        }

        private void FindMaxInterestVertex(List<VertexData> vertexDatas, out int vertex, out int max)
        {
            vertex = -1;
            max = -1;

            for (int i = 0; i < vertexDatas.Count; i++)
                if (vertexDatas[i].totalInterest > max)
                {
                    vertex = i;
                    max = vertexDatas[i].totalInterest;
                }
        }
        private int FindUntakenVertex(ref int i, LinkData linkData, List<VertexData> vertexDatas)
        {
            int untaken = -1;
            while (i < linkData.VertexNum)
            {
                if (!vertexDatas[i].isTaken)
                {
                    untaken = i;
                    break;
                }
                i++;
            }
            return untaken;
        }
        private void FillVertexDatasWithProfit(
            int first, LinkData linkData, List<VertexData> vertexDatas)
        {
            VertexData currentVertex;
            var queue = new SortedSet<VertexData>
            {
                vertexDatas[first]
            };
            vertexDatas[first].state = VertexState.enqueued;
            while (queue.Count > 0)
            {
                currentVertex = queue.Max();
                if (queue.Count % 100 == 0)
                    Console.WriteLine("Current {0} Max {1} Parent {2} | Queue size {3}",
                        currentVertex.index, currentVertex.totalInterest, currentVertex.parent,
                        queue.Count);
                foreach (var neighbour in linkData.IntLinks[currentVertex.index])
                {
                    //Console.WriteLine("Neighbour {0}", neighbour.slideIndex);
                    if (vertexDatas[neighbour.slideIndex].isTaken ||
                        vertexDatas[neighbour.slideIndex].state == VertexState.visited)
                        continue;
                    if (vertexDatas[neighbour.slideIndex].state == VertexState.unvisited)
                    {
                        queue.Add(vertexDatas[neighbour.slideIndex]);
                        vertexDatas[neighbour.slideIndex].state = VertexState.enqueued;
                    }
                    if (vertexDatas[currentVertex.index].totalInterest + neighbour.interest >
                        vertexDatas[neighbour.slideIndex].totalInterest)
                    {
                        queue.Remove(vertexDatas[neighbour.slideIndex]);
                        vertexDatas[neighbour.slideIndex].totalInterest =
                            vertexDatas[currentVertex.index].totalInterest + neighbour.interest;
                        queue.Add(vertexDatas[neighbour.slideIndex]);
                        vertexDatas[neighbour.slideIndex].parent = currentVertex.index;
                    }
                }
                if (!(queue.Remove(currentVertex)))
                {
                    Console.WriteLine(">>>>>>>>>>>> Beda <<<<<<<<<<<<<");
                }
                vertexDatas[currentVertex.index].state = VertexState.visited;
            }
        }
        private void FillVertexDatas(int first, LinkData linkData, List<VertexData> vertexDatas)
        {
            foreach (var vertex in vertexDatas)
            {
                vertex.state = VertexState.unvisited;
                vertex.parent = -1;
                vertex.totalInterest = 0;
            }
            FillVertexDatasWithProfit(first, linkData, vertexDatas);
        }
        private List<int> FindBestPath(
            int first, LinkData linkData, List<VertexData> vertexDatas, bool includeFirst)
        {
            //Console.WriteLine("Searching path for {0}", first);
            var ans = new List<int>();
            FillVertexDatas(first, linkData, vertexDatas);
            FindMaxInterestVertex(vertexDatas, out int curVertex, out int max);
            if (max == 0)
            {
                vertexDatas[first].isTaken = true;
                return (ans);
            }
            ans.Add(curVertex);
            vertexDatas[curVertex].isTaken = true;
            while (curVertex != first)
            {
                curVertex = vertexDatas[curVertex].parent;
                if (!(!includeFirst && curVertex == first))
                {
                    ans.Add(curVertex);
                    vertexDatas[curVertex].isTaken = true;
                }
                //Console.WriteLine("Added {0}", curVertex);
            }
            //Console.WriteLine("Search complete.");
            return ans;
        }
        /// <summary>
        /// Generates slideshow from local var AllSlides using greedy algo
        /// </summary>
        public void GenerateSlideShow(LinkData linkData)
        {
            var vertexDatas = new List<VertexData>();
            for (int j = 0; j < linkData.VertexNum; j++)
                vertexDatas.Add(new VertexData(j, VertexState.unvisited, 0, -1));
            int i = 0;
            while (true)
            {
                int untaken = FindUntakenVertex(ref i, linkData, vertexDatas);
                //if no unchecked vertexes left
                if (untaken == -1)
                    break;
                //find first chain
                var oneChain = FindBestPath(untaken, linkData, vertexDatas, (linkData.Repeats[untaken].amount == 1));
                SlideShow.AddRange(oneChain);
                vertexDatas[untaken].isTaken = false;
                //and find another chain
                var secondchain = FindBestPath(untaken, linkData, vertexDatas, true);
                secondchain.Reverse();
                SlideShow.AddRange(secondchain);
                vertexDatas[untaken].isTaken = true;
            }
            Console.WriteLine("Finished");
        }
        #endregion
    }
}
