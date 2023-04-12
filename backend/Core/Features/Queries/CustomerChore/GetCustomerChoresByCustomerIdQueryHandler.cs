using Core.Features.Queries.Chores;
using Core.Features.Queries.Customers;
using Core.Features.Queries.Periodics;
using Core.Repository.Interfaces;
using MapsterMapper;
using MediatR;
using Core.Features.Queries.ChoreStatuses;

namespace Core.Features.Queries.CustomerChores;

public class GetCustomerChoresByCustomerIdQueryHandler : IRequestHandler<GetCustomerChoresByCustomerIdQuery, IList<Domain.CustomerChore>>
{
    private readonly ICustomerChoreRepository _repo;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public GetCustomerChoresByCustomerIdQueryHandler(ICustomerChoreRepository repo, IMapper mapper, IMediator mediator)
    {
        _repo = repo;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<IList<Domain.CustomerChore>> Handle(GetCustomerChoresByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        var customerChores = _mapper.Map<IList<Domain.CustomerChore>>(await _repo.GetQuery(x => x.CustomerId == request.Id));
        var chores = await _mediator.Send(new GetAllChoresQuery(), cancellationToken);
        var customers = await _mediator.Send(new GetAllCustomersQuery(), cancellationToken);
        var periodic = await _mediator.Send(new GetAllPeriodicsQuery(), cancellationToken);

        foreach (var customerChore in customerChores)
        {
            var allChoreStatuses = await _mediator.Send(new GetAllChoreStatusesQuery());
            var customerChoreProgress = allChoreStatuses.Count(x => x.CustomerChoreId == customerChore.Id.ToString());
            customerChore.Progress = customerChoreProgress;
            customerChore.Status = customerChoreProgress == 0 ? "Ej påbörjad" : customerChoreProgress == customerChore.Frequency ? "Klar" : "Påbörjad";
            customerChore.Reset = "Resettar om sju dagar";
            customerChore.Chore = chores.FirstOrDefault(x => x.Id.ToString() == customerChore.ChoreId);
            customerChore.Customer = customers.FirstOrDefault(x => x.Id.ToString() == customerChore.CustomerId);
            customerChore.Periodic = periodic.FirstOrDefault(x => x.Id.ToString() == customerChore.PeriodicId);
        }

        return customerChores;
    }
}