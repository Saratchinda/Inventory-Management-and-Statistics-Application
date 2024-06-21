using Dancewave.modele;
using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Log_in.modele;
using Log_in.mvvm.mvvmDal;

namespace Log_in.mvvm.mvvmStat
{
    public class MainViewModel : ViewModelBase
    {
        //private List<Achat> _achats;

        public List<Achat> Achats
        {
            get => DAL.Achats;
            set
            {
                DAL.Achats = value;
                OnPropertyChanged(nameof(Achats));
            }
        }
        public List<Sale> Sales
        {
            get => DAL.Sales;
            set
            {
                DAL.Sales = value;
                OnPropertyChanged(nameof(Achats));
            }
        }
        public List<Article> Articles
        {
            get => DAL.Articles;
            set
            {
                DAL.Articles = value;
                OnPropertyChanged(nameof(Articles));
            }
        }




        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        public Func<double, string> AxisYFormatter { get; set; }

        public MainViewModel()
        {
            SeriesCollection = new SeriesCollection();
            Labels = new List<string>();
            AxisYFormatter = value => value.ToString("C"); // Formater l'axe Y en tant que devise
        }
        public void GenerateChartMultipleYearsAchat(int startYear, int endYear, string gamme = null, string famille = null, string fournisseur = null, string emplacement = null, string chartType = "Line")
        {
            if (Achats == null || Articles == null)
            {
                MessageBox.Show("Les listes d'achats ou d'articles ne sont pas initialisées.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var years = Enumerable.Range(startYear, endYear - startYear + 1).ToList();

            var moyennesParAnnee = years
                .GroupJoin(
                    Achats,
                    annee => annee,
                    achat => achat.DatePurchase.Year,
                    (annee, achats) => new
                    {
                        Annee = annee,
                        Moyenne = achats
                            .Join(Articles,
                                achat => achat.IdArticle,
                                article => article.Id,
                                (achat, article) => new { Achat = achat, Article = article })
                            .Where(item =>
                                (gamme == null || item.Article.Gamme == gamme)
                                && (famille == null || item.Article.Famille == famille)
                                && (fournisseur == null || item.Article.Fournisseur == fournisseur)
                                && (emplacement == null || item.Article.Emplacement == emplacement))
                            .Select(item => (double)item.Achat.TotalPrice)
                            .DefaultIfEmpty(0)
                            .Average()
                    })
                .OrderBy(x => x.Annee)
                .ToList();

            var chartValues = moyennesParAnnee.Select(x => (double)x.Moyenne).ToList();

            if (chartType == "Bar")
            {
                var barSeries = new ColumnSeries
                {
                    Title = $"Moyenne Total Price ({startYear}-{endYear})",
                    Values = new ChartValues<double>(chartValues)
                };

                // Add data series to the chart
                SeriesCollection.Add(barSeries);
            }
            else if (chartType == "Line")
            {
                var lineSeries = new LineSeries
                {
                    Title = $"Moyenne Total Price ({startYear}-{endYear})",
                    Values = new ChartValues<double>(chartValues)
                };

                // Add data series to the chart
                SeriesCollection.Add(lineSeries);
            }

            // Set the X-axis with year values
            Labels.AddRange(moyennesParAnnee.Select(x => x.Annee.ToString()));
        }
        public void GenerateChartYearlyAchat(int year, string chartType, string gamme = null, string famille = null, string fournisseur = null, string emplacement = null)
        {
            if (Achats == null || Articles == null)
            {
                MessageBox.Show("Les listes d'achats ou d'articles ne sont pas initialisées.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var months = Enumerable.Range(1, 12).ToList();

            var moyennesParMois = months
                .GroupJoin(
                    Achats,
                    mois => mois,
                    achat => achat.DatePurchase.Month,
                    (mois, achats) => new
                    {
                        Mois = mois,
                        Moyenne = achats
                            .Join(Articles,
                                achat => achat.IdArticle,
                                article => article.Id,
                                (achat, article) => new { Achat = achat, Article = article })
                            .Where(item =>
                                (gamme == null || item.Article.Gamme == gamme)
                                && (famille == null || item.Article.Famille == famille)
                                && (fournisseur == null || item.Article.Fournisseur == fournisseur)
                                && (emplacement == null || item.Article.Emplacement == emplacement))
                            .Select(item => (double)item.Achat.TotalPrice)
                            .DefaultIfEmpty(0)
                            .Average()
                    })
                .OrderBy(x => x.Mois)
                .ToList();

            var chartValues = moyennesParMois.Select(x => (double)x.Moyenne).ToList();

            if (chartType == "Bar")
            {
                var barSeries = new ColumnSeries
                {
                    Title = $"Moyenne Total Price ({year})",
                    Values = new ChartValues<double>(chartValues)
                };

                // Add data series to the chart
                SeriesCollection.Add(barSeries);
            }
            else if (chartType == "Line")
            {
                var lineSeries = new LineSeries
                {
                    Title = $"Moyenne Total Price ({year})",
                    Values = new ChartValues<double>(chartValues)
                };

                // Add data series to the chart
                SeriesCollection.Add(lineSeries);
            }

            // Set the X-axis with month labels
            Labels.AddRange(moyennesParMois.Select(x => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x.Mois)));
        }

        public void GenerateChartYearlyAchatCriteria(int? year, string chartType, string gamme = null, string famille = null, string fournisseur = null, string emplacement = null)
        {
            if (Achats == null || Articles == null)
            {
                MessageBox.Show("Les listes d'achats ou d'articles ne sont pas initialisées.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Filtrer les achats pour l'année spécifiée et les critères supplémentaires
            var achatsFiltres = Achats;

            if (year.HasValue)
            {
                int yearValue = year.Value; // Conversion explicite du type int? vers int
                achatsFiltres = achatsFiltres.Where(a => a.DatePurchase.Year == yearValue).ToList();
            }

            // Jointure manuelle avec la classe Article en utilisant IdArticle
            var achatsAvecArticles = achatsFiltres
                .Join(Articles,
                    achat => achat.IdArticle,
                    article => article.Id,
                    (achat, article) => new { Achat = achat, Article = article });



            // Filtrer les achats en fonction des critères supplémentaires
            if (gamme != null && famille != null && fournisseur != null && emplacement != null)
            {
                achatsAvecArticles = achatsAvecArticles.Where(item =>
                    item.Article.Gamme == gamme &&
                    item.Article.Famille == famille &&
                    item.Article.Fournisseur == fournisseur &&
                    item.Article.Emplacement == emplacement
                );
            }
            var moyennesParCritere = achatsAvecArticles
                .GroupBy(item => $"{item.Article.Gamme} - {item.Article.Famille} - {item.Article.Fournisseur} - {item.Article.Emplacement}")
                .Select(group => new
                {
                    Critere = group.Key,
                    Moyenne = group.Average(item => item.Achat.TotalPrice)
                })
                .OrderBy(item => item.Critere)
                .ToList();

            // Effacer les séries existantes avant d'ajouter la nouvelle série
            SeriesCollection.Clear();

            switch (chartType)
            {
                case "Bar":
                    var barSeries = new ColumnSeries
                    {
                        Title = $"Moyenne Total Price",
                        Values = new ChartValues<double>(moyennesParCritere.Select(item => (double)item.Moyenne))
                    };
                    // Ajouter la série de données au graphique
                    SeriesCollection.Add(barSeries);
                    break;

                case "Line":
                    var lineSeries = new LineSeries
                    {
                        Title = $"Moyenne Total Price",
                        Values = new ChartValues<double>(moyennesParCritere.Select(item => (double)item.Moyenne))
                    };
                    // Ajouter la série de données au graphique
                    SeriesCollection.Add(lineSeries);
                    break;

                // Ajoutez d'autres types de séries si nécessaire

                default:
                    throw new ArgumentOutOfRangeException(nameof(chartType), chartType, null);
            }

            // Définir les étiquettes de l'axe X avec les critères sélectionnés
            // Définir les étiquettes de l'axe X avec les critères sélectionnés
            Labels.Clear();

            if (year.HasValue)
            {
                // Filtrer les achats par année
                achatsAvecArticles = achatsAvecArticles.Where(item => item.Achat.DatePurchase.Year == year.Value);

                // Obtenir la liste des années uniques du mois
                var anneesDuMois = achatsAvecArticles
                    .Select(item => item.Achat.DatePurchase.Year)
                    .Distinct()
                    .OrderBy(year => year)
                    .ToList();

                // Ajouter les années du mois comme étiquettes
                Labels.AddRange(anneesDuMois.Select(year => year.ToString()));
            }
            else
            {
                // Ajouter les critères sélectionnés comme étiquettes
                if (gamme != null) Labels.Add(gamme);
                if (famille != null) Labels.Add(famille);
                if (fournisseur != null) Labels.Add(fournisseur);
                if (emplacement != null) Labels.Add(emplacement);
            }
        }

        public void GenerateChartMultipleMonthsAchat(int startYear, int startMonth, int endYear, int endMonth, string chartType, string gamme = null, string famille = null, string fournisseur = null, string emplacement = null)
        {
            if (Achats == null || Articles == null)
            {
                MessageBox.Show("Les listes d'achats ou d'articles ne sont pas initialisées.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Filtrer les achats pour les mois spécifiés et les critères supplémentaires
            var achatsFiltres = Achats
                .Where(a => a.DatePurchase.Year >= startYear && a.DatePurchase.Year <= endYear &&
                            ((a.DatePurchase.Year == startYear && a.DatePurchase.Month >= startMonth) ||
                             (a.DatePurchase.Year > startYear && a.DatePurchase.Year < endYear) ||
                             (a.DatePurchase.Year == endYear && a.DatePurchase.Month <= endMonth)))
                .ToList();

            // Jointure manuelle avec la classe Article en utilisant IdArticle
            var achatsAvecArticles = achatsFiltres
                .Join(Articles,
                    achat => achat.IdArticle,
                    article => article.Id,
                    (achat, article) => new { Achat = achat, Article = article })
                .ToList();

            // Filtrer les achats en fonction des critères supplémentaires
            achatsAvecArticles = achatsAvecArticles
                .Where(item =>
                    (gamme == null || item.Article.Gamme == gamme)
                    && (famille == null || item.Article.Famille == famille)
                    && (fournisseur == null || item.Article.Fournisseur == fournisseur)
                    && (emplacement == null || item.Article.Emplacement == emplacement))
                .ToList();

            var monthsInRange = Enumerable.Range(0, ((endYear - startYear) * 12) + endMonth - startMonth + 1)
                .Select(offset =>
                {
                    var currentDate = new DateTime(startYear, startMonth, 1).AddMonths(offset);
                    var achatsMois = achatsAvecArticles
                        .Where(achat => achat.Achat.DatePurchase.Year == currentDate.Year && achat.Achat.DatePurchase.Month == currentDate.Month);
                    var moyenne = achatsMois.Any() ? Math.Max(0, achatsMois.Average(item => item.Achat.TotalPrice)) : 0;
                    return new { Mois = currentDate.ToString("MMMM yyyy"), Moyenne = moyenne };
                })
                .ToList();

            // Effacer les séries existantes avant d'ajouter la nouvelle série
            SeriesCollection.Clear();

            switch (chartType)
            {
                case "Bar":
                    var barSeries = new ColumnSeries
                    {
                        Title = $"Moyenne Total Price ({startYear}-{startMonth} to {endYear}-{endMonth})",
                        Values = new ChartValues<double>(monthsInRange.Select(x => (double)x.Moyenne))
                    };
                    // Ajouter la série de données au graphique
                    SeriesCollection.Add(barSeries);
                    break;

                case "Line":
                    var lineSeries = new LineSeries
                    {
                        Title = $"Moyenne Total Price ({startYear}-{startMonth} to {endYear}-{endMonth})",
                        Values = new ChartValues<double>(monthsInRange.Select(x => (double)x.Moyenne))
                    };
                    // Ajouter la série de données au graphique
                    SeriesCollection.Add(lineSeries);
                    break;

                // Ajoutez d'autres types de séries si nécessaire

                default:
                    throw new ArgumentOutOfRangeException(nameof(chartType), chartType, null);
            }

            // Définir les étiquettes de l'axe X avec les noms des mois
            Labels.AddRange(monthsInRange.Select(x => x.Mois));
        }

        public void GenerateChartMonthlyAchat(int year, int month, string chartType, string gamme = null, string famille = null, string fournisseur = null, string emplacement = null)
        {
            if (Achats == null || Articles == null)
            {
                MessageBox.Show("Les listes d'achats ou d'articles ne sont pas initialisées.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Filtrer les achats pour l'année et le mois spécifiés et les critères supplémentaires
            var achatsFiltres = Achats
                .Where(a => a.DatePurchase.Year == year && a.DatePurchase.Month == month)
                .ToList();

            // Jointure manuelle avec la classe Article en utilisant IdArticle
            var achatsAvecArticles = achatsFiltres
                .Join(Articles,
                    achat => achat.IdArticle,
                    article => article.Id,
                    (achat, article) => new { Achat = achat, Article = article })
                .ToList();

            // Filtrer les achats en fonction des critères supplémentaires
            achatsAvecArticles = achatsAvecArticles
                .Where(item =>
                    (gamme == null || item.Article.Gamme == gamme)
                    && (famille == null || item.Article.Famille == famille)
                    && (fournisseur == null || item.Article.Fournisseur == fournisseur)
                    && (emplacement == null || item.Article.Emplacement == emplacement))
                .ToList();

            var daysInMonth = DateTime.DaysInMonth(year, month);

            var moyennesParJour = Enumerable.Range(1, daysInMonth)
                .Select(jour =>
                {
                    var achatsJour = achatsAvecArticles.Where(achat => achat.Achat.DatePurchase.Day == jour);
                    var moyenne = achatsJour.Any() ? Math.Max(0, achatsJour.Average(item => item.Achat.TotalPrice)) : 0;
                    return new { Jour = jour, Moyenne = moyenne };
                })
                .OrderBy(x => x.Jour)
                .ToList();

            // Effacer les séries existantes avant d'ajouter la nouvelle série
            SeriesCollection.Clear();

            switch (chartType)
            {
                case "Bar":
                    var barSeries = new ColumnSeries
                    {
                        Title = $"Moyenne Total Price ({year}-{month})",
                        Values = new ChartValues<double>(moyennesParJour.Select(x => (double)x.Moyenne))
                    };
                    // Ajouter la série de données au graphique
                    SeriesCollection.Add(barSeries);
                    break;

                case "Line":
                    var lineSeries = new LineSeries
                    {
                        Title = $"Moyenne Total Price ({year}-{month})",
                        Values = new ChartValues<double>(moyennesParJour.Select(x => (double)x.Moyenne))
                    };
                    // Ajouter la série de données au graphique
                    SeriesCollection.Add(lineSeries);
                    break;

                // Ajoutez d'autres types de séries si nécessaire

                default:
                    throw new ArgumentOutOfRangeException(nameof(chartType), chartType, null);
            }

            // Définir les étiquettes de l'axe X avec les numéros des jours du mois
            // Labels = moyennesParJour.Select(x => x.Jour.ToString()).ToList();
            // Labels = moyennesParJour.Select(x => x.Jour.ToString()).ToList();
            Labels.AddRange(moyennesParJour.Select(x => x.Jour.ToString()));
        }

        public void GenerateChartMultipleDaysAchat(int startYear, int startMonth, int startDay, int endYear, int endMonth, int endDay, string chartType, string gamme = null, string famille = null, string fournisseur = null, string emplacement = null)
        {
            if (Achats == null || Articles == null)
            {
                MessageBox.Show("Les listes d'achats ou d'articles ne sont pas initialisées.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Filtrer les achats pour la plage de jours spécifiée et les critères supplémentaires
            var achatsFiltres = Achats
                .Where(a => a.DatePurchase >= new DateTime(startYear, startMonth, startDay) &&
                            a.DatePurchase <= new DateTime(endYear, endMonth, endDay))
                .ToList();

            // Jointure manuelle avec la classe Article en utilisant IdArticle
            var achatsAvecArticles = achatsFiltres
                .Join(Articles,
                    achat => achat.IdArticle,
                    article => article.Id,
                    (achat, article) => new { Achat = achat, Article = article })
                .ToList();

            // Filtrer les achats en fonction des critères supplémentaires
            achatsAvecArticles = achatsAvecArticles
                .Where(item =>
                    (gamme == null || item.Article.Gamme == gamme)
                    && (famille == null || item.Article.Famille == famille)
                    && (fournisseur == null || item.Article.Fournisseur == fournisseur)
                    && (emplacement == null || item.Article.Emplacement == emplacement))
                .ToList();

            // Initialiser une liste pour stocker les totaux par heure
            var totalParHeure = new List<double>(24);

            // Calculer les totaux par heure
            for (int heure = 0; heure < 24; heure++)
            {
                var achatsParHeure = achatsAvecArticles
                    .Where(item => item.Achat.DatePurchase.Hour == heure)
                    .ToList();

                var totalParHeureActuelle = achatsParHeure.Any() ? achatsParHeure.Sum(item => item.Achat.TotalPrice) : 0;
                totalParHeure.Add(totalParHeureActuelle);
            }

            // Effacer les séries existantes avant d'ajouter la nouvelle série
            SeriesCollection.Clear();

            switch (chartType)
            {
                case "Bar":
                    var barSeries = new ColumnSeries
                    {
                        Title = $"Total Daily Price ({startYear}-{startMonth}-{startDay} to {endYear}-{endMonth}-{endDay})",
                        Values = new ChartValues<double>(totalParHeure)
                    };
                    // Ajouter la série de données au graphique
                    SeriesCollection.Add(barSeries);
                    break;

                case "Line":
                    var lineSeries = new LineSeries
                    {
                        Title = $"Total Daily Price ({startYear}-{startMonth}-{startDay} to {endYear}-{endMonth}-{endDay})",
                        Values = new ChartValues<double>(totalParHeure)
                    };
                    // Ajouter la série de données au graphique
                    SeriesCollection.Add(lineSeries);
                    break;

                // Ajoutez d'autres types de séries si nécessaire

                default:
                    throw new ArgumentOutOfRangeException(nameof(chartType), chartType, null);
            }

            // Définir les étiquettes de l'axe X (dans ce cas, ce serait les heures du jour)
            Labels.AddRange(Enumerable.Range(0, 24).Select(hour => hour.ToString()));
        }

        public void GenerateChartDailyAchat(int year, int month, int day, string chartType, string gamme = null, string famille = null, string fournisseur = null, string emplacement = null)
        {
            if (Achats == null || Articles == null)
            {
                MessageBox.Show("Les listes d'achats ou d'articles ne sont pas initialisées.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Filtrer les achats pour l'année, le mois, et le jour
            var achatsFiltres = Achats
                .Where(a => a.DatePurchase.Year == year && a.DatePurchase.Month == month && a.DatePurchase.Day == day)
                .ToList();

            // Jointure manuelle avec la classe Article en utilisant IdArticle
            var achatsAvecArticles = achatsFiltres
                .Join(Articles,
                    achat => achat.IdArticle,
                    article => article.Id,
                    (achat, article) => new { Achat = achat, Article = article })
                .ToList();

            // Filtrer les achats en fonction des critères supplémentaires
            achatsAvecArticles = achatsAvecArticles
                .Where(item =>
                    (gamme == null || item.Article.Gamme == gamme)
                    && (famille == null || item.Article.Famille == famille)
                    && (fournisseur == null || item.Article.Fournisseur == fournisseur)
                    && (emplacement == null || item.Article.Emplacement == emplacement))
                .ToList();

            // Initialiser une liste pour stocker les totaux par heure
            var totalParHeure = new List<double>(24);

            // Calculer les totaux par heure
            for (int heure = 0; heure < 24; heure++)
            {
                var achatsParHeure = achatsAvecArticles
                    .Where(item => item.Achat.DatePurchase.Hour == heure)
                    .ToList();

                var totalParHeureActuelle = achatsParHeure.Any() ? achatsParHeure.Sum(item => item.Achat.TotalPrice) : 0;
                totalParHeure.Add(totalParHeureActuelle);
            }

            // Effacer les séries existantes avant d'ajouter la nouvelle série
            SeriesCollection.Clear();

            switch (chartType)
            {
                case "Bar":
                    var barSeries = new ColumnSeries
                    {
                        Title = $"Total Daily Price ({year}-{month}-{day})",
                        Values = new ChartValues<double>(totalParHeure)
                    };
                    // Ajouter la série de données au graphique
                    SeriesCollection.Add(barSeries);
                    break;

                case "Line":
                    var lineSeries = new LineSeries
                    {
                        Title = $"Total Daily Price ({year}-{month}-{day})",
                        Values = new ChartValues<double>(totalParHeure)
                    };
                    // Ajouter la série de données au graphique
                    SeriesCollection.Add(lineSeries);
                    break;

                // Ajoutez d'autres types de séries si nécessaire

                default:
                    throw new ArgumentOutOfRangeException(nameof(chartType), chartType, null);
            }

            // Définir les étiquettes de l'axe X (dans ce cas, ce serait les heures du jour)
            Labels.AddRange(Enumerable.Range(0, 24).Select(hour => hour.ToString()));
        }

        public void GenerateChartPredictionsAchat(int startYear, int endYear, string chartType, string gamme = null, string famille = null, string fournisseur = null, string emplacement = null)
        {
            if (Achats == null || Articles == null)
            {
                MessageBox.Show("Les listes d'achats ou d'articles ne sont pas initialisées.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var years = Enumerable.Range(startYear, endYear - startYear + 1).ToList();

            var moyennesParAnnee = years
                .GroupJoin(
                    Achats,
                    annee => annee,
                    achat => achat.DatePurchase.Year,
                    (annee, achats) => new
                    {
                        Annee = annee,
                        Moyenne = achats
                            .Join(Articles,
                                achat => achat.IdArticle,
                                article => article.Id,
                                (achat, article) => new { Achat = achat, Article = article })
                            .Where(item =>
                                (gamme == null || item.Article.Gamme == gamme)
                                && (famille == null || item.Article.Famille == famille)
                                && (fournisseur == null || item.Article.Fournisseur == fournisseur)
                                && (emplacement == null || item.Article.Emplacement == emplacement))
                            .Select(item => (double)item.Achat.TotalPrice)
                            .DefaultIfEmpty(0)
                            .Average()
                    })
                .OrderBy(x => x.Annee)
                .ToList();

            // Extract years and corresponding averages
            var existingYears = moyennesParAnnee.Select(x => x.Annee).ToArray();
            var existingAverages = moyennesParAnnee.Select(x => (double)x.Moyenne).ToArray();

            // Interpolate values for missing years
            for (int year = startYear; year <= endYear; year++)
            {
                if (!existingYears.Contains(year))
                {
                    // Use linear interpolation to estimate the average for the missing year
                    double interpolatedAverage = LinearInterpolation(year, existingYears, existingAverages);

                    moyennesParAnnee.Add(new { Annee = year, Moyenne = interpolatedAverage });
                }
            }

            var chartValues = moyennesParAnnee.Select(x => (double)x.Moyenne).ToList();

            if (chartType == "Bar")
            {
                var barSeries = new ColumnSeries
                {
                    Title = $"Moyenne Total Price ({startYear}-{endYear})",
                    Values = new ChartValues<double>(chartValues)
                };

                // Add data series to the chart
                SeriesCollection.Add(barSeries);
            }
            else if (chartType == "Line")
            {
                var lineSeries = new LineSeries
                {
                    Title = $"Moyenne Total Price ({startYear}-{endYear})",
                    Values = new ChartValues<double>(chartValues)
                };

                // Add data series to the chart
                SeriesCollection.Add(lineSeries);
            }

            // Set the X-axis with year values
            Labels.AddRange(moyennesParAnnee.Select(x => x.Annee.ToString()));
        }

        // Function for linear interpolation
        private double LinearInterpolation(int targetYear, int[] existingYears, double[] existingValues)
        {
            int lowerIndex = Array.FindLastIndex(existingYears, year => year < targetYear);
            int upperIndex = Array.FindIndex(existingYears, year => year > targetYear);

            if (lowerIndex >= 0 && upperIndex >= 0)
            {
                double lowerYear = existingYears[lowerIndex];
                double upperYear = existingYears[upperIndex];

                double lowerValue = existingValues[lowerIndex];
                double upperValue = existingValues[upperIndex];

                // Linear interpolation formula
                return lowerValue + (upperValue - lowerValue) * (targetYear - lowerYear) / (upperYear - lowerYear);
            }

            // Return a default value if interpolation is not possible
            return 0.0;
        }
        public void GenerateChartMultipleYearsSale(int startYear, int endYear,  string chartType, string gamme = null, string famille = null, string fournisseur = null, string emplacement = null)
        {
            if (Sales == null || Articles == null)
            {
                MessageBox.Show("Les listes de ventes ou d'articles ne sont pas initialisées.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var years = Enumerable.Range(startYear, endYear - startYear + 1).ToList();

            var moyennesParAnnee = years
                .GroupJoin(
                    Sales,
                    annee => annee,
                    sale => sale.DateSale.Year,
                    (annee, sales) => new
                    {
                        Annee = annee,
                        Moyenne = sales
                            .Join(Articles,
                                sale => sale.IdArticle,
                                article => article.Id,
                                (sale, article) => new { Sale = sale, Article = article })
                            .Where(item =>
                                (gamme == null || item.Article.Gamme == gamme)
                                && (famille == null || item.Article.Famille == famille)
                                && (fournisseur == null || item.Article.Fournisseur == fournisseur)
                                && (emplacement == null || item.Article.Emplacement == emplacement))
                            .Select(item => (double)item.Sale.TotalPrice)
                            .DefaultIfEmpty(0)
                            .Average()
                    })
                .OrderBy(x => x.Annee)
                .ToList();

            var chartValues = moyennesParAnnee.Select(x => (double)x.Moyenne).ToList();

            if (chartType == "Bar")
            {
                var barSeries = new ColumnSeries
                {
                    Title = $"Moyenne Total Price ({startYear}-{endYear})",
                    Values = new ChartValues<double>(chartValues)
                };

                // Add data series to the chart
                SeriesCollection.Add(barSeries);
            }
            else if (chartType == "Line")
            {
                var lineSeries = new LineSeries
                {
                    Title = $"Moyenne Total Price ({startYear}-{endYear})",
                    Values = new ChartValues<double>(chartValues)
                };

                // Add data series to the chart
                SeriesCollection.Add(lineSeries);
            }

            // Set the X-axis with year values
            Labels.AddRange(moyennesParAnnee.Select(x => x.Annee.ToString()));
        }
        public void GenerateChartYearlySale(int year, string chartType, string gamme = null, string famille = null, string fournisseur = null, string emplacement = null)
        {
            if (Sales == null || Articles == null)
            {
                MessageBox.Show("Les listes d'achats ou d'articles ne sont pas initialisées.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var months = Enumerable.Range(1, 12).ToList();

            var moyennesParMois = months
                .GroupJoin(
                    Sales,
                    mois => mois,
                    sale => sale.DateSale.Month,
                    (mois, sales) => new
                    {
                        Mois = mois,
                        Moyenne = sales
                            .Join(Articles,
                                sale => sale.IdArticle,
                                article => article.Id,
                                (sale, article) => new { Sale = sale, Article = article })
                            .Where(item =>
                                (gamme == null || item.Article.Gamme == gamme)
                                && (famille == null || item.Article.Famille == famille)
                                && (fournisseur == null || item.Article.Fournisseur == fournisseur)
                                && (emplacement == null || item.Article.Emplacement == emplacement))
                            .Select(item => (double)item.Sale.TotalPrice)
                            .DefaultIfEmpty(0)
                            .Average()
                    })
                .OrderBy(x => x.Mois)
                .ToList();

            var chartValues = moyennesParMois.Select(x => (double)x.Moyenne).ToList();

            if (chartType == "Bar")
            {
                var barSeries = new ColumnSeries
                {
                    Title = $"Moyenne Total Price ({year})",
                    Values = new ChartValues<double>(chartValues)
                };

                // Add data series to the chart
                SeriesCollection.Add(barSeries);
            }
            else if (chartType == "Line")
            {
                var lineSeries = new LineSeries
                {
                    Title = $"Moyenne Total Price ({year})",
                    Values = new ChartValues<double>(chartValues)
                };

                // Add data series to the chart
                SeriesCollection.Add(lineSeries);
            }

            // Set the X-axis with month labels
            Labels.AddRange(moyennesParMois.Select(x => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x.Mois)));
        }
        public void GenerateChartYearlySaleCriteria(int? year, string chartType, string gamme = null, string famille = null, string fournisseur = null, string emplacement = null)
        {
            if (Sales == null || Articles == null)
            {
                MessageBox.Show("Les listes d'achats ou d'articles ne sont pas initialisées.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Filtrer les achats pour l'année spécifiée et les critères supplémentaires
            var salesFiltres = Sales;

            if (year.HasValue)
            {
                int yearValue = year.Value; // Conversion explicite du type int? vers int
                salesFiltres = salesFiltres.Where(a => a.DateSale.Year == yearValue).ToList();
            }

            // Jointure manuelle avec la classe Article en utilisant IdArticle
            var salessAvecArticles = salesFiltres
                .Join(Articles,
                    sale => sale.IdArticle,
                    article => article.Id,
                    (sale, article) => new { Sale = sale, Article = article });



            // Filtrer les achats en fonction des critères supplémentaires
            if (gamme != null && famille != null && fournisseur != null && emplacement != null)
            {
                salessAvecArticles = salessAvecArticles.Where(item =>
                    item.Article.Gamme == gamme &&
                    item.Article.Famille == famille &&
                    item.Article.Fournisseur == fournisseur &&
                    item.Article.Emplacement == emplacement
                );
            }
            var moyennesParCritere = salessAvecArticles
                .GroupBy(item => $"{item.Article.Gamme} - {item.Article.Famille} - {item.Article.Fournisseur} - {item.Article.Emplacement}")
                .Select(group => new
                {
                    Critere = group.Key,
                    Moyenne = group.Average(item => item.Sale.TotalPrice)
                })
                .OrderBy(item => item.Critere)
                .ToList();

            // Effacer les séries existantes avant d'ajouter la nouvelle série
            SeriesCollection.Clear();

            switch (chartType)
            {
                case "Bar":
                    var barSeries = new ColumnSeries
                    {
                        Title = $"Moyenne Total Price",
                        Values = new ChartValues<double>(moyennesParCritere.Select(item => (double)item.Moyenne))
                    };
                    // Ajouter la série de données au graphique
                    SeriesCollection.Add(barSeries);
                    break;

                case "Line":
                    var lineSeries = new LineSeries
                    {
                        Title = $"Moyenne Total Price",
                        Values = new ChartValues<double>(moyennesParCritere.Select(item => (double)item.Moyenne))
                    };
                    // Ajouter la série de données au graphique
                    SeriesCollection.Add(lineSeries);
                    break;

                // Ajoutez d'autres types de séries si nécessaire

                default:
                    throw new ArgumentOutOfRangeException(nameof(chartType), chartType, null);
            }

            // Définir les étiquettes de l'axe X avec les critères sélectionnés
            // Définir les étiquettes de l'axe X avec les critères sélectionnés
            Labels.Clear();

            if (year.HasValue)
            {
                // Filtrer les achats par année
                salessAvecArticles = salessAvecArticles.Where(item => item.Sale.DateSale.Year == year.Value);

                // Obtenir la liste des années uniques du mois
                var anneesDuMois = salessAvecArticles
                    .Select(item => item.Sale.DateSale.Year)
                    .Distinct()
                    .OrderBy(year => year)
                    .ToList();

                // Ajouter les années du mois comme étiquettes
                Labels.AddRange(anneesDuMois.Select(year => year.ToString()));
            }
            else
            {
                // Ajouter les critères sélectionnés comme étiquettes
                if (gamme != null) Labels.Add(gamme);
                if (famille != null) Labels.Add(famille);
                if (fournisseur != null) Labels.Add(fournisseur);
                if (emplacement != null) Labels.Add(emplacement);
            }
        }
        public void GenerateChartMultipleMonthsSale(int startYear, int startMonth, int endYear, int endMonth, string chartType, string gamme = null, string famille = null, string fournisseur = null, string emplacement = null)
        {
            if (Sales == null || Articles == null)
            {
                MessageBox.Show("Les listes de ventes ou d'articles ne sont pas initialisées.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Filtrer les achats pour les mois spécifiés et les critères supplémentaires
            var ventesFiltres = Sales
                .Where(a => a.DateSale.Year >= startYear && a.DateSale.Year <= endYear &&
                            ((a.DateSale.Year == startYear && a.DateSale.Month >= startMonth) ||
                             (a.DateSale.Year > startYear && a.DateSale.Year < endYear) ||
                             (a.DateSale.Year == endYear && a.DateSale.Month <= endMonth)))
                .ToList();

            // Jointure manuelle avec la classe Article en utilisant IdArticle
            var ventesAvecArticles = ventesFiltres
                .Join(Articles,
                    sale => sale.IdArticle,
                    article => article.Id,
                    (sale, article) => new { Sale = sale, Article = article })
                .ToList();

            // Filtrer les ventes en fonction des critères supplémentaires
            ventesAvecArticles = ventesAvecArticles
                .Where(item =>
                    (gamme == null || item.Article.Gamme == gamme)
                    && (famille == null || item.Article.Famille == famille)
                    && (fournisseur == null || item.Article.Fournisseur == fournisseur)
                    && (emplacement == null || item.Article.Emplacement == emplacement))
                .ToList();

            var monthsInRange = Enumerable.Range(0, ((endYear - startYear) * 12) + endMonth - startMonth + 1)
                .Select(offset =>
                {
                    var currentDate = new DateTime(startYear, startMonth, 1).AddMonths(offset);
                    var salesMois = ventesAvecArticles
                        .Where(sale => sale.Sale.DateSale.Year == currentDate.Year && sale.Sale.DateSale.Month == currentDate.Month);
                    var moyenne = salesMois.Any() ? Math.Max(0, salesMois.Average(item => item.Sale.TotalPrice)) : 0;
                    return new { Mois = currentDate.ToString("MMMM yyyy"), Moyenne = moyenne };
                })
                .ToList();

            // Effacer les séries existantes avant d'ajouter la nouvelle série
            SeriesCollection.Clear();

            switch (chartType)
            {
                case "Bar":
                    var barSeries = new ColumnSeries
                    {
                        Title = $"Moyenne Total Price ({startYear}-{startMonth} to {endYear}-{endMonth})",
                        Values = new ChartValues<double>(monthsInRange.Select(x => (double)x.Moyenne))
                    };
                    // Ajouter la série de données au graphique
                    SeriesCollection.Add(barSeries);
                    break;

                case "Line":
                    var lineSeries = new LineSeries
                    {
                        Title = $"Moyenne Total Price ({startYear}-{startMonth} to {endYear}-{endMonth})",
                        Values = new ChartValues<double>(monthsInRange.Select(x => (double)x.Moyenne))
                    };
                    // Ajouter la série de données au graphique
                    SeriesCollection.Add(lineSeries);
                    break;

                // Ajoutez d'autres types de séries si nécessaire

                default:
                    throw new ArgumentOutOfRangeException(nameof(chartType), chartType, null);
            }

            // Définir les étiquettes de l'axe X avec les noms des mois
            Labels.AddRange(monthsInRange.Select(x => x.Mois));
        }

        public void GenerateChartMonthlySale(int year, int month, string chartType, string gamme = null, string famille = null, string fournisseur = null, string emplacement = null)
        {
            if (Sales == null || Articles == null)
            {
                MessageBox.Show("Les listes de ventes ou d'articles ne sont pas initialisées.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Filtrer les ventes pour l'année et le mois spécifiés et les critères supplémentaires
            var salesFiltres = Sales
                .Where(a => a.DateSale.Year == year && a.DateSale.Month == month)
                .ToList();

            // Jointure manuelle avec la classe Article en utilisant IdArticle
            var salesAvecArticles = salesFiltres
                .Join(Articles,
                    sale => sale.IdArticle,
                    article => article.Id,
                    (sale, article) => new { Sale = sale, Article = article })
                .ToList();

            // Filtrer les ventes en fonction des critères supplémentaires
            salesAvecArticles = salesAvecArticles
                .Where(item =>
                    (gamme == null || item.Article.Gamme == gamme)
                    && (famille == null || item.Article.Famille == famille)
                    && (fournisseur == null || item.Article.Fournisseur == fournisseur)
                    && (emplacement == null || item.Article.Emplacement == emplacement))
                .ToList();

            var daysInMonth = DateTime.DaysInMonth(year, month);

            var moyennesParJour = Enumerable.Range(1, daysInMonth)
                .Select(jour =>
                {
                    var salesJour = salesAvecArticles.Where(sale => sale.Sale.DateSale.Day == jour);
                    var moyenne = salesJour.Any() ? Math.Max(0, salesJour.Average(item => item.Sale.TotalPrice)) : 0;
                    return new { Jour = jour, Moyenne = moyenne };
                })
                .OrderBy(x => x.Jour)
                .ToList();

            // Effacer les séries existantes avant d'ajouter la nouvelle série
            SeriesCollection.Clear();

            switch (chartType)
            {
                case "Bar":
                    var barSeries = new ColumnSeries
                    {
                        Title = $"Moyenne Total Price ({year}-{month})",
                        Values = new ChartValues<double>(moyennesParJour.Select(x => (double)x.Moyenne))
                    };
                    // Ajouter la série de données au graphique
                    SeriesCollection.Add(barSeries);
                    break;

                case "Line":
                    var lineSeries = new LineSeries
                    {
                        Title = $"Moyenne Total Price ({year}-{month})",
                        Values = new ChartValues<double>(moyennesParJour.Select(x => (double)x.Moyenne))
                    };
                    // Ajouter la série de données au graphique
                    SeriesCollection.Add(lineSeries);
                    break;

                // Ajoutez d'autres types de séries si nécessaire

                default:
                    throw new ArgumentOutOfRangeException(nameof(chartType), chartType, null);
            }

            // Définir les étiquettes de l'axe X avec les numéros des jours du mois
            // Labels = moyennesParJour.Select(x => x.Jour.ToString()).ToList();
            // Labels = moyennesParJour.Select(x => x.Jour.ToString()).ToList();
            Labels.AddRange(moyennesParJour.Select(x => x.Jour.ToString()));
        }

        public void GenerateChartMultipleDaysSale(int startYear, int startMonth, int startDay, int endYear, int endMonth, int endDay, string chartType, string gamme = null, string famille = null, string fournisseur = null, string emplacement = null)
        {
            if (Sales == null || Articles == null)
            {
                MessageBox.Show("Les listes de ventes ou d'articles ne sont pas initialisées.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Filtrer les ventes pour la plage de jours spécifiée et les critères supplémentaires
            var salesFiltres = Sales
                .Where(a => a.DateSale >= new DateTime(startYear, startMonth, startDay) &&
                            a.DateSale <= new DateTime(endYear, endMonth, endDay))
                .ToList();

            // Jointure manuelle avec la classe Article en utilisant IdArticle
            var salesAvecArticles = salesFiltres
                .Join(Articles,
                    sale => sale.IdArticle,
                    article => article.Id,
                    (sale, article) => new { Sale = sale, Article = article })
                .ToList();

            // Filtrer les ventes en fonction des critères supplémentaires
            salesAvecArticles = salesAvecArticles
                .Where(item =>
                    (gamme == null || item.Article.Gamme == gamme)
                    && (famille == null || item.Article.Famille == famille)
                    && (fournisseur == null || item.Article.Fournisseur == fournisseur)
                    && (emplacement == null || item.Article.Emplacement == emplacement))
                .ToList();

            // Initialiser une liste pour stocker les totaux par heure
            var totalParHeure = new List<double>(24);

            // Calculer les totaux par heure
            for (int heure = 0; heure < 24; heure++)
            {
                var salesParHeure = salesAvecArticles
                    .Where(item => item.Sale.DateSale.Hour == heure)
                    .ToList();

                var totalParHeureActuelle = salesParHeure.Any() ? salesParHeure.Sum(item => item.Sale.TotalPrice) : 0;
                totalParHeure.Add(totalParHeureActuelle);
            }

            // Effacer les séries existantes avant d'ajouter la nouvelle série
            SeriesCollection.Clear();

            switch (chartType)
            {
                case "Bar":
                    var barSeries = new ColumnSeries
                    {
                        Title = $"Total Daily Price ({startYear}-{startMonth}-{startDay} to {endYear}-{endMonth}-{endDay})",
                        Values = new ChartValues<double>(totalParHeure)
                    };
                    // Ajouter la série de données au graphique
                    SeriesCollection.Add(barSeries);
                    break;

                case "Line":
                    var lineSeries = new LineSeries
                    {
                        Title = $"Total Daily Price ({startYear}-{startMonth}-{startDay} to {endYear}-{endMonth}-{endDay})",
                        Values = new ChartValues<double>(totalParHeure)
                    };
                    // Ajouter la série de données au graphique
                    SeriesCollection.Add(lineSeries);
                    break;

                // Ajoutez d'autres types de séries si nécessaire

                default:
                    throw new ArgumentOutOfRangeException(nameof(chartType), chartType, null);
            }

            // Définir les étiquettes de l'axe X (dans ce cas, ce serait les heures du jour)
            Labels.AddRange(Enumerable.Range(0, 24).Select(hour => hour.ToString()));
        }
        public void GenerateChartDailySale(int year, int month, int day, string chartType, string gamme = null, string famille = null, string fournisseur = null, string emplacement = null)
        {
            if (Sales == null || Articles == null)
            {
                MessageBox.Show("Les listes de ventes ou d'articles ne sont pas initialisées.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Filtrer les ventes pour l'année, le mois, et le jour
            var salesFiltres = Sales
                .Where(a => a.DateSale.Year == year && a.DateSale.Month == month && a.DateSale.Day == day)
                .ToList();

            // Jointure manuelle avec la classe Article en utilisant IdArticle
            var salesAvecArticles = salesFiltres
                .Join(Articles,
                    sale => sale.IdArticle,
                    article => article.Id,
                    (sale, article) => new { Sale = sale, Article = article })
                .ToList();

            // Filtrer les ventes en fonction des critères supplémentaires
            salesAvecArticles = salesAvecArticles
                .Where(item =>
                    (gamme == null || item.Article.Gamme == gamme)
                    && (famille == null || item.Article.Famille == famille)
                    && (fournisseur == null || item.Article.Fournisseur == fournisseur)
                    && (emplacement == null || item.Article.Emplacement == emplacement))
                .ToList();

            // Initialiser une liste pour stocker les totaux par heure
            var totalParHeure = new List<double>(24);

            // Calculer les totaux par heure
            for (int heure = 0; heure < 24; heure++)
            {
                var salesParHeure = salesAvecArticles
                    .Where(item => item.Sale.DateSale.Hour == heure)
                    .ToList();

                var totalParHeureActuelle = salesParHeure.Any() ? salesParHeure.Sum(item => item.Sale.TotalPrice) : 0;
                totalParHeure.Add(totalParHeureActuelle);
            }

            // Effacer les séries existantes avant d'ajouter la nouvelle série
            SeriesCollection.Clear();

            switch (chartType)
            {
                case "Bar":
                    var barSeries = new ColumnSeries
                    {
                        Title = $"Total Daily Price ({year}-{month}-{day})",
                        Values = new ChartValues<double>(totalParHeure)
                    };
                    // Ajouter la série de données au graphique
                    SeriesCollection.Add(barSeries);
                    break;

                case "Line":
                    var lineSeries = new LineSeries
                    {
                        Title = $"Total Daily Price ({year}-{month}-{day})",
                        Values = new ChartValues<double>(totalParHeure)
                    };
                    // Ajouter la série de données au graphique
                    SeriesCollection.Add(lineSeries);
                    break;

                // Ajoutez d'autres types de séries si nécessaire

                default:
                    throw new ArgumentOutOfRangeException(nameof(chartType), chartType, null);
            }

            // Définir les étiquettes de l'axe X (dans ce cas, ce serait les heures du jour)
            Labels.AddRange(Enumerable.Range(0, 24).Select(hour => hour.ToString()));
        }

        public void GenerateChartPredictionsSale(int startYear, int endYear, string gamme = null, string famille = null, string fournisseur = null, string emplacement = null, string chartType = "Line")
        {
            if (Sales == null || Articles == null)
            {
                MessageBox.Show("Les listes de ventes ou d'articles ne sont pas initialisées.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var years = Enumerable.Range(startYear, endYear - startYear + 1).ToList();

            var moyennesParAnnee = years
                .GroupJoin(
                    Sales,
                    annee => annee,
                    sale => sale.DateSale.Year,
                    (annee, sales) => new
                    {
                        Annee = annee,
                        Moyenne = sales
                            .Join(Articles,
                                sale => sale.IdArticle,
                                article => article.Id,
                                (sale, article) => new { Sale = sale, Article = article })
                            .Where(item =>
                                (gamme == null || item.Article.Gamme == gamme)
                                && (famille == null || item.Article.Famille == famille)
                                && (fournisseur == null || item.Article.Fournisseur == fournisseur)
                                && (emplacement == null || item.Article.Emplacement == emplacement))
                            .Select(item => (double)item.Sale.TotalPrice)
                            .DefaultIfEmpty(0)
                            .Average()
                    })
                .OrderBy(x => x.Annee)
                .ToList();

            // Extract years and corresponding averages
            var existingYears = moyennesParAnnee.Select(x => x.Annee).ToArray();
            var existingAverages = moyennesParAnnee.Select(x => (double)x.Moyenne).ToArray();

            // Interpolate values for missing years
            for (int year = startYear; year <= endYear; year++)
            {
                if (!existingYears.Contains(year))
                {
                    // Use linear interpolation to estimate the average for the missing year
                    double interpolatedAverage = LinearInterpolation(year, existingYears, existingAverages);

                    moyennesParAnnee.Add(new { Annee = year, Moyenne = interpolatedAverage });
                }
            }

            var chartValues = moyennesParAnnee.Select(x => (double)x.Moyenne).ToList();

            if (chartType == "Bar")
            {
                var barSeries = new ColumnSeries
                {
                    Title = $"Moyenne Total Price ({startYear}-{endYear})",
                    Values = new ChartValues<double>(chartValues)
                };

                // Add data series to the chart
                SeriesCollection.Add(barSeries);
            }
            else if (chartType == "Line")
            {
                var lineSeries = new LineSeries
                {
                    Title = $"Moyenne Total Price ({startYear}-{endYear})",
                    Values = new ChartValues<double>(chartValues)
                };

                // Add data series to the chart
                SeriesCollection.Add(lineSeries);
            }

            // Set the X-axis with year values
            Labels.AddRange(moyennesParAnnee.Select(x => x.Annee.ToString()));
        }

        // Function for linear interpolation
        










        //???????????????
        public void GenerateChartSale(int year, string gamme = null, string famille = null, string fournisseur = null, string emplacement = null)
        {
            if (Sales == null || Articles == null)
            {
                MessageBox.Show("Les listes d'achats ou d'articles ne sont pas initialisées.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Filter purchases for the specified year and additional criteria
            var salefilter = Sales
                .Where(a => a.DateSale.Year == year)
                .ToList();

            // Join with the Article class using IdArticle
            var salesAvecArticles = salefilter
                .Join(Articles,
                    sale => sale.IdArticle,
                    article => article.Id,
                    (sale, article) => new { Sale = sale, Article = article })
                .ToList();

            // Filter purchases based on additional criteria
            salesAvecArticles = salesAvecArticles
                .Where(item =>
                    (gamme == null || item.Article.Gamme == gamme)
                    && (famille == null || item.Article.Famille == famille)
                    && (fournisseur == null || item.Article.Fournisseur == fournisseur)
                    && (emplacement == null || item.Article.Emplacement == emplacement))
                .ToList();

            var moyennesParMois = Enumerable.Range(1, 12)
                .GroupJoin(
                    salesAvecArticles,
                    mois => mois,
                    sale => sale.Sale.DateSale.Month,
                    (mois, sales) => new
                    {
                        Mois = mois,
                        Moyenne = sales.Any() ? Math.Max(0, sales.Average(item => item.Sale.TotalPrice)) : 0
                    })
                .OrderBy(x => x.Mois)
                .ToList();

            var barSeries = new ColumnSeries
            {
                Title = $"Moyenne Total Price ({year})",
                Values = new ChartValues<double>(moyennesParMois.Select(x => (double)x.Moyenne))
            };

            // Add data series to the chart
            SeriesCollection.Add(barSeries);

            // Set the X-axis with month names
            Labels.AddRange(moyennesParMois.Select(x => x.Mois.ToString()));
        }
        public void GenerateMultiYearChartSale(int startYear, int endYear, string gamme = null, string famille = null, string fournisseur = null, string emplacement = null)
        {
            if (Sales == null || Articles == null)
            {
                MessageBox.Show("Les listes de ventes ou d'articles ne sont pas initialisées.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Filter sales for the specified year range and additional criteria
            var salesFilter = Sales
                .Where(s => s.DateSale.Year >= startYear && s.DateSale.Year <= endYear)
                .ToList();

            // Join with the Article class using IdArticle
            var salesAvecArticles = salesFilter
                .Join(Articles,
                    sale => sale.IdArticle,
                    article => article.Id,
                    (sale, article) => new { Sale = sale, Article = article })
                .ToList();

            // Filter sales based on additional criteria
            salesAvecArticles = salesAvecArticles
                .Where(item =>
                    (gamme == null || item.Article.Gamme == gamme)
                    && (famille == null || item.Article.Famille == famille)
                    && (fournisseur == null || item.Article.Fournisseur == fournisseur)
                    && (emplacement == null || item.Article.Emplacement == emplacement))
                .ToList();

            var moyennesParMois = Enumerable.Range(1, 12)
                .GroupJoin(
                    salesAvecArticles,
                    mois => mois,
                    sale => sale.Sale.DateSale.Month,
                    (mois, sales) => new
                    {
                        Mois = mois,
                        Moyenne = sales.Any() ? Math.Max(0, sales.Average(item => item.Sale.TotalPrice)) : 0
                    })
                .OrderBy(x => x.Mois)
                .ToList();

            var barSeries = new ColumnSeries
            {
                Title = $"Moyenne Total Price ({startYear}-{endYear})",
                Values = new ChartValues<double>(moyennesParMois.Select(x => (double)x.Moyenne))
            };

            // Add data series to the chart
            SeriesCollection.Add(barSeries);

            // Set the X-axis with month names
            Labels.AddRange(moyennesParMois.Select(x => x.Mois.ToString()));
        }


    }
}




