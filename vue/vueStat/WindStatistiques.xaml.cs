using Dancewave.modele;
using Dancewave.vue;
using Log_in.mvvm.mvvmDal;
using MaterialDesignThemes.Wpf;
//using Log_in.vueStat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
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
using System.Windows.Shapes;
using Log_in.vue;
using Log_in.vue.vueStat;

namespace Log_in.vueStat
{
    /// <summary>
    /// Logique d'interaction pour WindStatistiques.xaml
    /// </summary>
    public partial class WindStatistiques : Page
    {
        public WindStatistiques()
        {
            InitializeComponent();
           
            List<DateTime> Dates = DAL.Achats.Select(achat => achat.DatePurchase).ToList();
            List<int> distinctYears = Dates.Select(date => date.Year).Distinct().OrderBy(year => year).ToList();
            cbAnne.ItemsSource = distinctYears;
            cbAnneeFin.ItemsSource = distinctYears;



           


            List<DateTime> Datess = DAL.Sales.Select(sale => sale.DateSale).ToList();
            List<int> distinctYearss = Dates.Select(date => date.Year).Distinct().OrderBy(year => year).ToList();
            cbAnne.ItemsSource = distinctYearss;
            cbAnneeFin.ItemsSource = distinctYearss;

            





            List<string> Famille = DAL.Articles.Select(article => article.Famille).Distinct().ToList();
            cbFamille.ItemsSource = Famille;

            List<string> Gamme = DAL.Articles.Select(article => article.Gamme).Distinct().ToList();
            cbGamme.ItemsSource = Gamme;
            List<string> Emplacement = DAL.Articles.Select(article => article.Emplacement).Distinct().ToList();
            cbEmplacement.ItemsSource = Emplacement;



            cbTypeGraph.Items.Add("Line");
            cbTypeGraph.Items.Add("Bar");

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
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
                    if (cbAnneeFin.SelectedItem != null)
                    {
                        int selectedAnneeFin = int.Parse(cbAnneeFin.SelectedItem.ToString());
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

                        windStatistique2.ViewModel.GenerateChartMultipleYearsAchat(selectedYear, selectedAnneeFin, famille: selectedFamille, gamme: selectedGamme, emplacement: selectedEmplacement, chartType: cbTypeGraph.Text);
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
                        windStatistique2.ViewModel.GenerateChartYearlyAchat(selectedYear, chartType: cbTypeGraph.Text, famille: selectedFamille, gamme: selectedGamme, emplacement: selectedEmplacement);
                        windStatistique2.ShowDialog();

                    }

                }
                else
                {
                    MessageBox.Show("Pas d'année");
                }
            }
            else
            if (chkVente.IsChecked == true)
            {
                if (cbAnne.SelectedItem != null)
                {
                    int selectedYear = int.Parse(cbAnne.SelectedItem.ToString());


                    string retour = "";
                    if (cbAnneeFin.SelectedItem != null)
                    {
                        int selectedAnneeFin = int.Parse(cbAnneeFin.SelectedItem.ToString());
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


                        windStatistique2.ViewModel.GenerateChartMultipleYearsSale(selectedYear, selectedAnneeFin, famille: selectedFamille, gamme: selectedGamme, emplacement: selectedEmplacement, chartType: cbTypeGraph.Text);
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
                        windStatistique2.ViewModel.GenerateChartYearlySale(selectedYear, chartType: cbTypeGraph.Text, famille: selectedFamille, gamme: selectedGamme, emplacement: selectedEmplacement);
                        windStatistique2.ShowDialog();

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

        private void chkAchat_Checked(object sender, RoutedEventArgs e)
        {
            UpdateSelections();
        }

        private void chkAchat_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateSelections();
        }

        private void chkVente_Checked(object sender, RoutedEventArgs e)
        {
            UpdateSelections();
        }

        private void chkVente_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateSelections();
        }

        private void cbTypeGraph_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSelections();
        }

        private void UpdateSelections()
        {
            /*// Mettez à jour les TextBlock avec les sélections actuelles
            txtAchatsVentes.Text = $"Achats: {chkAchat.IsChecked}, Ventes: {chkVente.IsChecked}";
            txtFamille.Text = $"Famille: {cbFamille.SelectedItem}";
            txtPeriode.Text = $"Période: {cbMoisDebut.SelectedItem} à {cbAnneeFin.SelectedItem}";
            txtTypeStats.Text = $"Type de graph: {cbTypeGraph.SelectedItem}";*/
        }

       

       

    }
    }

