using Application.Persistance;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Audits.Command
{
    public sealed class AuditListDeleteCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<AuditListDeleteCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<int>> Handle(AuditListDeleteCommand request, CancellationToken cancellationToken)
        {
            var audits = await _unitOfWork.AuditRepository.GetList();
            foreach (var audit in audits)
            {
                _unitOfWork.AuditRepository.Remove(audit);
            }

            var rowsAffected = await _unitOfWork.SaveChanges(cancellationToken);

            return rowsAffected;
        }    
    }
}
