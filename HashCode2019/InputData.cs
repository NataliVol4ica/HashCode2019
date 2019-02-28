using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCode2019
{
    class InputData
    {
        public List<Photo> Horizontals;
        public List<Photo> Verticals;
        public List<Photo> AllPhotos;
        public int Count;

        public InputData()
        {
            Horizontals = new List<Photo>();
            Verticals = new List<Photo>();
            AllPhotos = new List<Photo>();
        }

        public void Read(string path)
        {
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                Count = Convert.ToInt32(sr.ReadLine());
                for (int i = 0; i < Count; i++)
                    AddPhoto(sr.ReadLine(), i);
            }
        }

        static long TagToInt(string tag)
        {
            long ans = 0;
            foreach (char c in tag)
                ans = (ans << 8) + (int)c;
            return ans;
        }

        public void AddPhoto(string line, int index)
        {
            string[] data = line.Split(' ');
            int tagNum = Convert.ToInt32(data[1]);
            if (tagNum == 1)
                return;
            var P = new Photo(data[0], index);

            for (int i = 2; i < tagNum; i++)
            {
                P.StringTags.Add(data[i]);
                P.IntTags.Add(TagToInt(data[i]));
            }
            if (P.IsHorizontal)
                Horizontals.Add(P);
            else
                Verticals.Add(P);
            AllPhotos.Add(P);
        }
    }
}
