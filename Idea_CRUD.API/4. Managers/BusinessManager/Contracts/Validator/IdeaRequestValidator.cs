using Backend.Challenge._1._Common.Contracts.Requests.Idea;
using FluentValidation;

namespace Backend.Challenge._4._Managers.BusinessManager.Contracts.Validator
{
    public class IdeaRequestValidator: AbstractValidator<CreateIdeiaRequest>
    {
        public IdeaRequestValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(250);
            RuleFor(x => x.SummaryHtml).NotEmpty();
            RuleFor(x => x.AuthorId).NotEmpty();
            RuleFor(x => x.PublishedAt).NotEmpty();
            RuleFor(x => x.Status).NotEmpty();
        }
    }
}
