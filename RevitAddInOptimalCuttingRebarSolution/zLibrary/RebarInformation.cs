using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RevitAddInOptimalCuttingRebarSolution.zLibrary
{
    public class RebarInformation
    {
        //note: EC=Element Contain la elemtn chua cac thanh thep
        #region Property: information of element contain
        public string IDElementContain { get; }
        public Element ElementContain { get; }
        public Level ECLevel { get; }
        public Category ECCategory { get; }
        public Family ECFamily { get; }
        public FamilySymbol ECFamilySymbol { get; }
        public string ElementContainInformation { get; }
        public string FamilyName { get; }
        #endregion
        #region Property: information of rebar
        public string IDRebar { get; }
        public int Lenght { get; }
        public int Diameter { get; }
        public string materialName { get; }
        #endregion
        #region Property: View data
        //thể hiện trạng thái check box trong grid view left trong selection View
        public bool IsCheckedInSelectionViewDataLeft { get; set; }
        public bool IsCheckedInSelectionViewDataRight { get; set; }

        #endregion
        public RebarInformation(Rebar rebar)
        {
            //get document
            Document doc = rebar.Document;

            //get element contain information
            IDElementContain = rebar.GetHostId().ToString();
            ElementContain = doc.GetElement(rebar.GetHostId());
            ECCategory = ElementContain.Category;
            FamilyInstance familyInstance = ElementContain as FamilyInstance;
            ECFamilySymbol = familyInstance.Symbol;
            ECFamily = ECFamilySymbol.Family;
            ECLevel = familyInstance.Host as Level;
            ElementContainInformation = "ID: " + IDElementContain + " - " + ECFamily.Name + ", Section: " + ECFamilySymbol.Name + ", Level: " + ECLevel.Name + ", Category: " + ECCategory.Name;

            //get rebar information
            IDRebar = rebar.Id.ToString();
            Lenght = (int)(304.8 * rebar.TotalLength);
            Diameter = (int)(304.8 * rebar.GetBendData().BarDiameter);

            //get property view
            IsCheckedInSelectionViewDataLeft = false;
            IsCheckedInSelectionViewDataLeft = false;


        }
    }
    class RebarInforComparer : IEqualityComparer<RebarInformation>
    {
        public bool Equals(RebarInformation x, RebarInformation y)
        {
            return x.IDRebar == y.IDRebar;
        }
        public int GetHashCode(RebarInformation obj)
        {
            return obj.IDRebar.GetHashCode();
        }
    }
}
