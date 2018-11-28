using AttendanceApi.DataAccess.Interfaces;
using Models.Core.HR.Attendance;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AttendanceApi.DataAccess.Implementation
{
    public class AccessPointRepository : IAccessPointRepository
    {
        private readonly AttendanceDBContext _context = null;
        readonly ILogger _logger = Log.ForContext<AccessPointRepository>();

        public AccessPointRepository()
        {
            _context = AttendanceDBContext.Instance;
        }

        public async Task<IEnumerable<AccessPoint>> GetAllAccessPoints()
        {
            try
            {
                return await _context.AccessPoints.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while accessing AccessPoint list.");
                throw ex;
            }
        }

        public async Task<AccessPoint> Get(int id)
        {
            try
            {
                var filter = Builders<AccessPoint>.Filter.Eq("ID", id);

                return await _context.AccessPoints
                                .Find(filter)
                                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while accessing AccessPoint with ID {@ID}.", id);
                throw ex;
            }
        }

        public async Task<bool> Add(AccessPoint item)
        {
            try
            {
                var filter = Builders<AccessPoint>.Filter.Eq(x => x.ID, item.ID) &
                    Builders<AccessPoint>.Filter.Eq(x => x.IpAddress, item.IpAddress);
                var o = _context.AccessPoints
                                .Find(filter)
                                .FirstOrDefaultAsync()
                                .Result;
                if (o == null)
                {
                    await _context.AccessPoints.InsertOneAsync(item);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while adding AccessPoint with ID {@ID}.", item.ID);
                throw ex;
            }
        }

        public async Task<bool> RemoveAll()
        {
            try
            {
                DeleteResult actionResult = await _context.AccessPoints.DeleteManyAsync(new BsonDocument());

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while removing AccessPoint list.");
                throw ex;
            }
        }

        public async Task<bool> Remove(int id)
        {
            try
            {
                DeleteResult actionResult = await _context.AccessPoints.DeleteOneAsync(
                     Builders<AccessPoint>.Filter.Eq("ID", id));

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while removing AccessPoint with ID {@ID}.", id);
                throw ex;
            }
        }

        public async Task<bool> Update(int id, AccessPoint item)
        {
            try
            {
                item.ID = id; //Make sure ID of the item is assigned.
                ReplaceOneResult actionResult = await _context.AccessPoints
                                                .ReplaceOneAsync(n => n.ID.Equals(id)
                                                                , item
                                                                , new UpdateOptions { IsUpsert = true });
                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while updating AccessPoint with ID {@ID}.", id);
                throw ex;
            }
        }

        public async Task<bool> Exists(int id)
        {
            var filter = Builders<AccessPoint>.Filter.Eq(s => s.ID, id);
            try
            {
                var r = await _context.AccessPoints.FindAsync(filter);
                var accessPointsList = r.ToList();
                if (accessPointsList == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while querying existence of an AccessPoint with ID {@ID}.", id);
                throw ex;
            }
        }
    }
}
