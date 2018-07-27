using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

namespace MongoDbClient.ConsoleApp
{
    public class CsvDataReader
    {
        public IEnumerable<CustomerCsvModel> Read(Stream stream)
        {
            using (TextReader textReader = new StreamReader(stream))
            {
                using (var csv = new CsvReader(textReader))
                {
                    csv.Configuration.HasHeaderRecord = true;

                    csv.Configuration.BadDataFound = context => { Console.WriteLine($"Bad data found on row '{context.RawRow}'"); };

                    csv.Configuration.ReadingExceptionOccurred = exception => { Console.WriteLine($"Reading exception: {exception.Message}"); };

                    csv.Configuration.PrepareHeaderForMatch = h => h.Replace(" ", string.Empty).Trim();

                    var records = new HashSet<CustomerCsvModel>();

                    while (csv.Read())
                    {
                        var csvLine = csv.GetRecord<CustomerCsvModel>();
                        if (csvLine != null)
                        {
                            records.Add(csvLine);
                        }
                    }

                    return records;
                }
            }
        }
    }

    public class CustomerCsvModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string Title { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string AddressLine1 { get; set; }

        public string Town { get; set; }

        public string PostCode { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string BookingReference { get; set; }

        public string FlightPNR { get; set; }
    }

    public class CustomerCsvModelMap : ClassMap<CustomerCsvModel>
    {
        public CustomerCsvModelMap()
        {
            Map(m => m.Id).Name("id");
        }
    }
}