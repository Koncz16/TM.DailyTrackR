namespace TM.DailyTrackR
{
  using System.Windows;
    using TM.DailyTrackR.Common;
    using TM.DailyTrackR.View;
  using TM.DailyTrackR.ViewModel;

  public partial class App : Application
  {
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);

            //Register View

            //MainWindow window = new();
            //window.DataContext = new MainWindowViewModel();
            //window.ShowDialog();

            ViewService.Instance.RegisterView(typeof(MainWindowViewModel), typeof(MainWindow));
            ViewService.Instance.RegisterView(typeof(AddDataViewModel), typeof(AddDataWindow));
            ViewService.Instance.RegisterView(typeof(NewActivityViewModel), typeof(NewActivityWindow));
            ViewService.Instance.RegisterView(typeof(ChartViewModel), typeof(PieChartWindow));
            ViewService.Instance.RegisterView(typeof(LoginViewModel), typeof(LoginWindow));
            ViewService.Instance.ShowDialog(new MainWindowViewModel("User A", 1));
            //ViewService.Instance.ShowDialog(new LoginViewModel());


        }
    }
}
