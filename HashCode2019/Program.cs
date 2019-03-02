using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace HashCode2019
{
    class Program
    {
        public enum State
        {
            unvisited = 0,
            enqueued,
            visited
        }
        public class VertexData: IComparable<VertexData>
        {
            public int index;
            public State state;
            public int totalInterest;
            public int parent;
            public bool isTaken;
            public VertexData(int index, State state, int totalInterest, int parent)
            {
                this.isTaken = false;
                this.index = index;
                this.state = state;
                this.totalInterest = totalInterest;
                this.parent = parent;
            }
            public int CompareTo(VertexData right) //???
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

        public static List<VertexData> GetVertexDataWithMaxProfitValues(int first, LinkData linkData, List<VertexData> vertexDatas)
        {
          
            VertexData currentVertex;
            var queue = new SortedSet<VertexData>();
            queue.Add(vertexDatas[first]);
            vertexDatas[first].state = State.enqueued;
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
                    if (vertexDatas[neighbour.slideIndex].isTaken|| 
                        vertexDatas[neighbour.slideIndex].state == State.visited)
                        continue;
                    if (vertexDatas[neighbour.slideIndex].state == State.unvisited)
                    {
                        queue.Add(vertexDatas[neighbour.slideIndex]);
                        vertexDatas[neighbour.slideIndex].state = State.enqueued;
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
                vertexDatas[currentVertex.index].state = State.visited;
            }
            return vertexDatas;
        }
        public static List<int> FindBestPath(int first, LinkData linkData, List<VertexData> vertexDatas)
        {
            Console.WriteLine("Searching path for {0}", first);
            var ans = new List<int>();
            vertexDatas = GetVertexDataWithMaxProfitValues(first, linkData, vertexDatas);
            int curVertex = -1;
            int max = -1;
            for (int i = 0; i < vertexDatas.Count; i++)
                if (vertexDatas[i].totalInterest > max)
                {
                    curVertex = i;
                    max = vertexDatas[i].totalInterest;
                }
            ans.Add(curVertex);
            vertexDatas[curVertex].isTaken = true;
            while (curVertex != first)
            {
                curVertex = vertexDatas[curVertex].parent;
                ans.Add(curVertex);
                vertexDatas[curVertex].isTaken = true;
            }
            ans.Reverse();
            Console.WriteLine("Search complete.");
            return ans;
        }
        public static List<int> GreedyAlgo(LinkData linkData)
        {
            var vertexDatas = new List<VertexData>();
            for (int j = 0; j < linkData.VertexNum; j++)
                vertexDatas.Add(new VertexData(j, State.unvisited, 0, -1));
            //if pow(vertex) > 2 do another width search and concat two pathes
            int i = 0;
            int untaken = 0;
            var ans = new List<int>();
            while (true)
            {
                //find first unchecked vertex
                while (i < linkData.VertexNum)
                {
                    if (!vertexDatas[i].isTaken)
                    {
                        untaken = i;
                        break;
                    }
                    i++;
                }
                //if no unchecked vertexes left
                if (i == linkData.VertexNum)
                    break;
                //if vertex has no profitable neighbours
                if (linkData.Repeats[untaken].amount < 1)
                {
                    vertexDatas[untaken].isTaken = true;
                    i++;
                    continue;
                }
                //find first chain
                var oneChain = FindBestPath(untaken, linkData, vertexDatas);
                if (linkData.Repeats[untaken].amount == 1)
                {
                    ans.AddRange(oneChain);
                    i++;
                    continue;
                }
                //if it has two ways, reverse it
                oneChain.Reverse();
                oneChain.RemoveAt(oneChain.Count - 1);
                vertexDatas[untaken].isTaken = false;
                //and find another chain
                var secondchain = FindBestPath(untaken, linkData, vertexDatas);
                ans.AddRange(secondchain);
            }
            Console.WriteLine("Finished");
            return ans;
        }
        static void Main()
        {
            var linkData = new LinkData(DataAnalyzer.linksB, 80000);
            var result = GreedyAlgo(linkData);
            Tools.SaveIntAnswer(result, DataAnalyzer.ansB);
            Console.WriteLine("Press any key");
            Console.Read();
        }
    }
}
