using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Linq;

namespace RevitAddInOptimalCuttingRebarSolution.zLibrary
{
    public class ElementGA //Element
    {
        internal string ID;
        internal string Name;
        internal List<Rebar_CayThep> ListRebar;
        public ElementGA(string id,string name,List<Rebar_CayThep> listThep) 
        {
            this.ID = id;
            this.Name = name;
            this.ListRebar = listThep;
            //setup name
            //foreach (Thep t in ListThep) 
            //{
            //    t.Name = this.Name + "." + t.Material .Diameter .ToString ()+"."+t.Length .ToString ()  ;
            //}
        }
        public double GetSumMass() 
        {
            double result = 0;
            foreach(Rebar_CayThep thep in ListRebar) { result = result + thep.GetMass(); }
            return result;
        }
    }
}
