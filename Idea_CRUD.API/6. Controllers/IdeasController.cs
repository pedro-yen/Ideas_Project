using Backend.Challenge._1._Common.Contracts.Requests.Idea;
using Backend.Challenge.BusinessManager;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Backend.Challenge.Data.Models;

namespace Backend.Challenge._6._Controllers;

[ApiController]
[Route("api/[controller]")]
public class IdeasController : Controller
{
    private readonly IIdeasBusinessManager _ideasBusinessManager;

    public IdeasController(IIdeasBusinessManager ideasBusinessManager)
    {
        _ideasBusinessManager = ideasBusinessManager;
    }

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
    /// <param name="ideaId"></param>
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

    /// <summary>
    /// Update an existing idea
    /// </summary>
    /// <param name="ideaId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("ideas/{ideaId}/update")]
    public async Task<IActionResult> CreateUpdate(string ideaId, [FromBody] CreateStatusUpdateRequest request)
    {
        try
        {
            var decodedId = Uri.UnescapeDataString(ideaId);
            var update = await _ideasBusinessManager.CreateUpdateAsync(decodedId, request);
            return Ok(update);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("ideas/update/{updateId}/seen")]
    public async Task<IActionResult> MarkAsSeen(string updateId, [FromBody] SeenRequest request)
    {
        try
        {
            var decodedId = Uri.UnescapeDataString(updateId);
            await _ideasBusinessManager.MarkUpdateAsSeenAsync(decodedId, request.UserId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // TODO: MARK SEEN
    #endregion
    // TODO: An action to return a paged list of status updates

    // TODO: An action to add a status update
}
