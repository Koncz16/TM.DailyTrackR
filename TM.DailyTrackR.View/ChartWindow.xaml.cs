using System.Windows;
using TM.DailyTrackR.ViewModel;

namespace TM.DailyTrackR.View
{
    public partial class PieChartWindow : Window
    {
        public PieChartWindow()
        {
            InitializeComponent();
            DataContext = new ChartViewModel();
        }
    }
}
