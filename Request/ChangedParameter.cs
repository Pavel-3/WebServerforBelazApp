namespace Web_server.Request
{
    public class ChangedParameter
    {
        public string VehicleId { get; set; }
        public DateTime ChangeTime { get; set; }
        public Coordinates? Coordinates { get; set; }
        public string ParameterName { get; set; }
        public string SectionName { get; set; }
        public bool NewState { get; set; }
    }
}
