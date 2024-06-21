using Dancewave.modele;

using LiveCharts.Defaults;
using System.Text;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using Log_in.vue;
using Log_in.modele;
using MySql.Data.Types;

namespace Log_in.mvvm.mvvmDal
{
    public class DAL
    {
        public static ObservableCollection<ObservablePoint> StatisticData { get; set; } = new ObservableCollection<ObservablePoint>();

        public static List<Article>? Articles { get; set; }
        public static List<Achat>? Achats { get; set; }
        public static List<Sale>? Sales { get; set; }

        // Autres membres du ViewModel...

        static DAL()
        {
            UpdateArticlesFromDatabaseArticle();
            UpdateArticlesFromDatabaseAchat();
            UpdateArticlesFromDatabaseSale();

            UpdateStatisticsData();
        }
        public static void AjouterDonnees(int id, string nom, string email, string mdp)
        {
            string connectionString = "Server=localhost;Database=dancewavetech;Uid=root";
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(mdp);

                string insertQuery = "INSERT INTO utilisateur (id, nom, email, mdp) VALUES (@id, @nom, @email, @mdp)";
                MySqlCommand cmd = new MySqlCommand(insertQuery, connection);

                // Ajout des paramètres

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@nom", nom);

                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@mdp", hashedPassword);


                // Exécution de la commande
                cmd.ExecuteNonQuery();

                MessageBox.Show("Enregistrement ajouté avec succès à la table 'utilisateur'");

                connection.Close(); // Fermez la connexion après vérification.
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erreur de connexion à la base de données : " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close(); // Assurez-vous de fermer la connexion en cas de succès ou d'échec.
                }
            }
        }
        public static void UpdateStatisticsData()
        {
            // Mise à jour des données pour le graphique
            StatisticData.Clear();

            // Exemple : agréger les TotalPrice par date pour les achats
            var groupedAchatData = Achats.GroupBy(a => a.DatePurchase.Date)
                                         .Select(g => new ObservablePoint(g.Key.Ticks, g.Sum(a => a.TotalPrice)));

            // Exemple : agréger les TotalPrice par date pour les ventes
            var groupedSaleData = Sales.GroupBy(s => s.DateSale.Date)
                                       .Select(g => new ObservablePoint(g.Key.Ticks, g.Sum(s => s.TotalPrice)));

            // Combiner les résultats des achats et des ventes
            var combinedData = groupedAchatData.Concat(groupedSaleData);

            foreach (var dataPoint in combinedData)
            {
                StatisticData.Add(dataPoint);
            }
        }

        public static void AjouterArticle(string nom, string descrip, float prixAchat, float prixVente, string gamme, string famille, string fournisseur, string emplacement)
        {
            string connectionString = "Server=localhost;Database=dancewavetech;Uid=root";
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                string insertQuery = "INSERT INTO article ( nom, description, prix_achat, prix_vente, gamme, famille, fournisseur, emplacement) VALUES ( @nom, @description, @prix_achat, @prix_vente, @gamme, @famille, @fournisseur, @emplacement)";
                MySqlCommand cmd = new MySqlCommand(insertQuery, connection);

                // Ajout des paramètres

                cmd.Parameters.AddWithValue("@nom", nom);
                cmd.Parameters.AddWithValue("@description", descrip);
                cmd.Parameters.AddWithValue("@prix_achat", prixAchat);
                cmd.Parameters.AddWithValue("@prix_vente", prixVente);
                cmd.Parameters.AddWithValue("@gamme", gamme);
                cmd.Parameters.AddWithValue("@famille", famille);
                cmd.Parameters.AddWithValue("@fournisseur", fournisseur);
                cmd.Parameters.AddWithValue("@emplacement", emplacement);

                // Exécution de la commande
                cmd.ExecuteNonQuery();

                MessageBox.Show("Enregistrement ajouté avec succès à la table article");

                connection.Close(); // Fermez la connexion après vérification.
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erreur de connexion à la base de données : " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close(); // Assurez-vous de fermer la connexion en cas de succès ou d'échec.
                }
            }
        }

        public static bool IsQuantityAvailable(int articleId, int requestedQuantity)
        {
            // Récupérez la quantité totale achetée pour l'article spécifié
            int totalQuantityBought = GetQuantityBoughtForArticle(articleId);

            // Vérifiez si la quantité disponible est suffisante
            return totalQuantityBought >= requestedQuantity;
        }

        public static int GetQuantityBoughtForArticle(int articleId)
        {
            // Récupérez la quantité totale achetée pour l'article spécifié depuis la table "achat"
            string connectionString = "Server=localhost;Database=dancewavetech;Uid=root";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT SUM(quantite_achetee) FROM achat WHERE id_article = @articleId";
                MySqlCommand cmd = new MySqlCommand(selectQuery, connection);
                cmd.Parameters.AddWithValue("@articleId", articleId);

                object result = cmd.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToInt32(result) : 0;
            }
        }
        public static void AjouterAchat(DateTime date, int quantiteAchete, int idArticle)
        {
            string connectionString = "Server=localhost;Database=dancewavetech;Uid=root";
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                // Première jointure pour obtenir le prix_unitaire de la table article
                string insertQuery = "INSERT INTO achat ( date_achat, quantite_achetee, prix_unitaire, prix_total, id_article) " +
                                     "VALUES ( @date_achat, @quantite_achetee, (SELECT prix_achat FROM article WHERE id = @id_article), " +
                                     "@quantite_achetee * (SELECT prix_achat FROM article WHERE id = @id_article), @id_article)";

                MySqlCommand cmd = new MySqlCommand(insertQuery, connection);

                // Ajout des paramètres

                cmd.Parameters.AddWithValue("@date_achat", date);
                cmd.Parameters.AddWithValue("@quantite_achetee", quantiteAchete);
                cmd.Parameters.AddWithValue("@id_article", idArticle);

                // Exécution de la commande
                cmd.ExecuteNonQuery();

                MessageBox.Show("Enregistrement ajouté avec succès à la table 'achats'");

                connection.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erreur de connexion à la base de données : " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        public static void AjouterSale(DateTime date, int quantiteVendue, int idArticle)
        {
            string connectionString = "Server=localhost;Database=dancewavetech;Uid=root";
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                // Récupérer la quantité achetée correspondante
                int quantiteAchetee = GetQuantiteAchetee(idArticle);

                if (quantiteVendue > quantiteAchetee)
                {
                    MessageBox.Show("La quantité d'articles vendus ne peut dépasser la quantité d'articles achetés.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Première jointure pour obtenir le prix_unitaire de la table article
                string insertQuery = "INSERT INTO vente (date_vente, quantite_vendue, prix_unitaire, prix_total, id_article) " +
                                     "VALUES (@date_vente, @quantite_vendue, (SELECT prix_vente FROM article WHERE id = @id_article), " +
                                     "@quantite_vendue * (SELECT prix_vente FROM article WHERE id = @id_article), @id_article)";

                MySqlCommand cmd = new MySqlCommand(insertQuery, connection);

                // Ajout des paramètres
                cmd.Parameters.AddWithValue("@date_vente", date);
                cmd.Parameters.AddWithValue("@quantite_vendue", quantiteVendue);
                cmd.Parameters.AddWithValue("@id_article", idArticle);

                // Exécution de la commande
                cmd.ExecuteNonQuery();

                MessageBox.Show("Enregistrement ajouté avec succès à la table 'vente'");

                connection.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erreur de connexion à la base de données : " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private static int GetQuantiteAchetee(int idArticle)
        {
            string connectionString = "Server=localhost;Database=dancewavetech;Uid=root";
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                // Récupérer la quantité achetée correspondante
                string selectQuery = "SELECT quantite_achetee FROM achat WHERE id_article = @id_article ORDER BY date_achat DESC LIMIT 1";
                MySqlCommand selectCmd = new MySqlCommand(selectQuery, connection);
                selectCmd.Parameters.AddWithValue("@id_article", idArticle);

                object result = selectCmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erreur de connexion à la base de données : " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return 0; // Si une erreur se produit, ou si aucune quantité achetée n'est trouvée, retourner 0
        }


        public static void UpdateArticlesFromDatabaseArticle()
        {
            // Assurez-vous que la liste Articles est initialisée
            if (Articles == null)
            {
                Articles = new List<Article>();
            }

            // Effacez les données actuelles de la liste Articles
            Articles.Clear();

            // Récupérez les données de la table article et ajoutez-les à la liste Articles
            string connectionString = "Server=localhost;Database=dancewavetech;Uid=root";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT * FROM article";
                MySqlCommand cmd = new MySqlCommand(selectQuery, connection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        int Id = Convert.ToInt32(reader["id"]);
                        string Name = reader["nom"].ToString();
                        string Description = reader["description"].ToString();
                        float PrixAchat = Convert.ToSingle(reader["prix_achat"]);
                        float PrixVente = Convert.ToSingle(reader["prix_vente"]);
                        string Gamme = reader["gamme"].ToString();
                        string Famille = reader["famille"].ToString();
                        string Fournisseur = reader["fournisseur"].ToString();
                        string Enplacement = reader["emplacement"].ToString();
                        Article article = new Article(Id, Name, Description, PrixAchat, PrixVente, Gamme, Famille, Fournisseur, Enplacement);
                        Articles.Add(article);

                    }
                }
            }


        }
        public static void UpdateArticlesFromDatabaseAchat()
        {
            // Assurez-vous que la liste Articles est initialisée
            if (Achats == null)
            {
                Achats = new List<Achat>();
            }

            // Effacez les données actuelles de la liste Articles
            Achats.Clear();

            // Récupérez les données de la table article et ajoutez-les à la liste Articles
            string connectionString = "Server=localhost;Database=dancewavetech;Uid=root";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT * FROM achat";
                MySqlCommand cmd = new MySqlCommand(selectQuery, connection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        int Id = Convert.ToInt32(reader["id"]);
                        DateTime Datevenue = Convert.ToDateTime(reader["date_achat"]);
                        int quantite = Convert.ToInt32(reader["quantite_achetee"]);
                        float PrixUnit = Convert.ToSingle(reader["prix_unitaire"]);
                        float PrixTotal = Convert.ToSingle(reader["prix_total"]);
                        int IdArticle = Convert.ToInt32(reader["id_Article"]);
                        Achat achat = new Achat(Id, Datevenue, quantite, PrixUnit, PrixTotal, IdArticle);
                        Achats.Add(achat);

                    }
                }
            }


        }
        public static void UpdateArticlesFromDatabaseSale()
        {
            // Assurez-vous que la liste Articles est initialisée
            if (Sales == null)
            {
                Sales = new List<Sale>();
            }

            // Effacez les données actuelles de la liste Articles
            Sales.Clear();

            // Récupérez les données de la table article et ajoutez-les à la liste Articles
            string connectionString = "Server=localhost;Database=dancewavetech;Uid=root";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT * FROM vente";
                MySqlCommand cmd = new MySqlCommand(selectQuery, connection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        int Id = Convert.ToInt32(reader["id"]);
                        DateTime Datevenue = Convert.ToDateTime(reader["date_vente"]);
                        int quantite = Convert.ToInt32(reader["quantite_vendue"]);
                        float PrixUnit = Convert.ToSingle(reader["prix_unitaire"]);
                        float PrixTotal = Convert.ToSingle(reader["prix_total"]);
                        int IdArticle = Convert.ToInt32(reader["id_Article"]);
                        Sale sale = new Sale(Id, Datevenue, quantite, PrixUnit, PrixTotal, IdArticle);
                        Sales.Add(sale);

                    }
                }
            }


        }









        public static void ModifierDonnees(Article articleAmodifier)
        {
            
            string connectionString = "Server=localhost;Database=dancewavetech;Uid=root";
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                string insertQuery = "UPDATE article SET nom = @nom, description = @description, prix_achat = @prix_achat, prix_vente = @prix_vente, gamme = @gamme, famille = @famille, fournisseur = @fournisseur, emplacement = @emplacement WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(insertQuery, connection);

                // Ajout des paramètres
                cmd.Parameters.AddWithValue("@id", articleAmodifier.Id);
                cmd.Parameters.AddWithValue("@nom", articleAmodifier.Name);
                cmd.Parameters.AddWithValue("@description", articleAmodifier.Description);
                cmd.Parameters.AddWithValue("@prix_achat", articleAmodifier.PrixAchat);
                cmd.Parameters.AddWithValue("@prix_vente", articleAmodifier.PrixAchat);
                cmd.Parameters.AddWithValue("@gamme", articleAmodifier.Gamme);
                cmd.Parameters.AddWithValue("@famille", articleAmodifier.Famille);
                cmd.Parameters.AddWithValue("@fournisseur", articleAmodifier.Fournisseur);
                cmd.Parameters.AddWithValue("@emplacement", articleAmodifier.Emplacement);

                // Exécution de la commande
                cmd.ExecuteNonQuery();

                MessageBox.Show("Enregistrement ajouté avec succès à la table article");

                connection.Close(); // Fermez la connexion après vérification.
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erreur de connexion à la base de données : " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close(); // Assurez-vous de fermer la connexion en cas de succès ou d'échec.
                }
            }
        }
        public static void DeleteArticle(Article article)
        {


            string connectionString = "Server=localhost;Database=dancewavetech;Uid=root";
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();
                string deleteQuery = "DELETE FROM article WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(deleteQuery, connection);

                // Ajout du paramètre
                cmd.Parameters.AddWithValue("@id", article.Id);

                // Exécution de la commande
                cmd.ExecuteNonQuery();

                MessageBox.Show("Article supprimé avec succès de je fais un essai la table article");

                // Mettez à jour la liste Articles après la suppression
                UpdateArticlesFromDatabaseArticle();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erreur lors de la suppression de l'article : " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

            }
        }
        public static void DeleteAchat(Achat achat)
        {


            string connectionString = "Server=localhost;Database=dancewavetech;Uid=root";
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();
                string deleteQuery = "DELETE FROM achat WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(deleteQuery, connection);

                // Ajout du paramètre
                cmd.Parameters.AddWithValue("@id", achat.Id);

                // Exécution de la commande
                cmd.ExecuteNonQuery();

                MessageBox.Show("Achat supprimé avec succès de la table achat");

                // Mettez à jour la liste Articles après la suppression
                UpdateArticlesFromDatabaseArticle();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erreur lors de la suppression de l'achat : " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

            }
        }
        public static void DeleteSale(Sale sale)
        {


            string connectionString = "Server=localhost;Database=dancewavetech;Uid=root";
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();
                string deleteQuery = "DELETE FROM vente WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(deleteQuery, connection);

                // Ajout du paramètre
                cmd.Parameters.AddWithValue("@id", sale.Id);

                // Exécution de la commande
                cmd.ExecuteNonQuery();

                MessageBox.Show("Achat supprimé avec succès de la table achat");

                // Mettez à jour la liste Articles après la suppression
                UpdateArticlesFromDatabaseArticle();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erreur lors de la suppression de l'achat : " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

            }
        }




        public static bool VerifierIdentifiants(string email, string mdp)
        {
            string connectionString = "Server=localhost;Database=dancewavetech;Uid=root";
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                // Recherche de l'utilisateur avec l'email fourni
                string selectQuery = "SELECT id, email, mdp FROM utilisateur WHERE email = @email";
                MySqlCommand selectCmd = new MySqlCommand(selectQuery, connection);
                selectCmd.Parameters.AddWithValue("@email", email);

                using (MySqlDataReader reader = selectCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Utilisateur trouvé, vérifiez le mot de passe
                        string hashedPasswordFromDB = reader["mdp"].ToString();
                        if (BCrypt.Net.BCrypt.Verify(mdp, hashedPasswordFromDB))
                        {
                            // Mot de passe correct, vous pouvez stocker l'ID de l'utilisateur ou effectuer d'autres actions ici
                            return true;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erreur de connexion à la base de données : " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return false; // Utilisateur non trouvé ou mot de passe incorrect
        }
        public static void ModifierAchat(Achat AchatAModifier)
        {
            DeleteAchat(AchatAModifier);
            string connectionString = "Server=localhost;Database=dancewavetech;Uid=root";
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                string insertQuery = "INSERT INTO achat (id, date_achat, quantite_achetee, prix_unitaire, prix_total, id_article) " +
                                      "VALUES (@id, @date_achat, @quantite_achetee, (SELECT prix_achat FROM article WHERE id = @id_article), " +
                                      "@quantite_achetee * (SELECT prix_achat FROM article WHERE id = @id_article), @id_article)";

                MySqlCommand cmd = new MySqlCommand(insertQuery, connection);

                // Ajout des paramètres
                cmd.Parameters.AddWithValue("@id", AchatAModifier.Id);
                cmd.Parameters.AddWithValue("@date_achat", AchatAModifier.DatePurchase);
                cmd.Parameters.AddWithValue("@quantite_achetee", AchatAModifier.Quantity);
                cmd.Parameters.AddWithValue("@id_article", AchatAModifier.IdArticle);

                // Exécution de la commande
                cmd.ExecuteNonQuery();

                MessageBox.Show("Modification enregistrée  avec succès à la table 'article'");

                connection.Close(); // Fermez la connexion après vérification.
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erreur de connexion à la base de données : " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close(); // Assurez-vous de fermer la connexion en cas de succès ou d'échec.
                }
            }
        }
        public static void ModifierSale(Sale SaleAModifier)
        {
            DeleteSale(SaleAModifier);
            string connectionString = "Server=localhost;Database=dancewavetech;Uid=root";
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                string insertQuery = "INSERT INTO vente (id, date_vente, quantite_vendue, prix_unitaire, prix_total, id_article) " +
                                      "VALUES (@id, @date_vente, @quantite_vendue, (SELECT prix_vente FROM article WHERE id = @id_article), " +
                                      "@quantite_vendue * (SELECT prix_vente FROM article WHERE id = @id_article), @id_article)";

                MySqlCommand cmd = new MySqlCommand(insertQuery, connection);

                // Ajout des paramètres
                cmd.Parameters.AddWithValue("@id", SaleAModifier.Id);
                cmd.Parameters.AddWithValue("@date_vente", SaleAModifier.DateSale);
                cmd.Parameters.AddWithValue("@quantite_vendue", SaleAModifier.QuantitySale);
                cmd.Parameters.AddWithValue("@id_article", SaleAModifier.IdArticle);

                // Exécution de la commande
                cmd.ExecuteNonQuery();

                MessageBox.Show("Modification enregistrée  avec succès à la table 'article'");

                connection.Close(); // Fermez la connexion après vérification.
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erreur de connexion à la base de données : " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close(); // Assurez-vous de fermer la connexion en cas de succès ou d'échec.
                }
            }
        }

        public static bool CreerUtilisateur(string nom, string email, string mdp)
        {
            string connectionString = "Server=localhost;Database=dancewavetech;Uid=root";
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                // Vérifiez d'abord si l'utilisateur existe déjà avec l'e-mail fourni
                string selectQuery = "SELECT COUNT(*) FROM utilisateur WHERE email = @email";
                MySqlCommand selectCmd = new MySqlCommand(selectQuery, connection);
                selectCmd.Parameters.AddWithValue("@email", email);

                int existingUserCount = Convert.ToInt32(selectCmd.ExecuteScalar());
                if (existingUserCount > 0)
                {
                    MessageBox.Show("Un utilisateur avec cet e-mail existe déjà.");
                    return false;
                }

                // L'utilisateur n'existe pas, vous pouvez le créer
                string insertQuery = "INSERT INTO utilisateur (nom, email, mdp) VALUES (@nom, @email, @mdp)";
                MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection);
                insertCmd.Parameters.AddWithValue("@nom", nom);
                insertCmd.Parameters.AddWithValue("@email", email);
                insertCmd.Parameters.AddWithValue("@mdp", BCrypt.Net.BCrypt.HashPassword(mdp));

                int rowsAffected = insertCmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return true; // Utilisateur créé avec succès
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erreur de connexion à la base de données : " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return false; // Échec de la création de l'utilisateur
        }
        

        }
}





