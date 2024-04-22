using MongoDB.Driver;
using MongoDB.Bson;
using Web_server.Services.Interfaces;
using Web_server.Request;

namespace Web_server.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IMongoCollection<VehicleInformation> _documents;
        private readonly IMongoCollection<ChangedParameter> _changedParameterCollection;
        private readonly IMongoCollection<Report> _reportCollection;
        public VehicleService(IMongoDatabase database) 
        {
            _documents = database.GetCollection<VehicleInformation>("BelazDB");
            _changedParameterCollection = database.GetCollection<ChangedParameter>("ChangedParameter");
            _reportCollection = database.GetCollection<Report>("Report");
        }
        public async Task<ObjectId> CreateVehicleInfoDoc(VehicleInformation document)
        {
            document.RecordingTime = DateTime.UtcNow;
            var bsonDocument = document.ToBsonDocument();
            var previousData = await _documents.Find(x => x.VehicleId == document.VehicleId)
                                                      .SortByDescending(x => x.RecordingTime)
                                                      .FirstOrDefaultAsync();

            if (previousData != null)
            {
                await CompareParameters(previousData, document);
            }
            try
            {
                await _documents.InsertOneAsync(document);

                return document.Id;
            }
            catch
            {
                return ObjectId.Empty;
            }
        }
        public async Task<ObjectId> CreateReport(Report report)
        {
            report.RecordingTime = DateTime.UtcNow;
            try
            {
                await _reportCollection.InsertOneAsync(report);
                return report.Id;
            }
            catch
            {
                return ObjectId.Empty;
            }
        }
        private async Task CompareParameters(VehicleInformation previousData, VehicleInformation currentData)
        {
            foreach (var section in currentData.Sections)
            {
                if (previousData.Sections.TryGetValue(section.Key, out var prevSection))
                {
                    foreach (var parameter in section.Value)
                    {
                        if (prevSection.TryGetValue(parameter.Key, out var prevParameter))
                        {
                            if (prevParameter.State != parameter.Value.State)
                            {
                                var changeParameter = new ChangedParameter
                                {
                                    VehicleId = currentData.VehicleId,
                                    ChangeTime = DateTime.UtcNow,
                                    SectionName = section.Key,
                                    ParameterName = parameter.Key,
                                    NewState = parameter.Value.State
                                };
                                await _changedParameterCollection.InsertOneAsync(changeParameter);
                            }
                        }
                        else
                        {
                            var changeParameter = new ChangedParameter
                            {
                                VehicleId = currentData.VehicleId,
                                ChangeTime = DateTime.UtcNow,
                                SectionName = section.Key,
                                ParameterName = parameter.Key,
                                NewState = parameter.Value.State
                            };
                            await _changedParameterCollection.InsertOneAsync(changeParameter);
                        }
                    }
                }
                else
                {
                    foreach (var parameter in section.Value)
                    {
                        var changeParameter = new ChangedParameter
                        {
                            VehicleId = currentData.VehicleId,
                            ChangeTime = DateTime.UtcNow,
                            SectionName = section.Key,
                            ParameterName = parameter.Key,
                            NewState = parameter.Value.State
                        };
                        await _changedParameterCollection.InsertOneAsync(changeParameter);
                    }
                }
            }
            foreach (var prevSection in previousData.Sections)
            {
                if (!currentData.Sections.TryGetValue(prevSection.Key, out var currSection))
                {
                    foreach (var prevParameter in prevSection.Value)
                    {
                        var changeParameter = new ChangedParameter
                        {
                            VehicleId = currentData.VehicleId,
                            ChangeTime = DateTime.UtcNow,
                            SectionName = prevSection.Key,
                            ParameterName = prevParameter.Key,
                            NewState = false
                        };
                        await _changedParameterCollection.InsertOneAsync(changeParameter);
                    }
                }
                else
                {
                    foreach (var prevParameter in prevSection.Value)
                    {
                        if (!currSection.TryGetValue(prevParameter.Key, out _))
                        {
                            var changeParameter = new ChangedParameter
                            {
                                VehicleId = currentData.VehicleId,
                                ChangeTime = DateTime.UtcNow,
                                SectionName = prevSection.Key,
                                ParameterName = prevParameter.Key,
                                NewState = false
                            };
                            await _changedParameterCollection.InsertOneAsync(changeParameter);
                        }
                    }
                }
            }
        }

    }

}
