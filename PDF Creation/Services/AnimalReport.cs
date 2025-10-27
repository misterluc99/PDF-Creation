using PDF_Creation.Models;
using QuestPDF.Companion;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.IO;

namespace PDF_Creation.Services
{
    public static class AnimalReport
    {
        [Obsolete]
        public static void CreateAnimalReport(Animal animal)
        {
            // Optional, je nach Projekt (aber schadet nicht)
            // QuestPDF.Settings.License = LicenseType.Community;

            // Bildpfade
            string logoPath = "C:\\Users\\Gaming PC\\source\\repos\\PDF-Creation\\PDF Creation\\images\\petcarelogo.png";
            string imagePath = "C:\\Users\\Gaming PC\\source\\repos\\PDF-Creation\\PDF Creation\\images\\Anni Christmas.jpg";

            // Bytes laden (wenn Datei fehlt -> früh scheitern)
            byte[] logoBytes = File.ReadAllBytes(logoPath);
            byte[] animalImageBytes = File.ReadAllBytes(imagePath);

            // Speicherort
            var path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                $"Tierbericht_{(animal?.Name ?? "Unbekannt")}.pdf"
            );

            // Hilfs-Funktionen für sichere Texte
            string S(string? x) => string.IsNullOrWhiteSpace(x) ? "-" : x!;
            string D(DateTime? dt) => dt.HasValue ? dt.Value.ToString("dd.MM.yyyy") : "-";

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(20); // etwas Außenrand entspannt das Layout
                    page.DefaultTextStyle(x => x.FontSize(14));

                    // ---------- HEADER ----------
                    page.Header()
                        .Height(70) // feste Höhe verhindert Overflow
                        .PaddingBottom(6)
                        .Row(row =>
                        {
                            // Logo-Box: 120x60
                            row.ConstantItem(120)
                               .AlignMiddle()
                               .AlignLeft()
                               .Height(60)
                               .Image(logoBytes);

                            row.RelativeItem()
                               .AlignCenter()
                               .AlignMiddle()
                               

                               .Text(t =>
                               {
                                   t.Span($"Tierbericht – {S(animal?.Name)}")
                                    .FontSize(20)
                                    .Bold()
                                    .FontColor(Colors.Black)
                                    .WrapAnywhere(); // erlaubt Umbruch überall
                               });
                            row.ConstantItem(120); // Platzhalter für Ausrichtung
                        });

                    // ---------- CONTENT ----------
                    page.Content()
                        .Padding(10)
                        .Column(col =>
                        {
                            col.Spacing(18);

                        // Tierfoto: sichtbare, „sichere“ Box
                        col.Item()
                            .AlignLeft()
                            .Element(box =>
                            {
                                box
                                    .Width(300)
                                    .Height(200)
                                    .Padding(8)
                                    .Border(2)
                                    .BorderColor(Colors.Black)
                                    .Padding(20)
                                    .Background(Colors.White)
                                    .AlignCenter()
                                    .AlignMiddle()
                                    .Image(animalImageBytes, ImageScaling.FitArea);
                            }

                               );



                            // Tabelle
                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(130);
                                    columns.RelativeColumn();
                                });

                                void Row(string label, string value)
                                {
                                    table.Cell().Element(CellStyle).Text(txt => txt.Span(label).ExtraBold().WrapAnywhere());
                                    table.Cell().Element(CellStyle).Text(txt => txt.Span(value).WrapAnywhere());
                                }

                                Row("Name:", S(animal?.Name));
                                Row("Art:", S(animal?.Species));
                                Row("Rasse:", S(animal?.Breed));
                                Row("Geburtsdatum:", D(animal?.BirthDate));
                                Row("Gewicht:", $"{animal?.Weight:F2} kg");
                                Row("Besitzer:", S(animal?.Owner));
                                Row("PassNr.:", S(animal?.PassNr));
                                Row("Impfungen:", S(animal?.Impfungen));

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container
                                        .PaddingVertical(6)
                                        .PaddingHorizontal(8)
                                        .Border(1)
                                        .BorderColor(Colors.Black)
                                        .Padding(5);
                                }
                            });
                        });

                    // ---------- FOOTER ----------
                    page.Footer()
                        .AlignCenter()
                        .Text($"©️ ♥️PetCare • {DateTime.Now:dd.MM.yyyy}");
                });
            });

            document.GeneratePdf(path);
        }
    }
}
