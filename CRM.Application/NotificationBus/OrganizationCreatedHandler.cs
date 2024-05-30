using CRM.Application.Repositories;
using MediatR;

namespace CRM.Application.NotificationBus;

public class OrganizationCreatedHandler : INotificationHandler<OrganizationCreated>
{
    private readonly IOrganizationBuilderRepository _repository;

    public OrganizationCreatedHandler(IOrganizationBuilderRepository repository)
    {
        _repository = repository;
    }
    
    public async Task Handle(OrganizationCreated notification, CancellationToken cancellationToken)
    {
        await _repository.GenerateDataBase(notification.SlugTenant);
    }
}