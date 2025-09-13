using MediatR;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.Educations
{
    public record GetEducationByIdQuery(long Id) : IRequest<Education>;
    public class GetEducationByIdHandler : IRequestHandler<GetEducationByIdQuery, Education>
    {
        private readonly IEducationRepository _repo;

        public GetEducationByIdHandler(IEducationRepository repo)
        {
            _repo = repo;
        }

        public async Task<Education> Handle(GetEducationByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetByIdAsync(request.Id, cancellationToken);
        }
    }
}

