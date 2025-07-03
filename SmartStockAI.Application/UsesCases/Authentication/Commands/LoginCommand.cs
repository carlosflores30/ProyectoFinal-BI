using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Authentication;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Authentication.Commands;

public record LoginCommand(LoginRequest Request) : IRequest<LoginResponse>;

public class LoginCommandHandler(ILoginService _loginService) : IRequestHandler<LoginCommand, LoginResponse>
{

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return await _loginService.AttemptLoginAsync(
            request.Request.Email,
            request.Request.Password
        );
    }
}