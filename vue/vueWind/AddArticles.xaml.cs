using Dancewave.modele;
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
    /// Logique d'interaction pour AddArticles.xaml
    /// </summary>
    public partial class AddArticles : Page
    {
        public AddArticles()
        {
            InitializeComponent();
            lstArticles.ItemsSource = DAL.Articles;
        }

        private void btnAddItems_Click(object sender, RoutedEventArgs e)
        {
            WindAddArticle addArticle = new WindAddArticle();
            addArticle.ShowDialog();

        }

      
        private void lstArticles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstArticles.SelectedItem == null) return;
            lblVoir.Content = "";

            lblVoir.Content += ((Article)lstArticles.SelectedItem).Id + Environment.NewLine;
            lblVoir.Content += ((Article)lstArticles.SelectedItem).Name + Environment.NewLine;
            lblVoir.Content += ((Article)lstArticles.SelectedItem).Description + Environment.NewLine;
            lblVoir.Content += ((Article)lstArticles.SelectedItem).PrixAchat + Environment.NewLine;
            lblVoir.Content += ((Article)lstArticles.SelectedItem).Gamme + Environment.NewLine;
            lblVoir.Content += ((Article)lstArticles.SelectedItem).Famille + Environment.NewLine;
            lblVoir.Content += ((Article)lstArticles.SelectedItem).Fournisseur + Environment.NewLine;
            lblVoir.Content += ((Article)lstArticles.SelectedItem).Emplacement + Environment.NewLine;
        }

        private void OpenModifyWindow()
        {
            WindModidyArticle modifyWindow = new WindModidyArticle(this);

            
            modifyWindow.ArticleClosed += ModifyWindow_ArticleClosed;

            modifyWindow.Show();
        }

      

        private void ModifyWindow_ArticleClosed(object sender, EventArgs e)
        {
            // Lorsque l'événement ArticleClosed est déclenché, effacez le contenu du Label
            lblVoir.Content = "";
        }
        private void btnModify_Click(object sender, RoutedEventArgs e)
        {
            if (lstArticles.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner un article.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            

            WindModidyArticle windModidyArticle = new WindModidyArticle(this);
            windModidyArticle.ArticleAmodifier = (Article)lstArticles.SelectedItem;
            windModidyArticle.ShowDialog();
            DAL.UpdateArticlesFromDatabaseArticle();
            lstArticles.Items.Refresh();


        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lstArticles.SelectedItem == null)
            {

                MessageBox.Show("Veuillez sélectionner un article à supprimer.");
                return;
            }
            DAL.DeleteArticle((Article)lstArticles.SelectedItem);
            lstArticles.Items.Refresh();
        }

        private void ResetListView()
        {
            // Effacez la sélection et le contenu du Label
            lstArticles.SelectedItem = null;
            lblVoir.Content = "";

            // Effacez la ListView
            lstArticles.ItemsSource = null;
            lstArticles.Items.Clear();

            // Rechargez la liste des articles à partir de la base de données
            DAL.UpdateArticlesFromDatabaseArticle();
            lstArticles.ItemsSource = DAL.Articles;
        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (lstArticles != null && lstArticles.ItemsSource != null)
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(lstArticles.ItemsSource);

                // Appliquer le filtre en fonction du texte saisi dans le TextBox
                if (view != null)
                {
                    view.Filter = item =>
                    {
                        if (item is Article currentItem)
                        {

                            // Remplacez "Name" par le nom de la propriété que vous voulez filtrer
                            return currentItem.Name.Contains(txtFilter.Text) ||
                    currentItem.Id.ToString().Contains(txtFilter.Text) ||
                    currentItem.Famille.ToLower().Contains(txtFilter.Text);
                        }
                        return false;
                    };
                }
            }
        }

      
    }
}
