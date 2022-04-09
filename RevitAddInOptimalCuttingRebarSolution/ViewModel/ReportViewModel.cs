using RevitAddInOptimalCuttingRebarSolution.View;
using RevitAddInOptimalCuttingRebarSolution.zLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Data;

namespace RevitAddInOptimalCuttingRebarSolution.ViewModel
{
    public class ReportViewModel : BaseViewModel
    {
        private Report _ReportView;
        #region Thuoc tinh binding
        //1 - Data view report rebar input
        private ObservableCollection<FreshRebar> _DataRebarInput;
        public ObservableCollection<FreshRebar> DataRebarInput
        {
            get => _DataRebarInput;
            set { _DataRebarInput = value; OnPropertyChanged(); }
        }

        //2 - Data view Data rebar source
        private ObservableCollection<RebarInformation> _DataRebarSource;
        public ObservableCollection<RebarInformation> DataRebarSource
        {
            get => _DataRebarSource;
            set { _DataRebarSource = value; OnPropertyChanged(); }
        }

        //3 - Data view Data rebar cutted 
        private ObservableCollection<RebarInformation> _DataRebarHandled;
        public ObservableCollection<RebarInformation> DataRebarHandled
        {
            get => _DataRebarHandled;
            set { _DataRebarHandled = value; OnPropertyChanged(); }
        }

        //4 - Ty le hao hut kl 
        public double RatioWM { get; set; }
        #endregion

        #region Thuoc tinh Command

        #endregion 
        public ReportViewModel(List<RebarInformation> dataSource, List<FreshRebar> dataRebarInput, List<RebarInformation> dataRebarHandled, double ratioWM)
        {
            DataRebarInput = new ObservableCollection<FreshRebar>();
            DataRebarSource = new ObservableCollection<RebarInformation>();
            DataRebarHandled = new ObservableCollection<RebarInformation>();

            foreach (RebarInformation rebar in dataSource)
            {
                DataRebarSource.Add(rebar);
            }

            foreach (FreshRebar rebar in dataRebarInput)
            {
                DataRebarInput.Add(rebar);
            }

            foreach (RebarInformation rebar in dataRebarHandled)
            {
                DataRebarHandled.Add(rebar);
            }

            RatioWM = ratioWM;
            SetChartStackedColumn();
        }
        public void CreateReportView()
        {
            Report reportView = new Report();
            _ReportView = reportView;
            //SetDataRebarInput(reportView);
            SetDataRebarSource(reportView);
            SetDataRebarHandled(reportView);

            reportView.DataContext = this;

            reportView.ShowDialog();
        }

        #region Phương thức set các data grid view

        // 1 - Set data grid rebar in put - grid view report thep nguyen lieu
        private void SetDataRebarInput(Report viewReport)
        {
            viewReport.lvDataRebarInput.ItemsSource = DataRebarInput;
        }

        // 2 - Set data grid rebar source - grid view report thep tren ban ve do nguoi dung chon 
        private void SetDataRebarSource(Report viewReport)
        {
            viewReport.lvDataSource.ItemsSource = DataRebarSource;
            CollectionView viewDataSource = (CollectionView)CollectionViewSource.GetDefaultView(viewReport.lvDataSource.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("ElementContainInformation");
            viewDataSource.GroupDescriptions.Add(groupDescription);
        }

        // 3 - Set data grid rebar handle - grid view report thep sau khi duoc toi uu
        private void SetDataRebarHandled(Report viewReport)
        {
            viewReport.lvDataHandled.ItemsSource = DataRebarHandled;
            CollectionView viewDataHandled = (CollectionView)CollectionViewSource.GetDefaultView(viewReport.lvDataHandled.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("ElementContainInformation");
            viewDataHandled.GroupDescriptions.Add(groupDescription);
        }
        // 4 - Set data for chart stacked column
        private void SetChartStackedColumn()
        {
            SeriesCollection = new SeriesCollection
            {
                new StackedColumnSeries
                {
                    Values = new ChartValues<double> {4, 5, 6, 8},
                    StackMode = StackMode.Values, // this is not necessary, values is the default stack mode
                    DataLabels = true
                },
                new StackedColumnSeries
                {
                    Values = new ChartValues<double> {2, 5, 6, 7},
                    StackMode = StackMode.Values,
                    DataLabels = true
                }
            };

            //adding series updates and animates the chart
            SeriesCollection.Add(new StackedColumnSeries
            {
                Values = new ChartValues<double> { 6, 2, 7 },
                StackMode = StackMode.Values
            });

            //adding values also updates and animates
            SeriesCollection[2].Values.Add(4d);

            Labels = new[] { "Chrome", "Mozilla", "Opera", "IE" };
            Formatter = value => value + " Mill";
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
        #endregion 
    }


}
