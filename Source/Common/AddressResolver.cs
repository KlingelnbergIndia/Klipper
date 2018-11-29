using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common
{
    public class ServiceAddress
    {
        public string Address { get; set; }
        public string Port { get; set; }
        public string PortHttps { get; set; }
    }

    public static class AddressResolver
    {
        private static Dictionary<string, ServiceAddress> _dictionary = new Dictionary<string, ServiceAddress>()
        {
            {
                "AttendanceApi",
                new ServiceAddress()
                {
                    Address = "localhost",
                    Port = "5000",
                    PortHttps = "5001"
                }
            },
            {
                "EmployeeApi",
                new ServiceAddress()
                {
                    Address = "localhost",
                    Port = "6000",
                    PortHttps = "6001"
                }
            },
            {
                "OperationalsApi",
                new ServiceAddress()
                {
                    Address = "localhost",
                    Port = "4000",
                    PortHttps = "4001"
                }
            },
            {
                "KlipperApi",
                new ServiceAddress()
                {
                    Address = "localhost",
                    Port = "7000",
                    PortHttps = "7001"
                }
            },
            {
                "AuthServer",
                new ServiceAddress()
                {
                    Address = "localhost",
                    Port = "49333",
                    PortHttps = "49334"
                }
            }
        };

        public static bool UseHttps { get; set; } = false;

        static public string GetAddress(string serviceName)
        {
            if (_dictionary.Count == 0 || !_dictionary.Keys.Contains(serviceName))
            {
                throw new Exception("Address resolver failed to obtain addresses!");
            }

            var sa = _dictionary[serviceName];

            var address = (UseHttps ? "https://" : "http://") + sa.Address + ":" + (UseHttps ? sa.PortHttps : sa.Port) + "/";
            return address;
        }
        static public string GetAddress(string serviceName, bool useHttps)
        {
            if (_dictionary.Count == 0 || !_dictionary.Keys.Contains(serviceName))
            {
                throw new Exception("Address resolver failed to obtain addresses!");
            }

            var sa = _dictionary[serviceName];

            var address = (useHttps ? "https://" : "http://") + sa.Address + ":" + (useHttps ? sa.PortHttps : sa.Port) + "/";
            return address;
        }
    }
}
