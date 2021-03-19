using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;

namespace Step_04_TryAddEnumableDuplicatedDIs
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();

            // 
            // "서비스 : 구현 = 1 : N" 관계를 등록한다.
            // void TryAddEnumerable(this IServiceCollection services, IEnumerable<ServiceDescriptor> descriptors)
            //
            services.TryAddEnumerable(new[]
            {
                ServiceDescriptor.Transient<IGreeting, Hello>(),
                ServiceDescriptor.Transient<IGreeting, Hi>()
            });

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

    public class ConsoleApp
    {
        private readonly IEnumerable<IGreeting> _greetings;

        public ConsoleApp(IEnumerable<IGreeting> greetings)
        {
            _greetings = greetings;
        }

        public void Print()
        {
            foreach (var greeting in _greetings)
                Console.WriteLine(greeting.Message);
        }
    }
}
