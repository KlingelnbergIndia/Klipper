using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.Core.Authentication
{
    public class UserRole
    {
        [BsonId]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId _objectId { get; set; }

        public int ID { get; set; }

        [Required]
        [StringLength(25)]
        [Display(Name = "User Role")]
        public string Role { get; set; }
    }
}
