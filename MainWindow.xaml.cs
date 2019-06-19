using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //this.Loaded += new System.Windows.RoutedEventHandler(this.MainWindowLoaded);



            ObservableCollection<Measurment> _measurments = new ObservableCollection<Measurment>
            {
                new Measurment("11....+00000016 21.324+20818500 22.324+08236500 31..00+00000760 81..00-00050299 82..00+00000173 83..00-00000930 87..10+00000001 w\r\n")
                //new Measurment(2, 43, 35, 22),
                //new Measurment(3, 24, 35, 22),
                //new Measurment(4, 24, 35, 22),
                //new Measurment(5, 24, 35, 22)
            };

            MainVM mainVM = (MainVM)this.DataContext;
            mainVM.Measurments = _measurments;
        }

      
        private void MainWindowLoaded(object sender, EventArgs e)
        {           
            MainVM mainVM = (MainVM)this.DataContext;
            mainVM.Show_spOptions.Execute("");            
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
