using Microsoft.AspNetCore.Mvc;
using Web_server.Response;

namespace Web_server.Services.Interfaces
{
    public interface IAndroidAppService
    {
        public Task<List<BasicVehicleInformation>> GetVehicleList();
        public Task<DetailedVehicleInformation> GetDetailedVehicle(string id);
        public Task<DetailedVehicleInformation> GetDetailedVehicle(string id, DateTime dateTime);
        public Task<List<ReportResponse>> GetReports(string id);
        public Task<List<DateTime?>> GetRecordingTimeList(string id);


    }
}
