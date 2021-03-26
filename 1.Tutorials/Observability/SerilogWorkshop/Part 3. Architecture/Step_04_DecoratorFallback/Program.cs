using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Exceptions;
using SerilogTimings;
using Scrutor;
using System.Collections.Generic;
using System.Linq;
using Polly;
using MediatR.Pipeline;

namespace Step_04_DecoratorFallback
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

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LogBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(FallbackBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RetryBehavior<,>));

            services.Scan(scan => scan
                .FromAssemblies(typeof(Program).Assembly)
                .AddClasses(classes => classes.AssignableTo(typeof(IRetryRequest<,>)))
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsImplementedInterfaces()
                .WithTransientLifetime());

            IServiceProvider provider = services.BuildServiceProvider();
            IMediator mediator = provider.GetRequiredService<IMediator>();

            try
            {
                //await mediator.Send(new Divide(2021, 100));
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
