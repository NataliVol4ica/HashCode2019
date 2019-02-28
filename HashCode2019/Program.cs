using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCode2019
{
    class Program
    {
        static string pathA = @"D:\HashCode2019\a_example.txt";
        static string pathB = @"D:\HashCode2019\b_lovely_landscapes.txt";
        static string pathC = @"D:\HashCode2019\c_memorable_moments.txt";
        static string pathD = @"D:\HashCode2019\d_pet_pictures.txt";
        static string pathE = @"D:\HashCode2019\e_shiny_selfies.txt";

        static string ansA = @"D:\HashCode2019\a_ans.txt";
        static string ansB = @"D:\HashCode2019\b_ans.txt";
        static string ansC = @"D:\HashCode2019\c_ans.txt";
        static string ansD = @"D:\HashCode2019\d_ans.txt";
        static string ansE = @"D:\HashCode2019\e_ans.txt";
        static void PrintData(string path)
        {
            int size;
            int horizontals = 0;
            int verticals = 0;
            int tagNum = 0;
            int MaxTagSize = 0;
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                string line;
                size = Convert.ToInt32(sr.ReadLine());
                while ((line = sr.ReadLine()) != null)
                {
                    string[] data = line.Split(' ');
                    if (data[0] == "H")
                        horizontals++;
                    else
                        verticals++;
                    tagNum += Convert.ToInt32(data[1]);
                    for (int i = 2; i < data.Count(); i++)
                        if (data[i].Length > MaxTagSize)
                            MaxTagSize = data[i].Length;
                }
            }
            Console.WriteLine("Path {0}", path);
            Console.WriteLine("Test size = {0}", size);
            Console.WriteLine("Horizontals = {0}", horizontals);
            Console.WriteLine("Verticals = {0}", verticals);
            Console.WriteLine("AVG tag num = {0}", (int)(tagNum/size));
            Console.WriteLine("Max tag size = {0}", MaxTagSize);
        }

        static void LegacyDataPrint()
        {
            PrintData(pathA);
            PrintData(pathB);
            PrintData(pathC);
            PrintData(pathD);
            PrintData(pathE);
        }       


        static void CalcGroup(IEnumerable<Photo> Photos, List<Photo> answer, int tagNum)
        {
            Console.WriteLine("Group size is {0}", Photos.Count());
            int halfTagNum = tagNum / 2;
            bool foundMatch = false;
            /*foreach (var p in Photos)
                p.IsUsed = false;*/
            try
            {
                while (true)
                {
                    Photo current = Photos.Where(val => val.IsUsed == false).First();
                    current.IsUsed = true;
                    answer.Add(current);
                    while (true)
                    {
                        foreach (var p in Photos)
                            if (!p.IsUsed && 
                                Math.Abs(tagNum - Tools.CountSameTags(current, p) - halfTagNum) <= 1)
                            {
                                foundMatch = true;
                                answer.Add(p);
                                p.IsUsed = true;
                                current = p;
                                break;
                            }
                        if (!foundMatch)
                            break;
                      }
                }
            }
            catch
            {
                Console.WriteLine("Finished for {0}", /*Photos.First().IntTags.Count*/ tagNum);
            }
        }
        static void Main()
        {
            var answer = new List<Photo>();
            var input = new InputData();
            input.Read(pathB);
            Console.WriteLine(input.Count);
            //List<Photo> sorted = input.AllPhotos.OrderByDescending(photo => photo.IntTags.Count).ToList();
            List<int> tagNums = input.AllPhotos
                .Select(photo => photo.IntTags.Count)
                .Distinct()
                .OrderByDescending(tagNum => tagNum)
                .ToList();
            foreach (var t in tagNums)
            {
                var CurList = input.AllPhotos.Where(photo => photo.IntTags.Count == t);
                CalcGroup(CurList, answer, t);
            }
            Tools.PrintAnswer(answer, ansB);
            Console.ReadLine();
        }
    }
}
