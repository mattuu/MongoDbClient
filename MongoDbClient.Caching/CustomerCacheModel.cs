using System;

namespace MongoDbClient.Caching
{
    public class CustomerCacheModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string PostCode { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string BookingReference { get; set; }

        public string FlightPNR { get; set; }
    }
}