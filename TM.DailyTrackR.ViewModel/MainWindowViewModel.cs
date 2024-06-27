using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using TM.DailyTrackR.Common;
using TM.DailyTrackR.DataType.Models;
using TM.DailyTrackR.Logic;

namespace TM.DailyTrackR.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        private DateTime _selectedDate = DateTime.Now;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (SetProperty(ref _selectedDate, value))
                {
                    LoadData();
                    UpdateActivitiesDateText();
                }
            }
        }

        private string _activitiesDateText;
        public string ActivitiesDateText
        {
            get => _activitiesDateText;
            set => SetProperty(ref _activitiesDateText, value);
        }

        private ObservableCollection<ActivityModel> _dailyActivities = new ObservableCollection<ActivityModel>();
        public ObservableCollection<ActivityModel> DailyActivities
        {
            get => _dailyActivities;
            set => SetProperty(ref _dailyActivities, value);
        }

        private ObservableCollection<ActivityModel> _activitiesForAll = new ObservableCollection<ActivityModel>();
        public ObservableCollection<ActivityModel> ActivitiesForAll
        {
            get => _activitiesForAll;
            set => SetProperty(ref _activitiesForAll, value);
        }

        public DelegateCommand AddNewActivityCommand { get; }
        public DelegateCommand DeleteCommand { get; }
        public DelegateCommand CreateCommand { get; }

        public MainWindowViewModel()
        {
            DeleteCommand = new DelegateCommand(DeleteExecute);
            CreateCommand = new DelegateCommand(NewWindowExecute);
            AddNewActivityCommand = new DelegateCommand(NewWindowExecute);

            LoadData();
        }

        private void DeleteExecute()
        {
            if (SelectedActivity == null)
                return;

            var result = MessageBox.Show("Are you sure you want to delete this item?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var res = LogicHelper.Instance.ExampleController.DeleteActivity(SelectedActivity.Id);
                if (res == 0)
                {
                    DailyActivities.Remove(SelectedActivity);
                    MessageBox.Show("Item deleted successfully.", "Delete", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void NewWindowExecute()
        {
            ViewService.Instance.ShowWindow(new NewActivityViewModel());
        }

        private void LoadData()
        {
            LoadDataForUser(SelectedDate);
            LoadDataForAllUser(SelectedDate);
        }

        private void LoadDataForAllUser(DateTime date)
        {
            var data = LogicHelper.Instance.ExampleController.GetAllActivities(date);
            ActivitiesForAll = new ObservableCollection<ActivityModel>(data);
        }

        private void LoadDataForUser(DateTime date)
        {
            var data = LogicHelper.Instance.ExampleController.GetUserActivities(date);
            DailyActivities = new ObservableCollection<ActivityModel>(data);
        }

        private void UpdateActivitiesDateText()
        {
            ActivitiesDateText = $"Activities Date: {SelectedDate.ToString("dd.MM.yyyy")}";
        }

        private ActivityModel _selectedActivity;
        public ActivityModel SelectedActivity
        {
            get => _selectedActivity;
            set => SetProperty(ref _selectedActivity, value);
        }
    }
}
