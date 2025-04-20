using Backend.Challenge._1._Common.Contracts.Requests.Users;
using Backend.Challenge.Data.Models;
using Backend.Challenge.Dtos;
using Backend.Challenge.ServiceModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Challenge._2._Data.Repositories;

public interface IRepository
{
    #region Users
    Task<User> CreateUserAsync(User user);
    Task<List<User>> GetAllUsersAsync();
    Task<List<User>> GetUsersByIdsAsync(string[] ids);
    #endregion

    #region Idea
    Task<Idea> CreateIdeaAsync(Idea idea);

    Task<Idea> UpdateIdeaAsync(Idea idea);

    Task<StatusUpdate> CreateStatusAsync(StatusUpdate idea);

    Task<Idea> GetIdeaByIdeaIdAsync(string ideaId);

    Task<StatusUpdate> GetIdeaUpdateByUpdateIdAsync(string updateId);

    Task<List<Idea>> GetIdeasByUserAsync(string userId, int pageNumber, int pageSize);

    Task<List<StatusUpdate>> GetUnseenStatusUpdatesForUserAsync(string userId, int pageNumber, int pageSize);

    Task<List<StatusUpdate>> GetUpdatesForIdeaAsync(string ideaId, int pageNumber, int pageSize);

    #endregion
}
