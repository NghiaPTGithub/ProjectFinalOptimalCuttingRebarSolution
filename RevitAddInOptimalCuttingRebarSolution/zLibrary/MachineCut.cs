using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace RevitAddInOptimalCuttingRebarSolution.zLibrary
{
    public class MachineCut //GiaCongThep
    {
        internal int L;//chieu dai 1 cay thep nguyen
        internal List<Rebar_CayThep> ChainRebar;//trinh tu cat cac thep cung duong kinh - TrinhTuThep
        internal int CountFreshRebar;//Tinh so luong thep nguyen cay can dung - CountThepNguyen
        internal double MassFreshRebar;//KLThepNguyen
        internal double MassWasteRebar; //KLHaoHut
        internal int CountCutting;//SLVetCat
        public MachineCut(int length, List<Rebar_CayThep> trinhTuThep)
        {
            this.L = length;
            this.ChainRebar = trinhTuThep;
            this.CountFreshRebar = 0;
            this.MassFreshRebar = 0;
            this.MassWasteRebar = 0;
            this.CountCutting = 0;
        }
        public void CutThep()
        {
            List<int> thepInput = new List<int>();
            List<int> thepDu = new List<int>();
            List<int> thepVun = new List<int>();
            thepInput = ChainRebar.Select(x => x.Length).ToList();
            //Tim chieu dai thanh thep ngan nhat
            int lengthMin = thepInput.Min();
            //khai bao thanh thep nguyen lieu va thep du
            int lengthThepNguyenLieu = 0; int lenghtThepDu = 0;
            //Voi moi thanh thep thuc hien thao tac sau:
            foreach (int thep in thepInput)
            {
                //1.Lay thep gnuyen lieu
                //Tim trong list thep Du co thanh nao khong ngan hon thanh do va co chieu dai gan nhat voi chieu dai cua no
                //Neu khong co thi lay thanh thep moi -> so luong thep nguyen +1
                //Nguoc lai thi lay thanh do va loai thanh du ra khoi danh sanh
                lengthThepNguyenLieu = FindLength(thep, thepDu);
                if (lengthThepNguyenLieu == 0)
                {
                    lengthThepNguyenLieu = L;
                    CountFreshRebar++;
                    CountCutting++;
                }
                else
                {
                    thepDu.Remove(lengthThepNguyenLieu);
                }
                //2.Cat thep: tinh chieu dai thanh thep du = chieu dai nguyen lieu - chieu dai thep da chon
                //Neu thep du co chieu dai lon hon thanh thep ngan nhat thi cho vao kho thep du
                //Nguoc lai cho vao list thep vun
                lenghtThepDu = lengthThepNguyenLieu - thep;
                if (lenghtThepDu < lengthMin)
                {
                    thepVun.Add(lenghtThepDu);
                }
                else
                {
                    thepDu.Add(lenghtThepDu);
                    CountCutting++;
                }
            }
            //Sau khi cat xong het thi Tinh khoi luong thep nguyen, khoi luong hao hut=KL du + KLvun
            double KLR = ChainRebar[0].Material.SpecificWeight;
            MassFreshRebar = KLR * CountFreshRebar * L;
            MassWasteRebar = KLR * (thepDu.Sum() + thepVun.Sum());
        }
        private int FindLength(int length, List<int> listThepDu)
        {
            if (listThepDu.Count == 0)
            {
                return 0;
            }
            if (length > listThepDu.Max())
            {
                return 0;
            }
            else
            {
                int d = L;//do lech chieu dai 
                int thepChon = 0;//chieu dai thanh dc chon
                foreach (int thep in listThepDu)
                {
                    if (thep - length <= d && thep >= length)
                    {
                        d = thep - length;
                        thepChon = thep;
                    }
                }
                return thepChon;
            }

        }


    }

}
