using DocumentFormat.OpenXml.Wordprocessing;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TM.DailyTrackR.Common;
using TM.DailyTrackR.DataType.Enums;
using TM.DailyTrackR.DataType.Models;
using TM.DailyTrackR.Logic;

namespace TM.DailyTrackR.ViewModel
{
    public class LoginViewModel : BindableBase
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        private RoleEnum _roleId;
        public RoleEnum RoleId
        {
            get { return _roleId; }
            set { SetProperty(ref _roleId, value); }
        }

        public List<RoleEnum> RoleList { get; private set; }

        public DelegateCommand LoginCommand { get; private set; }

        public LoginViewModel()
        {
            RoleList = Enum.GetValues(typeof(RoleEnum)).Cast<RoleEnum>().ToList();
            RoleId = RoleEnum.User;
            LoginCommand = new DelegateCommand(OnLogin);


        }

        private void NewWindowExecute(UserModel user)
        {
            ViewService.Instance.ShowWindow(new MainWindowViewModel (user.Name, (int)user.RoleId));
        }
        private void OnLogin()
        {


            var user = LogicHelper.Instance.ExampleController.GetUserByName(Name);

            if (user != null && user.Password == Password)
            {
                MessageBox.Show("Login successful!", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                OpenMainWindow(user);
            }
            else
            {
                MessageBox.Show("Login error.", "Cancellation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void OpenMainWindow(UserModel user)
        {
            this.NewWindowExecute(user);

            Application.Current.MainWindow.Close();
        }



    }
}
