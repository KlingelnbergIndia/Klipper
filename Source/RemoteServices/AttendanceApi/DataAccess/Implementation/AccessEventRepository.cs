using AttendanceApi.DataAccess.Interfaces;
using Models.Core.HR.Attendance;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AttendanceApi.DataAccess.Implementation
{
    public class AccessEventRepository : IAccessEventRepository
    {
        private readonly AttendanceDBContext _context = null;
        readonly ILogger _logger = Log.ForContext<AccessEventRepository>();

        public AccessEventRepository()
        {
            _context = AttendanceDBContext.Instance;
        }

        public async Task<IEnumerable<AccessEvent>> GetAllAccessEvents()
        {
            try
            {
                return await _context.AccessEvents.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while accessing AccessEvent list.");
                throw ex;
            }
        }

        public async Task<AccessEvent> Get(int id)
        {
            try
            {
                var filter = Builders<AccessEvent>.Filter.Eq("ID", id);

                return await _context.AccessEvents
                                .Find(filter)
                                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while accessing AccessEvent with ID {@ID}.", id);
                throw ex;
            }
        }
        public async Task<IEnumerable<AccessEvent>> GetByEmployeeId(int employeeId)
        {
            try
            {
                var filter = Builders<AccessEvent>.Filter.Eq("EmployeeID", employeeId);

                return await _context.AccessEvents
                                .Find(filter)
                                .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while accessing AccessEvents with EmployeeID {@EmployeeID}.", employeeId);
                throw ex;
            }
        }

        public async Task<bool> Add(AccessEvent item)
        {
            try
            {
                var filter = Builders<AccessEvent>.Filter.Eq(x => x.ID, item.ID) &
                    Builders<AccessEvent>.Filter.Eq(x => x.EmployeeID, item.EmployeeID) &
                    Builders<AccessEvent>.Filter.Eq(x => x.AccessPointIPAddress, item.AccessPointIPAddress) &
                    Builders<AccessEvent>.Filter.Eq(x => x.AccessPointID, item.AccessPointID);
                var task = _context.AccessEvents
                                .Find(filter)
                                .FirstOrDefaultAsync();
                if(!task.IsCompleted)
                {
                    Thread.Sleep(5);
                }
                var o = task.Result;
                if (o == null)
                {
                    await _context.AccessEvents.InsertOneAsync(item);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while adding AccessEvent with ID {@ID}.", item.ID);
                throw ex;
            }
        }

        public async Task<bool> RemoveAll()
        {
            try
            {
                DeleteResult actionResult = await _context.AccessEvents.DeleteManyAsync(new BsonDocument());

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while removing AccessEvent list.");
                throw ex;
            }
        }

        public async Task<bool> Remove(int id)
        {
            try
            {
                DeleteResult actionResult = await _context.AccessEvents.DeleteOneAsync(
                     Builders<AccessEvent>.Filter.Eq("ID", id));

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while removing AccessEvent with ID {@ID}.", id);
                throw ex;
            }
        }

        public async Task<bool> Update(int id, AccessEvent item)
        {
            try
            {
                item.ID = id; //Make sure ID of the item is assigned.
                ReplaceOneResult actionResult = await _context.AccessEvents
                                                .ReplaceOneAsync(n => n.ID.Equals(id)
                                                                , item
                                                                , new UpdateOptions { IsUpsert = true });
                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while updating AccessEvent with ID {@ID}.", id);
                throw ex;
            }
        }

        public async Task<bool> Exists(int id)
        {
            var filter = Builders<AccessEvent>.Filter.Eq(s => s.ID, id);
            try
            {
                var r = await _context.AccessEvents.FindAsync(filter);
                var accessEventsList = r.ToList();
                if (accessEventsList == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while querying existence of an AccessEvent with ID {@ID}.", id);
                throw ex;
            }
        }
    }
}
