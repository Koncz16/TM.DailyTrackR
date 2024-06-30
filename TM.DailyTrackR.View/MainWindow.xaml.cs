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
    using System.Text;
    using ClosedXML.Excel;
    using System.Collections.Generic;

    public partial class MainWindow : Window
  {

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void DailyDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void ExportToCsv(object sender, RoutedEventArgs e)
        {
            //var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            //{
            //    Filter = "CSV Files (*.csv)|*.csv",
            //    DefaultExt = ".csv"
            //};
            if (textBlock.Text != "")
            {

                    var startDate = SelectorCalendar.SelectedDates.Min();
                var endDate = SelectorCalendar.SelectedDates.Max();
                var activitiesRange = LogicHelper.Instance.ExampleController.GetActivitiesBetweenDates(startDate, endDate);
                var filename = $"TeamWeekActivity_{startDate.ToString("dd.MM.yyyy")}_{endDate.ToString("dd.MM.yyyy")}.csv";
                string relativePath = $"{filename}";
                string fullPath = System.IO.Path.Combine(@"D:\Topmotive\tmDailyTrackR\TM.DailyTrackR\TM.DailyTrackR", relativePath);

                ExportToCsv(fullPath, activitiesRange);
                MessageBox.Show("Data successfully exported into CVS!", "Export Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Please select a period to export.", "Period Not Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
}


        public void ExportToCsv(string filePath, IEnumerable<ActivityModel> data)
        {
            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("Project Type,Task Type,Description,Status");

            foreach (var item in data)
            {
                csvBuilder.AppendLine($"{item.ProjectTypeDescription},{item.ActivityTypeId},{item.Description},{item.StatusId}");
            }

            File.WriteAllText(filePath, csvBuilder.ToString());
        }


        private void ExportToExcel(object sender, RoutedEventArgs e)
        {
            //var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            //{
            //    Filter = "Excel Files (*.xlsx)|*.xlsx",
            //    DefaultExt = ".xlsx"
            //};

            if (textBlock.Text != "")
            {
                var startDate = SelectorCalendar.SelectedDates.Min();
                var endDate = SelectorCalendar.SelectedDates.Max();
                var activitiesRange = LogicHelper.Instance.ExampleController.GetActivitiesBetweenDates(startDate, endDate);
                var filename = $"TeamWeekActivity_{startDate.ToString("dd.MM.yyyy")}_{endDate.ToString("dd.MM.yyyy")}.xlsx";
                string relativePath = $"{filename}";
                string fullPath = System.IO.Path.Combine(@"D:\Topmotive\tmDailyTrackR\TM.DailyTrackR\TM.DailyTrackR", relativePath);

                ExportToExcel(fullPath, activitiesRange);
                MessageBox.Show("Data successfully exported into excel!", "Export Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Please select a period to export.", "Period Not Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        public void ExportToExcel(string filePath, IEnumerable<ActivityModel> data)
    {
        var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Activities");

        // Header
        worksheet.Cell(1, 1).Value = "Project Type";
        worksheet.Cell(1, 2).Value = "Task Type";
        worksheet.Cell(1, 3).Value = "Description";
        worksheet.Cell(1, 4).Value = "Status";

        // Data
        var row = 2;
        foreach (var item in data)
        {
            worksheet.Cell(row, 1).Value = item.ProjectTypeDescription;
            worksheet.Cell(row, 2).Value = item.ActivityTypeId.ToString();
            worksheet.Cell(row, 3).Value = item.Description;
            worksheet.Cell(row, 4).Value = item.StatusId.ToString();
            row++;
        }

        workbook.SaveAs(filePath);
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
                MessageBox.Show("Data successfully exported into txt!", "Export Successful", MessageBoxButton.OK, MessageBoxImage.Information);
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