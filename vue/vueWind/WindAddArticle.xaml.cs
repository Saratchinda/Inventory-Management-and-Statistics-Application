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
    /// Logique d'interaction pour WindAddArticle.xaml
    /// </summary>
    public partial class WindAddArticle : Window
    {
        
        public WindAddArticle()
        {
            InitializeComponent();
           
        }

        private void btnAjouterAchat_Click(object sender, RoutedEventArgs e)
        {

            string mess = "";

            // Vérification du champ Nom
            if (string.IsNullOrEmpty(txtNom.Text))
            {
                mess += "Veuillez entrer le nom de l'article." + Environment.NewLine;
            }
            else if (!txtNom.Text.All(char.IsLetter))
            {
                mess += "Le champ Nom doit contenir uniquement des lettres." + Environment.NewLine;
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
            else
            {
                // Vérification si le champ Prix contient une valeur numérique
                if (!float.TryParse(txtPrix.Text, out _))
                {
                    mess += "Le champ Prix doit contenir une valeur numérique." + Environment.NewLine;
                }
            }

            // Vérification du champ Prix Vente
            if (string.IsNullOrEmpty(txtPrixVente.Text))
            {
                mess += "Veuillez entrer le prix de vente de l'article." + Environment.NewLine;
            }
            else
            {
                // Vérification si le champ Prix Vente contient une valeur numérique
                if (!float.TryParse(txtPrixVente.Text, out _))
                {
                    mess += "Le champ Prix Vente doit contenir une valeur numérique." + Environment.NewLine;
                }
            }

            // Vérification du champ Gamme (doit contenir du texte)
            if (string.IsNullOrEmpty(txtGamme.Text))
            {
                mess += "Veuillez entrer la gamme de l'article." + Environment.NewLine;
            }
            else if (!txtGamme.Text.All(char.IsLetter))
            {
                mess += "Le champ Gamme doit contenir uniquement des lettres." + Environment.NewLine;
            }

            // Vérification du champ Famille (doit contenir du texte)
            if (string.IsNullOrEmpty(txtFamille.Text))
            {
                mess += "Veuillez entrer la famille de l'article." + Environment.NewLine;
            }
            else if (!txtFamille.Text.All(char.IsLetter))
            {
                mess += "Le champ Famille doit contenir uniquement des lettres." + Environment.NewLine;
            }

            // Vérification du champ Fournisseur (doit contenir du texte)
            if (string.IsNullOrEmpty(txtFournisseur.Text))
            {
                mess += "Veuillez entrer le fournisseur de l'article." + Environment.NewLine;
            }
            else if (!txtFournisseur.Text.All(char.IsLetter))
            {
                mess += "Le champ Fournisseur doit contenir uniquement des lettres." + Environment.NewLine;
            }

            // Vérification du champ Emplacement (doit contenir du texte)
            if (string.IsNullOrEmpty(txtEmplacement.Text))
            {
                mess += "Veuillez entrer l'emplacement de l'article." + Environment.NewLine;
            }
            else if (!txtEmplacement.Text.All(char.IsLetter))
            {
                mess += "Le champ Emplacement doit contenir uniquement des lettres." + Environment.NewLine;
            }

            // Afficher tous les messages d'erreur en une seule boîte de message
            if (!string.IsNullOrEmpty(mess))
            {
                MessageBox.Show(mess, "Erreurs", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            

           

            // Si tous les champs sont remplis, continuer le traitement


            DAL.AjouterArticle( txtNom.Text.ToString(), txtDescription.Text.ToString(), float.Parse(txtPrix.Text), float.Parse(txtPrixVente.Text), txtGamme.Text.ToString(), txtFamille.Text.ToString(), txtFournisseur.Text.ToString(), txtEmplacement.Text.ToString());
            DAL.UpdateArticlesFromDatabaseArticle();

            
        }

        private void txtPrix_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtPrixVente_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtId_TextChanged(object sender, TextChangedEventArgs e)
        {

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

        private void txtNom_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtDescription_TextChanged(object sender, TextChangedEventArgs e)
        {

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

        private void txtFamille_TextChanged(object sender, TextChangedEventArgs e)
        {

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

        private void txtEmplacement_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtPrixVente_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, @"^[0-9]+$"))
            {
                e.Handled = true; // Ignore la saisie de lettres

                // Afficher le message d'erreur
                MessageBox.Show("Le champ Prix vente doit contenir uniquement des chiffres.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtPrix_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, @"^[0-9]+$"))
            {
                e.Handled = true; // Ignore la saisie de lettres

                // Afficher le message d'erreur
                MessageBox.Show("Le champ Prix achat doit contenir uniquement des chiffres.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Vérifier si le bouton "ajouter achat" a le focus
                if (btnAjouterAchat.IsFocused)
                {
                    // Appeler la fonction de clic sur le bouton "ajouter achat"
                    btnAjouterAchat_Click(sender, e);
                }
            }
        }

    }
}
