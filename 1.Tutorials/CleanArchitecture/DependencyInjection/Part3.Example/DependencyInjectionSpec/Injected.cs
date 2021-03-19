using DependencyInjectionSpec.Fakes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DependencyInjectionSpec
{
    public class Injected
    {
        private ServiceProvider CreateServiceProvider(IServiceCollection services)
        {
            return services.BuildServiceProvider();
        }

        //
        // 객체 그래프로 의존성을 자동으로 주입한다.
        //
        [Fact]
        public void OuterService_CanHave_OtherServicesInjected()
        {
            // Arrange
            var collection = new TestServiceCollection();
            var fakeService = new FakeService();
            collection.AddTransient<IFakeOuterService, FakeOuterService>();
            collection.AddSingleton<IFakeService>(fakeService);
            collection.AddTransient<IFakeMultipleService, FakeOneMultipleService>();
            collection.AddTransient<IFakeMultipleService, FakeTwoMultipleService>();
            var provider = CreateServiceProvider(collection);

            // Act
            var services = provider.GetService<IFakeOuterService>();

            // Assert
            Assert.Same(fakeService, services.SingleService);
            Assert.Collection(services.MultipleServices.OrderBy(s => s.GetType().FullName),
                service => Assert.IsType<FakeOneMultipleService>(service),
                service => Assert.IsType<FakeTwoMultipleService>(service));
        }

        //
        // 객체 그래프로 의존성을 자동으로 주입한다.
        //
        [Fact]
        public void FactoryServices_AreCreated_AsPartOfCreatingObjectGraph()
        {
            // Arrange
            var collection = new TestServiceCollection();
            collection.AddTransient<IFakeService, FakeService>();
            collection.AddTransient<IFactoryService>(p =>
            {
                var fakeService = p.GetService<IFakeService>();
                return new TransientFactoryService
                {
                    FakeService = fakeService,
                    Value = 42
                };
            });
            collection.AddScoped(p =>
            {
                var fakeService = p.GetService<IFakeService>();
                return new ScopedFactoryService
                {
                    FakeService = fakeService,
                };
            });
            collection.AddTransient<ServiceAcceptingFactoryService>();
            var provider = CreateServiceProvider(collection);

            // Act
            var service1 = provider.GetService<ServiceAcceptingFactoryService>();
            var service2 = provider.GetService<ServiceAcceptingFactoryService>();

            // Assert
            Assert.Equal(42, service1.TransientService.Value);
            Assert.NotNull(service1.TransientService.FakeService);

            Assert.Equal(42, service2.TransientService.Value);
            Assert.NotNull(service2.TransientService.FakeService);

            Assert.NotNull(service1.ScopedService.FakeService);

            // Verify scoping works
            Assert.NotSame(service1.TransientService, service2.TransientService);
            Assert.Same(service1.ScopedService, service2.ScopedService);
        }
    }
}
