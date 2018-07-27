using System;
using System.Threading.Tasks;
using MongoDbClient.Caching.Infrastructure;

namespace MongoDbClient.Caching
{
    public class CustomerIndexEngine : ICustomerIndexEngine
    {
        private readonly IIndexWriter<CustomerCacheModel> _indexWriter;

        public CustomerIndexEngine(IIndexWriter<CustomerCacheModel> indexWriter)
        {
            _indexWriter = indexWriter ?? throw new ArgumentNullException(nameof(indexWriter));
        }

        public Task AddOrUpdateCustomer(string id, string firstName, string surname, DateTime dateOfBirth, string postCode, string emailAddress, string phoneNumber, string bookingReference, string flightPNR)
        {
            return Task.Run(() =>
            {
                var model = new CustomerCacheModel
                {
                    Id = id,
                    FirstName = firstName,
                    Surname = surname,
                    DateOfBirth = dateOfBirth,
                    PostCode = postCode,
                    EmailAddress = emailAddress,
                    PhoneNumber = phoneNumber,
                    BookingReference = bookingReference,
                    FlightPNR = flightPNR
                };

                _indexWriter.AddOrUpdateIndex(model);
            });
        }
    }
}