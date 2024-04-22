using Web_server.Request;

namespace Web_server.Response
{
    public class DetailedVehicleInformation : BasicVehicleInformation
    {
        public Coordinates? Coordinates { get; set; }
        public DateTime? RecordingTime { get; set; }
        public Dictionary<string, Dictionary<string, Parameter>> Sections { get; set; }
    }
}
