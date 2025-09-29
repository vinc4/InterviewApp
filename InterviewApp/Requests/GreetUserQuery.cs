using MediatR;

namespace InterviewApp.Requests
{
    public class GreetUserQuery : IRequest<string>
    {
        public string? Language { get; }

        public GreetUserQuery(string? language = null)
        {
            Language = language;
        }
    }
}