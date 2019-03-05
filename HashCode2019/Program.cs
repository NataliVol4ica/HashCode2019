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
            string path = DataAnalyzer.pathB;
            string testname = path[16].ToString();
            Slideshow s = new Slideshow(testname);
            /*InputData id = new InputData();
            id.Read(path);
            s.ParseDataIntoSlides(id);
            s.OrderSlidesByFirstTag();
            s.SaveSlidesToFile();
            //*/
            s.ReadSlidesFromFile();
            LinkData ld = new LinkData(testname, s.VertexNum);
            int trouble = 0;
            foreach (var link in ld.Links)
            {
                int interestLink = Tools.CountInterest(s.Slides[link.slide1], s.Slides[link.slide2]);
                if (interestLink != link.interest)
                    trouble++;
                    //Console.WriteLine("Interest {0} Nominated {1}", interestLink, ld.Links[0].interest);
                //ld.SaveLinksToFile();
            }
            Console.WriteLine("Troubles {0}", trouble);
            /*
            s.GenerateSlideShow(ld);
            s.SaveAnsToFile();
            //*/
            /*
            
            /*var linkData = new LinkData(DataAnalyzer.linksB, 80000);
            var result = GreedyAlgo(linkData);
            Tools.SaveIntAnswer(result, DataAnalyzer.ansB);
            DataAnalyzer.AnalyzeAnswer(DataAnalyzer.pathB, DataAnalyzer.ansB);*/
            Console.WriteLine("Press any key");
            Console.Read();
        }
    }
}
