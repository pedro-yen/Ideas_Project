using Backend.Challenge._2._Data.Models;
using Backend.Challenge.Data.Models;
using Backend.Challenge.Dtos;
using Raven.Client.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Challenge._2._Data.Repositories;

public class MainRepository : IRepository
{
    private readonly IDocumentStore _store;
    public MainRepository(IDocumentStore store)
    {
        _store = store;
    }

    #region Users
    public async Task<User> CreateUserAsync(User user)
    {
        using var session = _store.OpenAsyncSession();
        await session.StoreAsync(user);
        await session.SaveChangesAsync();
        return user;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        using var session = _store.OpenAsyncSession();
        var users = await session.Query<User>().ToListAsync();
        return users;
    }

    public async Task<List<User>> GetUsersByIdsAsync(string[] ids)
    {
        using var session = _store.OpenAsyncSession();
        var usersDictionary = await session.LoadAsync<User>(ids); // Users dictionary with nulls for missing IDs
        var users = usersDictionary.Values.Where(u => u != null).ToList(); // Filter out null values

        return users;
    }
    #endregion

    #region Ideas
    public async Task<Idea> CreateIdeaAsync(Idea idea)
    {
        using var session = _store.OpenAsyncSession();
        await session.StoreAsync(idea);
        await session.SaveChangesAsync();
        return idea;
    }

    public async Task<Idea> UpdateIdeaAsync(Idea idea)
    {
        using var session = _store.OpenAsyncSession();
        await session.StoreAsync(idea);
        await session.SaveChangesAsync();
        return idea;
    }

    public async Task<StatusUpdate> CreateStatusAsync(StatusUpdate statusUpdate)
    {
        using var session = _store.OpenAsyncSession();
        await session.StoreAsync(statusUpdate);
        await session.SaveChangesAsync();
        return statusUpdate;
    }

    public async Task<Idea> GetIdeaByIdeaIdAsync(string ideaId)
    {
        using var session = _store.OpenAsyncSession();
        var idea = await session.LoadAsync<Idea>(ideaId);
        return idea;
    }

    public async Task<StatusUpdate> GetIdeaUpdateByUpdateIdAsync(string updateId)
    {

        using var session = _store.OpenAsyncSession();
        var latestUpdate = await session.LoadAsync<StatusUpdate>(updateId);
        return latestUpdate;
    }

    public async Task<List<Idea>> GetIdeasByUserAsync(string userId, int pageNumber, int pageSize)
    {
        using var session = _store.OpenAsyncSession();
        return await session.Query<Idea>()
                            .Where(i => i.UserIds.Contains(userId))
                            .OrderByDescending(update => update.PublishedAtUtc)
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
    }

    public async Task<List<StatusUpdate>> GetUnseenStatusUpdatesForUserAsync(string userId, int pageNumber, int pageSize)
    {
        using var session = _store.OpenAsyncSession();

        var ideaIds = await session.Query<Idea>()
            .Where(idea => idea.UserIds.Contains(userId))
            .Select(idea => idea.Id)
            .ToListAsync();

        var unseenUpdates = new List<StatusUpdate>();

        // 2. For each idea, get the latest status update
        foreach (var ideaId in ideaIds)
        {
            var latestUpdate = await session.Query<StatusUpdate>()
                .Where(su => su.IdeaId == ideaId)
                .OrderByDescending(su => su.PublishedAtUtc)
                .FirstOrDefaultAsync();

            if (latestUpdate != null && !latestUpdate.SeenByUserIds.Contains(userId))
            {
                unseenUpdates.Add(latestUpdate);
            }
        }

        return unseenUpdates.OrderByDescending(update => update.PublishedAtUtc)
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize).ToList();
    }

    public async Task<List<StatusUpdate>> GetUpdatesForIdeaAsync(string ideaId, int pageNumber, int pageSize)
    {
        using var session = _store.OpenAsyncSession();

        var updates = await session.Query<StatusUpdate>()
            .Where(update => update.IdeaId == ideaId)
            .OrderByDescending(update => update.PublishedAtUtc)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize) 
            .ToListAsync();

        return updates;
    }
    #endregion
}