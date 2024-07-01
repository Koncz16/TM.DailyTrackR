using LiveCharts;
using LiveCharts.Wpf;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using TM.DailyTrackR.Common;
using TM.DailyTrackR.DataType.Models;


namespace TM.DailyTrackR.ViewModel
{
    public class ChartViewModel : BindableBase
    {
        private SeriesCollection _seriesCollection;

        public SeriesCollection SeriesCollection
        {
            get => _seriesCollection;
            set => SetProperty(ref _seriesCollection, value);
        }

        public ChartViewModel()
        {
            LoadData();
        }

        private void LoadData()
        {
            var data = LogicHelper.Instance.ExampleController.GetUserAllActivities("User A");

            var groupedData = data.GroupBy(a => a.ProjectTypeDescription)
                                  .Select(g => new ProjectTypeData
                                  {
                                      ProjectType = g.Key,
                                      Count = g.Count()
                                  }).ToList();

            SeriesCollection = new SeriesCollection();

            foreach (var item in groupedData)
            {
                SeriesCollection.Add(new PieSeries
                {
                    Title = item.ProjectType,
                    Values = new ChartValues<int> { item.Count }
                });
            }
        }
    }

    public class ProjectTypeData
    {
        public string ProjectType { get; set; }
        public int Count { get; set; }
    }
}
