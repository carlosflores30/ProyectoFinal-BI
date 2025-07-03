using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Roles;
using SmartStockAI.Domain.Roles.Entities;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Roles.Commands;

public record CreateRoleCommand(CreateRoleRequest Request) : IRequest<RoleDto>;

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, RoleDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateRoleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<RoleDto> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = new Role
        {
            Name = request.Request.Name
        };

        await _unitOfWork.RolesRepository.AddAsync(role);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<RoleDto>(role);
    }
}