using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Step_05_Factory
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddTransient<Hello>();
            services.AddTransient<Hi>();

            //
            // Factory로 객체 생성하기 
            //
            // IServiceCollection AddTransient<TService>(Func<IServiceProvider, TService> implementationFactory)
            // IServiceCollection AddTransient<TService, TImplementation>(this IServiceCollection services, Func<IServiceProvider, TImplementation> implementationFactory)
            services.AddTransient<IGreeting>(sp =>
                new CompositeGreeting(new IGreeting[]
                {
                    sp.GetService<Hello>(),
                    sp.GetService<Hi>()
                }));

            services.AddTransient<ConsoleApp>();
            var provider = services.BuildServiceProvider();

            var app = provider.GetRequiredService<ConsoleApp>();
            app.Print();
        }
    }

    public interface IGreeting
    {
        string Message { get; }
    }

    public class Hello : IGreeting
    {
        public string Message => "Hello";
    }

    public class Hi : IGreeting
    {
        public string Message => "Hi";
    }

    public class CompositeGreeting : IGreeting
    {
        private readonly IEnumerable<IGreeting> _greetings;

        public CompositeGreeting(IEnumerable<IGreeting> greetings)
        {
            _greetings = greetings;
        }

        public string Message => _greetings.Aggregate(new StringBuilder(), (current, next) => current.Append(next.Message)).ToString();
    }

    public class ConsoleApp
    {
        private readonly IGreeting _greeting;

        public ConsoleApp(IGreeting greeting)
        {
            _greeting = greeting;
        }

        public void Print()
        {
            Console.WriteLine(_greeting.Message);
        }
    }
}
