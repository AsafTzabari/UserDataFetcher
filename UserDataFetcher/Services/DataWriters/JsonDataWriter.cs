using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
using UserDataFetcher.Interfaces;
using UserDataFetcher.Models;
using System.Text.Json;
using System.Text;

namespace UserDataFetcher.Services.DataWriters
{
    public class JsonDataWriter : IDataWriter
    {
        public async Task WriteDataAsync(List<User> users, string path)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };

                string jsonData = JsonSerializer.Serialize(users, options);

                await File.WriteAllTextAsync(
                    path,
                    jsonData,
                    Encoding.UTF8
                );
            }
            catch (Exception ex)
            {
                throw new Exception($"Error writing JSON file: {ex.Message}", ex);
            }
        }
    }
}
