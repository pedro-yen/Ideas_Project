using Backend.Challenge._1._Common.Contracts.Requests.Users;
using Backend.Challenge.Data.Models;
using Backend.Challenge.Dtos;
using Backend.Challenge.ServiceModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Challenge._2._Data.Repositories;

public interface IUserRepository
{
    #region Users
    Task<User> CreateUserAsync(User user);
    Task<List<User>> GetAllUsersAsync();
    Task<List<User>> GetUsersByIdsAsync(string[] ids);
    #endregion

    
}
