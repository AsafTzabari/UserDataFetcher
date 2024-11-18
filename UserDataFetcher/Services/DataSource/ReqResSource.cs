using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UserDataFetcher.Interfaces;
using UserDataFetcher.Models;

namespace UserDataFetcher.Services.DataSources
{
    public class ReqResSource : IUserDataSource
    {
        public string SourceName
        {
            get { return "ReqRes"; }
        }

        public async Task<List<User>> FetchUsersAsync(HttpClient client)
        {
            List<User> users = new List<User>();

            try
            {
                ReqResResponse? responseData = await client.GetFromJsonAsync<ReqResResponse>(
                    "https://reqres.in/api/users"
                );

                if (responseData == null || responseData.Data == null)
                {
                    return users;
                }

                foreach (var item in responseData.Data)
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
                Console.WriteLine($"Error fetching data from ReqRes API: {ex.Message}");
            }

            return users;
        }
    }


    public class ReqResResponse
    {
        public int Page { get; set; }
        public int PerPage { get; set; }
        public int Total { get; set; }
        public int TotalPages { get; set; }
        public List<ReqResUser> Data { get; set; }

        public ReqResResponse()
        {
            Data = new List<ReqResUser>();
        }
    }

    public class ReqResUser
    {
        public int Id { get; set; }
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }

        public ReqResUser()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            Avatar = string.Empty;
        }
    }
}