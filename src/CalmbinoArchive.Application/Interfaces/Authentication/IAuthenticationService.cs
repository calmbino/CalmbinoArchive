using CalmbinoArchive.Application.DTOs;

namespace CalmbinoArchive.Application.Interfaces.Authentication;

public interface IAuthenticationService
{
    // Task<ServiceResponse> SignUpUser(SignUpRequestDto dto);
    Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
}