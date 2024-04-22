namespace Web_server.Services
{
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Web_server.Request;
    using Web_server.Response;
    using Web_server.Services.Interfaces;
    public class AndroidAppService : IAndroidAppService
    {
        private readonly IMongoCollection<VehicleInformation> _documents;
        private readonly IMongoCollection<ChangedParameter> _changedParameterCollection;
        private readonly IMongoCollection<Report> _reportCollection;
        public AndroidAppService(IMongoDatabase database)
        {
            _documents = database.GetCollection<VehicleInformation>("BelazDB");
            _changedParameterCollection = database.GetCollection<ChangedParameter>("ChangedParameter");
            _reportCollection = database.GetCollection<Report>("Report");
        }

        public async Task<DetailedVehicleInformation?> GetDetailedVehicle(string id)
        {
            var filter = Builders<VehicleInformation>.Filter.Eq(x => x.VehicleId, id);

            var detailedVehicleInfo = await _documents.Find(filter).FirstOrDefaultAsync();

            if (detailedVehicleInfo != null)
            {
                var result = new DetailedVehicleInformation
                {
                    VehicleId = detailedVehicleInfo.VehicleId,
                    Model = detailedVehicleInfo.Model,
                    State = detailedVehicleInfo.State,
                    Coordinates = detailedVehicleInfo.Coordinates,
                    RecordingTime = detailedVehicleInfo.RecordingTime,
                    Sections = detailedVehicleInfo.Sections
                };

                return result;
            }
            else
            {
                return null;
            }
        }
        public async Task<DetailedVehicleInformation> GetDetailedVehicle(string id, DateTime dateTime)
        {
            var builder = Builders<VehicleInformation>.Filter;
            var filter = builder.Or(builder.Eq(x => x.VehicleId, id), builder.Eq(x => x.RecordingTime, dateTime));

            var detailedVehicleInfo = await _documents.Find(filter).FirstOrDefaultAsync();

            if (detailedVehicleInfo != null)
            {
                var result = new DetailedVehicleInformation
                {
                    VehicleId = detailedVehicleInfo.VehicleId,
                    Model = detailedVehicleInfo.Model,
                    State = detailedVehicleInfo.State,
                    Coordinates = detailedVehicleInfo.Coordinates,
                    RecordingTime = detailedVehicleInfo.RecordingTime,
                    Sections = detailedVehicleInfo.Sections
                };

                return result;
            }
            else
            {
                return null;
            }
        }
        public async Task<List<DateTime?>> GetRecordingTimeList(string id)
        {
            var filter = Builders<VehicleInformation>.Filter.Eq("VehicleId", id);
            var vehicles = await _documents.Find(filter).ToListAsync();
            var dateTimeList = vehicles.Select(x => x.RecordingTime).ToList();
            return dateTimeList;
        }

        public async Task<List<ReportResponse>> GetReports(string id)
        {
            var filter = Builders<Report>.Filter.Eq("VehicleId", id);
            var reports = await _reportCollection.Find(filter).ToListAsync();
            var reportResponse = reports.Select(x => new ReportResponse()
            {
                VehicleId = id,
                Model = x.Model,
                Description = x.Description,
                Coordinates = x.Coordinates,
                RecordingTime = x.RecordingTime
            }).ToList();
            return reportResponse;
        }

        public async Task<List<BasicVehicleInformation>> GetVehicleList()
        {
            var filter = Builders<VehicleInformation>.Filter.Empty;

            var projection = Builders<VehicleInformation>.Projection
                .Include(x => x.VehicleId)
                .Include(x => x.Model)
                .Include(x => x.State)
                .Exclude(x => x.Id);
            var cursor = await _documents.Find(filter).Project<BasicVehicleInformation>(projection).ToCursorAsync();
            var basicVehicleInfoList = await cursor.ToListAsync();

            return basicVehicleInfoList;
        }
    }
}
