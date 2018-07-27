using System;
using System.Threading.Tasks;

namespace MongoDbClient.Caching
{
    public interface IIndexEngine
    {
        Task AddOrUpdateCustomer(string id, string firstName, string surname, DateTime dateOfBirth, string postCode, string emailAddress, string phoneNumber, string bookingReference, string flightPNR);
    }
}