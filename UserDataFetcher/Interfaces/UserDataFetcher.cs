using System.Collections.Generic;
using System.Threading.Tasks;
using UserDataFetcher.Models;

namespace UserDataFetcher.Interfaces
{
    public interface IDataWriter
    {
        Task WriteDataAsync(List<User> users, string path);
    }
}