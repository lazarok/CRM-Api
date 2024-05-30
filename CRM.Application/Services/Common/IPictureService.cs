using CRM.Application.Services.DTOs;

namespace CRM.Application.Services.Common;

public interface IPictureService
{
    Task<MediaFileDto> GetPictureAsync(string name, CancellationToken cancellationToken = default);
    Task<MediaFileDto> GetPictureAsync(string containerName, string name, CancellationToken cancellationToken = default);
    Task<bool> HasPictureAsync(string name, CancellationToken cancellationToken = default);
    Task SavePictureAsync(string name, byte[] picture, CancellationToken cancellationToken = default);
    Task SavePictureAsync(string name, Stream stream, CancellationToken cancellationToken = default);
    void DeletePictureAsync(string name, CancellationToken cancellationToken = default);
}