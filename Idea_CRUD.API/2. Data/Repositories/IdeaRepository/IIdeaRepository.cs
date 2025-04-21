using Backend.Challenge.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Challenge._2._Data.Repositories.IdeaRepository
{
    public interface IIdeaRepository
    {
        #region Idea
        Task<Idea> CreateIdeaAsync(Idea idea);

        Task<Idea> UpdateIdeaAsync(Idea idea);

        Task<StatusUpdate> CreateStatusAsync(StatusUpdate idea);

        Task<Idea> GetIdeaByIdeaIdAsync(string ideaId);

        Task<StatusUpdate> GetIdeaUpdateByUpdateIdAsync(string updateId);

        Task<List<Idea>> GetIdeasByUserAsync(string userId, int pageNumber, int pageSize);

        Task<List<StatusUpdate>> GetUnseenStatusUpdatesForUserAsync(string userId, int pageNumber, int pageSize);

        Task<List<StatusUpdate>> GetUpdatesForIdeaAsync(string ideaId, int pageNumber, int pageSize);

        Task MarkAsSeenAsync(string updateId, string userId);
        #endregion
    }
}
