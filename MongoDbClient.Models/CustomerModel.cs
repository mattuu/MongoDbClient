using System;

namespace MongoDbClient.Models
{
    public class CustomerModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string Address { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime UpdatedOn { get; set; } = DateTime.Now;
    }
}