using OperationalsApi.DataAccess.Interfaces;
using Models.Core.Operationals;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OperationalsApi.DataAccess.Implementation
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly DepartmentDBContext _context = null;

        public DepartmentRepository()
        {
            _context = DepartmentDBContext.Instance;
        }

        public async Task AddDepartment(Department item)
        {
            try
            {
                var filter = Builders<Department>.Filter.Eq(x => x.ID, item.ID);

                var o = _context.Departments
                                .Find(filter)
                                .FirstOrDefaultAsync()
                                .Result;

                if (o == null)
                {
                    await _context.Departments.InsertOneAsync(item);
                }
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<IEnumerable<Department>> GetAllDepartments()
        {
            try
            {
                return await _context.Departments.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<Department> GetDepartment(int id)
        {
            try
            {
                var filter = Builders<Department>.Filter.Eq("ID", id);

                return await _context.Departments
                                .Find(filter)
                                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<Department> GetDepartmentByName(string departmentName)
        {
            try
            {
                var filter = Builders<Department>.Filter.Eq("Name", departmentName);

                return await _context.Departments
                                .Find(filter)
                                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveAllDepartments()
        {
            try
            {
                DeleteResult actionResult = await _context.Departments.DeleteManyAsync(new BsonDocument());

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveDepartment(int id)
        {
            try
            {
                DeleteResult actionResult = await _context.Departments.DeleteOneAsync(
                     Builders<Department>.Filter.Eq("ID", id));

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> UpdateDepartment(int id, Department item)
        {
            try
            {
                ReplaceOneResult actionResult = await _context.Departments
                                                .ReplaceOneAsync(n => n.ID.Equals(id)
                                                                , item
                                                                , new UpdateOptions { IsUpsert = true });
                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }
    }
}
