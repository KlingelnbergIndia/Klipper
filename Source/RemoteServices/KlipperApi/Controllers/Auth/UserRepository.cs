using Models.Core.Authentication;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KlipperApi.Controllers.Auth
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthContext _context = null;
        readonly ILogger _logger = Log.ForContext<UserRepository>();

        public UserRepository()
        {
            _context = AuthContext.Instance;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            try
            {
                return await _context.Users.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while accessing User list.");
                throw ex;
            }
        }

        public async Task<User> Get(int id)
        {
            try
            {
                var filter = Builders<User>.Filter.Eq("ID", id);

                return await _context.Users
                                .Find(filter)
                                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while accessing User with ID {@ID}.", id);
                throw ex;
            }
        }

        public async Task<bool> Add(User item)
        {
            try
            {
                var filter = Builders<User>.Filter.Eq(x => x.ID, item.ID);
                var o = _context.Users
                                .Find(filter)
                                .FirstOrDefaultAsync()
                                .Result;
                if (o == null)
                {
                    await _context.Users.InsertOneAsync(item);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while adding User with ID {@ID}.", item.ID);
                throw ex;
            }
        }

        public async Task<bool> RemoveAll()
        {
            try
            {
                DeleteResult actionResult = await _context.Users.DeleteManyAsync(new BsonDocument());

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while removing User list.");
                throw ex;
            }
        }

        public async Task<bool> Remove(int id)
        {
            try
            {
                DeleteResult actionResult = await _context.Users.DeleteOneAsync(
                     Builders<User>.Filter.Eq("ID", id));

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while removing User with ID {@ID}.", id);
                throw ex;
            }
        }

        public async Task<bool> UpdateUserDocument(int id, string title)
        {
            var item = await Get(id) ?? new User();
            return await Update(id, item);
        }

        public async Task<bool> Update(int id, User item)
        {
            try
            {
                item.ID = id; //Make sure ID of the item is assigned.
                ReplaceOneResult actionResult = await _context.Users
                                                .ReplaceOneAsync(n => n.ID.Equals(id)
                                                                , item
                                                                , new UpdateOptions { IsUpsert = true });
                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while updating User with ID {@ID}.", id);
                throw ex;
            }
        }

        public async Task<bool> Exists(int id)
        {
            var filter = Builders<User>.Filter.Eq(s => s.ID, id);
            try
            {
                var r = await _context.Users.FindAsync(filter);
                if(r.Current.GetEnumerator().Current == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while querying existence of an User with ID {@ID}.", id);
                throw ex;
            }
        }

        public bool ValidateCredentials(string userName, string passwordHash)
        {
            var user = GetByUserName(userName).Result;
            if(user != null)
            {
                return user.PasswordHash.Equals(passwordHash);
            }

            return false;
        }

        public async Task<User> GetByUserName(string userName)
        {
            var filter = Builders<User>.Filter.Eq(s => s.UserName, userName);
            try
            {
                return await _context.Users
                                .Find(filter)
                                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.Warning("Something went wrong while querying existence of an User with username {@UserName}.", userName);
                throw ex;
            }
        }
    }
}
