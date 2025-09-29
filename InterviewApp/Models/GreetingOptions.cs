using System.Collections.Generic;

namespace InterviewApp.Models
{
    public class GreetingOptions
    {
        public string Language { get; set; } = "English";
        public string DefaultMessage { get; set; } = "Welcome!";
        public Dictionary<string, string> Messages { get; set; } = new();
        public string FallbackLanguage { get; set; } = "English";
    }
}