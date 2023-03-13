using Domain.Domain;
using Domain.Features.Queries.CustomerChores;
using Domain.Repository.Interfaces;
using MapsterMapper;
using MediatR;

namespace Domain.Features.Queries.ChoreStatuses;

public class GetChoreStatusByIdQueryHandler : IRequestHandler<GetChoreStatusByIdQuery, IList<Domain.ChoreStatus>>
{
    private readonly IChoreStatusRepository _repo;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ICache _cache;

    public GetChoreStatusByIdQueryHandler(IChoreStatusRepository repo, IMapper mapper, IMediator mediator, ICache cache)
    {
        _cache = cache;
        _repo = repo;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<IList<Domain.ChoreStatus>> Handle(GetChoreStatusByIdQuery request, CancellationToken cancellationToken)
    {
        if (_cache.Exists("ChoreStatuses:"))
        {
            return await _cache.GetAsync<IList<Domain.ChoreStatus>>("ChoreStatuses:");
        }

        var ChoreStatuses = _mapper.Map<IList<Domain.ChoreStatus>>(await _repo.GetQuery(x => x.CustomerChoreId == request.Id));
        var customerChores = await _mediator.Send(new GetAllCustomerChoresQuery(), cancellationToken);

        foreach (var ChoreStatus in ChoreStatuses)
        {
            ChoreStatus.CustomerChoreId = customerChores.FirstOrDefault(x => x.Id.ToString() == ChoreStatus.CustomerChoreId).Id.ToString();
        }

        await _cache.SetAsync($"ChoreStatus:{request.Id}", ChoreStatuses);
        return ChoreStatuses;
    }
}