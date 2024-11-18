using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
using UserDataFetcher.Interfaces;
using UserDataFetcher.Models;
using System.Text;

namespace UserDataFetcher.Services.DataWriters
{
    public class CsvDataWriter : IDataWriter
    {
        public async Task WriteDataAsync(List<User> users, string path)
        {
            List<string> csvLines = new List<string>();

            csvLines.Add("FirstName,LastName,Email,SourceId");

            foreach (User user in users)
            {
                string firstName = EscapeCsvField(user.FirstName);
                string lastName = EscapeCsvField(user.LastName);
                string email = EscapeCsvField(user.Email);
                string sourceId = EscapeCsvField(user.SourceId);

                string line = $"{firstName},{lastName},{email},{sourceId}";
                csvLines.Add(line);
            }

            try
            {
                await File.WriteAllLinesAsync(path, csvLines, new UTF8Encoding(true));
            }
            catch (Exception ex)
            {
                throw new Exception($"Error writing CSV file: {ex.Message}", ex);
            }
        }

        private string EscapeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field))
            {
                return string.Empty;
            }

            field = field.Replace("\"", "\"\"");

            if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
            {
                field = $"\"{field}\"";
            }

            return field;
        }
    }
}