using Hangfire;
using MediatR;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Application.Interfaces.Notifications;
using SmartStockAI.Domain.Authentication.Interfaces;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Authentication.Commands;

public record ResetPasswordCommand(string Token, string NewPassword) : IRequest<Unit>;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordResetTokenRepository _tokenRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEmailService _emailService;

    public ResetPasswordCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordResetTokenRepository tokenRepository,
        IPasswordHasher passwordHasher,
        IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _tokenRepository = tokenRepository;
        _passwordHasher = passwordHasher;
        _emailService = emailService;
    }

    public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var token = await _tokenRepository.GetByTokenAsync(request.Token);
        if (token == null)
            throw new InvalidOperationException("El token no es válido.");

        if (token.Expiration < DateTime.UtcNow)
            throw new InvalidOperationException("El token ha expirado.");

        var hashedPassword = _passwordHasher.Hash(request.NewPassword);
        
        var usuario = await _unitOfWork.Users.GetByIdAsync(token.UserId);
        if (usuario == null)
            throw new InvalidOperationException("El usuario no existe.");
        
        await _unitOfWork.Users.UpdatePasswordAsync(token.UserId, hashedPassword);
        await _tokenRepository.DeleteAsync(token.Id);
        await _unitOfWork.SaveChangesAsync();
        
        await _emailService.SendAsync(usuario.Correo, 
            "Contraseña restablecida", 
            $"Hola {usuario.Nombre}, tu contraseña fue restablecida con éxito.");

        return Unit.Value;
    }
}