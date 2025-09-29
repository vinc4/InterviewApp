using MediatR;
using InterviewApp.Services;
using System.Threading.Tasks;
using System.Threading;

namespace InterviewApp.Requests
{
    public class GreetUserHandler : IRequestHandler<GreetUserQuery, string>
    {
        private readonly IGreetingService _greetingService;

        public GreetUserHandler(IGreetingService greetingService)
        {
            _greetingService = greetingService;
        }

        public Task<string> Handle(GreetUserQuery request, CancellationToken cancellationToken)
        {
            var greeting = request.Language is not null
                ? _greetingService.GetGreeting(request.Language)
                : _greetingService.GetGreeting();

            return Task.FromResult(greeting);
        }

    }
}