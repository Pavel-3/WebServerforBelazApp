namespace Web_server
{
    using Serilog;
    using MongoDB;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Web_server.Services;
    using Web_server.Services.Interfaces;
    using Web_server.Response;
    using Web_server.Request;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();
            builder.Host.UseSerilog();
            builder.Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConfiguration(builder.Configuration.GetSection("Logging"));
                loggingBuilder.AddSerilog(dispose: true);
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<IMongoClient, MongoClient>(s =>
            {
                var uri = s.GetRequiredService<IConfiguration>()["MongoDB:ConnectionString"];
                return new MongoClient(uri);
            });

            builder.Services.AddScoped(s =>
            {
                var client = s.GetRequiredService<IMongoClient>();
                var database = client.GetDatabase(s.GetRequiredService<IConfiguration>()["MongoDB:Database"]);
                return database;
            });


            builder.Services.AddScoped<IVehicleService, VehicleService>();
            builder.Services.AddScoped<IAndroidAppService, AndroidAppService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}