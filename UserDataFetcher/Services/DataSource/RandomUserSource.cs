using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using UserDataFetcher.Interfaces;
using UserDataFetcher.Models;
using System.Net.Http;
using System.Text.Json;

namespace UserDataFetcher.Services.DataSources
{
    public class RandomUserSource : IUserDataSource
    {
        public string SourceName
        {
            get { return "RandomUser"; }
        }

        public async Task<List<User>> FetchUsersAsync(HttpClient client)
        {
            List<User> users = new List<User>();

            try
            {
                string responseJson = await client.GetStringAsync("https://randomuser.me/api/?results=500");

                RandomUserResponse? responseData = JsonSerializer.Deserialize<RandomUserResponse>(
                    responseJson,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

                if (responseData == null || responseData.Results == null)
                {
                    return users;
                }

                foreach (var result in responseData.Results)
                {
                    if (result.Name != null && !string.IsNullOrEmpty(result.Email))
                    {
                        User user = new User
                        {
                            FirstName = result.Name.First ?? string.Empty,
                            LastName = result.Name.Last ?? string.Empty,
                            Email = result.Email,
                            SourceId = result.Login?.Uuid ?? string.Empty
                        };

                        users.Add(user);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data from RandomUser API: {ex.Message}");
            }

            return users;
        }
    }

    public class RandomUserResponse
    {
        public List<RandomUserResult> Results { get; set; }

        public RandomUserResponse()
        {
            Results = new List<RandomUserResult>();
        }
    }

    public class RandomUserResult
    {
        public RandomUserName? Name { get; set; }
        public string Email { get; set; }
        public RandomUserLogin? Login { get; set; }

        public RandomUserResult()
        {
            Email = string.Empty;
        }
    }

    public class RandomUserName
    {
        public string First { get; set; }
        public string Last { get; set; }

        public RandomUserName()
        {
            First = string.Empty;
            Last = string.Empty;
        }
    }

    public class RandomUserLogin
    {
        public string Uuid { get; set; }

        public RandomUserLogin()
        {
            Uuid = string.Empty;
        }
    }
}
