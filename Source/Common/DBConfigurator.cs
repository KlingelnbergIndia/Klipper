using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common
{
    public class DBConfiguration
    {
        public string Address { get; set; }
        public string Port { get; set; }
        public string DBUser { get; set; }
        public string Password { get; set; }
    }

    public static class DBConfigurator
    {
        private static Dictionary<string, DBConfiguration> _dictionary = new Dictionary<string, DBConfiguration>()
        {
            {
                "AttendanceDB",
                new DBConfiguration()
                {
                    Address = "localhost",
                    Port = "27017",
                    DBUser = "ApiUser",
                    Password = "Attendance#98765"
                }
            },
            {
                "AuthDB",
                new DBConfiguration()
                {
                    Address = "localhost",
                    Port = "27017",
                    DBUser = "ApiUser",
                    Password = "Auth#98765"
                }
            },
            {
                "EmployeeDB",
                new DBConfiguration()
                {
                    Address = "localhost",
                    Port = "27017",
                    DBUser = "ApiUser",
                    Password = "Employee#98765"
                }
            },
            {
                "OperationalsDB",
                new DBConfiguration()
                {
                    Address = "localhost",
                    Port = "27017",
                    DBUser = "ApiUser",
                    Password = "Operationals#98765"
                }
            },
            {
                "EmployeeXDB",
                new DBConfiguration()
                {
                    Address = "localhost",
                    Port = "27017",
                    DBUser = "ApiUser",
                    Password = "EmployeeX#98765"
                }
            },
        };

        static public string GetConnectionString(string databaseName)
        {
            if (_dictionary.Count == 0 || !_dictionary.Keys.Contains(databaseName))
            {
                throw new Exception("Database configurator failed to obtain connection string!");
            }
            var config = _dictionary[databaseName];
            var connectionString = "mongodb://" + config.DBUser + ":" + config.Password + "@" + config.Address + "/" + databaseName;
            return connectionString;
        }
    }
}
