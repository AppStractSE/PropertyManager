using MediatR;

namespace Core.Features.Commands.Chore;

public class AddChoreCommand : IRequest<Domain.Chore>
{
    public string SubCategoryId { get; set; }
    public string Description { get; set; }
    public string Title { get; set; }
}