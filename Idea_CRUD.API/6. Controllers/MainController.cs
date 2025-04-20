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
public class MainController : Controller
{
    private readonly IDocumentStore _store;
    private readonly IUsersBusinessManager _usersBusinessManager;
    private readonly IIdeasBusinessManager _ideasBusinessManager;

    public MainController(IDocumentStore store, IUsersBusinessManager usersBusinessManager,IIdeasBusinessManager ideasBusinessManager)
    {
        _store = store;
        _usersBusinessManager = usersBusinessManager;
        _ideasBusinessManager = ideasBusinessManager;
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

    #region Idea
    /// <summary>
    /// Create Idea
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("idea")]
    public async Task<IActionResult> CreateIdea(CreateIdeiaRequest request)
    {

        var idea = await _ideasBusinessManager.CreateIdeaAsync(request);

        return Ok(idea);
    }

    /// <summary>
    /// Gets all Ideas for User
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("users/{userId}/ideas")]
    public async Task<IActionResult> GetIdeasForUser(string userId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var decodedId = Uri.UnescapeDataString(userId);
        var ideas = await _ideasBusinessManager.GetIdeasForUserAsync(decodedId, pageNumber, pageSize);
        return Ok(ideas);
    }


    #endregion

    #region Update
    /// <summary>
    /// Gets all unseen updates for a userId
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("users/{userId}/unseen-updates")]
    public async Task<IActionResult> GetUnseenUpdates(string userId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var decodedUserId = Uri.UnescapeDataString(userId);
        var unseenUpdates = await _ideasBusinessManager.GetUnseenUpdatesForUserAsync(decodedUserId, pageNumber, pageSize);
        return Ok(unseenUpdates);
    }

    /// <summary>
    /// Gets All updates for an idea
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("ideas/{ideaId}")]
    public async Task<IActionResult> GetUpdatesForIdea(string ideaId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var decodedId = Uri.UnescapeDataString(ideaId);
        var updates = await _ideasBusinessManager.GetUpdatesForIdeaAsync(decodedId, pageNumber, pageSize);

        if (updates == null)
            return NotFound();

        return Ok(updates);
    }


    [HttpPost("ideas/update/{ideaId}")]
    public async Task<IActionResult> CreateUpdate(string ideaId, [FromBody] CreateStatusUpdateRequest request)
    {
        try
        {
            var update = await _ideasBusinessManager.CreateUpdateAsync(ideaId, request);
            return Ok(update);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // TODO: MARK SEEN
    #endregion
    // TODO: An action to return a paged list of status updates

    // TODO: An action to add a status update
}