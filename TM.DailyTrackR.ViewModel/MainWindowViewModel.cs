namespace TM.DailyTrackR.ViewModel
{
    using Prism.Commands;
    using Prism.Mvvm;
    using System;
    using TM.DailyTrackR.Common;
    using TM.DailyTrackR.Logic;

    public class MainWindowViewModel : BindableBase
    {
        private User user;
        private string username;
        private string password;
        private int number;
        private List<string> listOfNumber;

        private List<User>  listOfUser;

        public List<User> ListOfUser
        {
            get => listOfUser;
            set => this.SetProperty(ref this.listOfUser, value);
        }

        public List<string> ListOfNumber
        {
            get => listOfNumber;
            set => this.SetProperty(ref this.listOfNumber, value);
        }


        public int Number
        {
            get => number;
            set => this.SetProperty(ref this.number, value);
        }


        public MainWindowViewModel()
        {
            // LogicHelper.Instance.ExampleController.GetDataExample();
            //LogicHelper.Instance.ExampleController.InsertProjectType();
            // LogicHelper.Instance.ExampleController.InsertNewActivity();
            this.listOfNumber = new List<string>
            {
                "1",
                "2",
                "3",
                "4",
                "5",
                    "6",
            };
            this.listOfUser = new List<User>{
                new User { Username = "Ana", Password = "pita" },
                new User { Username = "Mihai", Password = "zvxvzx" },
                new User { Username = "Simon", Password = "asda" },
                new User { Username = "Ellaa", Password = "qwerqwr" }
            };

            this.user = new User { Username = "Ana", Password = "pita" };
            this.username = this.user.Username;
            this.password = this.user.Password;
            IncreaseNumber = new DelegateCommand(IncreaseNumberExecute, IncreaseNumberCanExecute);
            NewWindowCommand = new DelegateCommand(NewWindowExecute);
            DeleteElementFormIndex = new DelegateCommand(DeleteElementFormIndexExecute);

        }

     

        public DelegateCommand IncreaseNumber { get; }
        public DelegateCommand NewWindowCommand { get; }
        public DelegateCommand DeleteElementFormIndex { get; }

        public string Username
        {
            get => this.username;
            set => this.SetProperty<string>(ref this.username, value);
        }
        public string Password
        {
            get => this.password;
            set => this.SetProperty<string>(ref this.password, value);

        }

        private void IncreaseNumberExecute() => Number++;

        //private bool IncreaseNumberCanExecute()
        //{
        //    if( this.number < 5)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
        private bool IncreaseNumberCanExecute() => this.number < 5;

        private void DeleteElementFormIndexExecute()
        {
            this.listOfNumber.RemoveAt(2);
        }


        private void NewWindowExecute()
        {
            ViewService.Instance.ShowWindow(new AddDataViewModel());
        }
      

    }
}
