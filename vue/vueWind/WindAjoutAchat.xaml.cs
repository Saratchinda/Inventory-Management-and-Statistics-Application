using Log_in.mvvm.mvvmDal;
using Log_in.vue;
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

namespace Dancewave.vue
{
    /// <summary>
    /// Logique d'interaction pour WindAjoutAchat.xaml
    /// </summary>
    public partial class WindAjoutAchat : Window
    {
        private WindAchats _windAchatsPage;
        public WindAjoutAchat(WindAchats windAchatsPage)
        {
            InitializeComponent();
            List<int> lstCbIdArticle = DAL.Articles.Select(article => article.Id).Distinct().ToList();
            cbIdArticle.ItemsSource = lstCbIdArticle;
            _windAchatsPage = windAchatsPage;
            // Convertir en List<int>
        }

        private void btnAjouter_Click(object sender, RoutedEventArgs e)
        {

            string mess = "";

            // Vérifier si la date est sélectionnée
            if (!dtPicker.SelectedDate.HasValue)
            {
                mess += "Veuillez sélectionner une date." + Environment.NewLine;
            }

            // Vérifier si le champ Quantity est vide
            if (string.IsNullOrWhiteSpace(txtQuantity.Text))
            {
                mess += "Veuillez remplir le champ Quantity." + Environment.NewLine;
            }
            else
            {
                // Vérification si le champ Quantity contient une valeur numérique
                if (!int.TryParse(txtQuantity.Text, out _))
                {
                    mess += "Le champ Quantity doit contenir une valeur numérique." + Environment.NewLine;
                }
            }

            // Vérifier si le champ Unit Price est vide
            if (string.IsNullOrWhiteSpace(txtPrixUnit.Text))
            {
                mess += "Veuillez remplir le champ Unit Price." + Environment.NewLine;
            }
            else
            {
                // Vérification si le champ Unit Price contient une valeur numérique
                if (!float.TryParse(txtPrixUnit.Text, out _))
                {
                    mess += "Le champ Unit Price doit contenir une valeur numérique." + Environment.NewLine;
                }
            }

            // Vérifier si le champ Id Article est vide
            if (string.IsNullOrWhiteSpace(cbIdArticle.Text))
            {
                mess += "Veuillez remplir le champ Id Article." + Environment.NewLine;
            }
            else
            {
                // Vérification si le champ Id Article contient une valeur numérique
                if (!int.TryParse(cbIdArticle.Text, out _))
                {
                    mess += "Le champ Id Article doit contenir une valeur numérique." + Environment.NewLine;
                }
            }

            // Afficher tous les messages d'erreur en une seule boîte de message
            if (!string.IsNullOrWhiteSpace(mess))
            {
                MessageBox.Show(mess, "Erreurs", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            DAL.AjouterAchat(DateTime.Parse(dtPicker.Text),int.Parse(txtQuantity.Text),int.Parse(cbIdArticle.Text));
            DAL.UpdateArticlesFromDatabaseAchat();

            // Rafraîchir la liste dans la page WindAchats
            _windAchatsPage.RefreshPurchaseList();

            Close();
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

        private void txtPrixUnit_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtPrixUnit_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, @"^[0-9]+$"))
            {
                e.Handled = true; // Ignore la saisie de lettres

                // Afficher le message d'erreur
                MessageBox.Show("Le champ Prix unit doit contenir uniquement des chiffres.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtIdArticle_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, @"^[0-9]+$"))
            {
                e.Handled = true; // Ignore la saisie de lettres

                // Afficher le message d'erreur
                MessageBox.Show("Le champ Id Article doit contenir uniquement des chiffres.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Vérifier si le bouton "Sign in" a le focus
                if (btnAjouter.IsFocused)
                {
                    // Appeler la fonction de clic sur le bouton "Sign in"
                    btnAjouter_Click(sender, e);
                }
            }
        }
    }
}
