using PDF_Creation.Models;
using QuestPDF.Companion;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.IO;
using PDF_Creation;

namespace PDF_Creation.Services
{
    public static class AnimalReport
    {
        public static void CreateAnimalReport(Animal animal)
        {
            string filePath = "C:\\Users\\lucah\\source\\repos\\PDF-Creation\\PDF Creation\\images\\petcarelogo.png";
            byte[] logoBytes = File.ReadAllBytes(filePath);

            string filePath2 = "C:\\Users\\lucah\\source\\repos\\PDF-Creation\\PDF Creation\\images\\Anni Christmas.jpg";
            byte[] animalImageBytes = File.ReadAllBytes(filePath2);

            // Speicherpfad: Dokumente-Ordner (vor dem Erstellen des Dokuments berechnen)
            var path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                $"Tierbericht_{animal.Name}.pdf"
            );

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.DefaultTextStyle(x => x.FontSize(14));

                    page.Header()
                        .Background(Colors.White)
                        .PaddingBottom(10)
                        .AlignCenter()
                        .Row(row =>
                        {
                            row.ConstantItem(100)
                               .AlignMiddle()
                               .Image(logoBytes);

                            row.RelativeItem()
                               .AlignMiddle()
                               .Text($"Tierbericht – {animal.Name}")
                               .FontSize(30)
                               .Bold()
                               .FontColor(Colors.Black)
                               .AlignLeft();
                        });

                    page.Content()
                        .Background(Colors.White)
                        .Padding(20)
                        .Column(col =>
                        {
                            col.Spacing(20);

                            col.Item()
                                .AlignCenter()
                                .Image(animalImageBytes);

                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(130);
                                    columns.RelativeColumn();
                                });

                                table.Cell().Element(CellStyle).Text("Name:").ExtraBold();
                                table.Cell().Element(CellStyle).Text(animal.Name);
                                table.Cell().Element(CellStyle).Text("Art:").ExtraBold();
                                table.Cell().Element(CellStyle).Text(animal.Species);
                                table.Cell().Element(CellStyle).Text("Rasse:").ExtraBold();
                                table.Cell().Element(CellStyle).Text(animal.Breed);
                                table.Cell().Element(CellStyle).Text("Geburtsdatum:").ExtraBold();
                                table.Cell().Element(CellStyle).Text(animal.BirthDate.ToString("dd.MM.yyyy"));
                                table.Cell().Element(CellStyle).Text("Besitzer:").ExtraBold();
                                table.Cell().Element(CellStyle).Text(animal.Owner);
                                table.Cell().Element(CellStyle).Text("PassNr.:").ExtraBold();
                                table.Cell().Element(CellStyle).Text(animal.PassNr);

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container
                                        .PaddingVertical(6)
                                        .PaddingHorizontal(8)
                                        .BorderBottom(1)
                                        .BorderTop(1)
                                        .BorderRight(1)
                                        .BorderLeft(1)
                                        .Padding(5)
                                        .BorderColor(Colors.Black);
                                }
                            });
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text($"©️ PetCare • {DateTime.Now:dd.MM.yyyy}");
                });
            });

            // GeneratePdf außerhalb der Create-Lambda aufrufen
            document.GeneratePdf(path);
        }
    }
}
