using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UserDataFetcher.Models;

namespace UserDataFetcher.Interfaces
{
    public interface IUserDataSource
    {
        string SourceName { get; }
        Task<List<User>> FetchUsersAsync(HttpClient client);
    }
}