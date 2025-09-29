using InterviewApp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InterviewApp.Services
{
    public class GreetingService : IGreetingService
    {
        private readonly GreetingOptions _options;
        private readonly ILogger<GreetingService> _logger;
        private readonly bool _isConfigValid;

        public GreetingService(IOptions<GreetingOptions> options, ILogger<GreetingService> logger)
        {
            _options = options.Value;
            _logger = logger;

            if (string.IsNullOrWhiteSpace(_options.DefaultMessage))
            {
                _logger.LogError("Configuration error: 'DefaultMessage' is required in appsettings.json.");
                _isConfigValid = false;
                return;
            }

            if (string.IsNullOrWhiteSpace(_options.Language))
            {
                _logger.LogError("Configuration error: 'Language' is required in appsettings.json.");
                _isConfigValid = false;
                return;
            }

            _isConfigValid = true;
        }

        public void Run()
        {
            if (!_isConfigValid)
            {
                _logger.LogError("GreetingService cannot run due to invalid configuration.");
                return;
            }

            _logger.LogInformation("GreetingService started.");

            var greeting = GetGreeting();
            Console.WriteLine(greeting);

            _logger.LogInformation("Displayed greeting: {Greeting}", greeting);
        }

        public string GetGreeting(string language)
        {
            if (string.IsNullOrWhiteSpace(language))
            {
                _logger.LogError("Language cannot be empty.");
                return GetFallbackMessage("Language cannot be empty");
            }

            if (_options.Messages?.ContainsKey(language) == true)
            {
                return _options.Messages[language];
            }

            _logger.LogWarning("Unsupported language: {Language}", language);
            return GetFallbackMessage($"Language '{language}' not supported");
        }

        public string GetGreeting()
        {
            return GetGreeting(_options.Language);
        }

        public IEnumerable<string> GetSupportedLanguages()
        {
            return _options.Messages?.Keys ?? Enumerable.Empty<string>();
        }

        public bool IsLanguageSupported(string language)
        {
            return _options.Messages?.ContainsKey(language) == true;
        }

        private string GetFallbackMessage(string reason)
        {
            if (!string.IsNullOrWhiteSpace(_options.FallbackLanguage) &&
                _options.Messages?.ContainsKey(_options.FallbackLanguage) == true)
            {
                var fallbackMessage = _options.Messages[_options.FallbackLanguage];
                _logger.LogInformation("Using fallback language: {FallbackLanguage}", _options.FallbackLanguage);
                return $"{fallbackMessage} ({reason}, showing {_options.FallbackLanguage})";
            }

            var defaultMessage = !string.IsNullOrWhiteSpace(_options.DefaultMessage)
                ? _options.DefaultMessage
                : "Welcome!";

            _logger.LogInformation("Using default message.");
            return $"{defaultMessage} ({reason}, showing default)";
        }
    }
}