using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitAddInOptimalCuttingRebarSolution.View;
using RevitAddInOptimalCuttingRebarSolution.zLibrary;
using ComboBox = System.Windows.Controls.ComboBox;
using TextBox = System.Windows.Controls.TextBox;

namespace RevitAddInOptimalCuttingRebarSolution.ViewModel
{
    public class MaterialViewModel : BaseViewModel
    {
        #region Thuoc tinh binding 

        #endregion

        #region Thuoc tinh command
        public ICommand ApplyData { get; set; }//Lay du lieu ve va hien thi
        public ICommand GetSteels { get; set; }//Lay du lieu ve va tat window
        #endregion

        #region Thuoc tinh data
        public Document Doc;
        private ObservableCollection<Steel> _ListSteels;
        public ObservableCollection<Steel> ListSteels //Output
        {
            get => _ListSteels;
            set { _ListSteels = value; OnPropertyChanged(); }
        }
        public double defaultSPM3 { get; set; }
        private bool IsApply;
        private MaterialView MaterialView;
        #endregion

        //constructor
        public MaterialViewModel(List<int> listDiameters, Document doc)
        {
            Doc = doc;
            ListSteels = new ObservableCollection<Steel>();
            defaultSPM3 = 7850; //gia tri mac dinh cua khoi luong rieng tinh theo kg/m3
            int d = 0;
            for (int i = 0; i < listDiameters.Count; i++)
            {
                d = listDiameters[i];
                Steel newsteel = new Steel(d, true, defaultSPM3 * 3.14 * d * d / 4000000, 400, 11700);
                newsteel.ID = (i + 1).ToString();
                newsteel.SpecificWeightKGM3 = defaultSPM3;
                ListSteels.Add(newsteel);
            }
            IsApply = false;
            ApplyData = new RelayCommand<object>((p) => { return true; },
               (p) =>
               {
                   IsApply = true;
                   //Lay du lieu tu View ve
                   SetDataMaterial();
               });
            GetSteels = new RelayCommand<Window>((p) => { return true; },
               (p) =>
               {
                   if (!IsApply) { SetDataMaterial(); }
                   p.Close();
               });
        }
        public void CreateView()
        {
            MaterialView materialView = new MaterialView();
            MaterialView = materialView;
            materialView.DataContext = this;
            materialView.ShowDialog();
        }
        public void SetDataMaterial()
        {
            if (MaterialView.rbtnSelectDefault.IsChecked == true) return;
            else
            {
                for (int i = 0; i < ListSteels.Count; i++)
                {
                    ListSteels[i].DefautLenght = Convert.ToInt32(GetCellValue(MaterialView.dgMaterials, i, 2));  //*nho xu li ngoai le
                    ListSteels[i].SteelDeformed = Convert.ToBoolean(GetCellValue(MaterialView.dgMaterials, i, 3));  //*nho xu li ngoai le
                    if (MaterialView.rbtnSelectKgm3.IsChecked == true)
                    {
                        ListSteels[i].SpecificWeightKGM3 = Convert.ToDouble(GetCellValue(MaterialView.dgMaterials, i, 4));  //*nho xu li ngoai le
                    }
                    if (MaterialView.rbtnSelectKgm.IsChecked == true)
                    {
                        ListSteels[i].SpecificWeight = Convert.ToDouble(GetCellValue(MaterialView.dgMaterials, i, 5));  //*nho xu li ngoai le
                    }
                    ListSteels[i].Grade = Convert.ToDouble(GetCellValue(MaterialView.dgMaterials, i, 6));  //*nho xu li ngoai le                    
                }
            }
        }
    }
}


