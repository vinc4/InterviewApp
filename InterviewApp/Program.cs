using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using InterviewApp.Services;
using MediatR;
using System;
using InterviewApp.Models;

class Program
{
    static async Task Main(string[] args)
    {
        using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) => {
                services.AddTransient<IGreetingService, GreetingService>();
                services.AddTransient<ITimeGreetingService, TimeGreetingService>();
                services.AddMediatR(options => options.RegisterServicesFromAssemblyContaining<Program>());
                services.Configure<GreetingOptions>(context.Configuration.GetSection("Greeting"));
            })
            .Build();

        var mediator = host.Services.GetRequiredService<IMediator>();
        var greeting = await mediator.Send(new InterviewApp.Requests.GreetUserQuery());
        var timeGreeting = await mediator.Send(new InterviewApp.Requests.GetTimeGreetingQuery());
        Console.WriteLine($"{timeGreeting}, {greeting}");


        await host.RunAsync();
    }
}