using PDF_Creation.Services;
using QuestPDF.Infrastructure;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using PDF_Creation.Models;
using Microsoft.Win32;          
using System.Linq;              


namespace PDF_CreationApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // Diese Methode wird von der automatisch generierten Datei MainWindow.g.i.cs bereitgestellt,
            // die aus MainWindow.xaml erzeugt wird. Stelle sicher, dass MainWindow.xaml existiert
            // und korrekt im Projekt eingebunden ist (Build-Aktion: "Page").
            InitializeComponent();
        }

        private void BtnExportPdf_Click(object sender, RoutedEventArgs e)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            try
            {
                var animal = new PDF_Creation.Models.Animal
                {
                    Name = "Anissa von der Halben Welt",
                    Species = "Hund",
                    Breed = "Beagle",
                    BirthDate = new DateTime(2018, 3, 14),
                    Weight = 12.5,
                    Owner = "Jasmin Hanke",
                    PassNr = "1234",
                    Impfungen = "Tollwut, ..."
                };

                string safeName = SanitizeFileName($"Tierbericht_{animal.Name}.pdf");

                var dialog = new SaveFileDialog
                {
                    Title = "Tierbericht speichern unter...",
                    FileName = safeName,
                    Filter = "PDF-Dateien (*.pdf)|*.pdf|Alle Dateien (*.*)|*.*",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                };

                bool? result = dialog.ShowDialog(this);
                if (result != true)
                    return; // Abbrechen

                string outputPath = dialog.FileName;

                // Optionale Bilder (relativ aus dem Ausgabeverzeichnis \images\...)
                // Tipp: Wir kopieren die Bilder gleich in Schritt 3 immer ins Output-Verzeichnis.
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string? logoPath = Path.Combine(baseDir, "images", "petcarelogo.png");
                string? photoPath = Path.Combine(baseDir, "images", "Anni Christmas.jpg");

                PDF_Creation.Services.AnimalReport.CreateAnimalReport(animal, outputPath, logoPath, photoPath);

                MessageBox.Show($"PDF erstellt:\n{outputPath}", "PetCare", MessageBoxButton.OK, MessageBoxImage.Information);

                if (File.Exists(outputPath))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = outputPath,
                        UseShellExecute = true
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Erstellen des PDFs:\n{ex.Message}",
                    "PetCare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static string SanitizeFileName(string name)
        {
            var invalid = Path.GetInvalidFileNameChars();
            var cleaned = new string(name.Select(ch => invalid.Contains(ch) ? '_' : ch).ToArray());
            return string.IsNullOrWhiteSpace(cleaned) ? "Dokument.pdf" : cleaned;
        }
    }
}