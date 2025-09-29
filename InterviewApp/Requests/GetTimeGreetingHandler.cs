using MediatR;
using InterviewApp.Services;
using System.Threading;
using System.Threading.Tasks;

namespace InterviewApp.Requests
{
    public class GetTimeGreetingHandler : IRequestHandler<GetTimeGreetingQuery, string>
    {
        private readonly ITimeGreetingService _timeGreetingService;

        public GetTimeGreetingHandler(ITimeGreetingService timeGreetingService)
        {
            _timeGreetingService = timeGreetingService;
        }

        public Task<string> Handle(GetTimeGreetingQuery request, CancellationToken cancellationToken)
        {
            var timeGreeting = _timeGreetingService.GetTimeGreeting();
            return Task.FromResult(timeGreeting);
        }
    }
}