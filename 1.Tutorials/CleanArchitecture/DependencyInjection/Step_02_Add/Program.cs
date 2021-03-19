using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Step_02_Duplicated
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();

            // 
            // ServiceDescriptor로 "서비스"와 "구현"을 등록한다.
            //
            // IServiceCollection Add(this IServiceCollection collection, ServiceDescriptor descriptor);
            // IServiceCollection Add(this IServiceCollection collection, IEnumerable<ServiceDescriptor> descriptors);
            //
            services.Add(ServiceDescriptor.Transient<IGreeting, Hello>());

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
