using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NNNN.DataAccess.Implementation
{
    public class XXXXRepository : IXXXXRepository
    {
        private readonly YYYYContext _context = null;
        readonly ILogger _logger = Log.ForContext<XXXXRepository>();

        public XXXXRepository(IOptions<DBConnectionSettings> settings)
        {
            _context = YYYYContext.GetInstance(settings);
        }

        public async Task<IEnumerable<XXXX>> GetAllXXXXs()
        {
            try
            {
                return await _context.XXXXs.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while accessing XXXX list.");
                throw ex;
            }
        }

        public async Task<XXXX> Get(int id)
        {
            try
            {
                var filter = Builders<XXXX>.Filter.Eq("ID", id);

                return await _context.XXXXs
                                .Find(filter)
                                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while accessing XXXX with ID {@ID}.", id);
                throw ex;
            }
        }

        public async Task<bool> Add(XXXX item)
        {
            try
            {
                var filter = Builders<XXXX>.Filter.Eq(x => x.ID, item.ID);
                var o = _context.XXXXs
                                .Find(filter)
                                .FirstOrDefaultAsync()
                                .Result;
                if (o == null)
                {
                    await _context.XXXXs.InsertOneAsync(item);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while adding XXXX with ID {@ID}.", item.ID);
                throw ex;
            }
        }

        public async Task<bool> RemoveAll()
        {
            try
            {
                DeleteResult actionResult = await _context.XXXXs.DeleteManyAsync(new BsonDocument());

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while removing XXXX list.");
                throw ex;
            }
        }

        public async Task<bool> Remove(int id)
        {
            try
            {
                DeleteResult actionResult = await _context.XXXXs.DeleteOneAsync(
                     Builders<XXXX>.Filter.Eq("ID", id));

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while removing XXXX with ID {@ID}.", id);
                throw ex;
            }
        }

        public async Task<bool> UpdateXXXXDocument(int id, string title)
        {
            var item = await Get(id) ?? new XXXX();
            item.Title = title;
            item.LastUpdatedOn = DateTime.Now;

            return await Update(id, item);
        }

        public async Task<bool> Update(int id, XXXX item)
        {
            try
            {
                item.ID = id; //Make sure ID of the item is assigned.
                ReplaceOneResult actionResult = await _context.XXXXs
                                                .ReplaceOneAsync(n => n.ID.Equals(id)
                                                                , item
                                                                , new UpdateOptions { IsUpsert = true });
                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while updating XXXX with ID {@ID}.", id);
                throw ex;
            }
        }

        public async Task<bool> Exists(int id)
        {
            var filter = Builders<XXXX>.Filter.Eq(s => s.ID, id);
            try
            {
                var r = await _context.XXXXs.FindAsync(filter);
                if(r.Current.GetEnumerator().Current == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while querying existence of an XXXX with ID {@ID}.", id);
                throw ex;
            }
        }
    }
}
