using Web_server.Request;

namespace Web_server.Response
{
    public class ReportResponse
    {
        public string VehicleId { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public Coordinates? Coordinates { get; set; }
        public DateTime? RecordingTime { get; set; }
    }
}
