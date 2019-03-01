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
        
        public void AddPhoto(string line, int index)
        {
            var P = new Photo(index);
            P.ParseLine(line);
            if (P.IsHorizontal)
                Horizontals.Add(P);
            else
                Verticals.Add(P);
            AllPhotos.Add(P);
        }

        public void OrderPhotosByFirst()
        {
            AllPhotos = AllPhotos.OrderBy(photo => photo.IntTags[0]).ToList();
        }
    }
}
