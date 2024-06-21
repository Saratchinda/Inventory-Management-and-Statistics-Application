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
    /// Logique d'interaction pour WindModifyAchat.xaml
    /// </summary>
    public partial class WindModifyAchat : Window
    {
        public Achat AchatAModifier;
        public WindModifyAchat()
        {
            InitializeComponent();
            List<int> lstCbIdArticle = DAL.Articles.Select(article => article.Id).Distinct().ToList();
            cbIdArticle.ItemsSource = lstCbIdArticle;

            // Convertir en List<int>
            

        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            txtId.Text = this.AchatAModifier.Id.ToString();
            txtDate.Text = this.AchatAModifier.DatePurchase.ToString();
            txtQuantity.Text = this.AchatAModifier.Quantity.ToString();
            cbIdArticle.Text = this.AchatAModifier.IdArticle.ToString();
        }

        private void btnModifer_Click(object sender, RoutedEventArgs e)
        {
            // Vérification du champ Id
            if (string.IsNullOrEmpty(txtId.Text))
            {
                MessageBox.Show("Veuillez entrer l'ID de l'article.", "Erreur");
                return;
            }
            if(string.IsNullOrEmpty(txtId.Text))
            {
                MessageBox.Show("Veuillez entrer la date.", "Erreur");
            }
            if(string.IsNullOrEmpty (txtId.Text))
            {
                MessageBox.Show("Veuillez entrer la quantité", "Erreur");
            }
            if(string.IsNullOrEmpty (txtId.Text))
            {
                MessageBox.Show("Veuillez entrer le ID Article", "Erreur");
            }
            this.AchatAModifier.Id = int.Parse(txtId.Text);
            this.AchatAModifier.DatePurchase = DateTime.Parse(txtDate.Text);
            this.AchatAModifier.Quantity = int.Parse(txtQuantity.Text);
            this.AchatAModifier.IdArticle = int.Parse(cbIdArticle.Text);
            DAL.ModifierAchat(AchatAModifier);
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

        private void txtQuantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, @"^[0-9]+$"))
            {
                e.Handled = true; // Ignore la saisie de lettres

                // Afficher le message d'erreur
                MessageBox.Show("Le champ Quantity doit contenir uniquement des chiffres.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtIdArticle_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, @"^[0-9]+$"))
            {
                e.Handled = true; // Ignore la saisie de lettres

                // Afficher le message d'erreur
                MessageBox.Show("Le champ ID Article doit contenir uniquement des chiffres.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
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
