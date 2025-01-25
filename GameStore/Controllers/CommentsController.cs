namespace GameStore.Controllers
{
    using GameStore.BusinessLayer.AuthServices;
    using GameStore.BusinessLayer.Interfaces.RequestDto;
    using GameStore.BusinessLayer.Interfaces.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("comments")]
    public class CommentsController : ControllerBase
    {
        private readonly IBanService _banService;

        public CommentsController(IBanService banService)
        {
            _banService = banService;
        }

        [AllowAnonymous]
        [HttpGet("ban/durations")]
        public IActionResult GetBanDurations()
        {
            var durations = _banService.GetBanDurations();
            return Ok(durations);
        }

        [PermissionName("BanUsersFromCommenting")]
        [HttpPost("ban")]
        public IActionResult BanUser([FromBody] BanUserRequest request)
        {
                _banService.BanUser(request.User, request.Duration);
                return Ok($"User '{request.User}' has been banned for {request.Duration}.");
        }
    }
}
