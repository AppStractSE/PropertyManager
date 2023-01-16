using Domain.Features.Queries.Users;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Domain.Domain;
using Api.Dto.Request.User.v1;
using Domain.Features.Commands.User;
using Api.Dto.Response.User.v1;

namespace Api.Controllers.v1;

[ApiController]
[Route("/api/v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<UserController> _logger;

    public UserController(IMediator mediator, IMapper mapper, ILogger<UserController> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IList<User>>> GetAllUsers()
    {
        try
        {
            var result = await _mediator.Send(new GetAllUsersQuery());
            return Ok(result);
        }
        catch (Exception ex)
        {
             _logger.LogError(message: "Error in User controller: GetAllUsers");
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("GetUserById/")]
    public async Task<ActionResult<UserResponseDto>> GetUserById([FromQuery]GetUserByIdRequestDto request)
    {
        try
        {
            var result = await _mediator.Send(_mapper.Map<GetUserByIdRequestDto, GetUserByIdQuery>(request));
            return result != null ? Ok(_mapper.Map<UserResponseDto>(result)) : NoContent();
        }
        catch (Exception ex)
        {
             _logger.LogError(message: "Error in User controller: GetUserById");
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<User>> PostUserAsync(PostUserRequestDto request)
    {
        try
        {
            var result = await _mediator.Send(_mapper.Map<PostUserRequestDto, AddUserCommand>(request));
            return Ok(result); 
        }
        catch (Exception ex)
        { 
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    public async Task<ActionResult<User>> PutUserAsync(PutUserRequestDto request)
    {
        try
        {
            var result = await _mediator.Send(_mapper.Map<PutUserRequestDto, UpdateUserCommand>(request));
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}