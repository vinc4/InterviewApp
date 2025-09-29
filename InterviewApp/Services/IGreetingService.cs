using System.Collections.Generic;

namespace InterviewApp.Services;

public interface IGreetingService
{
    void Run();
    string GetGreeting();
    string GetGreeting(string language);
    IEnumerable<string> GetSupportedLanguages();
    bool IsLanguageSupported(string language);
}