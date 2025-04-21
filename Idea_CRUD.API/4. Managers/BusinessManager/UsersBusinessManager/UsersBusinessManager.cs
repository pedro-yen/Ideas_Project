using Backend.Challenge._1._Common.Contracts.Requests.Users;
using Backend.Challenge._2._Data.Repositories;
using Backend.Challenge.Dtos;
using Backend.Challenge.ServiceModels;
using Raven.Client.Documents;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Backend.Challenge.BusinessManager
{
    public class UsersBusinessManager : IUsersBusinessManager
    {
        private IUserRepository _repository;

        public UsersBusinessManager(IUserRepository repository)
        {
            _repository = repository;
        }
        public async Task<GetUserResponse> CreateNewUser(CreateUserRequest request)
        {
            //validate input
            //autommapper from request to user model
            var user = await _repository.CreateUserAsync(new User {
                Email = request?.Email,
                Username = request?.Username
            });

            return new GetUserResponse { Users = new System.Collections.Generic.Dictionary<string, User> { { user.Id, user } } };
        }
        public async Task<GetUserResponse> GetUsersAsync(string[] ids)
        {
            var users= new List<User>();
            if (ids == null || ids.Length == 0)
            {
                users = await FetchAllUsers(users);
            }
            else
            {
                users = await FetchUsersById(ids);
            }

            if (users == null || users.Count == 0 )
            {
                throw new KeyNotFoundException("No users were found for the provided IDs.");
            }

            return new GetUserResponse
            {
                Users = users.ToDictionary(
                    u => u?.Id,
                    u => u
                )
            };
        }

        private async Task<List<User>> FetchAllUsers(List<User> users)
        {
            users = await _repository.GetAllUsersAsync();
            return users;
        }

        private async Task<List<User>> FetchUsersById(string[] ids)
        {
            // Fetch specific users by IDs
            return await _repository.GetUsersByIdsAsync(ids);
        }
    }
}
