using PetCareApp.Models;
using QuestPDF.Companion;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.IO;

namespace PetCareApp.Services
{
    public static class AnimalReport
    {
        public static void CreateAnimalReport(Animal animal)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    //page.Margin(40);
                    //page.PageColor(Colors.Black);
                    page.DefaultTextStyle(x => x.FontSize(14));

                    // Header (Titel)
                    page.Header()
                        .AlignCenter()
                        .AlignMiddle()
                        .Background(Colors.White)
                        .PaddingBottom(10)
                        .Text($"Tierbericht – {animal.Name}")
                        .FontSize(30)
                        .Bold()
                        .FontColor(Colors.Black);

                    // Hauptinhalt
                    page.Content()
                        .Background(Colors.White)
                        .Padding(20)
                        .Column(col =>
                        {
                            col.Spacing(10);
                            col.Item().Text($"Name: {animal.Name}");
                            col.Item().Text($"Art: {animal.Species}");
                            col.Item().Text($"Rasse: {animal.Breed}");
                            col.Item().Text($"Geburtsdatum: {animal.BirthDate:dd.MM.yyyy}");
                            col.Item().Text($"Besitzer: {animal.Owner}");
                            col.Item().Text($"PassNr.: {animal.PassNr}");
                        });

                    // Fußzeile
                    page.Footer()
                        .AlignCenter()
                        .Text($"©️PetCare • {DateTime.Now:dd.MM.yyyy}");
                });
            });

            // Speicherpfad: Dokumente-Ordner
            var path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                $"Tierbericht_{animal.Name}.pdf"
            );

            document.GeneratePdf(path);
        }
    }
}
