using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace HashCode2019
{
    //TODO
    //inside the group:
    //find number of each tag uniqueity
    //count uniqueity coefficient for every photo
    //order them by


    class Chain
    {
        List<int> photos;
    }
    class Program
    {
        //greedy algo        
        public static void GreedyAlgo()
        {
            //order vertexes by neighbours number.
            //get first unadded
            //width search. Find the most farther vertex.
            //[expensive improvement]find the least busy path to there
               //from pathes of fixed length by returning back in generations
            //remember the path, mark path vertexes as busy
            //if pow(vertex) > 2 do another width search and concat two pathes
        }
        static void Main()
        {
            Console.WriteLine("Press any key");
            Console.Read();
        }
    }
}
