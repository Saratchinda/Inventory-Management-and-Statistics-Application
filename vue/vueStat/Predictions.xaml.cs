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
    /// Logique d'interaction pour Predictions.xaml
    /// </summary>
    public partial class Predictions : Page
    {
        public Predictions()
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

            string selectedFamille = cbFamille.SelectedItem?.ToString();
            string selectedGamme = cbGamme.SelectedItem?.ToString();
            string selectedEmplacement = cbEmplacement.SelectedItem?.ToString();


            if (chkAchat.IsChecked == true)
            {

                if (cbAnne.SelectedItem != null)
                {
                    int selectedYear = int.Parse(cbAnne.SelectedItem.ToString());


                    string retour = "";
                    if (!string.IsNullOrEmpty(txtAnneeFin.Text))
                    {
                        int selectedAnneeFin = int.Parse(txtAnneeFin.Text);
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

                        windStatistique2.ViewModel.GenerateChartPredictionsAchat(selectedYear, selectedAnneeFin, famille: selectedFamille, gamme: selectedGamme, emplacement: selectedEmplacement, chartType: cbTypeGraph.Text);
                        windStatistique2.ShowDialog();

                    }
                    else
                    {
                        MessageBox.Show("Rentrez l'année de prédiction");
                    }

                }
                else
                {
                    MessageBox.Show("Pas d'année initiale à prendre en compte pour la prédiction");
                }
            }
            else
            if (chkVente.IsChecked == true)
            {
                if (cbAnne.SelectedItem != null)
                {
                    int selectedYear = int.Parse(cbAnne.SelectedItem.ToString());


                    string retour = "";
                    if(string.IsNullOrEmpty(txtAnneeFin.Text))
                    {
                        int selectedAnneeFin = int.Parse(txtAnneeFin.Text);
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


                        windStatistique2.ViewModel.GenerateChartPredictionsSale(selectedYear, selectedAnneeFin, famille: selectedFamille, gamme: selectedGamme, emplacement: selectedEmplacement, chartType: cbTypeGraph.Text);
                        windStatistique2.ShowDialog();


                    }
                    else
                    {
                        MessageBox.Show("Rentrez l'année de prédiction");

                    }

                }
                else
                {
                    MessageBox.Show("Pas d'année");
                }

            }
            else
            {
                MessageBox.Show("Veuillez choisir s'il s'agit des ventes ou des achats");
            }

        }

    }
}
