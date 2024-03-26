namespace AgencyService.Application.Contracts.Api;

public interface ICurrentUserService
{
    string? UserId { get; }
}
