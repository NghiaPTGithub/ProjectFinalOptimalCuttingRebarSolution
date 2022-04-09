using System;
using System.Collections;
using System.Text;

namespace RevitAddInOptimalCuttingRebarSolution.zLibrary
{
    public class DotBienArrayList
    {
        internal ArrayList TrinhTu;
        public DotBienArrayList(ArrayList arr)
        {
            TrinhTu = arr;
        }
        public ArrayList GetNewArrayList(int hesoDotBien) 
        {
            ArrayList child = new ArrayList();
            int n = TrinhTu.Count;
            Random r = new Random();
            int count = r.Next(hesoDotBien) + 1;
            int p1, p2;
            child = TrinhTu;
            while (count > 0)
            {
                p1 = r.Next(n);
                p2 = r.Next(n);
                var temp = child[p1];
                child[p1] = child[p2];
                child[p2] = temp;
                count--;
            }
            return child;
        }
    }
}
