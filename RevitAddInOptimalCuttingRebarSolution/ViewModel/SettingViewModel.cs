using RevitAddInOptimalCuttingRebarSolution.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RevitAddInOptimalCuttingRebarSolution.zLibrary;
using System.Windows.Input;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Data;

namespace RevitAddInOptimalCuttingRebarSolution.ViewModel
{
    public class SettingViewModel : BaseViewModel
    {

        private bool _isValid = true;
        private Document _doc;
        private Setting SettingWindow;

        #region Data binding

        private int _PopulationSize;
        public int PopulationSize
        {
            get => _PopulationSize;
            set { _PopulationSize = value; OnPropertyChanged(); }
        }

        private int _MaxGenerationQuantity;
        public int MaxGenerationQuantity
        {
            get => _MaxGenerationQuantity;
            set { _MaxGenerationQuantity = value; OnPropertyChanged(); }
        }

        private double _RatioHybridizeElement;
        public double RatioHybridizeElement
        {
            get => _RatioHybridizeElement;
            set { _RatioHybridizeElement = value; OnPropertyChanged(); }
        }

        private double _RatioMutateElement;
        public double RatioMutateElement
        {
            get => _RatioMutateElement;
            set { _RatioMutateElement = value; OnPropertyChanged(); }
        }

        private double _RatioMutateLocal;
        public double RatioMutateLocal
        {
            get => _RatioMutateLocal;
            set { _RatioMutateLocal = value; OnPropertyChanged(); }
        }

        private int _FactorMutateElement;
        public int FactorMutateElement
        {
            get => _FactorMutateElement;
            set { _FactorMutateElement = value; OnPropertyChanged(); }
        }

        private int _FactorMutateLocal;
        public int FactorMutateLocal
        {
            get => _FactorMutateLocal;
            set { _FactorMutateLocal = value; OnPropertyChanged(); }
        }

        private double _RatioMutateElementLocal;
        public double RatioMutateElementLocal
        {
            get => _RatioMutateElementLocal;
            set { _RatioMutateElementLocal = value; OnPropertyChanged(); }
        }
        #endregion

        #region Thuoc tinh Commnad
        public ICommand GetSetting { get; set; }
        public ICommand SetDefaultSetting { get; set; }
        #endregion
        public SettingViewModel(Document doc, SettingViewModel settingViewModel )
        {
            _doc = doc;
            if(settingViewModel != null) 
            {
                PopulationSize = settingViewModel .PopulationSize;
                MaxGenerationQuantity = settingViewModel.MaxGenerationQuantity;
                RatioHybridizeElement = settingViewModel.RatioHybridizeElement;
                RatioMutateElement = settingViewModel.RatioMutateElement;
                RatioMutateLocal = settingViewModel.RatioMutateLocal;
                FactorMutateElement = settingViewModel.FactorMutateElement;
                FactorMutateLocal = settingViewModel.FactorMutateLocal;
                RatioMutateElementLocal = settingViewModel.RatioMutateElementLocal;
            }
            else 
            {
                SetDefault();
            }           

            GetSetting = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                //lay du lieu tu window ve
                string textGot = "";

                //kich thuoc quan the
                textGot = SettingWindow.tbxPopulationSize.Text;
                if (Utils.IsInteger(textGot, true) && Int32.Parse(textGot) > 0)
                {
                    PopulationSize = Int32.Parse(textGot);
                }
                else
                {
                    _isValid = false;
                }

                //so luong ca the toi da trong quan the
                textGot = SettingWindow.tbxMaxGenerationQuantity.Text;
                if (Utils.IsInteger(textGot, true) && Int32.Parse(textGot) > 0)
                {
                    MaxGenerationQuantity = Int32.Parse(textGot);
                }
                else
                {
                    _isValid = false;
                }

                //ti le lai ghep cac element
                textGot = SettingWindow.tbxRatioHybridizeElement.Text;
                if (Utils.IsDouble(textGot, true)
                    && Double.Parse(textGot, System.Globalization.CultureInfo.InvariantCulture) >= 0
                    && Double.Parse(textGot, System.Globalization.CultureInfo.InvariantCulture) <= 1)
                {
                    RatioHybridizeElement = Double.Parse(textGot, System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    _isValid = false;
                }

                //ty le dot bien muc element
                textGot = SettingWindow.tbxRatioMutateElement.Text;
                if (Utils.IsDouble(textGot, true)
                    && Double.Parse(textGot, System.Globalization.CultureInfo.InvariantCulture) >= 0
                    && Double.Parse(textGot, System.Globalization.CultureInfo.InvariantCulture) <= 1)
                {
                    RatioMutateElement = Double.Parse(textGot, System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    _isValid = false;
                }

                //ty le dot bien cuc bo
                textGot = SettingWindow.tbxRatioMutateLocal.Text;
                if (Utils.IsDouble(textGot, true)
                    && Double.Parse(textGot, System.Globalization.CultureInfo.InvariantCulture) >= 0
                    && Double.Parse(textGot, System.Globalization.CultureInfo.InvariantCulture) <= 1)
                {
                    RatioMutateLocal = Double.Parse(textGot, System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    _isValid = false;
                }

                //he so dot bien element
                textGot = SettingWindow.tbxFactorMutateElement.Text;
                if (Utils.IsInteger(textGot, true) && Int32.Parse(textGot) > 0)
                {
                    FactorMutateElement = Int32.Parse(textGot);
                }
                else
                {
                    _isValid = false;
                }

                //he so dot bien cuc bo
                textGot = SettingWindow.tbxFactorMutateLocal.Text;
                if (Utils.IsInteger(textGot, true) && Int32.Parse(textGot) > 0)
                {
                    FactorMutateLocal = Int32.Parse(textGot);
                }
                else
                {
                    _isValid = false;
                }

                //ty le dot bien cuc bo tai element
                textGot = SettingWindow.tbxRatioMutateElementLocal.Text;
                if (Utils.IsDouble(textGot, true)
                    && Double.Parse(textGot, System.Globalization.CultureInfo.InvariantCulture) >= 0
                    && Double.Parse(textGot, System.Globalization.CultureInfo.InvariantCulture) <= 1)
                {
                    RatioMutateElementLocal = Double.Parse(textGot, System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    _isValid = false;
                }

                if (_isValid)
                {
                    SettingWindow.Close();
                    //using (Transaction tran = new Transaction(_doc, "Mess Successful"))
                    //{
                    //    tran.Start();
                    //    TaskDialog.Show("Successful", "Algorithm parameters is set successfully !");
                    //    tran.Commit();
                    //}
                }
                else
                {
                    using (Transaction tran = new Transaction(_doc, "Mess Attention"))
                    {
                        tran.Start();
                        TaskDialog.Show("Attention", "Data in put is invalid. Plese reset paramters");
                        tran.Commit();
                    }
                }
            });

            SetDefaultSetting = new RelayCommand<object>((p) => { return true; },(p) =>               
            {
                SetDefault();               
            });
        }
        public void CreatSettingView()
        {
            Setting settingView = new Setting();
            settingView.DataContext = this;
            SettingWindow = settingView;
            settingView.ShowDialog();
        }
        private void SetDefault() 
        {
            //Set default
            PopulationSize = 100;
            MaxGenerationQuantity = 1000;
            RatioHybridizeElement = 0.5;
            RatioMutateElement = 0.5;
            RatioMutateLocal = 0.9;
            FactorMutateElement = 2;
            FactorMutateLocal = 2;
            RatioMutateElementLocal = 0.8;
        }
    }
}
