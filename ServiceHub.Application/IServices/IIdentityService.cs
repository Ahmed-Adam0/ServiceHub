using System.Threading.Tasks;
using ServiceHub.Application.DTOs;

namespace ServiceHub.Application.IServices;

public interface IIdentityService
{
    Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request);
}
