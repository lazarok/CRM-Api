using MediatR;

namespace CRM.Application.NotificationBus;

public class OrganizationCreated : INotification
{
    public required string SlugTenant { get; set; }
}