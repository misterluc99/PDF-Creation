using System;
using System.IO;
using PDF_Creation.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PDF_Creation.Services
{
    public static class AnimalReport
    {
        /// <summary>
        /// Bevorzugte Methode: generiert den Bericht an outputPath.
        /// logoPath / photoPath sind optional. Fehlende Bilder führen nicht zum Absturz.
        /// </summary>
        public static void CreateAnimalReport(Animal animal, string outputPath, string? logoPath = null, string? photoPath = null)
        {
            if (animal == null) throw new ArgumentNullException(nameof(animal));
            if (string.IsNullOrWhiteSpace(outputPath)) throw new ArgumentException("Ausgabepfad darf nicht leer sein.", nameof(outputPath));

            var dir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

           

            byte[]? logoBytes = TryReadBytes(logoPath);
            byte[]? animalImageBytes = TryReadBytes(photoPath);

            string S(string? x) => string.IsNullOrWhiteSpace(x) ? "-" : x!;
            string D(DateTime? d) => d.HasValue ? d.Value.ToString("dd.MM.yyyy") : "-";

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(20);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(14));

                    // HEADER
                    page.Header()
                        .Height(70)
                        .PaddingBottom(6)
                        .Row(row =>
                        {
                            row.ConstantItem(120)
                               .AlignMiddle()
                               .AlignLeft()
                               .Height(60)
                               .Element(b =>
                               {
                                   if (logoBytes != null)
                                       b.Image(logoBytes, ImageScaling.FitArea);
                                   else
                                       b.Text("PetCare")
                                        .FontSize(18)
                                        .SemiBold()
                                        .FontColor(Colors.Blue.Medium);
                               });

                            row.RelativeItem()
                               .AlignCenter()
                               .AlignMiddle()
                               .Text(t =>
                               {
                                   t.Span($"Tierbericht – {S(animal.Name)}")
                                    .FontSize(20).Bold().FontColor(Colors.Black).WrapAnywhere();
                               });

                            row.ConstantItem(120);
                        });

                    // CONTENT
                    page.Content()
                        .Padding(10)
                        .Column(col =>
                        {
                            col.Spacing(18);

                            // Foto-Box
                            col.Item().AlignLeft().Element(box =>
                            {
                                var frame = box
                                    .Width(300)
                                    .Height(200)
                                    .Padding(8)
                                    .Border(2)
                                    .BorderColor(Colors.Grey.Darken2)
                                    .Background(Colors.White)
                                    .AlignCenter()
                                    .AlignMiddle();

                                if (animalImageBytes != null)
                                    frame.Image(animalImageBytes, ImageScaling.FitArea);
                                else
                                    frame.Text(txt => txt.Span("Kein Foto verfügbar").FontSize(12).Italic().FontColor(Colors.Grey.Darken2));
                            });

                            // Daten-Tabelle
                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(130);
                                    columns.RelativeColumn();
                                });

                                static IContainer CellStyle(IContainer c) => c
                                    .PaddingVertical(6)
                                    .PaddingHorizontal(8)
                                    .Border(1)
                                    .BorderColor(Colors.Grey.Darken2)
                                    .Padding(5);

                                void Row(string label, string value)
                                {
                                    table.Cell().Element(CellStyle).Text(x => x.Span(label).ExtraBold().WrapAnywhere());
                                    table.Cell().Element(CellStyle).Text(x => x.Span(value).WrapAnywhere());
                                }

                                Row("Name:", S(animal.Name));
                                Row("Art:", S(animal.Species));
                                Row("Rasse:", S(animal.Breed));
                                Row("Geburtsdatum:", D(animal.BirthDate));
                                Row("Gewicht:", animal.Weight != 0 ? $"{animal.Weight:F2} kg" : "-");
                                Row("Besitzer:", S(animal.Owner));
                                Row("PassNr.:", S(animal.PassNr));
                                Row("Impfungen:", S(animal.Impfungen));
                            });
                        });

                    // FOOTER
                    page.Footer()
                        .AlignCenter()
                        .Text($"♥️ PetCare • {DateTime.Now:dd.MM.yyyy}");
                });
            });

            document.GeneratePdf(outputPath);
        }

        /// <summary>
        /// Alte Signatur (Kompatibilität): speichert in "Eigene Dokumente".
        /// </summary>
        public static void CreateAnimalReport(Animal animal)
        {
            var defaultPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                $"Tierbericht_{(string.IsNullOrWhiteSpace(animal?.Name) ? "Unbekannt" : animal!.Name)}.pdf");

            CreateAnimalReport(
                animal,
                defaultPath,
                logoPath: Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images", "petcarelogo.png"),
                photoPath: Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images", "Anni Christmas.jpg")
            );
        }

        private static byte[]? TryReadBytes(string? path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path) || !File.Exists(path)) return null;
                return File.ReadAllBytes(path);
            }
            catch { return null; }
        }
    }
}

