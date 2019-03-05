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
       static void Main()
        {
            string testname = "d";
            Slideshow s = new Slideshow(testname);
            InputData id = new InputData();
            id.Read(DataAnalyzer.pathD);
            s.ParseDataIntoSlides(id);
            s.OrderSlidesByFirstTag();
            s.SaveSlidesToFile();
            s.ReadSlidesFromFile();
            LinkData ld = new LinkData(testname, s);
            s.GenerateSlideShow(ld);
            s.SaveAnsToFile();
            //ld.SaveLinksToFile();


            /*var linkData = new LinkData(DataAnalyzer.linksB, 80000);
            var result = GreedyAlgo(linkData);
            Tools.SaveIntAnswer(result, DataAnalyzer.ansB);
            DataAnalyzer.AnalyzeAnswer(DataAnalyzer.pathB, DataAnalyzer.ansB);*/
            Console.WriteLine("Press any key");
            Console.Read();
        }
    }
}
