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
        // Document
        private Document _doc;

        // Base informations
        public string Id;
        public double Diameter;
        public double Length;

        // Element contain
        public Element ElementContain;
        public string ElementContainId;
        public string ElementContainName;

        // Material
        public MaterialModel Material;
        public string MaterialContent;

        // Form
        public RebarForm RebarForm;

        // Input rebar
        public string InputRebarId;
        public double InputRebarLength;

        // Residual rebar
        public double ResidualRebarLength;

        // Waste rebar
        public bool IsWasteRebar;
        #endregion

        #region Constructor
        public RebarElement(Autodesk.Revit.DB.Structure.Rebar rebar)
        {
            _doc = rebar.Document;

            Id = rebar.Id.ToString();
            Diameter = rebar.GetBendData().BarDiameter;
            Length = rebar.TotalLength;

            ElementContain = _doc.GetElement(rebar.GetHostId());
            ElementContainId = ElementContain.Id.ToString();
            ElementContainName = ElementContain.Name;

            InputRebarId = "new";
            InputRebarLength = 11700;
            ResidualRebarLength = 0;
            IsWasteRebar = true;
        }
        #endregion

        #region Method

        #endregion
    }
}
