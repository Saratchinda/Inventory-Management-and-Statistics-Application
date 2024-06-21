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
    /// Logique d'interaction pour WindAddSale.xaml
    /// </summary>
    public partial class WindAddSale : Window
    {
        private WindSales _windSalesPage;
        public WindAddSale(WindSales windSalesPage)
        {
            InitializeComponent();
            List<int> lstCbIdArticle = DAL.Articles.Select(article => article.Id).Distinct().ToList();
            cbIdArticle.ItemsSource = lstCbIdArticle;
            _windSalesPage = windSalesPage;

            // Convertir en List<int>
        }

        private void btnAjouter_Click(object sender, RoutedEventArgs e)
        {
            List<string> errors = new List<string>();

            // Initialiser les variables avec des valeurs par défaut
            DateTime selectedDate = DateTime.MinValue;
            int requestedQuantity = 0;
            int articleId = 0;

            // Vérifier si la date est sélectionnée
            if (!dtPicker.SelectedDate.HasValue)
            {
                errors.Add("Veuillez sélectionner une date.");
            }

            // Vérifier si le champ Quantity est vide
            if (string.IsNullOrWhiteSpace(txtQuantity.Text))
            {
                errors.Add("Veuillez remplir le champ Quantity.");
            }
            else
            {
                // Vérification si le champ Quantity contient une valeur numérique
                if (!int.TryParse(txtQuantity.Text, out requestedQuantity))
                {
                    errors.Add("Le champ Quantity doit contenir une valeur numérique entière.");
                }
            }

            // Vérifier si le champ ID Article est vide
            if (string.IsNullOrWhiteSpace(cbIdArticle.Text))
            {
                errors.Add("Veuillez remplir le champ ID Article.");
            }
            else
            {
                // Vérification si le champ ID Article contient une valeur numérique
                if (!int.TryParse(cbIdArticle.Text, out articleId))
                {
                    errors.Add("Le champ ID Article doit contenir une valeur numérique entière.");
                }
            }

            // Vérifier la quantité d'articles vendus par rapport à la quantité d'articles achetés
            if (requestedQuantity > 0 && articleId > 0)
            {
                if (!DAL.IsQuantityAvailable(articleId, requestedQuantity))
                {
                    errors.Add("La quantité d’articles vendus ne peut dépasser la quantité d’articles achetés.");
                }
            }
            else
            {
                errors.Add("Veuillez entrer des valeurs numériques valides pour la quantité et l'ID Article.");
            }

            // Afficher tous les messages d'erreur en même temps
            if (errors.Count > 0)
            {
                string errorMessage = string.Join(Environment.NewLine, errors);
                MessageBox.Show(errorMessage, "Erreurs", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Sortir de la méthode si des erreurs sont présentes
            }

            // Si aucune erreur, procéder à l'ajout de la vente
            try
            {
                // Convertir la date sélectionnée en DateTime
                selectedDate = dtPicker.SelectedDate.Value;

                DAL.AjouterSale(selectedDate, requestedQuantity, articleId);
                DAL.UpdateArticlesFromDatabaseSale();
            }
            catch (FormatException)
            {
                MessageBox.Show("Format de date invalide.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Rafraîchir la liste dans la page WindSales
            _windSalesPage.RefreshSaleList();

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

        private void txtIdArticle_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, @"^[0-9]+$"))
            {
                e.Handled = true; // Ignore la saisie de lettres

                // Afficher le message d'erreur
                MessageBox.Show("Le champ prix doit contenir uniquement des chiffres.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
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
