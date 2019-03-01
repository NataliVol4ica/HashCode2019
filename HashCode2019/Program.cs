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
    

        //for links
        //count num of how many times meets every vershina. if both 1 - put anywhere. ...

    class Link
    {
        public int slide1;
        public int slide2;
        public int interest;
        public Link(int slide1, int slide2, int interest)
        {
            this.slide1 = slide1;
            this.slide2 = slide2;
            this.interest = interest;
        }
        public override string ToString()
        {
            return String.Format("{0} {1} {2}", slide1, slide2, interest);
        }
    }
    class Chain
    {
        List<int> slides;
    }
    class Program
    {
        public static void Algo(string testPath, string ansPath)
        {
            var input = new InputData();
            input.Read(testPath);
            input.OrderPhotosByFirst();

            var links = new List<Link>();
            int interest;
            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < input.AllPhotos.Count - 1; i++)
            {
                for (int j = i + 1; j < input.AllPhotos.Count; j++)
                {
                    var left = input.AllPhotos[i];
                    var right = input.AllPhotos[j];
                    //because photo array is ordered by first tag
                    if (left.Max < right.Min)
                        break;
                    if ((interest = Tools.CountInterest(left, right)) > 0)
                        links.Add(new Link(left.Index, right.Index, interest));
                }
                if (i % 100 == 1)
                {
                    sw.Stop();
                    Console.WriteLine("{0} / {1} | {2}", i, input.AllPhotos.Count - 1, sw.Elapsed);
                    sw.Restart();
                }
            }
            Console.WriteLine("Ordering the link list");
            links = links.OrderByDescending(link => link.interest).ToList();
            Console.WriteLine("Writing to file");
            Tools.SaveLinkList(DataAnalyzer.linksB, links);
            //Console.WriteLine(input.Count);           
            //DataAnalyzer.AnalyzeAnswer(testPath, ansPath);
        }
        static void Main()
        {
            //Legacy.Algo(DataAnalyzer.pathB, DataAnalyzer.ansB);
            Algo(DataAnalyzer.pathB, DataAnalyzer.ansB);
            Console.WriteLine("Press any key");
            Console.Read();
        }


       

    }
}
