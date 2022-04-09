using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace RevitAddInOptimalCuttingRebarSolution.zLibrary
{
    public class Steel
    {
        public string ID{get;set;}
        public string Name { get; set; }
        public bool SteelDeformed { get; set; }
        public int Diameter { get; set; }
        public double SpecificWeight { get; set; }
        public double SpecificWeightKGM3 { get; set; }
        public double Grade { get; set; }
        public int DefautLenght { get; set; }

        public List<bool> SteelDeformedSelecttion { get; set; }

        public Steel(int diameter,bool steelDeformed,double specificWeight,double grade, int defautLenght)
        {
            this.ID = "";
            this.Name = "Diameter " + diameter  + " (mm)";
            this.SteelDeformed = steelDeformed;
            this.Diameter = diameter;
            this.SpecificWeight = specificWeight;
            this.Grade = grade;
            this.DefautLenght = defautLenght;


            bool a = true;
            bool b = false;
            SteelDeformedSelecttion = new List<bool>() { a, b };
        }
    }

}
