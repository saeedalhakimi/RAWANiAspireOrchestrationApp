using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RAWANiAspireOrchestrationApp.ApiService.Application.Contracts.UserProfileDtos.Requests;
using RAWANiAspireOrchestrationApp.ApiService.Application.MDItoRS.UserProfileMDi.Commands;
using RAWANiAspireOrchestrationApp.ApiService.Application.MDItoRS.UserProfileMDi.Queries;
using RAWANiAspireOrchestrationApp.ApiService.Presentation.Filters;

namespace RAWANiAspireOrchestrationApp.ApiService.Presentation.Controllers.V1.UserProfile
{
    [Route(ApiRoutes.BaseRoute)] // api/v1/UserProfiles
    [ApiVersion("1")]
    [ApiController]
    public class UserProfilesController : BaseController<UserProfilesController>
    {
        private readonly ILogger<UserProfilesController> _logger;
        private readonly MediatR.IMediator _mediator;
        public UserProfilesController(ILogger<UserProfilesController> logger, MediatR.IMediator mediator) : base(logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost(Name = "CreateUserProfile")] // api/v1/UserProfiles
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status499ClientClosedRequest)]
        [ValidateModel]
        public async Task<IActionResult> CreateUserProfile(
            [FromForm] CreateUserProfileDto createUserProfileDto,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Proccessing Create user profile request... ");
            var command = new CreateUserProfileCommand
            {
                Firstname = createUserProfileDto.Firstname,
                Lastname = createUserProfileDto.Lastname,
                Email = createUserProfileDto.Email,
                DateOfBirth = createUserProfileDto.DateOfBirth,
                PhoneNumber = createUserProfileDto.PhoneNumber,
                CurrentCity = createUserProfileDto.CurrentCity
            };

            _logger.LogInformation("Sending CreateUserProfileCommand to Mediator... ");
            var result = await _mediator.Send(command, cancellationToken);
            if (!result.IsSuccess) return HandleErrorResponse(result);

            _logger.LogInformation("Request Proccessed successfully... ");
            return CreatedAtRoute("CreateUserProfile", result);
        }

        [HttpGet(Name = "GetAllUserProfiles")] // api/v1/UserProfiles
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [NormalizePagination]
        public async Task<IActionResult> GetAllUserProfiles(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string sortColumn = "CreatedAt",
            [FromQuery] string sortDirection = "ASC",
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Proccessing Get all user profiles request... ");
            var command = new GetAllUserProfilesQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SortColumn = sortColumn,
                SortDirection = sortDirection
            };
            _logger.LogInformation("Sending GetAllUserProfilesQuery to Mediator... ");
            var result = await _mediator.Send(command, cancellationToken);
            if (!result.IsSuccess) return HandleErrorResponse(result);
            _logger.LogInformation("Request Proccessed successfully... ");
            return Ok(result);
        }

        [HttpGet(ApiRoutes.UserProfileRouts.IdRoute, Name = "GetUserProfileByUserProfileID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status499ClientClosedRequest)]
        [ValidateGuid("userProfileId")]
        public async Task<IActionResult> GetUserProfileByUserProfileID(
            [FromRoute] Guid userProfileId,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Proccessing Get user profile request... ");
            var command = new GetUserProfileByUserProfileIDQuery
            {
                UserProfileID = userProfileId
            };
            _logger.LogInformation("Sending GetUserProfileByUserProfileIDQuery to Mediator... ");
            var result = await _mediator.Send(command, cancellationToken);
            if (!result.IsSuccess) return HandleErrorResponse(result);
            _logger.LogInformation("Request Proccessed successfully... ");
            return Ok(result);
        }

    }
}
