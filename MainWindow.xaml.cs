
using Log_in.mvvm.mvvmDal;
using Log_in.vue;
using System.Windows;
using System.Windows.Input;


namespace Log_in
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Vérifier si le bouton "Sign in" a le focus
                if (sign_in.IsFocused)
                {
                    // Appeler la fonction de clic sur le bouton "Sign in"
                    sign_in_Click(sender, e);
                }
            }
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void sign_in_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail2.Text;
            string mdp = txtMdp2.Password.ToString();
            Welcome welcome = new Welcome();

            if (DAL.VerifierIdentifiants(email, mdp))
            {
                MessageBox.Show("Connexion réussie !");
                welcome.ShowDialog();
                // Ajoutez ici le code pour rediriger vers la page suivante ou effectuer d'autres actions après une connexion réussie.
            }
            else
            {
                MessageBox.Show("Identifiants incorrects. Veuillez réessayer.");
            }
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            Close(); // Ferme la fenêtre
        }
    }
}

