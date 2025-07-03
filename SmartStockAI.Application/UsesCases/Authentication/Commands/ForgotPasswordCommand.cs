using AutoMapper;
using MediatR;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Domain.Authentication.Entities;
using SmartStockAI.Domain.Authentication.Interfaces;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Authentication.Commands;

public record ForgotPasswordCommand(string Email) : IRequest<Unit>;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordResetTokenRepository _tokenRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly IFrontendUrlProvider _urlProvider;

    public ForgotPasswordCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordResetTokenRepository tokenRepository,
        IPasswordHasher passwordHasher,
        IMapper mapper,
        IEmailService emailService,
        IFrontendUrlProvider urlProvider)
    {
        _unitOfWork = unitOfWork;
        _tokenRepository = tokenRepository;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
        _emailService = emailService;
        _urlProvider = urlProvider;
    }

    public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(request.Email);
        if (user == null)
            throw new InvalidOperationException("No existe un usuario con ese correo electrónico.");

        var token = new PasswordResetToken
        {
            Id = Guid.NewGuid(),
            Token = Guid.NewGuid().ToString("N"),
            Expiration = DateTime.UtcNow.AddMinutes(15),
            UserId = user.Id
        };

        await _tokenRepository.AddAsync(token);
        await _unitOfWork.SaveChangesAsync();

        var link = _urlProvider.GetResetPasswordUrl(token.Token);
        var body = $"Hola {user.Nombre},\n\n" +
                   $"Haz clic en el siguiente enlace para restablecer tu contraseña:\n\n" +
                   $"{link}\n\n" +
                   $"Este enlace expirará en 15 minutos.\n\n" +
                   $"Si no solicitaste este cambio, puedes ignorar este mensaje.";

        await _emailService.SendAsync(user.Correo, "Recuperación de contraseña", body);

        return Unit.Value;
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
    }
}