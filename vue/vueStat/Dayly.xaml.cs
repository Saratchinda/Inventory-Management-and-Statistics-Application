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
    /// Logique d'interaction pour Dayly.xaml
    /// </summary>
    public partial class Dayly : Page
    {
        public Dayly()
        {
            InitializeComponent();
            List<DateTime> Dates = DAL.Achats.Select(achat => achat.DatePurchase).ToList();
            List<int> distinctMonths = Dates.Select(date => date.Month).Distinct().OrderBy(month => month).ToList();
            cbMois.ItemsSource = distinctMonths;
            cbMoisFin.ItemsSource = distinctMonths;
            List<int> distinctDays = Dates.Select(date => date.Month).Distinct().OrderBy(day => day).ToList();
            cbTJourDebut.ItemsSource = distinctDays;
            cbJourFin.ItemsSource = distinctDays;




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
                if (cbAnneeFin.SelectedItem != null && cbAnne.SelectedItem != null)
                {
                    int selectedYaer = int.Parse(cbAnne.SelectedItem.ToString());
                    int selectedYearEnd = int.Parse(cbAnneeFin.SelectedItem.ToString());
                    if (cbMois.SelectedItem != null && cbMoisFin.SelectedItem != null)
                    {
                        int selectedMonth = int.Parse(cbAnne.SelectedItem.ToString());
                        int selectedMonthEnd = int.Parse(cbAnneeFin.SelectedItem.ToString());
                        if (chkAchat.IsChecked == true)
                        {

                            if (cbTJourDebut.SelectedItem != null)
                            {
                                int selectedDay = int.Parse(cbTJourDebut.SelectedItem.ToString());


                                string retour = "";
                                if (cbJourFin.SelectedItem != null)
                                {
                                    int selectedDayFin = int.Parse(cbJourFin.SelectedItem.ToString());
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

                                    windStatistique2.ViewModel.GenerateChartMultipleDaysAchat(selectedYaer, selectedMonth, selectedDay, selectedYearEnd, selectedMonthEnd, selectedDayFin, famille: selectedFamille, gamme: selectedGamme, emplacement: selectedEmplacement, chartType: cbTypeGraph.Text);
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
                                    windStatistique2.ViewModel.GenerateChartDailyAchat(selectedYaer, selectedMonth, selectedDay, chartType: cbTypeGraph.Text, famille: selectedFamille, gamme: selectedGamme, emplacement: selectedEmplacement);
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
                                int selectedDay = int.Parse(cbTJourDebut.SelectedItem.ToString());


                                string retour = "";
                                if (cbMoisFin.SelectedItem != null)
                                {
                                    int selectedDayEnd = int.Parse(cbJourFin.SelectedItem.ToString());
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


                                    windStatistique2.ViewModel.GenerateChartMultipleDaysSale(selectedYaer, selectedDay, selectedDay, selectedYearEnd, selectedMonthEnd, selectedDayEnd, famille: selectedFamille, gamme: selectedGamme, emplacement: selectedEmplacement, chartType: cbTypeGraph.Text);
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
                                    windStatistique2.ViewModel.GenerateChartDailySale(selectedYaer, selectedDay, selectedDay, chartType: cbTypeGraph.Text, famille: selectedFamille, gamme: selectedGamme, emplacement: selectedEmplacement);
                                    windStatistique2.ShowDialog();

                                }

                            }
                            else
                            {
                                MessageBox.Show("Pas de jour");
                            }

                        }
                        else
                        {
                            MessageBox.Show("Veuillez choisir s'il s'agit des ventes ou des achats");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Choisir les mois");
                    }
                }
                else
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
