namespace GameStore.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MongoDB.Services.IServices;

    [ApiController]
    [Route("shippers")]
    [Authorize]
    public class ShippersController : ControllerBase
    {
        private readonly IShipperService _shipperService;

        public ShippersController(IShipperService shipperService)
        {
            _shipperService = shipperService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetShippers()
        {
            var shippers = await _shipperService.GetAllShippersAsync();
            return Ok(shippers);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetShipperById(int id)
        {
            var shipper = await _shipperService.GetShipperByIdAsync(id);
            return Ok(shipper);
        }
    }
}
