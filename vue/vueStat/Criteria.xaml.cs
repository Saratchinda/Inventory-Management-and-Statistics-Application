using Dancewave.vue;
using Log_in.mvvm.mvvmDal;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Log_in.vue.vueStat
{
    /// <summary>
    /// Logique d'interaction pour Yearly.xaml
    /// </summary>
    public partial class Yearly : Page
    {
        public Yearly()
        {
            InitializeComponent();
            List<DateTime> Dates = DAL.Achats.Select(achat => achat.DatePurchase).ToList();
            List<int> distinctYears = Dates.Select(date => date.Year).Distinct().OrderBy(year => year).ToList();
            cbAnne.ItemsSource = distinctYears;



            List<DateTime> Datess = DAL.Sales.Select(sale => sale.DateSale).ToList();
            List<int> distinctYearss = Dates.Select(date => date.Year).Distinct().OrderBy(year => year).ToList();
            cbAnne.ItemsSource = distinctYearss;





            List<string> Famille = DAL.Articles.Select(article => article.Famille).Distinct().ToList();
            cbFamille.ItemsSource = Famille;

            List<string> Gamme = DAL.Articles.Select(article => article.Gamme).Distinct().ToList();
            cbGamme.ItemsSource = Gamme;
            List<string> Emplacement = DAL.Articles.Select(article => article.Emplacement).Distinct().ToList();
            cbEmplacement.ItemsSource = Emplacement;



            cbTypeGraph.Items.Add("Line");
            cbTypeGraph.Items.Add("Bar");
        }
        private void btnCalculer_Click(object sender, RoutedEventArgs e)
        {
            WindStatistique2 windStatistique2 = new WindStatistique2();
            //int selectedYear = int.Parse(cbAnne.SelectedItem.ToString());
            string selectedFamille = cbFamille.SelectedItem?.ToString();
            string selectedGamme = cbGamme.SelectedItem?.ToString();
            string selectedEmplacement = cbEmplacement.SelectedItem?.ToString();
            //int selectedYear = int.Parse(cbAnne.SelectedItem.ToString());
            //int selectedMonth = int.Parse(cbMoisDebut.SelectedItem.ToString());

            if (cbTypeGraph.SelectedItem != null)
            {
                if (chkAchat.IsChecked == true)
                {

                    if (cbAnne.SelectedItem != null)
                    {
                        int selectedYear = int.Parse(cbAnne.SelectedItem.ToString());


                        string retour = "";

                        if (string.IsNullOrEmpty(selectedFamille))
                        {
                            retour += "pas de famille" + Environment.NewLine;
                        }
                        if (string.IsNullOrEmpty(selectedGamme))
                        {
                            retour += "pas de gamme" + Environment.NewLine;
                        }
                        if (string.IsNullOrEmpty(selectedEmplacement))
                        {
                            retour += "pas d'emplacement" + Environment.NewLine;
                        }

                        if (!string.IsNullOrEmpty(retour))
                        {
                            MessageBox.Show(retour);
                        }

                        windStatistique2.ViewModel.GenerateChartYearlyAchatCriteria(selectedYear, famille: selectedFamille, gamme: selectedGamme, emplacement: selectedEmplacement, chartType: cbTypeGraph.Text);
                        windStatistique2.ShowDialog();




                    }
                    else
                    {
                        MessageBox.Show("Statistique de critère");
                        windStatistique2.ViewModel.GenerateChartYearlyAchatCriteria(null, famille: selectedFamille, gamme: selectedGamme, emplacement: selectedEmplacement, chartType: cbTypeGraph.Text);
                        windStatistique2.ShowDialog();

                    }
                }
                else
                if (chkVente.IsChecked == true)
                {
                    if (cbAnne.SelectedItem != null)
                    {
                        int selectedYear = int.Parse(cbAnne.SelectedItem.ToString());


                        string retour = "";

                        if (string.IsNullOrEmpty(selectedFamille))
                        {
                            retour += "pas de famille" + Environment.NewLine;
                        }
                        if (string.IsNullOrEmpty(selectedGamme))
                        {
                            retour += "pas de gamme" + Environment.NewLine;
                        }
                        if (string.IsNullOrEmpty(selectedEmplacement))
                        {
                            retour += "pas d'emplacement" + Environment.NewLine;
                        }



                        if (!string.IsNullOrEmpty(retour))
                        {
                            MessageBox.Show(retour);
                        }


                        windStatistique2.ViewModel.GenerateChartYearlySaleCriteria(selectedYear, famille: selectedFamille, gamme: selectedGamme, emplacement: selectedEmplacement, chartType: cbTypeGraph.Text);
                        windStatistique2.ShowDialog();




                    }
                    else
                    {
                        MessageBox.Show("Statistique de critère");
                        windStatistique2.ViewModel.GenerateChartYearlySaleCriteria(null, famille: selectedFamille, gamme: selectedGamme, emplacement: selectedEmplacement, chartType: cbTypeGraph.Text);
                        windStatistique2.ShowDialog();
                    }

                }
                else
                {
                    MessageBox.Show("Veuillez choisir s'il s'agit des ventes ou des achats");
                }
            }
            else
            {
                MessageBox.Show("Veuillez choisir un type de graph");

            }

        }
    }
}
