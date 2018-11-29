using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common
{
    public class LoggingConfiguration
    {
        public string Path { get; set; }
    }

    public static class LoggingConfigurator
    {
        private static Dictionary<string, LoggingConfiguration> _dictionary = new Dictionary<string, LoggingConfiguration>()
        {
            {
                "AttendanceApi",
                new LoggingConfiguration()
                {
                    Path = "C:/Temp/ApiLogs/"
                }
            },
            {
                "EmployeeApi",
                new LoggingConfiguration()
                {
                    Path = "C:/Temp/ApiLogs/"
                }
            },
            {
                "KlipperApi",
                new LoggingConfiguration()
                {
                    Path = "C:/Temp/ApiLogs/"
                }
            },
            {
                "OperationalsApi",
                new LoggingConfiguration()
                {
                    Path = "C:/Temp/ApiLogs/"
                }
            },
        };

        static public LoggingConfiguration GetConfiguration(string serviceName)
        {
            if (_dictionary.Count == 0 || !_dictionary.Keys.Contains(serviceName))
            {
                throw new Exception("Logging configurator failed to obtain sink path!");
            }
            return _dictionary[serviceName];
        }
    }
}
