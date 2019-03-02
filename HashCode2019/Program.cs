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
        public static void GreedyAlgo(LinkData linkData)
        {
            //order vertexes by neighbours number. +
            //get first unadded
            //width search. Find the biggest profit || most farther vertex.
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
                //call width again
                //mark as checked
                //combine pathes
                //save result to the answer list
            }
            Console.WriteLine("Finished");
        }
        static void Main()
        {
            var linkData = new LinkData(DataAnalyzer.linksB, 80000);
            GreedyAlgo(linkData);
            Console.WriteLine("Press any key");
            Console.Read();
        }
    }
}
