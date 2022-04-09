using System;
using System.Collections.Generic;
using System.Text;

namespace RevitAddInOptimalCuttingRebarSolution.zLibrary
{
    public class FreshRebar //NguyenLieuThep
    {
        public Steel Material { get; set; }//VatLieu
        public int L { get; set; }
        public int Count { get; set; }
        public double HaoHutDuTinh { get; set; }
        public int Diameter { get; set; }
        public FreshRebar(Steel material)
        {
            this.Material = material;
            this.L = Material.DefautLenght;
            this.Count = 0;
            this.HaoHutDuTinh = 0;
            Diameter = material.Diameter;
        }
    }
}
