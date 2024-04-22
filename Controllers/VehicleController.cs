using Microsoft.AspNetCore.Mvc;
using Serilog;
using Web_server.Request;
using Web_server.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web_server.Controllers
{

    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }
        [HttpPost]
        [Route("api/CreateVehicle")]
        public async Task<IActionResult> CreateVehicle(VehicleInformation vehicleInformationDocument)
        {
            try
            {
                var id = await _vehicleService.CreateVehicleInfoDoc(vehicleInformationDocument);
                return Ok(id.ToString());
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("api/CreateReport")]
        public async Task<IActionResult> CreateReport(Report vehicleReport)
        {
            try
            {
                var id = await _vehicleService.CreateReport(vehicleReport);
                return Ok(id.ToString());
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

    }
}
