using DependencyInjectionSpec.Fakes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Xunit;

namespace DependencyInjectionSpec
{
    public class LifetimeSpec
    {
        private ServiceProvider CreateServiceProvider(IServiceCollection services)
        {
            return services.BuildServiceProvider();
        }

        //
        // Transient은 요청별로 개별 인스턴스를 생성하여 의존성을 주입한다.
        // 
        [Fact]
        public void ReturnDifferentInstances_PerResolution_ForTransientServices()
        {
            // Arrange
            var collection = new TestServiceCollection();
            collection.AddTransient(typeof(IFakeService), typeof(FakeService));
            var provider = CreateServiceProvider(collection);

            // Act
            var service1 = provider.GetService<IFakeService>();
            var service2 = provider.GetService<IFakeService>();

            // Assert
            Assert.IsType<FakeService>(service1);
            Assert.IsType<FakeService>(service2);
            Assert.NotSame(service1, service2);
        }

        //
        // Transient은 요청별로 개별 인스턴스를 생성하여 의존성을 주입한다.
        //
        [Fact]
        public void TransientService_CanBeResolved_FromProvider()
        {
            // Arrange
            var collection = new TestServiceCollection();
            collection.AddTransient(typeof(IFakeService), typeof(FakeService));
            var provider = CreateServiceProvider(collection);

            // Act
            var service1 = provider.GetService<IFakeService>();
            var service2 = provider.GetService<IFakeService>();

            // Assert
            Assert.NotNull(service1);
            Assert.NotSame(service1, service2);
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

        //
        // Scope은 Singleton이다.
        //
        [Fact]
        public void ScopedServiceCanBeResolved()
        {
            // Arrange
            var collection = new TestServiceCollection();
            collection.AddScoped<IFakeScopedService, FakeService>();
            var provider = CreateServiceProvider(collection);

            // Act
            using (var scope = provider.CreateScope())
            {
                var providerScopedService = provider.GetService<IFakeScopedService>();
                var scopedService1 = scope.ServiceProvider.GetService<IFakeScopedService>();
                var scopedService2 = scope.ServiceProvider.GetService<IFakeScopedService>();

                // Assert
                Assert.NotSame(providerScopedService, scopedService1);
                Assert.Same(scopedService1, scopedService2);
            }
        }

        //
        // Scope은 Scope 단위의 Singleton이다.
        //
        [Fact]
        public void NestedScopedServiceCanBeResolved()
        {
            // Arrange
            var collection = new TestServiceCollection();
            collection.AddScoped<IFakeScopedService, FakeService>();
            var provider = CreateServiceProvider(collection);

            // Act
            using (var outerScope = provider.CreateScope())
            using (var innerScope = outerScope.ServiceProvider.CreateScope())
            {
                var outerScopedService = outerScope.ServiceProvider.GetService<IFakeScopedService>();
                var innerScopedService = innerScope.ServiceProvider.GetService<IFakeScopedService>();

                // Assert
                Assert.NotNull(outerScopedService);
                Assert.NotNull(innerScopedService);
                Assert.NotSame(outerScopedService, innerScopedService);
            }
        }

        //
        // cachedScopeFactory?
        // Scope은 인스턴스를 Dispose 시킨다.
        //
        [Fact]
        public void ScopedServices_FromCachedScopeFactory_CanBeResolvedAndDisposed()
        {
            // Arrange
            var collection = new TestServiceCollection();
            collection.AddScoped<IFakeScopedService, FakeService>();
            var provider = CreateServiceProvider(collection);
            var cachedScopeFactory = provider.GetService<IServiceScopeFactory>();

            // Act
            for (var i = 0; i < 3; i++)
            {
                FakeService outerScopedService;
                using (var outerScope = cachedScopeFactory.CreateScope())
                {
                    FakeService innerScopedService;
                    using (var innerScope = outerScope.ServiceProvider.CreateScope())
                    {
                        outerScopedService = outerScope.ServiceProvider.GetService<IFakeScopedService>() as FakeService;
                        innerScopedService = innerScope.ServiceProvider.GetService<IFakeScopedService>() as FakeService;

                        // Assert
                        Assert.NotNull(outerScopedService);
                        Assert.NotNull(innerScopedService);
                        Assert.NotSame(outerScopedService, innerScopedService);
                    }

                    Assert.False(outerScopedService.Disposed);
                    Assert.True(innerScopedService.Disposed);
                }

                Assert.True(outerScopedService.Disposed);
            }
        }
    }
}
