using System;

namespace HashCode2019
{
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
}
