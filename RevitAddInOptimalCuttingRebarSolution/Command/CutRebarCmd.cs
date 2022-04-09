using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application = Autodesk.Revit.ApplicationServices.Application;
using RevitAddInOptimalCuttingRebarSolution.View;
using RevitAddInOptimalCuttingRebarSolution.ViewModel;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Structure;
using System.Windows.Data;

namespace RevitAddInOptimalCuttingRebarSolution.Command
{
    [Transaction(TransactionMode.Manual)]
    public class CutRebarCmd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //get document
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application app = uiapp.Application;
            Document doc = uidoc.Document;
              
            //yeu cau nguoi dung chon doi tuong, neu k co doi tuong duoc chon thi thong bao
            List<Reference> pickObjects = uidoc.Selection.PickObjects(ObjectType.Element, "CHON DOI TUONG").ToList();
            if (pickObjects.Count == 0)
            {
                TaskDialog.Show("Error Selection", "Ban chua chon doi tuong");
                return Result.Cancelled;
            }
            List<Element> elements1 = new List<Element>();
            foreach (Reference f in pickObjects)
            {
                elements1.Add(doc.GetElement(f));
            }

            //lay ra cac doi tuong thep
            List<Rebar> listRebars = new List<Rebar>();
            foreach (Reference r in pickObjects)
            {
                Element e1 = doc.GetElement(r);
                if (e1 is Rebar)
                {
                    listRebars.Add(e1 as Rebar);
                }
            }

            //neu khong co thep thi bao lai cho nguoi dung
            if (listRebars.Count == 0)
            {
                TaskDialog.Show("Warning", "Khong co doi tuong thep nao duoc chon");
                return Result.Cancelled;
            }
            #region Main Code

            //khoi tao mainViewModel de khoi tao MainView
            MainViewModel mainViewModel = new MainViewModel(uidoc, listRebars);
            mainViewModel.CreatMainView();

            #endregion
            //return
            return Result.Succeeded;
        }
    }
}
