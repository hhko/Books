using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Exceptions;
using SerilogTimings;

namespace Step_02_DecoratorPattern
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.WithExceptionDetails()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithAssemblyName()
                .Enrich.WithAssemblyVersion()
                .Enrich.WithProcessId()
                .Enrich.WithProcessName()
                .Enrich.WithThreadId()
                .Enrich.WithThreadName()
                //.WriteTo.Console(formatter: new JsonFormatter())
                .WriteTo.Console()
                .CreateLogger();

            Thread.CurrentThread.Name = "Main Thread";

            IServiceCollection services = new ServiceCollection();
            services.AddSingleton(typeof(ILogger), Log.Logger);
            services.AddMediatR(typeof(Program));

            //
            // behavior 의존성을 등록한다.
            //
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LogBehavior<,>));

            IServiceProvider provider = services.BuildServiceProvider();
            IMediator mediator = provider.GetRequiredService<IMediator>();

            try
            {
                await mediator.Send(new Divide(2021, 0));
            }
            catch (Exception exp)
            {
                Log.Error(exp, "최상단에서 예외가 발생한다.");
            }

            Log.CloseAndFlush();
        }
    }
}
