namespace GameStore.Controllers
{
    using GameStore.BusinessLayer.AuthServices;
    using GameStore.BusinessLayer.Interfaces.IFacade;
    using GameStore.BusinessLayer.Interfaces.RequestDto;
    using GameStore.BusinessLayer.Interfaces.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("publishers")]
    [Authorize]
    public class PublisherController : ControllerBase
    {
        private readonly IPublisherService _publisherService;
        private readonly IPublisherFacade _publisherFacade;

        public PublisherController(IPublisherService publisherService, IPublisherFacade publishingFacade)
        {
            _publisherService = publisherService;
            _publisherFacade = publishingFacade;
        }

        [PermissionName("AddPublisher")]
        [HttpPost]
        public async Task<IActionResult> AddPublisher([FromBody] PublisherRequest publisherDto)
        {
            var addedPublisher = await _publisherService.AddPublisherAsync(publisherDto);
            return Ok($"Publisher with ID {addedPublisher.Id} added successfully.");
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPublisherById(string id)
        {
            var response = await _publisherFacade.GetPublisherByIdAsync(id);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("company/{companyName}")]
        public async Task<IActionResult> GetPublisherByName(string companyName)
        {
            var publisher = await _publisherFacade.GetPublisherByNameAsync(companyName);
            return Ok(publisher);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllPublishers()
        {
            var publishers = await _publisherFacade.GetAllPublishersAsync();
            return Ok(publishers);
        }

        [PermissionName("UpdatePublisher")]
        [HttpPut]
        public async Task<IActionResult> UpdatePublisher([FromBody] UpdatePublisherRequest request)
        {

            var updatedPublisher = await _publisherService.UpdatePublisherAsync(request);
            return Ok($"Publisher with ID {updatedPublisher.Id} updated successfully");
        }

        [PermissionName("DeletePublisher")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublisher(Guid id)
        {
            await _publisherService.DeletePublisherAsync(id);
            return Ok("Publisher deleted successfully.");
        }

        [AllowAnonymous]
        [HttpGet("{companyName}/games")]
        public async Task<IActionResult> GetGamesByPublisherName(string companyName)
        {

            var games = await _publisherService.GetGamesByPublisherNameAsync(companyName);
            return Ok(games);
        }
    }
}
