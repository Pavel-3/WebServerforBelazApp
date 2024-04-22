namespace Web_server.Services.Interfaces
{
    using MongoDB.Bson;
    using Web_server.Request;

    public interface IVehicleService
    {
        public Task<ObjectId> CreateVehicleInfoDoc(VehicleInformation document);
        public Task<ObjectId> CreateReport(Report report);

    }
}
