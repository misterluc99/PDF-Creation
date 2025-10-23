using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDF_Creation.Services
{
    public static class FileHandler
    {
        /// <summary>
        /// Lädt eine Datei von einem Dateipfad und gibt es als Base64-String zurück, wenn die Datei die Maximalgröße nicht überschreitet.
        /// </summary>
        /// <param name="filePath">Der Dateipfad. Kann relativ oder absolut sein.</param>
        /// <param name="maxSizeInMb">Maximale erlaubte Dateigröße der Datei in MB.</param>
        /// <returns>Base64-String oder leerer String, wenn Datei zu groß ist</returns>
        /// <exception cref="FileNotFoundException">Wird ausgelöst, wenn die durch <paramref name="filePath"/> angegebene Datei nicht existiert.</exception>
        public static string GetDataAsBase64(string filePath, double maxSizeInMb = 5.0)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Bilddatei nicht gefunden.", filePath);
            }

            FileInfo fileInfo = new FileInfo(filePath);
            double fileSizeInMb = fileInfo.Length / (1024.0 * 1024.0);

            if (fileSizeInMb > maxSizeInMb)
            {
                return string.Empty;
            }

            byte[] fileBytes = File.ReadAllBytes(filePath);
            return Convert.ToBase64String(fileBytes);
        }

        /// <summary>
        /// Wandelt einen Base64-String in ein byte-Array um.
        /// </summary>
        /// <param name="base64String">Der Base64-kodierte String der Datei.</param>
        /// <returns>Ein <see cref="BitmapImage"/>-Objekt .</returns>
        /// <exception cref="ArgumentNullException">Wird ausgelöst, wenn <paramref name="base64String"/> null oder leer ist.</exception>
        public static async Task<byte[]> ConvertBase64ToBytesAsync(string base64String)
        {
            if (string.IsNullOrWhiteSpace(base64String))
                throw new ArgumentException("Base64-String darf nicht leer oder null sein.", nameof(base64String));

            try
            {
                return await Task.Run(() =>
                {
                    byte[] bytes = Convert.FromBase64String(base64String);
                    return bytes;
                });
            }
            catch
            {
                return Array.Empty<byte>();
            }
        }
    }
}
