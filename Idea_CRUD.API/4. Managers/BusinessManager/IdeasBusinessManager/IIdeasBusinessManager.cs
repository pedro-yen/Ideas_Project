using Backend.Challenge._1._Common.Contracts.Requests.Idea;
using Backend.Challenge._2._Data.Repositories.DTOs;
using Backend.Challenge.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Challenge.BusinessManager
{
    public interface IIdeasBusinessManager
    {
        Task<Idea> CreateIdeaAsync(CreateIdeiaRequest idea);

        Task<List<Idea>> GetIdeasForUserAsync(string userId, int pageNumber, int pageSize);

        Task<List<StatusUpdate>> GetUnseenUpdatesForUserAsync(string userId, int pageNumber, int pageSize);

        Task<List<StatusUpdate>> GetUpdatesForIdeaAsync(string ideaId, int pageNumber, int pageSize);

        Task<StatusUpdate> CreateUpdateAsync(string ideaId, CreateStatusUpdateRequest request);
    }
}
