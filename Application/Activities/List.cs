using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class List
    {
        // MediatR request that return a value - List<Activity>
        // (a single request being handled by a single handler)
        public class Query : IRequest<Result<List<Activity>>> {}

        // handler that will actually perform a request
        public class Handler : IRequestHandler<Query, Result<List<Activity>>>
        {
        private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<List<Activity>>> Handle(Query request, CancellationToken cancellationToken)
            {   
                return Result<List<Activity>>.Success(await _context.Activities.ToListAsync());
            }
        }
    }
}