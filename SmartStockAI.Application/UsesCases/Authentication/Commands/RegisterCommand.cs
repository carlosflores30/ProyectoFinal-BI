using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Authentication;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Domain.Authentication.Entities;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Authentication.Commands;

public record RegisterCommand(RegisterRequest Request) : IRequest<LoginResponse>;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, LoginResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;

    public RegisterCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IJwtService jwtService,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
        _mapper = mapper;
    }
    public async Task<LoginResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _unitOfWork.Users.GetByEmailAsync(request.Request.Email);
        if (existingUser != null)
            throw new InvalidOperationException("Un usuario con este email ya existe.");

        var hashedPassword = _passwordHasher.Hash(request.Request.Password);

        var newuser = new Usuario
        {
            Nombre = request.Request.FirstName,
            Apellido = request.Request.LastName,
            Correo = request.Request.Email,
            Contrasena = hashedPassword,
            Telefono = request.Request.Phone,
            IdRol = request.Request.RoleId,
            IntentosFallidos = 0,
            BloqueadoHasta = null,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.Users.AddAsync(newuser);
        await _unitOfWork.SaveChangesAsync();

        var token = _jwtService.GenerateToken(newuser.Id, newuser.Correo, newuser.RoleName ?? "User", newuser.NegocioId);
        
        var role = await _unitOfWork.RolesRepository.GetByIdAsync(newuser.IdRol);


        return new LoginResponse
        {
            FullName = $"{newuser.Nombre} {newuser.Apellido}",
            Email = newuser.Correo,
            Token = token,
            Role = role?.Name
        };
    }
}