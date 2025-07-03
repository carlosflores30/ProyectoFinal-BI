using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Roles;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Roles.Queries;

public record GetAllRolesQuery() : IRequest<List<RoleDto>>;

public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, List<RoleDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllRolesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _unitOfWork.RolesRepository.GetAllAsync();
        return _mapper.Map<List<RoleDto>>(roles);
    }
}