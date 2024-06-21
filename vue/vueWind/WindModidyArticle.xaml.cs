using Dancewave.modele;
using Log_in.mvvm.mvvmDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Logique d'interaction pour WindModidyArticle.xaml
    /// </summary>
    public partial class WindModidyArticle : Window
    {
        public Article ArticleAmodifier;
        private AddArticles _addArticlesPage;

        // Événement pour signaler la modification
        public event EventHandler ArticleModified;
        public event EventHandler ArticleClosed;
        public WindModidyArticle(AddArticles addArticlesPage)
        {
            InitializeComponent();
            _addArticlesPage = addArticlesPage;
        }

        private void btnModifer_Click(object sender, RoutedEventArgs e)
        {
            string mess = "";

            // Vérification du champ Nom
            if (string.IsNullOrEmpty(txtNom.Text))
            {
                mess += "Veuillez entrer le nom de l'article." + Environment.NewLine;
            }

            // Vérification du champ Description
            if (string.IsNullOrEmpty(txtDescription.Text))
            {
                mess += "Veuillez entrer la description de l'article." + Environment.NewLine;
            }

            // Vérification du champ Prix
            if (string.IsNullOrEmpty(txtPrix.Text))
            {
                mess += "Veuillez entrer le prix de l'article." + Environment.NewLine;
            }

            // Vérification du champ Gamme
            if (string.IsNullOrEmpty(txtGamme.Text))
            {
                mess += "Veuillez entrer la gamme de l'article." + Environment.NewLine;
            }

            // Vérification du champ Famille
            if (string.IsNullOrEmpty(txtFamille.Text))
            {
                mess += "Veuillez entrer la famille de l'article." + Environment.NewLine;
            }

            // Vérification du champ Fournisseur
            if (string.IsNullOrEmpty(txtFournisseur.Text))
            {
                mess += "Veuillez entrer le fournisseur de l'article." + Environment.NewLine;
            }

            // Vérification du champ Emplacement
            if (string.IsNullOrEmpty(txtEmplacement.Text))
            {
                mess += "Veuillez entrer l'emplacement de l'article." + Environment.NewLine;
            }

            // Afficher tous les messages d'erreur en une seule boîte de message
            if (!string.IsNullOrEmpty(mess))
            {
                MessageBox.Show(mess, "Erreurs", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            this.ArticleAmodifier.Id = int.Parse(txtId.Text);
            this.ArticleAmodifier.Name = txtNom.Text;
            this.ArticleAmodifier.Description = txtDescription.Text;
            this.ArticleAmodifier.PrixAchat = float.Parse(txtPrix.Text);
            this.ArticleAmodifier.Gamme = txtGamme.Text;
            this.ArticleAmodifier.Famille = txtFamille.Text;
            this.ArticleAmodifier.Fournisseur = txtFournisseur.Text;
            this.ArticleAmodifier.Emplacement = txtEmplacement.Text;
            DAL.ModifierDonnees(ArticleAmodifier);

           
            Close();
        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            txtId.Text = this.ArticleAmodifier.Id.ToString();
            txtNom.Text = this.ArticleAmodifier.Name;
            txtDescription.Text = this.ArticleAmodifier.Description;
            txtPrix.Text = this.ArticleAmodifier.PrixAchat.ToString();
            txtGamme.Text = this.ArticleAmodifier.Gamme;
            txtFamille.Text = this.ArticleAmodifier.Famille;
            txtFournisseur.Text = this.ArticleAmodifier.Fournisseur;
            txtEmplacement.Text = this.ArticleAmodifier.Emplacement;
        }

        private void txtPrix_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, @"^[0-9]+$"))
            {
                e.Handled = true; // Ignore la saisie de lettres

                // Afficher le message d'erreur
                MessageBox.Show("Le champ Prix doit contenir uniquement des chiffres.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtNom_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Regex.IsMatch(e.Text, @"[0-9]"))
            {
                e.Handled = true; // Ignore la saisie de chiffres

                // Afficher le message d'erreur
                MessageBox.Show("Le champ Nom doit contenir uniquement des lettres.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtDescription_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Regex.IsMatch(e.Text, @"[0-9]"))
            {
                e.Handled = true; // Ignore la saisie de chiffres

                // Afficher le message d'erreur
                MessageBox.Show("Le champ Description doit contenir uniquement des lettres.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtGamme_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Regex.IsMatch(e.Text, @"[0-9]"))
            {
                e.Handled = true; // Ignore la saisie de chiffres

                // Afficher le message d'erreur
                MessageBox.Show("Le champ Gamme doit contenir uniquement des lettres.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtFamille_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Regex.IsMatch(e.Text, @"[0-9]"))
            {
                e.Handled = true; // Ignore la saisie de chiffres

                // Afficher le message d'erreur
                MessageBox.Show("Le champ Famille doit contenir uniquement des lettres.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtFournisseur_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Regex.IsMatch(e.Text, @"[0-9]"))
            {
                e.Handled = true; // Ignore la saisie de chiffres

                // Afficher le message d'erreur
                MessageBox.Show("Le champ Fournisseur doit contenir uniquement des lettres.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtEmplacement_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Regex.IsMatch(e.Text, @"[0-9]"))
            {
                e.Handled = true; // Ignore la saisie de chiffres

                // Afficher le message d'erreur
                MessageBox.Show("Le champ Emplacement doit contenir uniquement des lettres.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtId_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, @"^[0-9]+$"))
            {
                e.Handled = true; // Ignore la saisie de lettres

                // Afficher le message d'erreur
                MessageBox.Show("Le champ ID doit contenir uniquement des chiffres.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Vérifier si le bouton "modifier article" a le focus
                if (btnModifer.IsFocused)
                {
                    // Appeler la fonction de clic sur le bouton "modifier article"
                    btnModifer_Click(sender, e);
                }
            }
        }

    }
}
