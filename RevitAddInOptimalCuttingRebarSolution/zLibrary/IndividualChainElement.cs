using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace RevitAddInOptimalCuttingRebarSolution.zLibrary 
{
    public class IndividualChainElement//TrinhTuElement
    {
        internal string ID;
        internal List<ElementGA> ChainElement; //ListElement
        internal List<FreshRebar> ListInputRebar;//ListThepNguyenCanDung
        internal double RatioWasteMass;//TyLeHaoHutKL
        internal int CountCutting; //SLVetCat
        public IndividualChainElement(string id, List<ElementGA> listElement)
        {
            ID = id;
            ChainElement = listElement;
            RatioWasteMass = 0;
            CountCutting = 0;
        }
        public IndividualChainElement() { /*this.ListElement = new List<Element>(); this.TyLeHaoHutKL = 0; this.SLVetCat = 0;*/ }
        public List<Rebar_CayThep> ToListThep()//phuong thuc phan ra chuoi element thanh thanh chuoi cac thanh thep
        {
            List<Rebar_CayThep> result = new List<Rebar_CayThep>();
            foreach (ElementGA e in ChainElement)
            {
                result.AddRange(e.ListRebar);
            }
            return result;
        }
        public void TinhChiPhiTyLeHaoHutKL(List<Steel> listSteel)
        {
            double KLThepNguyen = 0;
            double KLHaoHut = 0;
            ListInputRebar = new List<FreshRebar>();
            //1. Loc trong trinh tu thep cac trinh tu con ung voi moi loai vat lieu
            foreach (Steel vl in listSteel)
            {
                List<Rebar_CayThep> trinhTuThep = new List<Rebar_CayThep>();
                trinhTuThep = this.ToListThep();//lay trinh tu thep tu list element
                FreshRebar nl = new FreshRebar(vl);
                List<Rebar_CayThep> lt = new List<Rebar_CayThep>();
                lt = trinhTuThep.Where(s => s.Material.ID == vl.ID).ToList();
                //2. Tinh KL thep nguyen cua moi loai vat lieu, KL hao hut
                MachineCut g = new MachineCut(vl.DefautLenght, lt);
                g.CutThep();
                nl.Count = g.CountFreshRebar;
                nl.HaoHutDuTinh = g.MassWasteRebar;
                ListInputRebar.Add(nl);
                KLThepNguyen = KLThepNguyen + g.MassFreshRebar;
                KLHaoHut = KLHaoHut + g.MassWasteRebar;
                CountCutting =+ g.CountCutting;
            }
            //3.Tinh ty le hao hut
            RatioWasteMass = KLHaoHut / (KLThepNguyen - KLHaoHut);
        }
        public string ListElementToString()
        {
            string result = "";
            foreach (ElementGA e in ChainElement)
            {
                result = result + e.Name + " -> ";
            }
            return result;
        }
        public string ListThepToString()
        {
            string result = "";
            foreach (ElementGA e in ChainElement)
            {
                foreach (Rebar_CayThep t in e.ListRebar)
                {
                    result = result + t.Name + " -> ";
                }
                result = result + "\n";
            }
            return result;
        }

    }
}
