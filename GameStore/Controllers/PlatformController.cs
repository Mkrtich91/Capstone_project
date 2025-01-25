namespace GameStore.Controllers
{
    using System;
    using GameStore.BusinessLayer.AuthServices;
    using GameStore.BusinessLayer.Interfaces.DTO;
    using GameStore.BusinessLayer.Interfaces.RequestDto;
    using GameStore.BusinessLayer.Interfaces.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("platforms")]
    public class PlatformController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly IPlatformService _platformService;

        public PlatformController(IGameService gameService, IPlatformService platformService)
        {
            _gameService = gameService;
            _platformService = platformService;

        }

        [AllowAnonymous]
        [HttpGet("{id}/games")]
        public async Task<IActionResult> GetGamesByPlatform(Guid id)
        {
            var games = await _gameService.GetGamesByPlatformIdAsync(id);
            return Ok(games);
        }

        [PermissionName("AddPlatform")]
        [HttpPost]
        public async Task<IActionResult> CreatePlatform([FromBody] CreatePlatformRequest request)
        {

           var platform = await _platformService.CreatePlatformAsync(request);

           return Ok($"Platform with ID '{platform.Id}' created successfully.");
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlatformByIdAsync(Guid id)
        {
            var platformDto = await _platformService.GetPlatformByIdAsync(id);

            return Ok(platformDto);

        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllPlatformsAsync()
        {
            var platformDtos = await _platformService.GetAllPlatformsAsync();

            return Ok(platformDtos);
        }

        [PermissionName("UpdatePlatform")]
        [HttpPut]
        public async Task<IActionResult> UpdatePlatform(UpdatePlatformDto request)
        {
                var platform = await _platformService.UpdatePlatformAsync(request);

                return Ok($"Platform with ID {platform.Id} updated successfully");
        }

        [PermissionName("DeletePlatform")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlatform(Guid id)
        {
            await _platformService.DeletePlatformAsync(id);
            return Ok("Platform deleted successfully");
        }
    }
}
