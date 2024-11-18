using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using UserDataFetcher.Interfaces;
using UserDataFetcher.Models;

namespace UserDataFetcher.Services.DataSources
{
    public class DummyJsonSource : IUserDataSource
    {
        public string SourceName
        {
            get { return "DummyJson"; }
        }

        public async Task<List<User>> FetchUsersAsync(HttpClient client)
        {
            List<User> users = new List<User>();

            try
            {
                DummyJsonResponse? responseData = await client.GetFromJsonAsync<DummyJsonResponse>(
                    "https://dummyjson.com/users"
                );

                if (responseData == null || responseData.Users == null)
                {
                    return users;
                }

                foreach (var item in responseData.Users)
                {
                    User user = new User
                    {
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        Email = item.Email,
                        SourceId = item.Id.ToString()
                    };

                    users.Add(user);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data from DummyJson API: {ex.Message}");
            }

            return users;
        }
    }

    public class DummyJsonResponse
    {
        public List<DummyJsonUser> Users { get; set; }
        public int Total { get; set; }
        public int Skip { get; set; }
        public int Limit { get; set; }

        public DummyJsonResponse()
        {
            Users = new List<DummyJsonUser>();
        }
    }

    public class DummyJsonUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public DummyJsonUser()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
        }
    }
}