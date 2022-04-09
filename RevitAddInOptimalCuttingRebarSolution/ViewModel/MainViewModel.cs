using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MoreLinq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Application = Autodesk.Revit.ApplicationServices.Application;
using RevitAddInOptimalCuttingRebarSolution.View;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Structure;
using RevitAddInOptimalCuttingRebarSolution.zLibrary;
using System.Windows.Data;

namespace RevitAddInOptimalCuttingRebarSolution.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        #region Thuoc tinh Data, binding
        public Document Doc;
        public UIDocument UiDoc;
        public List<Rebar> ListRebar;
        private List<RebarInformation> ListRebarInfor;

        private bool _SuccessfulHandle = false;

        //data sau khi duoc chon tron selecton view
        private ObservableCollection<RebarInformation> _SelectedRebarInfor;
        public ObservableCollection<RebarInformation> SelectedRebarInfor
        {
            get => _SelectedRebarInfor;
            set { _SelectedRebarInfor = value; OnPropertyChanged(); }
        }

        public List<RebarInformation> _ListSelectedRebarInfor;
        //data sau khi xu ly de report
        private List<RebarInformation> _DataToReport;

        //data thep nguyen lieu
        private List<FreshRebar> _DataRebarInput;

        //ty le hao hut tinh duoc
        private double _RatioWastageMass;

        //tao tham chieu den main view
        private MainView _MainView;

        //Du lieu setting thuat toan
        //1.Setting
        private SettingViewModel _SettingViewModel;
        public List<Steel> ListSteels;

        //2.Selection 
        private SelectionViewModel _SelectionViewModel;

        //3.Material
        private MaterialViewModel _MaterialViewModel;

        #endregion

        #region Thuoc tinh command
        public ICommand OpenReportView { get; set; }//icommand de mo window report
        public ICommand OpenSettingView { get; set; }//icommand de mo window setting
        public ICommand OpenSelectView { get; set; }//icommand de mo window selection
        public ICommand CutRebarCommand { get; set; }//icommand cat thep
        public ICommand OpenMaterialView { get; set; }//icommand cat thep
        #endregion 

        //constructor
        public MainViewModel(UIDocument uidoc, List<Rebar> listRebar)
        {
            //Khoi tao cac thuoc tinh
            UiDoc = uidoc;
            Doc = uidoc.Document;
            ListRebar = listRebar;
            SelectedRebarInfor = new ObservableCollection<RebarInformation>();

            #region 1. khoi tao mainview
            //1.1. Tao list chua du lieu rebar
            ListRebarInfor = new List<RebarInformation>();
            foreach (Rebar rebar in ListRebar)
            {
                ListRebarInfor.Add(new RebarInformation(rebar));
            }

            //1.2. Tao du lieu selected
            foreach (RebarInformation rebarInformation in ListRebarInfor)
            {
                SelectedRebarInfor.Add(rebarInformation);
            }

            #endregion

            #region 2. Cac Icommand Thuc hien chuyen view

            OpenReportView = new RelayCommand<object>((p) => { return true; },
            (p) =>
            {
                if (!_SuccessfulHandle)
                {
                    using (Transaction tran = new Transaction(Doc, "Mess Attention"))
                    {
                        tran.Start();
                        TaskDialog.Show("Attention", "You must handle data before retriving the report !");
                        tran.Commit();
                    }
                }
                else
                {
                    ReportViewModel reportViewModel = new ReportViewModel(SelectedRebarInfor.ToList(), _DataRebarInput, _DataToReport, _RatioWastageMass);
                    reportViewModel.CreateReportView();
                }

            });

            OpenSettingView = new RelayCommand<object>((p) => { return true; },
            (p) =>
            {
                _SettingViewModel = new SettingViewModel(Doc,_SettingViewModel);
                _SettingViewModel.CreatSettingView();

            });

            OpenSelectView = new RelayCommand<object>((p) => { return true; },
            (p) =>
            {
                _SelectionViewModel = new SelectionViewModel(ListRebarInfor, Doc);
                _SelectionViewModel.CreateSelectionView();
                _ListSelectedRebarInfor = _SelectionViewModel.DataSelected;
                SelectedRebarInfor = new ObservableCollection<RebarInformation>();
                SelectedRebarInfor = Utils.ToObservableCollection(_ListSelectedRebarInfor);
                SetMainView(_MainView, SelectedRebarInfor);
            });

            OpenMaterialView = new RelayCommand<object>((p) => { return true; },
            (p) =>
            {
                List<int> dataSelectedDiameters = SelectedRebarInfor.DistinctBy(x => x.Diameter).Select(x => x.Diameter).ToList();
                _MaterialViewModel = new MaterialViewModel(dataSelectedDiameters, Doc);
                _MaterialViewModel.CreateView();
                //Tham chieu den list steel da set
                ListSteels = new List<Steel>();
                ListSteels = _MaterialViewModel.ListSteels.ToList();
            });

            CutRebarCommand = new RelayCommand<object>((p) => { return true; },
            (p) =>
            {
                if (_SettingViewModel == null)
                {
                    using (Transaction tran = new Transaction(Doc, "Mess Attention"))
                    {
                        tran.Start();
                        TaskDialog.Show("Attention", "You must set algorithm information before handling data !");
                        tran.Commit();
                    }
                }
                else
                {
                    if (_MaterialViewModel == null)
                    {
                        using (Transaction tran = new Transaction(Doc, "Mess Attention"))
                        {
                            tran.Start();
                            TaskDialog.Show("Attention", "You must set material information before handling data !");
                            tran.Commit();
                        }
                    }
                    else
                    {
                        //1. Kiem tra xem vat lieu da duoc set day du chua
                        //1.1. Lay ve duong kinh tu material model view
                        List<int> materialViewDiameters = _MaterialViewModel.ListSteels.DistinctBy(x => x.Diameter).Select(x => x.Diameter).ToList();

                        //1.2. Lay ve duong kinh tu data selected
                        List<int> dataSelectedDiameters = SelectedRebarInfor.DistinctBy(x => x.Diameter).Select(x => x.Diameter).ToList();

                        //1.3. So sanh su dong bo ve duong kinh thep
                        bool DiameterIsSynchronized = true;
                        if (materialViewDiameters.Count != dataSelectedDiameters.Count)
                            DiameterIsSynchronized = false;
                        for (int i = 0; i < materialViewDiameters.Count; i++)
                        {
                            if (!(dataSelectedDiameters.IndexOf(materialViewDiameters[i]) >= 0))
                                DiameterIsSynchronized = false;
                        }

                        if (DiameterIsSynchronized)
                        {
                            if (_SelectionViewModel != null)
                            {
                                if (_SelectionViewModel.DataSelected.Count != 0)
                                {
                                    SelectedRebarInfor = new ObservableCollection<RebarInformation>();
                                    foreach (RebarInformation rebar in _SelectionViewModel.DataSelected)
                                    {
                                        SelectedRebarInfor.Add(rebar);
                                    }
                                }
                            }

                            var watch = new System.Diagnostics.Stopwatch();
                            //time start program
                            watch.Start();

                            //handle data
                            HandleRebars handle = new HandleRebars(ListSteels, _SettingViewModel, SelectedRebarInfor.ToList(), Doc);

                            //time end program
                            watch.Stop();

                            _DataToReport = handle.DataRebarHandeled;
                            _DataRebarInput = handle.DataRebarInput;
                            _RatioWastageMass = Math.Round(handle.RatioWastageMass, 3);
                            _SuccessfulHandle = true;

                            using (Transaction tran = new Transaction(Doc, "Mess Successful"))
                            {
                                tran.Start();
                                TaskDialog.Show("Successful", "Data is handled successfully, you can retrive the report ! \nExecution time: " + watch.ElapsedMilliseconds/1000 + "s");
                                tran.Commit();
                            }
                        }
                        else
                        {
                            using (Transaction tran = new Transaction(Doc, "Mess Attention"))
                            {
                                tran.Start();
                                TaskDialog.Show("Attention", "Material is not synchronized. Please reset material !");
                                tran.Commit();
                            }
                        }
                    }
                }

            });

            #endregion            
        }

        public void CreatMainView()
        {
            //1.3. Khoi tao mainview
            MainView mv = new MainView();
            SetMainView(mv, SelectedRebarInfor);
            mv.DataContext = this;
            _MainView = mv;
            //1.4. Hien thi MainView
            mv.ShowDialog();
        }

        //Set main view
        private void SetMainView(MainView mainView, ObservableCollection<RebarInformation> listDataRebar)
        {
            mainView.lvMainView.ItemsSource = listDataRebar;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(mainView.lvMainView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("ElementContainInformation");
            view.GroupDescriptions.Add(groupDescription);
        }

        public void CutRebar()
        {

        }
    }
}
