namespace TM.DailyTrackR.View
{
  using System.Windows;
    using static System.Net.Mime.MediaTypeNames;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Diagnostics;
    using TM.DailyTrackR.Common;
    using TM.DailyTrackR.ViewModel;
    using TM.DailyTrackR.DataType.Models;
    using System.IO;
    using TM.DailyTrackR.DataType.Enums;

    public partial class MainWindow : Window
  {

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
        private void ExportToFile(object sender, RoutedEventArgs e)
        {
            if (textBlock.Text != "")

            {
                var startDate = SelectorCalendar.SelectedDates.Min();
                var endDate = SelectorCalendar.SelectedDates.Max();
                var activitiesRange = LogicHelper.Instance.ExampleController.GetActivitiesBetweenDates(startDate, endDate);
                var filename = $"TeamWeekActivity_{startDate.ToString("dd.MM.yyyy")}_{endDate.ToString("dd.MM.yyyy")}.txt";
                string relativePath = $"{filename}";
                // string fullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
                string fullPath = System.IO.Path.Combine(@"D:\Topmotive\tmDailyTrackR\TM.DailyTrackR\TM.DailyTrackR", relativePath);

                ExportActivitiesToTxt(activitiesRange, fullPath, startDate, endDate);
                MessageBox.Show("Data successfully exported!", "Export Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Please select a period to export.", "Period Not Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        public void ExportActivitiesToTxt(List<ActivityModel> activities, string filePath, DateTime startDate, DateTime endDate)
        {
            var groupedActivities = activities
            .OrderBy(a => a.ProjectTypeDescription)
            .ThenBy(a => a.ActivityTypeId)
            .ThenBy(a => a.StatusId)
            .GroupBy(a => new { a.ProjectTypeDescription, a.ActivityTypeId });

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine($"Team Activity in the period {startDate.ToString("dd.MM.yyyy")} – {endDate.ToString("dd.MM.yyyy")}");
                    writer.WriteLine();

                    foreach (var group in groupedActivities)
                    {
                        writer.WriteLine($"{group.Key.ProjectTypeDescription}:");

                        var fixTasks = group.Where(a => a.ActivityTypeId == TaskTypeEnum.Fix).OrderBy(a => a.Description);
                        var newTasks = group.Where(a => a.ActivityTypeId == TaskTypeEnum.New).OrderBy(a => a.Description);

                        if (fixTasks.Any())
                        {
                            writer.WriteLine("    Fix:");
                            foreach (var activity in fixTasks)
                            {
                                string statusName = ((StatusEnum)activity.StatusId).ToString();
                                writer.WriteLine($"        {statusName} - {activity.Description}");
                            }
                            writer.WriteLine();
                        }

                        if (newTasks.Any())
                        {
                            writer.WriteLine("    New:");
                            foreach (var activity in newTasks)
                            {
                                string statusName = ((StatusEnum)activity.StatusId).ToString();
                                writer.WriteLine($"        {statusName} - {activity.Description}");
                            }
                            writer.WriteLine();
                        }
                    }
                }
        }

            private void SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectorCalendar.SelectedDates.Count > 0)
            {
                var startDate = SelectorCalendar.SelectedDates.Min();
                var endDate = SelectorCalendar.SelectedDates.Max();
                textBlock.Text = $"{startDate.ToShortDateString()} - {endDate.ToShortDateString()}";
            }
            else
            {
                textBlock.Text = "No selected range.";
            }
        }

        private void CurrentCellChanged(object sender, EventArgs e)
        {
            var dataGrid = sender as DataGrid;
            if (dataGrid != null && dataGrid.SelectedItem != null)
            {
                var viewModel = DataContext as MainWindowViewModel;
                if (viewModel != null)
                {
                    var editedItem = dataGrid.SelectedItem as ActivityModel;
                    if (editedItem != null)
                    {
                        LogicHelper.Instance.ExampleController.UpdateActivity(editedItem);
                        MessageBox.Show("Item Updated successfully.", "Update", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                }
            }
        }
    
    
    }

}