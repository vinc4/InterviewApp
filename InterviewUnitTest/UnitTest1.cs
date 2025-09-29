using InterviewApp.Models;
using InterviewApp.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using System.Collections.Generic;

namespace InterviewUnitTest
{
    public class GreetingServiceTests
    {
        [Fact]
        public void Run_InvalidConfig_LogsErrorAndDoesNotDisplayGreeting()
        {
            var options = Options.Create(new GreetingOptions
            {
                DefaultMessage = "",
                Language = ""
            });
            var loggerMock = new Mock<ILogger<GreetingService>>();

            var service = new GreetingService(options, loggerMock.Object);

            service.Run();

            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("cannot run due to invalid configuration")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public void GetGreeting_SupportedLanguage_ReturnsConfiguredMessage()
        {
            var options = Options.Create(new GreetingOptions
            {
                DefaultMessage = "Welcome!",
                Language = "English",
                Messages = new Dictionary<string, string>
                {
                    { "English", "Hello!" }
                }
            });
            var loggerMock = new Mock<ILogger<GreetingService>>();
            var service = new GreetingService(options, loggerMock.Object);

         
            var result = service.GetGreeting("English");

            Assert.Equal("Hello!", result);
        }

        [Fact]
        public void GetGreeting_UnsupportedLanguage_ReturnsFallbackMessage()
        {
           
            var options = Options.Create(new GreetingOptions
            {
                DefaultMessage = "Welcome!",
                Language = "Spanish",
                Messages = new Dictionary<string, string>
                {
                    { "English", "Hello!" }
                },
                FallbackLanguage = "English"
            });
            var loggerMock = new Mock<ILogger<GreetingService>>();
            var service = new GreetingService(options, loggerMock.Object);

            
            var result = service.GetGreeting("Zulu");


            Assert.Contains("Hello!", result);
            Assert.Contains("showing English", result);
        }

        [Fact]
        public void GetGreeting_EmptyLanguage_ReturnsFallbackAndLogsError()
        {
            var options = Options.Create(new GreetingOptions
            {
                DefaultMessage = "Welcome!",
                Language = "English",
                Messages = new Dictionary<string, string>
                {
                    { "English", "Hello!" }
                }
            });
            var loggerMock = new Mock<ILogger<GreetingService>>();
            var service = new GreetingService(options, loggerMock.Object);

            var result = service.GetGreeting("");
            Assert.Contains("Hello!", result); 
            Assert.Contains("cannot be empty", result);

            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Language cannot be empty")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public void GetGreeting_UnsupportedLanguage_NoFallbackLanguage_UsesDefaultMessage()
        {
            var options = Options.Create(new GreetingOptions
            { 
                DefaultMessage = "Welcome!",    
                Language = "German",
                FallbackLanguage = null, // Explicitly set to null so no fallback is used
                Messages = new Dictionary<string, string>
                {
                    { "English", "Hello!" }
                }
            });
            var loggerMock = new Mock<ILogger<GreetingService>>();
            var service = new GreetingService(options, loggerMock.Object);

            var result = service.GetGreeting("German");
            Assert.Contains("Welcome!", result); 
            Assert.Contains("showing default", result);
        }

        [Fact]
        public void IsLanguageSupported_ReturnsTrueForSupportedLanguage()
        {
            var options = Options.Create(new GreetingOptions
            {
                DefaultMessage = "Welcome!",
                Language = "English",
                Messages = new Dictionary<string, string>
                {
                    { "English", "Hello!" }
                }
            });
            var loggerMock = new Mock<ILogger<GreetingService>>();
            var service = new GreetingService(options, loggerMock.Object);

            Assert.True(service.IsLanguageSupported("English"));
            Assert.False(service.IsLanguageSupported("Zulu"));
        }

        [Fact]
        public void GetSupportedLanguages_ReturnsAllConfiguredLanguages()
        {
            var options = Options.Create(new GreetingOptions
            {
                DefaultMessage = "Welcome!",
                Language = "English",
                Messages = new Dictionary<string, string>
                {
                    { "English", "Hello!" },
                    { "Afrikaans", "Hallo!" }
                }
            });
            var loggerMock = new Mock<ILogger<GreetingService>>();
            var service = new GreetingService(options, loggerMock.Object);

            var supported = service.GetSupportedLanguages();
            Assert.Contains("English", supported);
            Assert.Contains("Afrikaans", supported);
            Assert.DoesNotContain("Zulu", supported);
        }
    }
}
