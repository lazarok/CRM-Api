using MediatR;

namespace CRM.Application.ServiceBus;

public class OrganizationCreated : INotification
{
    
}

public class OrganizationCreatedHandler : INotificationHandler<OrganizationCreated>
{
    public Task Handle(OrganizationCreated notification, CancellationToken cancellationToken)
    {
        // USE Services / applications
        throw new NotImplementedException();
    }
}