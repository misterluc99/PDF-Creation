using PetCareApp.Models;
using PetCareApp.Services;
using QuestPDF.Infrastructure;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace PetCareApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
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
                    Name = "Anissa",
                    Species = "Hund",
                    Breed = "Beagle",
                    BirthDate = new DateTime(2018, 3, 14),
                    Owner = "Jasmin Hanke",
                    PassNr = "Kein Pass da Ilegale Immigrantin",
                    
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
