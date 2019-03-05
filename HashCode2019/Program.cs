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
            string testname = "b";
            Slideshow s = new Slideshow(testname);
            /*
             InputData id = new InputData();
            id.Read(DataAnalyzer.pathB);
            s.ParseDataIntoSlides(id);
            s.OrderSlidesByFirstTag();
            s.SaveSlidesToFile();
            */
            s.ReadSlidesFromFile();
            LinkData ld = new LinkData(testname, s);

            ld.SaveLinksToFile();
            /*s.GenerateSlideShow(ld);
            s.SaveAnsToFile();*/
            int interestLink = Tools.CountInterest(s.Slides[ld.Links[0].slide1], s.Slides[ld.Links[0].slide1]);
            Console.WriteLine("Interest {0} Nominated {1}", interestLink, ld.Links[0].interest);
            
            /*var linkData = new LinkData(DataAnalyzer.linksB, 80000);
            var result = GreedyAlgo(linkData);
            Tools.SaveIntAnswer(result, DataAnalyzer.ansB);
            DataAnalyzer.AnalyzeAnswer(DataAnalyzer.pathB, DataAnalyzer.ansB);*/
            Console.WriteLine("Press any key");
            Console.Read();
        }
    }
}
