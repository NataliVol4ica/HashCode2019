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
        public class VertexData: IComparer<VertexData>
        {
            public int index;
            public State state;
            public int totalInterest;
            public int parent;
            public VertexData(int index, State state, int totalInterest, int parent)
            {
                this.index = index;
                this.state = state;
                this.totalInterest = totalInterest;
                this.parent = parent;
            }
            public int Compare(VertexData left, VertexData right)
            {
                return left.totalInterest - right.totalInterest;
            }
        }

        public static List<VertexData> GetVertexDataWithMaxProfitValues(int first, LinkData linkData)
        {
            int i = 0;
            var vertexDatas = Enumerable.Repeat(
                new VertexData
                (i++, State.unvisited, 0, -1), linkData.VertexNum)
                .ToList();
            VertexData currentVertex;
            var queue = new SortedSet<VertexData>();
            queue.Add(vertexDatas[first]);
            vertexDatas[first].state = State.enqueued;
            vertexDatas[first].totalInterest = 7;
            while (queue.Count > 0)
            {
                currentVertex = queue.Max();
                Console.WriteLine("Current {0} Max {1} Parent {2} | Queue size {3}",
                    currentVertex.index, currentVertex.totalInterest, currentVertex.parent,
                    queue.Count);
                foreach (var neighbour in linkData.IntLinks[currentVertex.index])
                {
                    if (vertexDatas[neighbour.slideIndex].state == State.visited)
                        continue;
                    if (vertexDatas[neighbour.slideIndex].state == State.unvisited)
                    {
                        queue.Add(vertexDatas[neighbour.slideIndex]);
                        vertexDatas[neighbour.slideIndex].state = State.enqueued;
                    }
                    if (vertexDatas[currentVertex.index].totalInterest + neighbour.interest >
                        vertexDatas[neighbour.slideIndex].totalInterest)
                    {
                        vertexDatas[neighbour.slideIndex].totalInterest =
                            vertexDatas[currentVertex.index].totalInterest + neighbour.interest;
                        vertexDatas[neighbour.slideIndex].parent = currentVertex.index;
                    }
                }
                queue.Remove(currentVertex);
                vertexDatas[currentVertex.index].state = State.visited;
            }
            return vertexDatas;
        }
        public static List<int> FindBestPath(int first, LinkData linkData)
        {
            Console.WriteLine("Searching path for {0}", first);
            var ans = new List<int>();
            var vertexDatas = GetVertexDataWithMaxProfitValues(first, linkData);
            int curVertex = 0;
            int max = vertexDatas[0].totalInterest;
            for (int i = 1; i < vertexDatas.Count; i++)
                if (vertexDatas[i].totalInterest > max)
                {
                    curVertex = i;
                    max = vertexDatas[i].totalInterest;
                }
            ans.Add(curVertex);
            while (curVertex != first)
            {
                curVertex = vertexDatas[curVertex].parent;
                ans.Add(curVertex);
            }
            ans.Reverse();
            Console.WriteLine("Search complete.");
            return ans;
        }
        public static void GreedyAlgo(LinkData linkData)
        {
            //order vertexes by neighbours number. +
            //get first unadded
            //depth search. Find the biggest profit || most farther vertex.
            //[expensive improvement]find the least busy path to there
            //from pathes of fixed length by returning back in generations
            //remember the path, mark path vertexes as busy
            //if pow(vertex) > 2 do another width search and concat two pathes
            var checkd = Enumerable.Repeat(false, linkData.VertexNum).ToList();
            int i = 0;
            int curIdx = 0;
            while (true)
            {
                //find first unchecked vertex
                while (i < linkData.VertexNum)
                {
                    if (!checkd[i])
                    {
                        curIdx = i;
                        break;
                    }
                    i++;
                }
                //no unchecked vertexes left
                if (i == linkData.VertexNum)
                    break;
                //if vertex has no profitable neighbours
                if (linkData.Repeats[curIdx].amount < 1)
                {
                    checkd[curIdx] = true;
                    i++;
                    continue;
                }
                checkd[curIdx] = true; //antiendlesscycle
                //find one chain
                //mark as checked
                //if more than 1 path comes through this vertex
                //if (linkData.Repeats[curIdx].amount > 1)
                //call profitCount again
                //mark as checked
                //combine pathes
                //save result to the answer list
            }
            Console.WriteLine("Finished");
        }
        static void Main()
        {
            var linkData = new LinkData(DataAnalyzer.linksB, 80000);
            //GreedyAlgo(linkData);
            var anyAns = FindBestPath(0, linkData);
            Tools.SaveIntAnswer(anyAns, DataAnalyzer.ansB);
            Console.WriteLine("Press any key");
            Console.Read();
        }
    }
}
