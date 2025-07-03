using MediatR;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Roles.Commands;

public record DeleteRoleCommand(int Id) : IRequest<Unit>;

public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRoleCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.RolesRepository.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync();
        return Unit.Value;
    }
}