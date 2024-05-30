namespace CRM.Application.Repositories;

public interface IOrganizationBuilderRepository
{
    Task GenerateDataBase(string slugTenant);
}