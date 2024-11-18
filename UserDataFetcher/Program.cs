using UserDataFetcher.Models;
using UserDataFetcher.Interfaces;
using UserDataFetcher.Services.DataSources;
using UserDataFetcher.Services.DataWriters;

namespace UserDataFetcher
{
    public class Program
    {
        public static async Task Main()
        {
            List<IUserDataSource> dataSources = new List<IUserDataSource>
            {
                new RandomUserSource(),
                new JsonPlaceholderSource(),
                new DummyJsonSource(),
                new ReqResSource()
            };

            try
            {
                Console.WriteLine("Please enter the folder path where you want to save the file:");
                string? inputPath = Console.ReadLine();
                string folderPath = inputPath ?? string.Empty;

                if (string.IsNullOrEmpty(folderPath))
                {
                    Console.WriteLine("Error: Folder path cannot be empty!");
                    return;
                }

                if (!Directory.Exists(folderPath))
                {
                    Console.WriteLine("Error: The specified folder does not exist!");
                    return;
                }

                Console.WriteLine("\nPlease enter the desired format (JSON or CSV):");
                string? inputFormat = Console.ReadLine();
                string format = inputFormat?.ToUpper() ?? string.Empty;

                if (format != "JSON" && format != "CSV")
                {
                    Console.WriteLine("Error: Invalid format! Please specify either JSON or CSV.");
                    return;
                }

                using (var httpClient = new HttpClient())
                {
                    Console.WriteLine("\nStarting to fetch data from all sources...\n");

                    var fetchTasks = new List<Task<List<User>>>();

                    foreach (var source in dataSources)
                    {
                        var fetchTask = FetchFromSourceAsync(source, httpClient);
                        fetchTasks.Add(fetchTask);
                    }

                    var allUsers = new List<User>();
                    var results = await Task.WhenAll(fetchTasks);

                    foreach (var userList in results)
                    {
                        if (userList != null)
                        {
                            allUsers.AddRange(userList);
                        }
                    }

                    IDataWriter dataWriter = format == "JSON"
                        ? new JsonDataWriter()
                        : new CsvDataWriter();

                    string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    string fileName = $"users_{timestamp}.{format.ToLower()}";
                    string fullPath = Path.Combine(folderPath, fileName);

                    Console.WriteLine("\nWriting data to file...");
                    await dataWriter.WriteDataAsync(allUsers, fullPath);

                    Console.WriteLine($"\nOperation completed successfully!");
                    Console.WriteLine($"Total users fetched: {allUsers.Count}");
                    Console.WriteLine($"Data has been saved to: {fullPath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nAn error occurred: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        private static async Task<List<User>> FetchFromSourceAsync(IUserDataSource source, HttpClient client)
        {
            try
            {
                Console.WriteLine($"Fetching data from {source.SourceName}...");
                var users = await source.FetchUsersAsync(client);
                Console.WriteLine($"Successfully fetched {users.Count} users from {source.SourceName}");
                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching from {source.SourceName}: {ex.Message}");
                return new List<User>();
            }
        }
    }
}