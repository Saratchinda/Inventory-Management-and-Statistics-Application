using Log_in.mvvm.mvvmStat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Windows.Markup;


namespace Dancewave.vue
{
    /// <summary>
    /// Logique d'interaction pour WindStatistique2.xaml
    /// </summary>
    public partial class WindStatistique2 : Window
    {
        public MainViewModel ViewModel { get; set; }
        public WindStatistique2()
        {
            InitializeComponent();
            InitializeComponent();
            ViewModel = new MainViewModel();
            DataContext = ViewModel;
           // ViewModel.GenerateAverageChart(2023, 11, 2024, 6);

        }
        private void SaveAsPdf(string filePath)
        {
            try
            {
                // Convertir la grille en un rendu visuel
                RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)cartesianChart1.ActualWidth, (int)cartesianChart1.ActualHeight, 96, 96, PixelFormats.Pbgra32);
                renderTargetBitmap.Render(cartesianChart1);
                // Convertir le rendu visuel en une image JPEG ou PNG
                BitmapEncoder encoder = new PngBitmapEncoder(); // Modifier en JPEGBitmapEncoder pour JPEG
                encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

                // Enregistrer l'image dans un flux mémoire
                using (MemoryStream ms = new MemoryStream())
                {
                    encoder.Save(ms);
                    ms.Seek(0, SeekOrigin.Begin);

                    // Créer un document PDF
                    PdfDocument document = new PdfDocument();
                    PdfPage page = document.AddPage();

                    // Dessiner l'image sur la page PDF
                    XGraphics gfx = XGraphics.FromPdfPage(page);
                    XImage image = XImage.FromStream(ms);
                    gfx.DrawImage(image, 0, 0);

                    // Enregistrer le document PDF
                    document.Save(filePath);

                    MessageBox.Show("Fichier PDF créé avec succès : " + filePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la création du fichier PDF : " + ex.Message);
            }
        }

        private void btnPdf_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Fichier PDF (*.pdf)|*.pdf";
            saveFileDialog.Title = "Enregistrer le fichier PDF";

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                SaveAsPdf(filePath);
            }
        }

       
    }
    
}
