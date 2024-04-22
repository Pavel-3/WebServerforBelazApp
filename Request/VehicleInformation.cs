using Amazon.Runtime.Internal.Auth;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;
using System.Reflection.Metadata;

namespace Web_server.Request
{
    public class VehicleInformation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string VehicleId { get; set; }
        public string Model { get; set; }
        public string State { get; set; }
        public Coordinates? Coordinates { get; set; }
        public DateTime? RecordingTime { get; set; }
        public Dictionary<string, Dictionary<string, Parameter>> Sections { get; set; }
    }
    public class Coordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
    public class Parameter
    {
        public string Value { get; set; }
        public bool State { get; set; }
    }
}
