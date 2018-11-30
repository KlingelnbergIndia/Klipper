using Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Core.HR.Attendance
{
    public enum LeaveType
    {
        PersonalLeave,
        SickLeave,
        PaternalLeave,
        MaternalLeave,
        CompensatoryOff,
        EducationalLeave,
        Sabattical,
        UnpaidLeave
    }

    public enum LeaveStatus
    {
        Applied,
        Approved,
        Rejected
    }

    public class Leave
    {
        [BsonId]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId _objectId { get; set; }

        public int ID { get; set; }

        [BsonDateTimeOptions]
        public DateTime StartDate { get; set; }

        [BsonDateTimeOptions]
        public DateTime EndDate { get; set; }

        public int EmployeeID { get; set; }

        public LeaveType LeaveType { get; set; }

        public bool IsHalfDay { get; set; }

        public LeaveStatus LeaveStatus { get; set; }

        public string Description { get; set; }

        public int ApprovingAuthorityID { get; set; }

        public int NoOfDays { get; set; }
        
        [BsonDateTimeOptions]
        public DateTime CreationDate { get; set; }
    }
}
