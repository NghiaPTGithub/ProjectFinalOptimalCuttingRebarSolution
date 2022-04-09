using RevitAddInOptimalCuttingRebarSolution.Model.ElementContain;
using RevitAddInOptimalCuttingRebarSolution.Model.Material;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAddInOptimalCuttingRebarSolution.Model.Rebar
{
    public class RebarElement
    {
        #region Property
        // Base informations
        public string Id;
        public double Diameter;
        public ElementContainRebar ElementContain;
        public MaterialModel Material;
        public RebarForm RebarForm;

        #region Informations of cutting rebar
        // Input rebar
        public string InputRebarId;
        public int InputRebarLength;
        // Residual Rebar
        public int ResidualRebarLength;
        // Waste Rebar
        public bool IsWasteRebar;
        #endregion
        #endregion

        #region Constructor
        public RebarElement(Autodesk.Revit.DB.Structure.Rebar rebar)
        {
            Id = rebar.Id.ToString();
            Diameter = rebar.GetBendData().BarDiameter;
        }
        #endregion

        #region Method

        #endregion
    }
}
