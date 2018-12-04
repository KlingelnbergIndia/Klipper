using System;
using System.Collections.Generic;
using System.Text;
using Models.Core.Helpers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Models.Core.HR.Attendance
{
    public class LeaveBalance
    {
        [BsonId]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId _objectId { get; set; }

        public int EmployeeID { get; set; }

        public int Personal { get; set; }

        public int Sick { get; set; }
    }
}

