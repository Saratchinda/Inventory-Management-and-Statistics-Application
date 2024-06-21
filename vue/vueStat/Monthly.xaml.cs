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
    /// Logique d'interaction pour Monthly.xaml
    /// </summary>
    public partial class Monthly : Page
    {
        public Monthly()
        {
            InitializeComponent();
            List<DateTime> Dates = DAL.Achats.Select(achat => achat.DatePurchase).ToList();
            List<int> distinctMonths = Dates.Select(date => date.Month).Distinct().OrderBy(month => month).ToList();
            cbMois.ItemsSource = distinctMonths;
            cbMoisFin.ItemsSource = distinctMonths;


          

            /*List<DateTime> Datess = DAL.Sales.Select(sale => sale.DateSale).ToList();
            List<int> distinctYearss = Datess.Select(date => date.Month).Distinct().OrderBy(month => month).ToList();
            cbMois.ItemsSource = distinctYearss;
            cbMoisFin.ItemsSource = distinctYearss;*/

          

            List<string> Famille = DAL.Articles.Select(article => article.Famille).Distinct().ToList();
            cbFamille.ItemsSource = Famille;

            List<string> Gamme = DAL.Articles.Select(article => article.Gamme).Distinct().ToList();
            cbGamme.ItemsSource = Gamme;
            List<string> Emplacement = DAL.Articles.Select(article => article.Emplacement).Distinct().ToList();
            cbEmplacement.ItemsSource = Emplacement;

            List<int> distinctYears = Dates.Select(date => date.Year).Distinct().OrderBy(year => year).ToList();
            cbAnne.ItemsSource = distinctYears;
            cbAnneeFin.ItemsSource = distinctYears;
            /*List<int> distinctYearsSale = Datess.Select(date => date.Year).Distinct().OrderBy(year => year).ToList();
            cbAnne.ItemsSource = distinctYearsSale;
            cbAnneeFin.ItemsSource = distinctYearsSale;*/


            cbTypeGraph.Items.Add("Line");
            cbTypeGraph.Items.Add("Bar");
        }

        private void btnCalculer_Click(object sender, RoutedEventArgs e)
        {
            WindStatistique2 windStatistique2 = new WindStatistique2();
            
            string selectedFamille = cbFamille.SelectedItem?.ToString();
            string selectedGamme = cbGamme.SelectedItem?.ToString();
            string selectedEmplacement = cbEmplacement.SelectedItem?.ToString();
            
            if (cbTypeGraph.SelectedItem != null)
            {
                if (cbAnneeFin.SelectedItem != null && cbAnne.SelectedItem!= null)
                {
                    int selectedYaer = int.Parse(cbAnne.SelectedItem.ToString());
                    int selectedYearEnd = int.Parse(cbAnneeFin.SelectedItem.ToString());
                    if (chkAchat.IsChecked == true)
                    {

                        if (cbMois.SelectedItem != null)
                        {
                            int selectedMonth = int.Parse(cbMois.SelectedItem.ToString());


                            string retour = "";
                            if (cbMoisFin.SelectedItem != null)
                            {
                                int selectedMoisFin = int.Parse(cbMoisFin.SelectedItem.ToString());
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

                                windStatistique2.ViewModel.GenerateChartMultipleMonthsAchat(selectedYaer, selectedMonth, selectedYearEnd, selectedMoisFin, famille: selectedFamille, gamme: selectedGamme, emplacement: selectedEmplacement, chartType: cbTypeGraph.Text);
                                windStatistique2.ShowDialog();

                            }
                            else
                            {
                                retour += "Pas de moyenne annuelle" + Environment.NewLine;
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
                                windStatistique2.ViewModel.GenerateChartMonthlyAchat(selectedYaer, selectedMonth, chartType: cbTypeGraph.Text, famille: selectedFamille, gamme: selectedGamme, emplacement: selectedEmplacement);
                                windStatistique2.ShowDialog();

                            }

                        }
                        else
                        {
                            MessageBox.Show("Pas de mois");
                        }

                    }
                    else
                    if (chkVente.IsChecked == true)
                    {
                        if (cbMois.SelectedItem != null)
                        {
                            int selectedMonth = int.Parse(cbMois.SelectedItem.ToString());


                            string retour = "";
                            if (cbMoisFin.SelectedItem != null)
                            {
                                int selectedMonthFin = int.Parse(cbMoisFin.SelectedItem.ToString());
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


                                windStatistique2.ViewModel.GenerateChartMultipleMonthsSale(selectedYaer, selectedMonth , selectedYearEnd, selectedMonthFin, famille: selectedFamille, gamme: selectedGamme, emplacement: selectedEmplacement, chartType: cbTypeGraph.Text);
                                windStatistique2.ShowDialog();


                            }
                            else
                            {
                                retour += "Pas de moyenne annuelle" + Environment.NewLine;
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
                                windStatistique2.ViewModel.GenerateChartMonthlySale( selectedYaer, selectedMonth, chartType: cbTypeGraph.Text, famille: selectedFamille, gamme: selectedGamme, emplacement: selectedEmplacement);
                                windStatistique2.ShowDialog();

                            }

                        }
                        else
                        {
                            MessageBox.Show("Pas de mois");
                        }

                    }
                    else
                    {
                        MessageBox.Show("Veuillez choisir s'il s'agit des ventes ou des achats");
                    }
                }else
                {
                    MessageBox.Show("Veuillez choisir soit l'année les années");
                }
            }
            else
            {
                MessageBox.Show("Veuillez choisir un type de graph");

            }

        }
    }
}
