using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
namespace RevitAddInOptimalCuttingRebarSolution.Application
{
    [Transaction(TransactionMode.Manual)]
    public class CutRebarApp : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            //create tab
            string nametab = "Nghia Add-in";//create tab name
            application.CreateRibbonTab(nametab);
            //create panel
            RibbonPanel panel = application.CreateRibbonPanel(nametab, "Cut Rebar");
            //create button
            string path = Assembly.GetExecutingAssembly().Location;
            PushButtonData pushButtonData = new PushButtonData("btnCutRebar", "Optimal Cutting Rebar", path, "RevitAddInOptimalCuttingRebarSolution.Command.CutRebarCmd");
            //add button to panel
            PushButton pushButton = panel.AddItem(pushButtonData) as PushButton;
            //add image to button
            Uri uri = new Uri(@"C:\ProgramData\Autodesk\Revit\Addins\2021\CutRebarIcon.32.png");
            BitmapImage image = new BitmapImage(uri);
            pushButton.LargeImage = image;
            return Result.Succeeded;
        }

    }
}