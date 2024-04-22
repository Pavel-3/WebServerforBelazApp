using Amazon.Util;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Web_server.Services;
using Web_server.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web_server.Controllers
{
    [ApiController]
    public class AndroidAppController : ControllerBase
    {
        private readonly IAndroidAppService _androidAppService;
        public AndroidAppController(IAndroidAppService androidAppService)
        {
            _androidAppService = androidAppService;
        }
        [HttpGet]
        [Route("api/GetAllVehicle")]
        public async Task<IActionResult> GetAllVehicle()
        {
            try
            {
                var vehicles = await _androidAppService.GetVehicleList();
                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("api/GetDetailedVehicle")]
        public async Task<IActionResult> GetDetailedVehicle(string id)
        {
            try
            {
                var vehicle = await _androidAppService.GetDetailedVehicle(id);
                return Ok(vehicle);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("api/GetDetailedVehicleWithTime")]
        public async Task<IActionResult> GetDetailedVehicleWithTime(string id, DateTime dateTime)
        {
            try
            {
                var vehicle = await _androidAppService.GetDetailedVehicle(id, dateTime);
                return Ok(vehicle);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("api/GetReports")]
        public async Task<IActionResult> GetReports(string id)
        {
            try
            {
                var reports = await _androidAppService.GetReports(id);
                return Ok(reports);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("api/GetRecordingTime")]
        public async Task<IActionResult> GetRecordingTimeList(string id)
        {
            try
            {
                var recordedTime = await _androidAppService.GetRecordingTimeList(id);
                return Ok(recordedTime);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
