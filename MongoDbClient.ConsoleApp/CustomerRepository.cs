using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDbClient.ConsoleApp.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDbClient.ConsoleApp
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly NoteContext _context;

        public CustomerRepository(IOptions<Settings> settings)
        {
            _context = new NoteContext(settings);
        }

        // query after Id or InternalId (BSonId value)
        //
        public async Task<Customer> GetCustomer(string id)
        {
            try
            {
                var internalId = GetInternalId(id);
                return await _context.Notes
                                     .Find(note => note.Id == id
                                                   || note.InternalId == internalId)
                                     .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                //TODO log or manage the exception
                throw ex;
            }
        }

        // query after body text, updated time, and header image size
        //
        public async Task<IEnumerable<Customer>> GetCustomersById(string[] ids)
        {
            try
            {
                var query = _context.Notes.Find(note => ids.Contains(note.Id));

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                //TODO log or manage the exception
                throw ex;
            }
        }

        private ObjectId GetInternalId(string id)
        {
            if (!ObjectId.TryParse(id, out var internalId))
            {
                internalId = ObjectId.Empty;
            }

            return internalId;
        }

        public async Task<bool> AddOrUpdateCustomer(string id, Customer item)
        {
            try
            {
                var actionResult = await _context.Notes.ReplaceOneAsync(n => n.Id.Equals(id),
                                                                        item,
                                                                        new UpdateOptions
                                                                        {
                                                                            IsUpsert = true
                                                                        });

                return actionResult.IsAcknowledged && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                //TODO log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveCustomer(string id)
        {
            try
            {
                var actionResult = await _context.Notes.DeleteOneAsync(Builders<Customer>.Filter.Eq("Id", id));

                return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                //TODO log or manage the exception
                throw ex;
            }
        }
    }
}