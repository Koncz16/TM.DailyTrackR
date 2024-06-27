using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.DailyTrackR.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;
    using TM.DailyTrackR.DataType.Enums;
    using System.Windows;
    using TM.DailyTrackR.Common;

    public class NewActivityViewModel : BindableBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public NewActivityViewModel()
        {
            FillComboboxes();
        }

        private void FillComboboxes()
        {
            TaskTypes = Enum.GetValues(typeof(TaskTypeEnum)).Cast<TaskTypeEnum>().ToList();
            Statuses = Enum.GetValues(typeof(StatusEnum)).Cast<StatusEnum>().ToList();
            ProjectTypes = Enum.GetValues(typeof(ProjectTypeEnum)).Cast<ProjectTypeEnum>().ToList();
        }

        public void SaveActivity()
        {
            bool isAnyFieldFilled = !string.IsNullOrEmpty(Description) ||
                                    SelectedProjectType != null ||
                                    SelectedTaskType != null ||
                                    SelectedStatus != null ||
                                    ActivityDate.HasValue;

            if (isAnyFieldFilled)
            {
                var resp = LogicHelper.Instance.ExampleController.InsertNewActivity(
                    (int)SelectedProjectType,
                    (int)SelectedTaskType ,
                    Description,
                    (int)SelectedStatus ,
                    (DateTime)ActivityDate);
                if (resp == 0)
                {

                    MessageBox.Show("Activity saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Failed to save activity. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            else
            {
                MessageBox.Show("Fill at least one field, please!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        private TaskTypeEnum? _selectedTaskType;
        public TaskTypeEnum? SelectedTaskType
        {
            get => _selectedTaskType;
            set
            {
                _selectedTaskType = value;
                OnPropertyChanged();
            }
        }

        private ProjectTypeEnum? _selectedProjectType;
        public ProjectTypeEnum? SelectedProjectType
        {
            get => _selectedProjectType;
            set
            {
                _selectedProjectType = value;
                OnPropertyChanged();
            }
        }

        private StatusEnum? _selectedStatus;
        public StatusEnum? SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                _selectedStatus = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _activityDate;
        public DateTime? ActivityDate
        {
            get => _activityDate;
            set
            {
                _activityDate = value;
                OnPropertyChanged();
            }
        }

        public List<TaskTypeEnum> TaskTypes { get; private set; }
        public List<ProjectTypeEnum> ProjectTypes { get; private set; }
        public List<StatusEnum> Statuses { get; private set; }
    }

}
