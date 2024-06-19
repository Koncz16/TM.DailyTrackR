namespace TM.DailyTrackR.View
{
  using System.Windows;
    using static System.Net.Mime.MediaTypeNames;
    using System.Windows.Controls;
    using System.Windows.Data;

  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
            //DataContext = new MainWindowViewModel()


    //        List<string> list = new List<string>
    //{
    //            "Ana are mere",
    //            "I need Jesus",
    //            "3 lei kg la kartofi",
    //            "!!!!Bogdan Marcel"
    //        };
    //        List<Test> listOfTests = new List<Test>
    //        {
    //       new Test("text1",true, list, "https://www.google.com/"),
    //       new Test("text1",false, list, "https://www.google.com/"),
    //       new Test("text1",true, list, "https://www.google.com/"),
    //       new Test("text1",false, list, "https://www.google.com/")
    //   };
    //        this.AddItemsInDatagrid(listOfTests);

    //        //listOfNumbers = new List<string>
    //        //{
    //        //   "1",
    //        //   "2",
    //        //   "3",
    //        //   "4",
    //        //  "5",
    //        // "6",

    //        // };
    //        //Combo.ItemsSource = listOfNumbers;
    //        //Combo.SelectedIndex = 0;
    //        //Combo.Items.Clear(); (only ui items)
        }
        private void ApasaEvent(object sender, EventArgs e)
        {
            // MessageBox.Show("Imagine frumix");
            //BindingExpression bindingExpression = text.GetBindingExpression(TextBox.TextProperty);
            //bindingExpression.UpdateSource();
        }
        private void AddItemsInDatagrid(List<Test> listOfTests)
        {
            //ItemsDataGrid.ItemsSource = listOfTests;
        }
    }

}