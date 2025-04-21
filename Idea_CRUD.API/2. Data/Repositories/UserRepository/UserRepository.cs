using Backend.Challenge._2._Data.Models;
using Backend.Challenge.Data.Models;
using Backend.Challenge.Dtos;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Challenge._2._Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDocumentStore _store;
    public UserRepository(IDocumentStore store)
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

        var users = await session.LoadAsync<User>(ids);

        // returns only the users that have valid id no null reference
        return users.Values
                    .Where(user => user != null)
                    .ToList();

    }
    #endregion

  
}