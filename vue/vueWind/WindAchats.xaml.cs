using Dancewave.modele;
using Dancewave.vue;
using Log_in.mvvm.mvvmDal;
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
    /// Logique d'interaction pour WindAchats.xaml
    /// </summary>
    public partial class WindAchats : Page
    {
        public WindAchats()
        {
            InitializeComponent();
            lstAchat.ItemsSource = DAL.Achats;
            lstAchat.Items.Refresh();
        }

        private void btnAddPurchase_Click(object sender, RoutedEventArgs e)
        {
            WindAjoutAchat windAjoutAchat = new WindAjoutAchat(this);
            windAjoutAchat.ShowDialog();
        }

        // Ajoutez cette méthode pour rafraîchir la liste d'achats
        public void RefreshPurchaseList()
        {
            DAL.UpdateArticlesFromDatabaseAchat();
            lstAchat.Items.Refresh();
        }

        private void lstAchat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstAchat.SelectedItem == null) return;
            lblVoir.Content = "";

            lblVoir.Content += ((Achat)lstAchat.SelectedItem).Id + Environment.NewLine;
            lblVoir.Content += ((Achat)lstAchat.SelectedItem).DatePurchase + Environment.NewLine;
            lblVoir.Content += ((Achat)lstAchat.SelectedItem).Quantity + Environment.NewLine;
            lblVoir.Content += ((Achat)lstAchat.SelectedItem).UnitPrice + Environment.NewLine;
            lblVoir.Content += ((Achat)lstAchat.SelectedItem).TotalPrice + Environment.NewLine;
            lblVoir.Content += ((Achat)lstAchat.SelectedItem).IdArticle + Environment.NewLine;
        }

        private void btnModifyPurchase_Click(object sender, RoutedEventArgs e)
        {
            if (lstAchat.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner un achat à modifier.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            WindModifyAchat windModif = new WindModifyAchat();
            windModif.AchatAModifier = (Achat)lstAchat.SelectedItem;
            windModif.ShowDialog();
            DAL.UpdateArticlesFromDatabaseAchat();
            lstAchat.Items.Refresh();
        }

        private void btnDeletePurchase_Click(object sender, RoutedEventArgs e)
        {
            if (lstAchat.SelectedItem == null)
            {

                MessageBox.Show("Veuillez sélectionner un article à supprimer.");
                return;
            }
            DAL.DeleteAchat((Achat)lstAchat.SelectedItem);

            DAL.UpdateArticlesFromDatabaseAchat();
            lstAchat.Items.Refresh();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (lstAchat != null && lstAchat.ItemsSource != null)
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(lstAchat.ItemsSource);

                // Appliquer le filtre en fonction du texte saisi dans le TextBox
                if (view != null)
                {
                    view.Filter = item =>
                    {
                        if (item is Achat currentItem)
                        {

                            // Remplacez "Name" par le nom de la propriété que vous voulez filtrer
                            return currentItem.IdArticle.ToString().Contains(txtFilter.Text) ||
                    currentItem.Id.ToString().Contains(txtFilter.Text) ||
                    currentItem.UnitPrice.ToString().ToLower().Contains(txtFilter.Text);
                        }
                        return false;
                    };
                }
            }
        }

     
    }
}
