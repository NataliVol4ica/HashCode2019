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

    struct Pair
    {
        public int index;
        public int amount;
    }
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
        public Link(int[] param_s)
        {
            slide1 = param_s[0];
            slide2 = param_s[1];
            interest = param_s[2];
        }
        public override string ToString()
        {
            return String.Format("{0} {1} {2} ", slide1, slide2, interest);
        }
    }
    class Chain
    {
        List<int> slides;
    }
    class Program
    {
        public static void FindLinks(string testPath, string ansPath)
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
        public static List<Link> ReadLinks(string path)
        {
            var links = new List<Link>();
            using (StreamReader sr = new StreamReader(path))
            {
                int lineNumber = Convert.ToInt32(sr.ReadLine());
                for (int i = 0; i < lineNumber; i++)
                {
                    links.Add(new Link(sr.ReadLine().Split(' ').Where(val => !String.IsNullOrEmpty(val)).Select(p => Convert.ToInt32(p)).ToArray()));
                }
            }
            return links;
        }
        
        public static void OrderLinks(string path)
        {
            var links = ReadLinks(path);
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
        public static void CountUniquePhotos(List<Link> links)
        {
            var repeats = Enumerable.Repeat(0, 80000).ToList();
            foreach (var link in links)
            {
                repeats[link.slide1]++;
                repeats[link.slide2]++;
            }
            int total = 0;
            for (int i = 0; i < 80000; i++)
            {
                if (repeats[i] != 0)
                    total++;
            }
            
            var pairs = new List<Pair>();
            for (int i = 0; i < 80000; i++)
                pairs.Add(new Pair { index = i, amount = repeats[i] });
            pairs = pairs.OrderBy(pair => pair.amount).ThenBy(pair => pair.index).ToList();
            using (StreamWriter sw = new StreamWriter(@"D:\HashCode2019\temp.txt"))
            {
                sw.WriteLine(total);
                for (int i = 0; i < 80000; i++)
                {
                    sw.WriteLine("{0} x {1}", pairs[i].index, pairs[i].amount);
                }
            }
        }
        static void Main()
        {
            //Legacy.Algo(DataAnalyzer.pathB, DataAnalyzer.ansB);
            //FindLinks(DataAnalyzer.pathB, DataAnalyzer.ansB);
            OrderLinks(DataAnalyzer.linksB);
            var links = ReadLinks(DataAnalyzer.linksB);
            CountUniquePhotos(links);

            Console.WriteLine("Press any key");
            Console.Read();
        }
    }
}
