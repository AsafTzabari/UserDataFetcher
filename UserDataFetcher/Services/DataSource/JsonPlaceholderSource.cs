using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using UserDataFetcher.Interfaces;
using UserDataFetcher.Models;

namespace UserDataFetcher.Services.DataSources
{
    public class JsonPlaceholderSource : IUserDataSource
    {
        public string SourceName
        {
            get { return "JsonPlaceholder"; }
        }

        public async Task<List<User>> FetchUsersAsync(HttpClient client)
        {
            List<User> users = new List<User>();

            try
            {
                List<JsonPlaceholderUser>? responseData = await client.GetFromJsonAsync<List<JsonPlaceholderUser>>(
                    "https://jsonplaceholder.typicode.com/users"
                );

                if (responseData == null)
                {
                    return users;
                }

                foreach (var item in responseData)
                {
                    string[] nameParts = item.Name.Split(' ');
                    string firstName = nameParts.Length > 0 ? nameParts[0] : string.Empty;
                    string lastName = string.Empty;

                    if (nameParts.Length > 1)
                    {
                        lastName = string.Join(" ", nameParts.Skip(1));
                    }

                    User user = new User
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Email = item.Email,
                        SourceId = item.Id.ToString()
                    };

                    users.Add(user);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data from JsonPlaceholder API: {ex.Message}");
            }

            return users;
        }
    }

        public class JsonPlaceholderUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public JsonPlaceholderUser()
        {
            Name = string.Empty;
            Email = string.Empty;
        }
    }
}