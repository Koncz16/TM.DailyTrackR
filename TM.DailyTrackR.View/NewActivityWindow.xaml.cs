using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using TM.DailyTrackR.Common;
using TM.DailyTrackR.DataType.Enums;
using TM.DailyTrackR.ViewModel;


namespace TM.DailyTrackR.View
{ 
    public partial class NewActivityWindow : Window
    { //   Megcsinálni, hogy adatbázisból vegye ki a dolgokat és akkor modelleket csinálni.
      //   Kitenni a mode.Name-et az ablakba és elmenteni a model.id-t az uj Activity beszurásához
      // Es akkor ez is szebb lesz
        public NewActivityWindow()
        {
            InitializeComponent();
            DataContext = new NewActivityViewModel();
        }

        private void OnSaveButtonClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is NewActivityViewModel viewModel)
            {
                viewModel.SaveActivity();
            }
        }

    }

    }

