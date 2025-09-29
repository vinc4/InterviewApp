using System;

namespace InterviewApp.Services
{
    public class TimeGreetingService : ITimeGreetingService
    {
        public string GetTimeGreeting()
        {
            var hour = DateTime.Now.Hour;
            if (hour < 12)
                return "Good morning";
            if (hour < 18)
                return "Good afternoon";
            return "Good evening";
        }
    }
}