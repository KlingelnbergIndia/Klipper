using Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;

namespace Models.Core.HR.Attendance
{
    public enum RegularizationReasonType
    {
        OutstationDuty,
        CustomerVisit,
        VendorVisit,
        Conference,
        Exhibition,
        WorkFromOtherLocation,
        OfficialWorkOutside,
    }

    public enum RegularizationType
    {
        ManualTimeEventEntry,
        BlockTimeSpanEntry
    }

    public enum RegularizationStatus
    {
        Applied,
        Approved,
        Rejected
    }

    public class Regularization
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

        public RegularizationReasonType ReasonType { get; set; } = RegularizationReasonType.CustomerVisit;

        public RegularizationType RegularizationType { get; set; } = RegularizationType.ManualTimeEventEntry;

        public AccessEvent ManualEventEntry { get; set; } = null;

        public TimeSpan BlockTimeSpanEntry { get; set; } = TimeSpan.FromHours(0);

        public RegularizationStatus Status { get; set; }

        public string Description { get; set; }
    }
}
