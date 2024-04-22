using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Web_server.Request
{
    public class Report
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string VehicleId { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public Coordinates? Coordinates { get; set; }
        public DateTime? RecordingTime { get; set; }

    }
}
