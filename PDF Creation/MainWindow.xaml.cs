using PDF_Creation.Services;
using QuestPDF.Infrastructure;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using PDF_Creation.Models;

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
            
            QuestPDF.Settings.License = LicenseType.Community; // 🟦 hier hinzufügen
            try
            {
                // Beispiel-Daten – später durch deine echten Daten ersetzen
                var animal = new Animal
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

                // PDF erzeugen
                AnimalReport.CreateAnimalReport(animal);

                // Pfad wie in AnimalReport.cs:
                var path = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    $"Tierbericht_{animal.Name}.pdf"
                );

                MessageBox.Show($"PDF erstellt:\n{path}", "PetCare", MessageBoxButton.OK, MessageBoxImage.Information);

                // PDF direkt öffnen (mit Standard-PDF-Viewer)
                if (File.Exists(path))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = path,
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
    }
}
