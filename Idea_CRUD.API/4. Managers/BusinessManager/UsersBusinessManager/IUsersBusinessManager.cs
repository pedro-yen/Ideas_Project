using Backend.Challenge._1._Common.Contracts.Requests.Users;
using Backend.Challenge.ServiceModels;
using System.Threading.Tasks;

namespace Backend.Challenge.BusinessManager
{
    public interface IUsersBusinessManager
    {
        Task<GetUserResponse> CreateNewUser(CreateUserRequest request);
        Task<GetUserResponse> GetUsersAsync(string[] ids);
    }
}
