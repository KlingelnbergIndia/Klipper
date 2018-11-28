using AttendanceDataExtractor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceDataExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            AccessDBReader.Read();

            //var accessPoints = JsonConvert.SerializeObject(AttendanceManager.AccessPoints, Formatting.Indented);
            //System.IO.File.WriteAllText(@"C:\Temp\Attendance\AccessPoints.txt", accessPoints);

            //var employees = JsonConvert.SerializeObject(AttendanceManager.Employees, Formatting.Indented);
            //System.IO.File.WriteAllText(@"C:\Temp\Attendance\Employees.txt", employees);

            var accessEvents = JsonConvert.SerializeObject(AttendanceManager.AccessEvents, Formatting.Indented);
            System.IO.File.WriteAllText(@"C:\Temp\Attendance\AccessEvents.txt", accessEvents);

            AttendanceManager.LoadToMongoDB();
            //AttendanceManager.LoadUsersToDB();
        }
    }
}
