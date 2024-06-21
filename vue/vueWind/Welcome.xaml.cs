using Dancewave.vue;
using Log_in.vue.vueStat;
using Log_in.vue.vueWind;
using Log_in.vueStat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
namespace Log_in.vue
{
    /// <summary>
    /// Logique d'interaction pour Welcome.xaml
    /// </summary>
    public partial class Welcome : Window
    {
        public Welcome()
        {
            InitializeComponent();
            
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Dashboard_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Dashboard());
        }

        private void BtnItems_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AddArticles());
        }

        private void BtnSales_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new WindSales());
        }

        private void BtnPurchases_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new WindAchats());
        }

        private void BtnStats_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Yearly());

        }

        private void Log_out_Click(object sender, RoutedEventArgs e)
        {
            Close();
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
        }

        private void Dayly_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Dayly());
        }

        private void Yearly_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new WindStatistiques());
        }

        private void Monthly_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Monthly());
        }

        private void Predictions_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Predictions());
        }

        private void btnDaily_Click(object sender, RoutedEventArgs e)
        {

            MainFrame.Navigate(new Dayly());
        }
    }
}
