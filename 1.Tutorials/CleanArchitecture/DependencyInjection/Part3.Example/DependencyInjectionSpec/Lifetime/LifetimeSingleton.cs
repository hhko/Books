using DependencyInjectionSpec.Fakes;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace DependencyInjectionSpec.Lifetime
{
    public class LifetimeSingleton
    {
        private ServiceProvider CreateServiceProvider(IServiceCollection services)
        {
            return services.BuildServiceProvider();
        }

        [Fact]
        public void ServiceInstance_CanBeResolved()
        {
            // Arrange
            var collection = new TestServiceCollection();
            var instance = new FakeService();
            collection.AddSingleton(typeof(IFakeServiceInstance), instance);
            var provider = CreateServiceProvider(collection);

            // Act
            var service = provider.GetService<IFakeServiceInstance>();

            // Assert
            Assert.Same(instance, service);
        }

        //
        // Singleton은 요청과 상관없이 한번만 인스턴스를 생성하여 의존성을 주입한다.
        //
        [Fact]
        public void ReturnSameInstances_PerResolution_ForSingletons()
        {
            // Arrange
            var collection = new TestServiceCollection();
            collection.AddSingleton(typeof(IFakeService), typeof(FakeService));
            var provider = CreateServiceProvider(collection);

            // Act
            var service1 = provider.GetService<IFakeService>();
            var service2 = provider.GetService<IFakeService>();

            // Assert
            Assert.IsType<FakeService>(service1);
            Assert.IsType<FakeService>(service2);
            Assert.Same(service1, service2);
        }

        //
        // Singleton
        //
        [Fact]
        public void SingletonService_CanBeResolved()
        {
            // Arrange
            var collection = new TestServiceCollection();
            collection.AddSingleton<IFakeSingletonService, FakeService>();
            var provider = CreateServiceProvider(collection);

            // Act
            var service1 = provider.GetService<IFakeSingletonService>();
            var service2 = provider.GetService<IFakeSingletonService>();

            // Assert
            Assert.NotNull(service1);
            Assert.Same(service1, service2);
        }

        [Fact]
        public void SingletonServices_Come_FromRootProvider()
        {
            // Arrange
            var collection = new TestServiceCollection();
            collection.AddSingleton<IFakeSingletonService, FakeService>();
            var provider = CreateServiceProvider(collection);
            FakeService disposableService1;
            FakeService disposableService2;

            // Act and Assert
            using (var scope = provider.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<IFakeSingletonService>();
                disposableService1 = Assert.IsType<FakeService>(service);
                Assert.False(disposableService1.Disposed);
            }

            Assert.False(disposableService1.Disposed);

            using (var scope = provider.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<IFakeSingletonService>();
                disposableService2 = Assert.IsType<FakeService>(service);
                Assert.False(disposableService2.Disposed);
            }

            Assert.False(disposableService2.Disposed);
            Assert.Same(disposableService1, disposableService2);
        }

        

        
    }
}
