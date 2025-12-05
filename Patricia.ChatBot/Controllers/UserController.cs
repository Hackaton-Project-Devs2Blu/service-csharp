namespace Patricia.ChatBot.Controllers;

using Microsoft.AspNetCore.Mvc;
using Patricia.ChatBot.Dto;
using Patricia.ChatBot.Services;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly UserService _service;

    public UsersController(UserService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserRequestDto dto)
    {
        try
        {
            var result = await _service.Create(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto dto)
    {
        try
        {
            var result = await _service.Login(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var user = await _service.GetById(id);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var deleted = await _service.Delete(id);
        return deleted ? Ok() : NotFound();
    }
}
