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
        private IRepository _repository;

        public UsersBusinessManager(IRepository repository)
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
            List<User> users;
            if (ids == null || ids.Length == 0)
            {
                // Fetch all users
                users = await _repository.GetAllUsersAsync();

            }
            else
            {
                users = await FetchUsersById(ids);
            }

            return new GetUserResponse
            {
                Users = users.ToDictionary(
                    u => u.Id,
                    u => u
                )
            };
        }

        private async Task<List<User>> FetchUsersById(string[] ids)
        {
            // Fetch specific users by IDs
            var users = await _repository.GetUsersByIdsAsync(ids);
            var foundIds = users.Select(u => u.Id).ToArray();
            var notFoundIds = ids.Except(foundIds).ToArray();

            //Log unfound ids
            if (notFoundIds.Any())
            {
                Console.WriteLine($"The following IDs were not found: {string.Join(", ", notFoundIds)}");
            }

            return users;
        }
    }
}
