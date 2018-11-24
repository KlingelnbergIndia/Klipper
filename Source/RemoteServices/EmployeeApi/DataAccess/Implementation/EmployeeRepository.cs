using EmployeeApi.DataAccess.Interfaces;
using Models.Core.Employment;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeApi.DataAccess.Implementation
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDBContext _context = null;
        readonly ILogger _logger = Log.ForContext<EmployeeRepository>();

        public EmployeeRepository()
        {
            _context = EmployeeDBContext.Instance;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            try
            {
                return await _context.Employees.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while accessing employee list.");
                throw ex;
            }
        }

        public async Task<Employee> Get(int id)
        {
            try
            {
                var filter = Builders<Employee>.Filter.Eq("ID", id);

                return await _context.Employees
                                .Find(filter)
                                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while accessing employee with ID {@ID}.", id);
                throw ex;
            }
        }

        public async Task<bool> Add(Employee item)
        {
            try
            {
                var filter = Builders<Employee>.Filter.Eq(x => x.ID, item.ID);
                var o = _context.Employees
                                .Find(filter)
                                .FirstOrDefaultAsync()
                                .Result;
                if (o == null)
                {
                    await _context.Employees.InsertOneAsync(item);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while adding employee with ID {@ID}.", item.ID);
                throw ex;
            }
        }

        public async Task<bool> RemoveAll()
        {
            try
            {
                DeleteResult actionResult = await _context.Employees.DeleteManyAsync(new BsonDocument());

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while removing employee list.");
                throw ex;
            }
        }

        public async Task<bool> Remove(int id)
        {
            try
            {
                DeleteResult actionResult = await _context.Employees.DeleteOneAsync(
                     Builders<Employee>.Filter.Eq("ID", id));

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while removing employee with ID {@ID}.", id);
                throw ex;
            }
        }

        public async Task<bool> UpdateEmployee_Title(string id, string title)
        {
            var eID = int.Parse(id);
            var filter = Builders<Employee>.Filter.Eq(s => s.ID, eID);
            var update = Builders<Employee>.Update
                            .Set(s => s.Title, title)
                            .CurrentDate(s => s.LastUpdatedOn);
            try
            {
                UpdateResult actionResult = await _context.Employees.UpdateOneAsync(filter, update);

                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while updating employee with ID {@ID}.", id);
                throw ex;
            }
        }

        public async Task<bool> UpdateEmployeeDocument(int id, string title)
        {
            var item = await Get(id) ?? new Employee();
            item.Title = title;
            item.LastUpdatedOn = DateTime.Now;

            return await Update(id, item);
        }

        public async Task<bool> Update(int id, Employee item)
        {
            try
            {
                item.ID = id; //Make sure ID of the item is assigned.
                ReplaceOneResult actionResult = await _context.Employees
                                                .ReplaceOneAsync(n => n.ID.Equals(id)
                                                                , item
                                                                , new UpdateOptions { IsUpsert = true });
                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while updating employee with ID {@ID}.", id);
                throw ex;
            }
        }

        public async Task<bool> Exists(int id)
        {
            var filter = Builders<Employee>.Filter.Eq("ID", id);
            try
            {
                var e = await _context.Employees
                .Find(filter)
                .FirstOrDefaultAsync();
                if(e == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while querying existence of an employee with ID {@ID}.", id);
                throw ex;
            }
        }
    }
}
