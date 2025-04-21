using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Challenge._1._Common.Contracts.Requests.Idea;
using Backend.Challenge._1._Common.Contracts.Requests.Users;
using Backend.Challenge.BusinessManager;
using Backend.Challenge.Data.Models;
using Backend.Challenge.Dtos;
using Backend.Challenge.ServiceModels;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;

namespace Backend.Challenge.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : Controller
{
    private readonly IUsersBusinessManager _usersBusinessManager;

    public UsersController(IUsersBusinessManager usersBusinessManager)
    {
        _usersBusinessManager = usersBusinessManager;
    }

    #region Users
    /// <summary>
    /// Create new User
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("users")]
    public async Task<IActionResult> CreateNewUser(CreateUserRequest request)
    {
        return Ok(await _usersBusinessManager.CreateNewUser(request));
    }

    /// <summary>
    /// Gets Users by Id -- if Empty returns all users
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers([FromQuery] string[] ids)
    {
        try
        {
            var response = await _usersBusinessManager.GetUsersAsync(ids);
            return Ok(response);
        }

        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
    #endregion

    
}