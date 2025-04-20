using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    [HttpPost("users")]
    public async Task<IActionResult> CreateNewUser(CreateUserRequest request)
    {
        return Ok(await _usersBusinessManager.CreateNewUser(request));
    }

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
            return NotFound(ex.Message); // Return 404 if no users are found
        }
    }
    #endregion

    #region Idea
    [HttpPost("idea")]
    public async Task<IActionResult> CreateIdea(Idea idea)
    {
        using var session = _store.OpenAsyncSession();

        idea.CreatedAtUtc = DateTime.UtcNow;

        await session.StoreAsync(idea);
        await session.SaveChangesAsync();

        return Ok(idea); // Now you can use idea.Id when creating StatusUpdates
    }

    [HttpGet("idea/{id}")]
    public async Task<IActionResult> GetIdea(string id)
    {
        using var session = _store.OpenAsyncSession();

        var idea = await session.LoadAsync<Idea>(id);

        if (idea == null)
            return NotFound();

        return Ok(idea);
    }
    #endregion

    #region StatusUpdate
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        using var session = _store.OpenAsyncSession();

        var update = await session.LoadAsync<StatusUpdate>(id);

        if (update == null)
            return NotFound();

        return Ok(update);
    }

    [HttpPost]
    public async Task<IActionResult> Create(StatusUpdate update)
    {
        using var session = _store.OpenAsyncSession();

        // 🛡️ 1. Check if the Idea exists
        var idea = await session.LoadAsync<Idea>(update.IdeaId);
        if (idea == null)
        {
            return BadRequest($"Idea with ID '{update.IdeaId}' does not exist.");
        }

        // 2. Set timestamps and store
        update.PublishedAtUtc = DateTime.UtcNow;
        await session.StoreAsync(update);
        await session.SaveChangesAsync();

        return Ok(update);
    }
    #endregion
    // TODO: An action to return a paged list of status updates

    // TODO: An action to add a status update
}