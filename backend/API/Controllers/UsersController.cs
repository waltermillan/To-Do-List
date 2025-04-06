using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Helpers;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;

namespace API.Controllers;

public class UsersController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILoggingService _loggingService;

    public UsersController(IUnitOfWork unitOfWork, IMapper mapper, ILoggingService loggingService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _loggingService = loggingService;
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Auth(string usr, string psw)
    {
        try
        {
            var user = await _unitOfWork.Users.GetByUsrAsync(usr);

            if (user is null)
            {
                Log.Logger.Information($"Login attempt failed for the user: {user}");
                return Unauthorized(new { Code = 1, Message = "Invalid username or password" });
            }

            // Verify if the provided password matches the stored hash
            var isAuthenticated = PasswordHasher.VerifyPassword(psw, user.Password);

            if (!isAuthenticated)
            {
                Log.Logger.Information($"Login attempt failed for the user: {user}");
                return Unauthorized(new { Code = 1, Message = "Invalid username or password" });
            }

            Log.Logger.Information($"User '{usr}' authenticated successfully.");
            return Ok(new { Code = 0, Message = "User authenticated successfully" });

        }
        catch (Exception ex)
        {
            var message = "There was an issue authenticating the user. Please try again later.";
            Log.Logger.Error(message, ex);

            return StatusCode(500, new { Message = message, Details = ex.Message });
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<User>>> Get()
    {
        var users = await _unitOfWork.Users.GetAllAsync();
        return _mapper.Map<List<User>>(users);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<User>> Get(int id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);

        if (user is null)
            return NotFound();

        return _mapper.Map<User>(user);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<User>> Post(User oUser)
    {
        var msg = string.Empty;
        try
        {
            var user = _mapper.Map<User>(oUser);

            user.Password = PasswordHasher.HashPassword(user.Password); // Data encrypted!

            _unitOfWork.Users.Add(user);
            await _unitOfWork.SaveAsync();

            if (user is null)
                return BadRequest();

            oUser.Id = user.Id;

            object[] obj = new object[] { user.Id, user.Name };
            msg = $"User {user} added successfully.";
            Log.Logger.Information(msg);

            return CreatedAtAction(nameof(Post), new { id = oUser.Id }, oUser);
        }
        catch (Exception exception)
        {
            msg = $"Error adding User {User}.";
            Log.Logger.Error(msg, exception);
            return BadRequest(msg);
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<User>> Put([FromBody] User oUser)
    {
        var msg = string.Empty;
        try
        {
            if (oUser is null)
                return NotFound();

            var user = _mapper.Map<User>(oUser);
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveAsync();

            object[] obj = new object[] { user.Id, user.Name };
            msg = $"User {user} updated successfully.";

            Log.Logger.Information(msg);

            return oUser;
        }
        catch (Exception exception)
        {
            msg = $"Error updating User {User}.";
            Log.Logger.Error(msg, exception);
            return BadRequest(msg);
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var msg = string.Empty;
        try
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);

            if (user is null)
                return NotFound();

            _unitOfWork.Users.Remove(user);
            await _unitOfWork.SaveAsync();

            object[] obj = new object[] { user.Id, user.Name };
            msg = $"User {user} deleted successfully.";
            Log.Logger.Information(msg);

            return NoContent();
        }
        catch (Exception exception)
        {
            msg = $"Error deleting User {User}.";
            Log.Logger.Error(msg, exception);
            return BadRequest(msg);
        }
    }
}