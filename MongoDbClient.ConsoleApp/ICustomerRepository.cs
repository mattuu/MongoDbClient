using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDbClient.ConsoleApp.Models;

namespace MongoDbClient.ConsoleApp
{
    public interface ICustomerRepository
    {
        Task<Customer> GetCustomer(string id);

        Task<IEnumerable<Customer>> GetCustomersById(string[] ids);

        Task<bool> AddOrUpdateCustomer(string id, Customer item);

        Task<bool> RemoveCustomer(string id);
    }
}