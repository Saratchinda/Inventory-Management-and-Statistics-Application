using Dancewave.modele;
using Log_in.modele;
using Log_in.mvvm.mvvmDal;
using Log_in.vue;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Log_in.vue
{
    /// <summary>
    /// Logique d'interaction pour WindSales.xaml
    /// </summary>
    public partial class WindSales : Page
    {
        public WindSales()
        {
            InitializeComponent();
            lstSale.ItemsSource = DAL.Sales;
            lstSale.Items.Refresh();
        }

        private void btnAddSale_Click(object sender, RoutedEventArgs e)
        {
            WindAddSale windAddSale = new WindAddSale(this);
            windAddSale.ShowDialog();
        }

        // Ajoutez cette méthode pour rafraîchir la liste de ventes
        public void RefreshSaleList()
        {
            DAL.UpdateArticlesFromDatabaseSale();
            lstSale.Items.Refresh();
        }
        private void btnModifySale_Click(object sender, RoutedEventArgs e)
        {
            if (lstSale.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner une vente à modifier.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Arrêter le traitement car aucune vente n'est sélectionnée
            }


            WindModifySale windModifySale = new WindModifySale();
            windModifySale.SaleAModifier = (Sale)lstSale.SelectedItem;
            windModifySale.ShowDialog();
            DAL.UpdateArticlesFromDatabaseAchat();
            lstSale.Items.Refresh();
        }

        private void btnDeleteSale_Click(object sender, RoutedEventArgs e)
        {
            if (lstSale.SelectedItem == null)
            {

                MessageBox.Show("Veuillez sélectionner un achat à supprimer.");
                return;
            }
            DAL.DeleteSale((Sale)lstSale.SelectedItem);

            DAL.UpdateArticlesFromDatabaseSale();
            lstSale.Items.Refresh();
        }

        private void lstSale_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstSale.SelectedItem == null) return;
            lblVoir.Content = "";

            lblVoir.Content += ((Sale)lstSale.SelectedItem).Id + Environment.NewLine;
            lblVoir.Content += ((Sale)lstSale.SelectedItem).DateSale + Environment.NewLine;
            lblVoir.Content += ((Sale)lstSale.SelectedItem).QuantitySale + Environment.NewLine;
            lblVoir.Content += ((Sale)lstSale.SelectedItem).UnitPrice + Environment.NewLine;
            lblVoir.Content += ((Sale)lstSale.SelectedItem).TotalPrice + Environment.NewLine;
            lblVoir.Content += ((Sale)lstSale.SelectedItem).IdArticle + Environment.NewLine;
        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (lstSale != null && lstSale.ItemsSource != null)
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(lstSale.ItemsSource);

                // Appliquer le filtre en fonction du texte saisi dans le TextBox
                if (view != null)
                {
                    view.Filter = item =>
                    {
                        if (item is Sale currentItem)
                        {

                            // Remplacez "Name" par le nom de la propriété que vous voulez filtrer
                            return currentItem.Id.ToString().Contains(txtFilter.Text) ||
                    currentItem.IdArticle.ToString().Contains(txtFilter.Text) ||
                    currentItem.UnitPrice.ToString().ToLower().Contains(txtFilter.Text);
                        }
                        return false;
                    };
                }
            }
        }
    }
}
