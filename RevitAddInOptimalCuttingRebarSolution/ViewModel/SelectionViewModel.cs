using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using RevitAddInOptimalCuttingRebarSolution.View;
using RevitAddInOptimalCuttingRebarSolution.zLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace RevitAddInOptimalCuttingRebarSolution.ViewModel
{
    public class SelectionViewModel : BaseViewModel
    {
        #region thuoc tinh data

        //1 - du lieu nguon
        private DataRebarInformation _DataSourceRebarInfor;
        private List<RebarInformation> _DataSource;

        //2 - Data left: data thu duoc sau khi filter cac checkbox trong menu tro thanh data left 
        private List<RebarInformation> _DataLeft;
        private ObservableCollection<RebarInformation> _DataViewLeft;
        public ObservableCollection<RebarInformation> DataViewLeft
        {
            get => _DataViewLeft;
            set { _DataViewLeft = value; OnPropertyChanged(); }
        }

        //3 - Data right: data thu duoc sau khi nguoi dung lua chon tu grid data left view
        private List<RebarInformation> _DataRight;
        private ObservableCollection<RebarInformation> _DataViewRight;
        public ObservableCollection<RebarInformation> DataViewRight
        {
            get => _DataViewRight;
            set { _DataViewRight = value; OnPropertyChanged(); }
        }

        //4 - Data selected
        public List<RebarInformation> DataSelected;
        #endregion

        #region Thuoc tinh binding treeview

        //1 - level   
        private ObservableCollection<Level> _AllLevels;
        public ObservableCollection<Level> AllLevels
        {
            get => _AllLevels;
            set { _AllLevels = value; OnPropertyChanged(); }
        }
        private Level _LevelMin;
        public Level LevelMin
        {
            get => _LevelMin;
            set { _LevelMin = value; OnPropertyChanged(); }
        }
        private Level _LevelMax;
        public Level LevelMax
        {
            get => _LevelMax;
            set { _LevelMax = value; OnPropertyChanged(); }
        }

        //Level tree view
        private ObservableCollection<GroupLevels> _AllLevelTreeView;
        public ObservableCollection<GroupLevels> AllLevelTreeView
        {
            get => _AllLevelTreeView;
            set { _AllLevelTreeView = value; OnPropertyChanged(); }
        }

        //Category tree view
        private ObservableCollection<GroupCategories> _AllCategoryTreeView;
        public ObservableCollection<GroupCategories> AllCategoryTreeView
        {
            get => _AllCategoryTreeView;
            set { _AllCategoryTreeView = value; OnPropertyChanged(); }
        }

        //Family tree view
        private ObservableCollection<GroupFamilies> _AllFamilyTreeView;
        public ObservableCollection<GroupFamilies> AllFamilyTreeView
        {
            get => _AllFamilyTreeView;
            set { _AllFamilyTreeView = value; OnPropertyChanged(); }
        }

        //Type tree view
        private ObservableCollection<GroupType> _AllTypesTreeView;
        public ObservableCollection<GroupType> AllTypesTreeView
        {
            get => _AllTypesTreeView;
            set { _AllTypesTreeView = value; OnPropertyChanged(); }
        }

        //Diameter tree view
        private ObservableCollection<GroupDiameter> _AllDiametersTreeView;
        public ObservableCollection<GroupDiameter> AllDiametersTreeView
        {
            get => _AllDiametersTreeView;
            set { _AllDiametersTreeView = value; OnPropertyChanged(); }
        }

        #endregion

        #region Thuoc tinh command
        public ICommand GetAllRebars { get; set; }
        public ICommand GetRebarPickObject { get; set; }
        public ICommand FilterTreeView { get; set; }
        public ICommand FilterKeyWordDataViewLeft { get; set; }
        public ICommand FilterKeyWordDataViewRight { get; set; }
        public ICommand MoveDataGridViewLeftToRight { get; set; }
        public ICommand RemoveDataRightCommand { get; set; }
        public ICommand SelectAllViewLeft { get; set; }
        public ICommand SelectAllViewRight { get; set; }
        public ICommand Apply { get; set; }
        #endregion

        private Selection ViewSelection = null;
        private Document Doc;
        public SelectionViewModel(List<RebarInformation> listRebarInfor, Document doc)
        {
            Doc = doc;
            //set default ListRebarInfor
            _DataSourceRebarInfor = new DataRebarInformation(listRebarInfor);

            #region Tao Data Source = Data pick Object OR Data All Rebar
            //Data pickobject
            DataRebarInformation DataPickObj = new DataRebarInformation(listRebarInfor);

            //Data all rebar
            ElementCategoryFilter filterRebar = new ElementCategoryFilter(BuiltInCategory.OST_Rebar);
            FilteredElementCollector collectorRebars = new FilteredElementCollector(Doc);
            IList<Element> listAllRebars = collectorRebars
                .WherePasses(filterRebar)
                .WhereElementIsNotElementType()
                .ToElements();
            List<RebarInformation> listAllrebarInfor = new List<RebarInformation>();
            foreach (Element e in listAllRebars)
            {
                listAllrebarInfor.Add(new RebarInformation(e as Rebar));
            }
            DataRebarInformation DataAllRebar = new DataRebarInformation(listAllrebarInfor);
            #endregion

            #region Binding du lieu cho cac view
            SetProperty(_DataSourceRebarInfor);

            #endregion 

            #region Command

            //set datasource
            GetAllRebars = new RelayCommand<object>((p) => { return true; },
            (p) =>
            {
                SetProperty(DataAllRebar);
                SetGridViewLeft(ViewSelection, DataViewLeft);
            });

            GetRebarPickObject = new RelayCommand<object>((p) => { return true; },
            (p) =>
            {
                SetProperty(DataPickObj);
                SetGridViewLeft(ViewSelection, DataViewLeft);

            });

            FilterKeyWordDataViewLeft = new RelayCommand<object>((p) => { return true; },
            (p) =>
            {
                string keyWord = ViewSelection.tbKeyWordLeft.Text;
                SetDataViewLeft(keyWord);
                SetGridViewLeft(ViewSelection, DataViewLeft);
            });

            FilterTreeView = new RelayCommand<object>((p) => { return true; },
            (p) =>
            {
                SetDataLeft();
                SetDataViewLeft("");
                SetGridViewLeft(ViewSelection, DataViewLeft);
            });

            MoveDataGridViewLeftToRight = new RelayCommand<object>((p) => { return true; },
            (p) =>
            {
                MoveDataLeftToRight();
                SetDataViewRight("");
                SetGridViewRight(ViewSelection, DataViewRight);
            });

            RemoveDataRightCommand = new RelayCommand<object>((p) => { return true; },
            (p) =>
            {
                RemoveDataRight();
                SetDataViewRight("");
                SetGridViewRight(ViewSelection, DataViewRight);
            });
            SelectAllViewLeft = new RelayCommand<object>((p) => { return true; },
            (p) =>
            {

                foreach (RebarInformation rebar in DataViewLeft)
                {
                    rebar.IsCheckedInSelectionViewDataLeft = true;
                }
                ObservableCollection<RebarInformation> result = new ObservableCollection<RebarInformation>();
                foreach (RebarInformation rebar in DataViewLeft)
                {
                    result.Add(rebar);
                }
                DataViewLeft = new ObservableCollection<RebarInformation>();
                DataViewLeft = result;
                SetGridViewLeft(ViewSelection, DataViewLeft);

            });

            SelectAllViewRight = new RelayCommand<object>((p) => { return true; },
            (p) =>
            {
                foreach (RebarInformation rebar in DataViewRight)
                {
                    rebar.IsCheckedInSelectionViewDataRight = true;
                }
                ObservableCollection<RebarInformation> result = new ObservableCollection<RebarInformation>();
                foreach (RebarInformation rebar in DataViewRight)
                {
                    result.Add(rebar);
                }
                DataViewRight = new ObservableCollection<RebarInformation>();
                DataViewRight = result;
                SetGridViewRight(ViewSelection, DataViewRight);

            });

            FilterKeyWordDataViewRight = new RelayCommand<object>((p) => { return true; },
            (p) =>
            {
                string keyWord = ViewSelection.tbKeyWordRight.Text;
                SetDataViewRight(keyWord);
                SetGridViewRight(ViewSelection, DataViewRight);
            });

            Apply = new RelayCommand<object>((p) => { return true; },
            (p) =>
            {
                if (_DataRight.Count == 0)
                {
                    using (Transaction tran = new Transaction(Doc, "Mess Attention"))
                    {
                        tran.Start();
                        TaskDialog.Show("Attention", "Nothing is selected, appication will select all data !");
                        tran.Commit();
                    }
                    DataSelected = _DataSource;

                }
                else 
                {
                    DataSelected = _DataRight;
                }
                ViewSelection.Close();
            });

            #endregion
        }

        public void CreateSelectionView()
        {
            Selection selectionView = new Selection();    
            SetGridViewLeft(selectionView, DataViewLeft);
            selectionView.Closing += new CancelEventHandler(SelectionWindow_Closing);
            selectionView.DataContext = this;
            ViewSelection = selectionView;
            selectionView.ShowDialog();
            
        }
        private void SelectionWindow_Closing(object sender, CancelEventArgs e)
        {
            //Your code to handle the event
            if (_DataRight.Count == 0)
            {
                using (Transaction tran = new Transaction(Doc, "Mess Attention"))
                {
                    tran.Start();
                    TaskDialog.Show("Attention", "Nothing is selected, appication will select all data !");
                    tran.Commit();
                }
                DataSelected = _DataSource;

            }
            else
            {
                DataSelected = _DataRight;
            }
        }

        private void SetGridViewLeft(Selection selectionView, ObservableCollection<RebarInformation> listDataRebar)
        {
            selectionView.lvDataLeft.ItemsSource = listDataRebar;
            CollectionView viewDataLeft = (CollectionView)CollectionViewSource.GetDefaultView(selectionView.lvDataLeft.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("ElementContainInformation");
            viewDataLeft.GroupDescriptions.Add(groupDescription);
        }

        private void SetGridViewRight(Selection selectionView, ObservableCollection<RebarInformation> listDataRebar)
        {
            selectionView.lvDataRight.ItemsSource = listDataRebar;
            CollectionView viewDataRight = (CollectionView)CollectionViewSource.GetDefaultView(selectionView.lvDataRight.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("ElementContainInformation");
            viewDataRight.GroupDescriptions.Add(groupDescription);
        }

        public void SetProperty(DataRebarInformation dataRebarInformation)
        {
            //Set data source
            _DataSourceRebarInfor = dataRebarInformation;
            _DataSource = _DataSourceRebarInfor.ListRebarInfor;
            _DataLeft = new List<RebarInformation>();
            if (_DataRight == null)
                _DataRight = new List<RebarInformation>();
            if (DataSelected == null)
                DataSelected = new List<RebarInformation>();

            //Set level
            LevelMax = dataRebarInformation.MaxLevel;
            LevelMin = dataRebarInformation.MinLevel;
            AllLevels = new ObservableCollection<Level>();
            foreach (Level level in dataRebarInformation.AllLevels)
            {
                AllLevels.Add(level);
            }

            //Set tree view level
            GroupLevels groupLevels = new GroupLevels(dataRebarInformation.AllLevels);
            AllLevelTreeView = new ObservableCollection<GroupLevels>();
            AllLevelTreeView.Add(groupLevels);

            //Set tree view category
            GroupCategories groupCategories = new GroupCategories(dataRebarInformation.AllCategoris);
            AllCategoryTreeView = new ObservableCollection<GroupCategories>();
            AllCategoryTreeView.Add(groupCategories);

            //Set tree view family
            GroupFamilies groupFamilies = new GroupFamilies(dataRebarInformation.AllFamilies);
            AllFamilyTreeView = new ObservableCollection<GroupFamilies>();
            AllFamilyTreeView.Add(groupFamilies);

            //Set tree view type
            GroupType groupType = new GroupType(dataRebarInformation.AllTypes);
            AllTypesTreeView = new ObservableCollection<GroupType>();
            AllTypesTreeView.Add(groupType);

            //Set tree view diameter
            GroupDiameter groupDiameter = new GroupDiameter(dataRebarInformation.AllDiameters);
            AllDiametersTreeView = new ObservableCollection<GroupDiameter>();
            AllDiametersTreeView.Add(groupDiameter);

            //Set data left
            SetDataLeft();
            SetDataViewLeft("");

        }

        // (1) phương thức lấy về data thảo mãn sự lựa chọn trong menu tree view
        private void SetDataLeft()
        {
            _DataLeft = new List<RebarInformation>();
            List<RebarInformation> result = new List<RebarInformation>();

            result = _DataSource
                        .Where(x => GetListLevelIdChecked().IndexOf(x.ECLevel.Id.ToString()) >= 0)
                        .Where(x => GetListCategoryIdChecked().IndexOf(x.ECCategory.Id.ToString()) >= 0)
                        .Where(x => GetListFamilyIdChecked().IndexOf(x.ECFamily.Id.ToString()) >= 0)
                        .Where(x => GetListTypeIdChecked().IndexOf(x.ECFamilySymbol.Id.ToString()) >= 0)
                        .Where(x => GetListDiameterChecked().IndexOf(x.Diameter.ToString()) >= 0)
                        .ToList();
            _DataLeft = result.Distinct(new RebarInforComparer()).ToList();
        }

        // (2) Phương thức lấy về data thỏa mãn keyword trong text box left từ data tree view filter va nạp vào data view left
        private void SetDataViewLeft(string id)
        {
            DataViewLeft = new ObservableCollection<RebarInformation>();
            List<RebarInformation> DataFilterKeyWordLeft = Utils.FindRebarInforByKeyword(_DataLeft, id);
            foreach (RebarInformation data in DataFilterKeyWordLeft)
            {
                DataViewLeft.Add(data);
            }
        }

        // (3) Phuong thuc chuyen data duoc chon tu left sang right
        private void MoveDataLeftToRight()
        {
            //chi chuyen nhung phan tu dc chon trong data view left va khong ton tai trong data view right
            foreach (RebarInformation rebar in DataViewLeft)
            {
                var rebarInfor = _DataRight.Where(x => x.IDRebar == rebar.IDRebar);
                if (rebarInfor.ToList().Count == 0 && rebar.IsCheckedInSelectionViewDataLeft)
                    _DataRight.Add(rebar);
            }
        }

        // (4) Phuong thuc chuyen data right ve data view right
        private void SetDataViewRight(string id)
        {
            DataViewRight = new ObservableCollection<RebarInformation>();
            List<RebarInformation> DataFilterKeyWordRight = Utils.FindRebarInforByKeyword(_DataRight, id);
            foreach (RebarInformation data in DataFilterKeyWordRight)
            {
                DataViewRight.Add(data);
            }
        }

        // (5) Phuong thuc chuyen xoa data right
        private void RemoveDataRight()
        {
            List<RebarInformation> rebars = new List<RebarInformation>();
            foreach (RebarInformation rebar in _DataRight)
            {
                if (rebar.IsCheckedInSelectionViewDataRight)
                    rebars.Add(rebar);
            }
            foreach (RebarInformation rebar in rebars)
            {
                _DataRight.Remove(rebar);
            }

        }
        #region Các phương thức set tree view menu

        //1 - Phương thức lấy về các category được check trong treeview menu
        private List<string> GetListCategoryIdChecked()
        {
            List<string> result = new List<string>();
            if (AllCategoryTreeView.Count > 0)
            {
                foreach (CategoryReadString category in AllCategoryTreeView[0].Members)
                {
                    if (category.IsChecked)
                        result.Add(category.Id);
                }
            }
            return result;
        }

        //2 - Phương thức lấy về các family được check trong treeview menu
        private List<string> GetListFamilyIdChecked()
        {
            List<string> result = new List<string>();
            if (AllFamilyTreeView.Count > 0)
            {
                foreach (FamilyReadString family in AllFamilyTreeView[0].Members)
                {
                    if (family.IsChecked)
                        result.Add(family.Id);
                }
            }
            return result;
        }

        //3 - Phương thức lấy về các type được check trong treeview menu
        private List<string> GetListTypeIdChecked()
        {
            List<string> result = new List<string>();
            if (AllTypesTreeView.Count > 0)
            {
                foreach (TypeReadString type in AllTypesTreeView[0].Members)
                {
                    if (type.IsChecked)
                        result.Add(type.Id);
                }
            }
            return result;
        }

        //4 - Phương thức lấy về các level được check trong treeview menu
        private List<string> GetListLevelIdChecked()
        {
            List<string> result = new List<string>();
            if (AllLevelTreeView.Count > 0)
            {
                foreach (LevelReadString level in AllLevelTreeView[0].Members)
                {
                    if (level.IsChecked)
                        result.Add(level.Id);
                }
            }
            return result;
        }

        //5 - Phương thức lấy về các diameter được check trong treeview menu
        private List<string> GetListDiameterChecked()
        {
            List<string> result = new List<string>();
            if (AllDiametersTreeView.Count > 0)
            {
                foreach (DiameterReadString d in AllDiametersTreeView[0].Members)
                {
                    if (d.IsChecked)
                        result.Add(d.Name);
                }
            }
            return result;
        }

        #endregion 

    }

    #region Class For Binding Tree view
    //Level
    public class GroupLevels
    {
        public string Name { get; set; }
        public ObservableCollection<LevelReadString> Members { get; set; }
        public GroupLevels(List<Level> levels)
        {
            Members = new ObservableCollection<LevelReadString>();
            foreach (Level level in levels)
            {
                Members.Add(new LevelReadString(level));
            }
        }
    }
    public class LevelReadString
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public bool IsChecked { get; set; }
        public LevelReadString(Level level)
        {
            Name = level.Name;
            Id = level.Id.ToString();
            IsChecked = true;
        }
    }


    //Category
    public class GroupCategories
    {
        public string Name { get; set; }
        public ObservableCollection<CategoryReadString> Members { get; set; }
        public GroupCategories(List<Category> categories)
        {
            Members = new ObservableCollection<CategoryReadString>();
            foreach (Category category in categories)
            {
                Members.Add(new CategoryReadString(category));
            }
        }
    }
    public class CategoryReadString
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public bool IsChecked { get; set; }
        public CategoryReadString(Category category)
        {
            Name = category.Name;
            Id = category.Id.ToString();
            IsChecked = true;
        }
    }

    //Family
    public class GroupFamilies
    {
        public string Name { get; set; }
        public ObservableCollection<FamilyReadString> Members { get; set; }
        public GroupFamilies(List<Family> families)
        {
            Members = new ObservableCollection<FamilyReadString>();
            foreach (Family family in families)
            {
                Members.Add(new FamilyReadString(family));
            }
        }
    }
    public class FamilyReadString
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public bool IsChecked { get; set; }
        public FamilyReadString(Family family)
        {
            Name = family.Name;
            Id = family.Id.ToString();
            IsChecked = true;
        }
    }

    //Type
    public class GroupType
    {
        public string Name { get; set; }
        public ObservableCollection<TypeReadString> Members { get; set; }
        public GroupType(List<FamilySymbol> familySymbols)
        {
            Members = new ObservableCollection<TypeReadString>();
            foreach (FamilySymbol familySymbol in familySymbols)
            {
                Members.Add(new TypeReadString(familySymbol));
            }
        }
    }
    public class TypeReadString
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public bool IsChecked { get; set; }

        public TypeReadString(FamilySymbol familySymbol)
        {
            Name = familySymbol.Name;
            Id = familySymbol.Id.ToString();
            IsChecked = true;
        }
    }

    //Diameter
    public class GroupDiameter
    {
        public string Name { get; set; }
        public ObservableCollection<DiameterReadString> Members { get; set; }
        public GroupDiameter(List<int> diameters)
        {
            Members = new ObservableCollection<DiameterReadString>();
            foreach (int d in diameters)
            {
                Members.Add(new DiameterReadString(d));
            }
        }
    }
    public class DiameterReadString
    {
        public string Name { get; set; }
        public bool IsChecked { get; set; }
        public DiameterReadString(int diameter)
        {
            Name = diameter.ToString();
            IsChecked = true;
        }
    }

    #endregion 
}

