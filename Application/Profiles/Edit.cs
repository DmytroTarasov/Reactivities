using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;

namespace Application.Profiles
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>> {
            public string DisplayName { get; set; }
            public string Bio { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
            }
            public class CommandValidator : AbstractValidator<Command>
            {
                public CommandValidator()
                {
                    // set the validator for Profile 
                    RuleFor(x => x.DisplayName).NotEmpty();
                }
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                // user to be edited
                var user = await _context.Users
                    .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

                // Console.WriteLine("Request bio: " + request.Profile.Bio);
                // Console.WriteLine("Request displayName: " + request.Profile.DisplayName);
                user.Bio = request.Bio;
                user.DisplayName = request.DisplayName;

                if (user == null) return null;

                var result = await _context.SaveChangesAsync() > 0;
                if (!result) return Result<Unit>.Failure("Failed to update profile");
                
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}