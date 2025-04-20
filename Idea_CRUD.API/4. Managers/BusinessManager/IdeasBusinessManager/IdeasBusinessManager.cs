using Backend.Challenge._1._Common.Contracts.Requests.Idea;
using Backend.Challenge._2._Data.Repositories;
using Backend.Challenge._2._Data.Repositories.DTOs;
using Backend.Challenge._4._Managers.BusinessManager.Contracts.Validator;
using Backend.Challenge.Data.Models;
using Backend.Challenge.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Challenge.BusinessManager
{
    public class IdeasBusinessManager : IIdeasBusinessManager
    {
     private readonly IRepository _repository;

        public IdeasBusinessManager(IRepository repository)
        {
            _repository = repository;
        }
        public async Task<Idea> CreateIdeaAsync(CreateIdeiaRequest request)
        {
            await new IdeaRequestValidator().ValidateAndThrowAsync(request);

            //make in idea format 
            var idea = new Idea
            {
                AuthorId = request.AuthorId,
                UserIds = request.UserIds,
                PublishedAtUtc = DateTime.UtcNow
            };

            idea = await _repository.CreateIdeaAsync(idea);

            var statusUpdate = new StatusUpdate
            {
                IdeaId = idea.Id,
                Title = request.Title,
                Status = request.Status,
                SummaryHtml = request.SummaryHtml,
                AuthorId = request.AuthorId,
                PublishedAtUtc = DateTime.UtcNow
            };

            statusUpdate = await _repository.CreateStatusAsync(statusUpdate);

            idea.LatestStatus = statusUpdate.Id;

            return await _repository.UpdateIdeaAsync(idea);
        }

        public async Task<List<Idea>> GetIdeasForUserAsync(string userId, int pageNumber, int pageSize)
        {
            return await _repository.GetIdeasByUserAsync(userId, pageNumber, pageSize);
        }

        public async Task<List<StatusUpdate>> GetUnseenUpdatesForUserAsync(string userId, int pageNumber, int pageSize)
        {
            return await _repository.GetUnseenStatusUpdatesForUserAsync(userId, pageNumber, pageSize);
        }

        public async Task<List<StatusUpdate>> GetUpdatesForIdeaAsync(string ideaId, int pageNumber, int pageSize)
        {
            return await _repository.GetUpdatesForIdeaAsync(ideaId, pageNumber, pageSize);
        }

        public async Task<StatusUpdate> CreateUpdateAsync(string ideaId, CreateStatusUpdateRequest request)
        {
            var idea = await _repository.GetIdeaByIdeaIdAsync(ideaId);
            if (idea == null)
            {
                throw new KeyNotFoundException("Idea not found");
            }

            var latestUpdate = await _repository.GetIdeaUpdateByUpdateIdAsync(idea.Id);

            var newUpdate = new StatusUpdate
            {
                IdeaId = ideaId,
                Title = request.Title ?? latestUpdate?.Title,
                SummaryHtml = request.SummaryHtml ?? latestUpdate?.SummaryHtml,
                Status = request.Status ?? latestUpdate.Status,
                AuthorId = request.AuthorId,
                PublishedAtUtc = DateTime.UtcNow,
                SeenByUserIds = new List<string>()
            };

            newUpdate = await _repository.CreateStatusAsync(newUpdate);

            idea.LatestStatus = newUpdate.Id;
            await _repository.UpdateIdeaAsync(idea);

            return newUpdate;


        }
    }
}
